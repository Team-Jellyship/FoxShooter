using UnityEngine;
/// Simple recoil: Adds immediate pitch/yaw kick, then springs back to zero.
/// Apply to camera pivot (local rotation)
/// </summary>

public class ViewRecoil : MonoBehaviour
{
    [SerializeField] private Transform pivotPoint;
    
    [Header("Kick (Degrees per Shot)")]
    public float pitchKick = 1.2f;      // Up
    public float yawKick = 0.4f;        // Side-to-side
    public float yawRandomness = 1.0f;  // 1 = full random range

    [Header("Spring Settings")]
    [Tooltip("How fast recoil returns to center (higher = snappier).")]
    public float returnStrength = 20f;

    [Tooltip("How much damping is applied (higher = less oscillation).")]
    public float damping = 18f;

    [Header("Limits")]
    public float maxPitch = 20f;

    private Vector2 _recoilOffset;      // x = yaw, y = pitch
    private Vector2 _recoilVelocity;    // Spring Velocity

    private void LateUpdate()
    {
        //Critically damped-ish spring back to zero
        // v += (-k * x - c * v) * dt; x += v * dt

        var acceleration = -returnStrength * _recoilOffset - damping * _recoilVelocity;
        _recoilVelocity += acceleration * Time.deltaTime;
        _recoilOffset += _recoilVelocity * Time.deltaTime;

        //recoilOffset.y = Mathf.Clamp(recoilOffset.y, -maxPitch, maxPitch);
        
        //Apply local rotation offset (pitch up is negative X rotation in unity)
        pivotPoint.localRotation = Quaternion.Euler(-_recoilOffset.y, _recoilOffset.x, 0.0f);
    }

    /// <summary>Call on each shot.</summary>
    public void AddRecoil(float recoilMultiplier = 1.0f)
    {
        var yaw = Random.Range(-yawKick, yawKick) * yawRandomness;

        _recoilOffset.y += pitchKick * recoilMultiplier;
        _recoilOffset.x += yaw * recoilMultiplier;
    }

    /// <summary>Optional: clear recoil immediately.</summary>
    public void ResetRecoil()
    {
        _recoilOffset = Vector2.zero;
        _recoilVelocity = Vector2.zero;
        pivotPoint.localRotation = Quaternion.identity;
    }
}
