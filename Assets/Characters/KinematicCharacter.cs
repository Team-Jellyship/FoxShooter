using System;
using FoxShooter.Game;
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
		private const float NegativeKillY = -200.0f;
		private Animator _animator; // For Animation Triggers

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
		private float groundCheckDistance = 0.05f;

		[SerializeField]
		private float groundCheckDistanceAerial = 0.075f;

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
		[SerializeField] private float lookSensitivityHorizontal = 1.0f;
		[SerializeField] private float lookSensitivityVertical = 1.0f;
		[SerializeField] private Camera playerCamera;
		[SerializeField] private float cameraPitch;
		[SerializeField] private bool lookAt;
		[SerializeField][Min(0.0f)] private float lookAtSpeed = 5.0f;
		[SerializeField] [Range(-180.0f, 180.0f)] private float lookAtYaw;


		[SerializeField] private Vector3 additionalLocalSpaceVelocity;
		[SerializeField] private Vector3 currentVelocity;
		
		private bool _immobilized;
		private bool _grounded;
		private bool _isWalking; // Used for head bob animation
		private bool _isJumping;
		private bool _pendingJumpImpulse; // Jump impulses are a little special
		private Vector2 _moveInput;
		private Vector3 _groundNormal;
		private Vector3 _impulses;
		private TimerHandle _coyoteTimer;
		private TimerHandle _jumpTimer;
		
		// Component cached references
		private CharacterController _characterController;
		private CharacterStats _stats;
		
		private void Awake()
		{
			_characterController = GetComponent<CharacterController>();
			_stats = GetComponent<CharacterStats>();
			_animator = GetComponent<Animator>();
		}

		private void Start()
		{
			_coyoteTimer = TimerManager.instance.CreateTimer(this, StopJumping);
			_jumpTimer = TimerManager.instance.CreateTimer(this, StopJumping);

			if (_stats == null)
			{
				return;
			}
			
			_stats.RegisterEffectAppliedCallback(Game.Game.instance.statusEffects.stunned, () => _immobilized = true, this);
			_stats.RegisterEffectRemovedCallback(Game.Game.instance.statusEffects.stunned, () =>
			{
				_immobilized = false;
				currentVelocity = Vector3.zero;
			}, this);
		}

		private void FixedUpdate()
		{
			if (_immobilized)
			{
				_characterController.Move(GetVectorInLocalSpace(additionalLocalSpaceVelocity) * Time.fixedDeltaTime);
				return;
			}
			
			
			var onFloor = _grounded;
			CheckGround();
			CheckForHeadbob();
			switch (_grounded)
			{
				case true when !onFloor:
					Land();
					break;

				case false when onFloor:
					LeavePlatform();
					break;
			}
			
			var currentVelocity2D = new Vector2(currentVelocity.x, currentVelocity.z);
			var currentMaxSpeed = GetMaxHorizontalSpeed();
			var rotatedInput = playerCamera ? StarMath.RotateVector(_moveInput, -transform.rotation.eulerAngles.y * Mathf.Deg2Rad) : _moveInput;

			var extraFrictionFactor = Vector2.Dot(rotatedInput, currentVelocity2D.normalized) * -0.5f + 0.5f;
			currentVelocity2D = _moveInput.Equals(Vector2.zero) ?
				StarMath.MoveTo(currentVelocity2D, Vector2.zero, GetFriction() * Time.fixedDeltaTime) :
				StarMath.MoveTo(currentVelocity2D, rotatedInput * currentMaxSpeed, (GetAcceleration() + extraFrictionFactor * GetFriction()) * Time.fixedDeltaTime);
			
			if (!_grounded)
			{
				var jumpFactor = _isJumping ? jumpHeldGravityFactor : 1.0f;
				currentVelocity.y += GravityConstant * gravity  * Time.fixedDeltaTime * jumpFactor;
			}

			currentVelocity.x = currentVelocity2D.x;
			currentVelocity.z = currentVelocity2D.y;
			
			currentVelocity += ConsumeImpulses();
			if (_pendingJumpImpulse)
			{
				currentVelocity.y = MathF.Max(jumpStrength, currentVelocity.y + jumpStrength);
				_pendingJumpImpulse = false;
			}

			currentVelocity.y = StarMath.ClampTowards(currentVelocity.y, -maxAirSpeedVertical, maxAirSpeedVertical, friction);
			_characterController.Move(currentVelocity * Time.fixedDeltaTime);
			currentVelocity = _characterController.velocity;

			if (lookAt)
			{
				var characterRotation = _characterController.transform.eulerAngles;
				var currentYaw = characterRotation.y;
				var desiredYaw = lookAtYaw;
				characterRotation.y = Mathf.MoveTowardsAngle(currentYaw, desiredYaw, lookAtSpeed);
				_characterController.transform.eulerAngles = characterRotation;
			}

			if (!_isJumping)
			{
				_animator.SetBool("isWalking", _isWalking);
			}
			
			_animator.SetBool("isJumping", _isJumping);

			if (_characterController.transform.position.y < NegativeKillY)
			{
				// This should only happen once
				
				// ReSharper disable once Unity.PerformanceCriticalCodeInvocation
				_stats?.Kill(null);
			}
			
		}

		public void MoveInput(InputAction.CallbackContext context)
		{
			_moveInput = context.ReadValue<Vector2>();
		}

		public void MoveInput(Vector2 velocity)
		{
			var currentAcceleration = GetAcceleration();
			var velocityDelta = velocity - currentVelocity.To2D();
			var inputFactor = MathF.Min(1.0f, velocityDelta.magnitude / (currentAcceleration * Time.fixedDeltaTime));
			_moveInput = inputFactor * velocity.normalized;
		}

		public void Look(InputAction.CallbackContext context)
		{
			var look = context.ReadValue<Vector2>();
			transform.Rotate(transform.up, look.x * lookSensitivityHorizontal);

			if (look.y == 0.0f) { return; }
			cameraPitch = Mathf.Clamp(cameraPitch - look.y * lookSensitivityVertical, -85.0f, 85.0f);
			playerCamera.transform.localEulerAngles = new Vector3(cameraPitch, 0.0f, 0.0f);
		}

		public void AddImpulse(Vector3 impulse)
		{
			_impulses += impulse;
		}

		public void Jump(InputAction.CallbackContext context)
		{
			if (context.phase != InputActionPhase.Performed)
			{
				return;
			}
			
			if (context.action.IsPressed())
			{
				StartJumping();
			}
			else
			{
				StopJumping();
			}
		}

		public void LookAt(Vector3 location)
		{
			lookAt = true;
			var direction = (location - _characterController.transform.position).To2D().normalized;
			lookAtYaw = -MathF.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90.0f;
		}

		private void Land()
		{
			numJumpsRemaining = numJumps;
			_coyoteTimer.Pause();
		}

		private void LeavePlatform()
		{
			if (!_isJumping)
			{
				_coyoteTimer.Start(coyoteTime);
			}
		}

		private void StartJumping()
		{
			if (CanJump())
			{
				Jump();
			}
		}

		private void StopJumping()
		{
			_isJumping = false;
		}
		
		private void Jump()
		{
			_coyoteTimer.Pause();
			_jumpTimer.Start(jumpTime);
			_isJumping = true;
			// AddImpulse(new Vector3(0.0f, jumpStrength, 0.0f));
			_pendingJumpImpulse = true;
		
			--numJumpsRemaining;
		}

		
		private bool CanJump()
		{
			return numJumpsRemaining > 0 && !_immobilized;
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

		private Vector3 ConsumeImpulses()
		{
			var impulses = _impulses;
			_impulses = Vector3.zero;
			return impulses;
		}

		private Vector3 GetVectorInLocalSpace(Vector3 vector)
		{
			return vector.z * playerCamera.transform.forward +
			       vector.y * playerCamera.transform.up +
			       vector.x * playerCamera.transform.right;
		}
		
		private void CheckGround()
        {
            // Make sure that the ground check distance while already in air is very small, to prevent suddenly snapping to ground
            var chosenGroundCheckDistance = _grounded ? _characterController.skinWidth + groundCheckDistance : groundCheckDistanceAerial;

            // reset values before the ground check
            _grounded = false;
            _groundNormal = Vector3.up;
            
            if (currentVelocity.y > 0.1f)
            {
	            return;
            }
            
            var bottom = transform.position + _characterController.center + Vector3.up * (-_characterController.height * 0.5f + _characterController.radius);
            var top = bottom + Vector3.up * (_characterController.height - _characterController.radius);
            if (!Physics.CapsuleCast(bottom, top, _characterController.radius, Vector3.down, out var hit, chosenGroundCheckDistance, groundLayers,
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
            if (hit.distance < _characterController.skinWidth)
            {
	            _characterController.Move(Vector3.down * hit.distance);
            }
        }

        void CheckForHeadbob()
        {
	        if (currentVelocity.magnitude > 0.1f)
	        {
		        _isWalking = true;
	        }
	        else
	        {
		        _isWalking = false;
	        }
        }
		
        private bool IsNormalUnderSlopeLimit(Vector3 normal)
        {
	        return Vector3.Angle(transform.up, normal) <= _characterController.slopeLimit;
        }

        private void OnDrawGizmos()
        {
	        StarDebug.DrawArrow(transform.position, transform.position + currentVelocity, Color.blueViolet);
	        StarDebug.DrawArrow(transform.position, transform.position + _moveInput.To3D(), Color.mediumSeaGreen);
        }
    }
}