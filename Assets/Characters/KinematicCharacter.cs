using System;
using FoxShooter.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FoxShooter.Characters
{
    [RequireComponent(typeof(CharacterController))]
    public class KinematicCharacter : MonoBehaviour
    {
	    private const float GravityConstant = -9.8f;
		private const float DefaultMaxFallSpeed = 1000.0f;
		private const float NegativeKillY = 1000.0f;

		// MOVEMENT
		[Header("Movement")]
		// How quickly the character accelerates
		[SerializeField]
		private float acceleration = 50.0f;
		
		// The max speed the character can travel while on the ground
		[SerializeField]
		private float maxSpeed = 5.0f;

		// The max speed the character can travel while on the ground and crouched
		[SerializeField]
		private float crouchedMaxSpeed = 5.0f;

		// How quickly the character decelerates when it either has no move input,
		// or has somehow moved beyond its max speed, e.g., its aerial speed is higher
		// than its walk speed, and it just landed on the ground
		[SerializeField]
		private float friction = 50.0f;
		
		[SerializeField] private bool isCrouched;
		
		
		// AERIAL
		[Header("Aerial")]
		// Max horizontal speed in the air
		[SerializeField]
		private float maxAirSpeedHorizontal = 10.0f;
		
		// Max vertical speed in the air
		[SerializeField]
		private float maxAirSpeedVertical = DefaultMaxFallSpeed;

		// Acceleration in the air, as a factor of regular acceleration
		[SerializeField] private float airControlFactor = 0.5f;

		// Downward acceleration due to gravity, in Gs
		// 9.8m/s^2
		[SerializeField]
		private float gravity = 1.0f;

		[SerializeField]
		private float groundCheckDistance = 4.0f;

		[SerializeField]
		private float groundCheckDistanceAerial = 4.0f;

		[SerializeField]
		private LayerMask groundLayers;
		
		
		// JUMPING
		[Header("Jumping")]
		
		// The speed the character will travel vertically while jumping
		[SerializeField]
		private float jumpStrength = 100.0f;

		[SerializeField]
		private float jumpHeldGravityFactor = 0.5f;

		// How many times the character can jump without landing
		// Walking off of a platform will consume one jump
		[SerializeField] private uint numJumps = 2;

		// Number of jumps remaining. Should not be set, this is only to
		// show in the editor
		[SerializeField] private uint numJumpsRemaining;
		
		// Maximum time a regular jump can be held
		[SerializeField]
		private float jumpTime = 0.5f;
		
		// Grace period after leaving a platform before the first jump is automatically consumed
		[SerializeField]
		private float coyoteTime = 0.5f;

		[Header("Look")]
		[SerializeField] private float lookSensitivity = 1.0f;

		[SerializeField] private Camera camera;

		[SerializeField] private Vector3 currentVelocity;

		[SerializeField] private float cameraPitch;
		
		private bool _grounded;
		private bool _isJumping;
		private Vector2 _moveInput;
		private Vector3 _groundNormal;

		
		// Component cached references
		private CharacterController _characterController;
		private void Awake()
		{
			_characterController = GetComponent<CharacterController>();
		}

		private void FixedUpdate()
		{
			var currentVelocity2D = new Vector2(currentVelocity.x, currentVelocity.z);
			var currentMaxSpeed = GetMaxHorizontalSpeed();
			var rotatedInput = StarMath.RotateVector(_moveInput, -transform.rotation.eulerAngles.y * Mathf.Deg2Rad);

			currentVelocity2D = _moveInput.Equals(Vector2.zero) ?
				StarMath.MoveTo(currentVelocity2D, Vector2.zero, GetFriction() * Time.fixedDeltaTime) :
				StarMath.MoveTo(currentVelocity2D, rotatedInput * currentMaxSpeed,GetAcceleration() * Time.fixedDeltaTime);
			
			if (!_grounded)
			{
				var jumpFactor = _isJumping ? jumpHeldGravityFactor : 1.0f;
				currentVelocity.y += GravityConstant * gravity  * Time.fixedDeltaTime * jumpFactor;
			}
			else
			{
				currentVelocity.y = 0.0f;
			}

			currentVelocity.x = currentVelocity2D.x;
			currentVelocity.z = currentVelocity2D.y;
			var moveHitResult = _characterController.Move(currentVelocity * Time.fixedDeltaTime);
			currentVelocity = _characterController.velocity;

			CheckGround();
			/*if (_grounded && !onFloorBeforeMove)
			{
				Land();
			}
			else if (!_grounded && onFloorBeforeMove)
			{
				LeavePlatform();
			}*/
		}

		public void OnMove(InputValue value)
		{
			_moveInput = value.Get<Vector2>();
		}

		public void OnLook(InputValue value)
		{
			var look = value.Get<Vector2>();
			transform.Rotate(transform.up, look.x * lookSensitivity);

			if (look.y == 0.0f) { return; }
			cameraPitch = Mathf.Clamp(cameraPitch + look.y, -85.0f, 85.0f);
			camera.transform.localEulerAngles = new Vector3(cameraPitch, 0.0f, 0.0f);
		}
		
		private float GetMaxHorizontalSpeed()
		{
			return _grounded ? (isCrouched ? crouchedMaxSpeed : maxSpeed) :
				maxAirSpeedHorizontal;
		}
		
		private float GetAcceleration()
		{
			return _grounded ? acceleration : acceleration * airControlFactor;
		}
		
		private float GetFriction()
		{
			return _grounded ? friction : friction * airControlFactor;
		}
		
		private void CheckGround()
        {
            // Make sure that the ground check distance while already in air is very small, to prevent suddenly snapping to ground
            var chosenGroundCheckDistance = _grounded ? _characterController.skinWidth + groundCheckDistance : groundCheckDistanceAerial;

            // reset values before the ground check
            _grounded = false;
            _groundNormal = Vector3.up;
            
            // if we're grounded, collect info about the ground normal with a downward capsule cast representing our character capsule
            if (!Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(_characterController.height),
                _characterController.radius, Vector3.down, out var hit, chosenGroundCheckDistance, groundLayers,
                QueryTriggerInteraction.Ignore))
            {
	            return;
            }
            // storing the upward direction for the surface found
            _groundNormal = hit.normal;

            // Only consider this a valid ground hit if the ground normal goes in the same direction as the character up
            // and if the slope angle is lower than the character controller's limit
            if (!(Vector3.Dot(hit.normal, transform.up) > 0f) || !IsNormalUnderSlopeLimit(_groundNormal))
            {
	            return;
            }
            
            _grounded = true;
            if (hit.distance >_characterController.skinWidth)
            {
	            _characterController.Move(Vector3.down * hit.distance);
            }
        }
		
        private bool IsNormalUnderSlopeLimit(Vector3 normal)
        {
	        return Vector3.Angle(transform.up, normal) <= _characterController.slopeLimit;
        }
        
        private Vector3 GetCapsuleBottomHemisphere()
        {
	        return transform.position + transform.up * _characterController.radius;
        }

        // Gets the center point of the top hemisphere of the character controller capsule    
        private Vector3 GetCapsuleTopHemisphere(float atHeight)
        {
	        return transform.position + transform.up * (atHeight - _characterController.radius);
        }
    }
}