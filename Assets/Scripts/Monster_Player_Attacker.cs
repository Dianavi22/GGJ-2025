using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterType0_Attacker : MonoBehaviour
{
    public GameObject player;

    [SerializeField]
    private float _speed;

    private Rigidbody2D _rigidbody;

    private float _timeElapsed = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        _rigidbody = GetComponent<Rigidbody2D>();

        _rigidbody.SetRotation(Mathf.Rad2Deg * GetXAngle(transform.position, player.transform.position));
    }

    void FixedUpdate()
    {
        _timeElapsed += Time.deltaTime;

        if (_timeElapsed < 1f)
        {
            float t = _timeElapsed / 1;
            // Move
            float smoothSpeed = (1 / (t + 0.1f) * _speed);
            _rigidbody.MovePosition(_rigidbody.position + smoothSpeed * Time.fixedDeltaTime * (Vector2)transform.right);
        }
        else if (_timeElapsed < 2f)
        {
            // Pause
        }
        else
        {
            // Reset timer to loop
            _timeElapsed = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO do damage.
        Destroy(gameObject);
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
    public static float GetXAngle(Vector2 origin, Vector2 target)
    {
        return Mathf.Atan2(target.y - origin.y, target.x - origin.x);
    }
}
