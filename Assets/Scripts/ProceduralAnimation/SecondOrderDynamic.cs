using Math;
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
        (_k1, _k2, _k3) = CustomMath.ComputeSecondOrderConstants(_f, _zeta, _r);
        _position = transform.position;
    }

    private void FixedUpdate() {
        _previousTargetPosition = Target.position;

        (Vector3 y, Vector3 dy) = CustomMath.SeconOrderDynamic(_k1, _k2, _k3, Time.fixedDeltaTime, Target.position, null, _position, _velocity, _previousTargetPosition);

        _position = y;
        _velocity = dy;

        _rigidbody.MovePosition(_position);
        _rigidbody.MoveRotation(Target.rotation);
    }
}
