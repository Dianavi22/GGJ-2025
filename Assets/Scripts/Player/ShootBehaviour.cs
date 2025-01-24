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

        private Coroutine _shootCoroutine;
        private float _elapsedTime;
        private bool _isInCooldown = false;

        private void Start() {
            _slider.value = 0;
        }

        private void Update() {
            if (_isInCooldown) {
                _elapsedTime += Time.deltaTime;
                _isInCooldown = _elapsedTime < _cooldown;
            }

            if (_isInCooldown) {
                return;
            }

            if (_shootKeys.Any(Input.GetKeyDown) || Input.GetMouseButtonDown(0)) {
                if (_shootCoroutine != null) {
                    Shoot(_perfectShotRange.x <= _slider.value && _slider.value <= _perfectShotRange.y);
                } else {
                    _shootCoroutine = StartCoroutine(FillGradientCoroutine());
                }
            }
        }

        private IEnumerator FillGradientCoroutine() {
            float elapsedTime = 0;

            while (elapsedTime < _sliderFillDuration) {
                float k = elapsedTime / _sliderFillDuration;
                _slider.value = Mathf.Lerp(0, 1, k);

                yield return null;

                elapsedTime += Time.deltaTime;
            }

            Shoot(false);
        }

        private void Shoot(bool isTimed) {
            // Firing
            PlayerProjectile projectile = Instantiate(_projectilePrefab, _shootPosition.position, Quaternion.identity).GetComponent<PlayerProjectile>();
            projectile.Direction = transform.right;

            // Resetting values
            StopCoroutine(_shootCoroutine);
            _shootCoroutine = null;
            _slider.value = 0;
            _elapsedTime = 0;
            _isInCooldown = true;
        }
    }
}