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
        } else if (_timeElapsed < 1.01f) {
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
}
