using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SecondOrderDynamic : MonoBehaviour {
    public Transform Target;

    // Speed at which the system respond to changes to the input & frequency the system will vibrate at
    [SerializeField, Range(0, 10), Tooltip("Frequency (in Hz)")] private float _f = 1;

    // How the system come settle to the target
    [SerializeField, Range(0, 2), Tooltip("Damping Coefficient")] private float _zeta = 0.5f;

    // If negative: anticipate the motion, if == 0: takes time, if > 0: reacts immediately if r > 1 overshoot the motion
    [SerializeField, Range(-5, 5), Tooltip("Initial Respond")] private float _r = 2;

    private Vector3 _position, _velocity, _previousTargetPosition;
    private Rigidbody2D _rigidbody;
    private float _k1, _k2, _k3;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        (_k1, _k2, _k3) = ComputeSecondOrderConstants(_f, _zeta, _r);
        _position = transform.position;
    }

    private void FixedUpdate() {
        _previousTargetPosition = Target.position;

        (Vector3 y, Vector3 dy) = SeconOrderDynamic(_k1, _k2, _k3, Time.fixedDeltaTime, Target.position, null, _position, _velocity, _previousTargetPosition);

        _position = y;
        _velocity = dy;

        _rigidbody.MovePosition(_position);
        _rigidbody.MoveRotation(Target.rotation);
    }

    // Computing Second Order Dynamic coefficient from frequency, damping and reaction
    public static (float k1, float k2, float k3) ComputeSecondOrderConstants(float f, float z, float r) {
        float k1 = z / (Mathf.PI * f);
        float k2 = 1 / ((2 * Mathf.PI * f) * (2 * Mathf.PI * f));
        float k3 = r * z / (2 * Mathf.PI * f);

        return (k1, k2, k3);
    }

    // x is the target position
    // y is the modified X position
    // dx or dy are velocities (first position derivatives)
    // if dx is unknown, it is estimated using mean velocity computation
    public static (Vector3 y, Vector3 dy) SeconOrderDynamic(float k1, float k2, float k3, float t, Vector3 x, Vector3? dx, Vector3 y, Vector3 dy, Vector3 previousX) {
        if (dx == null) {
            dx = (x - previousX) / t;
        }

        Vector3 yRes = y + t * dy;
        Vector3 dyRes = dy + t * (x + k3 * (Vector3)dx - y - k1 * dy) / k2;

        return (yRes, dyRes);
    }
}
