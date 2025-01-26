using System.Collections.Generic;
using UnityEngine;

public class SimpleMonster : MonoBehaviour {
    [SerializeField] protected float _speed;

    [Header("Audio Config")]
    [SerializeField] protected AudioSource _mainSource;
    [SerializeField] protected AudioClip _hitClip;
    [SerializeField] protected List<AudioClip> _spawnClips;

    protected AudioClip _inBubbleClip;

    private float _timeElapsed = 0;

    protected Rigidbody _rigidbody;

    protected virtual void Awake() {
        _inBubbleClip = _mainSource.clip;

        _mainSource.clip = _spawnClips[Random.Range(0, _spawnClips.Count)];
        _mainSource.Play();
    }

    // Start is called before the first frame update
    protected virtual void Start() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    protected void DoBasicMove() {
        if (_rigidbody == null) {
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
        _mainSource.clip = _hitClip;
        _mainSource.Play();

        Destroy(gameObject);
    }
}
