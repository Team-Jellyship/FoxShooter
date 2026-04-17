using UnityEngine;
/// Simple recoil: Adds immediate pitch/yaw kick, then springs back to zero.
/// Apply to camera pivot (local rotation)
/// </summary>

public class ViewRecoil : MonoBehaviour
{
    [SerializeField] Transform pivotPoint;
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

    private Vector2 recoilOffset;      // x = yaw, y = pitch
    private Vector2 recoilVelocity;    // Spring Velocity

    private void LateUpdate()
    {
        //Critically damped-ish spring back to zero
        // v += (-k * x - c * v) * dt; x += v * dt
        float dt = Time.deltaTime;

        Vector2 accel = (-returnStrength * recoilOffset) - (damping * recoilVelocity);
        recoilVelocity += accel * dt;
        recoilOffset += recoilVelocity * dt;

        //recoilOffset.y = Mathf.Clamp(recoilOffset.y, -maxPitch, maxPitch);
        
        //Apply local rotation offset (pitch up is negative X rotation in unity)
        pivotPoint.localRotation = Quaternion.Euler(-recoilOffset.y, recoilOffset.x, 0f);
    }

    /// <summary>Call on each shot.</summary>
    public void AddRecoil(float recoilMultiplier = 1f)
    {
        float yaw = Random.Range(-yawKick, yawKick) * yawRandomness;

        recoilOffset.y += pitchKick * recoilMultiplier;
        recoilOffset.x += yaw * recoilMultiplier;
        
        //recoilOffset.y = Mathf.Clamp(recoilOffset.y, -maxPitch, maxPitch);
    }

    /// <summary>Optional: clear recoil immediately.</summary>
    public void ResetRecoil()
    {
        recoilOffset = Vector2.zero;
        recoilVelocity = Vector2.zero;
        pivotPoint.localRotation = Quaternion.identity;
    }
}
