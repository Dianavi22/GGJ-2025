using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Player {
    public class ShootBehaviour : MonoBehaviour {
        [Header("Shoot config")]
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private Transform _shootPosition;
        [SerializeField] private float _cooldown;

        [Header("Shoot UI setting")]
        [SerializeField] private Slider _slider;
        [SerializeField] private float _sliderFillDuration;
        [SerializeField] private Vector2 _perfectShotRange;

        [Header("Keys setting")]
        [SerializeField] private List<KeyCode> _shootKeys;
        [SerializeField] private bool _longpress = true;

        private Coroutine _shootCoroutine;
        private float _cooldownElapsedTime, _chargingElapsedTime;
        private bool _isInCooldown = false;

        private Action _shootCallback;

        private void Start() {
            _slider.value = 0;

            _shootCallback = _longpress ? PressShot : ClickShoot;
        }

        private void Update() {
            if (_isInCooldown) {
                _cooldownElapsedTime += Time.deltaTime;
                _isInCooldown = _cooldownElapsedTime < _cooldown;
            }

            if (_isInCooldown) {
                return;
            }

            _shootCallback?.Invoke();
        }

        private void ClickShoot() {
            if (_shootKeys.Any(Input.GetKeyDown) || Input.GetMouseButtonDown(0)) {
                if (_shootCoroutine != null) {
                    Shoot(IsPerfectRange());
                } else {
                    _shootCoroutine = StartCoroutine(FillGradientCoroutine());
                }
            }
        }

        private void PressShot() {
            if (_shootKeys.Any(Input.GetKey) || Input.GetMouseButton(0)) {
                _chargingElapsedTime += Time.deltaTime;
                float k = _chargingElapsedTime / _sliderFillDuration;
                _slider.value = Mathf.Lerp(0, 1, k);
            } else if(0 < _slider.value) {
                Shoot(IsPerfectRange());
            }
        }

        private bool IsPerfectRange() {
            return _perfectShotRange.x <= _slider.value && _slider.value <= _perfectShotRange.y;
        }

        private IEnumerator FillGradientCoroutine() {
            _chargingElapsedTime = 0;

            while (_chargingElapsedTime < _sliderFillDuration) {
                float k = _chargingElapsedTime / _sliderFillDuration;
                _slider.value = Mathf.Lerp(0, 1, k);

                yield return null;

                _chargingElapsedTime += Time.deltaTime;
            }

            Shoot(false);
        }

        private void Shoot(bool isTimed) {
            // Firing
            PlayerProjectile projectile = Instantiate(_projectilePrefab, _shootPosition.position, Quaternion.identity).GetComponent<PlayerProjectile>();
            projectile.Direction = transform.right;

            // Resetting values
            if (_shootCoroutine != null) {
                StopCoroutine(_shootCoroutine);
            }
            
            _shootCoroutine = null;
            _slider.value = 0;
            _cooldownElapsedTime = 0;
            _isInCooldown = true;
            _chargingElapsedTime = 0;
        }
    }
}