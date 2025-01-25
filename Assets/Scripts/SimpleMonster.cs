using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMonster : MonoBehaviour
{
    [SerializeField]
    protected float _speed;
    private float _timeElapsed = 0;

    protected Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    protected void DoBasicMove() {

        if(_rigidbody == null) {
            _rigidbody = GetComponent<Rigidbody>();
        }

        _timeElapsed += Time.deltaTime;

        if (_timeElapsed < 1f) {
            float t = _timeElapsed / 1;
            // Move
            float smoothSpeed = (1 / (t + 0.1f) * _speed);
            _rigidbody.MovePosition(_rigidbody.position + smoothSpeed * Time.fixedDeltaTime * transform.forward);
        } else if (_timeElapsed < 2f) {
            float reduced_speed = (_speed / 10);
            _rigidbody.MovePosition(_rigidbody.position + reduced_speed * Time.fixedDeltaTime * transform.forward);
        } else {
            // Reset timer to loop
            _timeElapsed = 0;
        }
    }

    public void TakeDamage() {
        // TODO
        Debug.Log("It hits");
    }

    /// <summary>
    /// This method returns the angle (related to the X axis) between two points.
    /// <example>For example:
    /// <code>
    /// Vector2 origin = new(0, 0);
    /// Vector2 target = new(0.5, 0.5);
    /// float angle = CustomMaths.GetXAngle(origin, target);
    /// </code>
    /// results in <c>angle</c> to be ~= 0.785398 radian (= 45°)
    /// </example>
    /// </summary>
    /// <param name="origin">The origin point</param>
    /// <param name="target">The targeted point</param>
    /// <returns>The angle between <paramref name="target"/> and <paramref name="origin"/> (related to the X angle) in radian</returns>
    public static float GetXAngle(Vector2 origin, Vector2 target) {
        return Mathf.Atan2(target.y - origin.y, target.x - origin.x);
    }
}
