using Assets.Scripts;
using Extensions;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player {
    public class ShootBehaviour : MonoBehaviour {
        [Header("Shoot config")]
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private GameObject _projectileSpecialPrefab;
        [SerializeField] private Transform _shootPosition;
        [SerializeField] private float _cooldown;

        [Header("Shoot UI setting")]
        [SerializeField] private Slider _slider;
        [SerializeField] private float _sliderFillDuration;
        [SerializeField] private Vector2 _perfectShotRange;

        [Header("Sound Setting")]
        [SerializeField] private AudioSource _shootSource;
        [SerializeField] private AudioSource _chargeSource;
        [SerializeField] private AudioClip _perfectShotClip;
        private AudioClip _regularShotClip;

        [Header("Keys setting")]
        [SerializeField] private List<KeyCode> _shootKeys;
        [SerializeField] private bool _longpress = true;

        [SerializeField] private ParticleSystem _shootPart;
        [SerializeField] private ParticleSystem _shootSpecialPart;
        [SerializeField] Animator _shootAnim;
        private Coroutine _shootCoroutine;
        private float _cooldownElapsedTime, _chargingElapsedTime;
        private bool _isInCooldown = false;
        private int _combo = 0;
        
        [SerializeField] GameObject comboGuiTextGameobject;

        //Used to toggle to "isPlayingAnimation" when the combo is 3.
        private WobblyText comboTextAnimation;

        private Action _shootCallback;

        private void Awake() {
            _regularShotClip = _shootSource.clip;
        }

        private void Start() {
            AudioManager.Instance.OnSfxVolumeChanged.AddListener(_shootSource.UpdateVolume);
            AudioManager.Instance.OnSfxVolumeChanged.AddListener(_chargeSource.UpdateVolume);

            _slider.value = 0;
            comboTextAnimation = comboGuiTextGameobject.GetComponent<WobblyText>();
            _shootCallback = _longpress ? PressShot : ClickShoot;
        }

        private void Update() {
            if (_isInCooldown) {
                _cooldownElapsedTime += Time.deltaTime;
                _isInCooldown = _cooldownElapsedTime < _cooldown;
                if (!_isInCooldown) {
                    _shootAnim.SetBool("isShooting", false);

                }
            }

            if (_isInCooldown) {
                return;
            }

            _shootCallback?.Invoke();
        }

        private void ClickShoot() {
            if (_shootKeys.Any(Input.GetKeyDown) || Input.GetMouseButtonDown(0) && GameManager.Instance.isPlaying) {
                if (_shootCoroutine != null) {
                    Shoot(IsPerfectRange());
                } else {
                    _chargeSource.Play();
                    _shootCoroutine = StartCoroutine(FillGradientCoroutine());
                }
            }
        }

        private void PressShot() {
            if (_shootKeys.Any(Input.GetKey) || Input.GetMouseButton(0)) {
                if (_chargingElapsedTime == 0) {
                    _chargeSource.Play();
                }

                _chargingElapsedTime += Time.deltaTime;
                float k = _chargingElapsedTime / _sliderFillDuration;
                _slider.value = Mathf.Lerp(0, 1, k);
            } else if (0 < _slider.value) {
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
            GameObject projectileToSpawn;
            AudioClip audioClipToPlay;

            if (isTimed) {
                projectileToSpawn = _projectileSpecialPrefab;
                audioClipToPlay = _perfectShotClip;
            } else {
                projectileToSpawn = _projectilePrefab;
                audioClipToPlay = _regularShotClip;
            }

            if (_shootSource.clip != audioClipToPlay) {
                _shootSource.clip = audioClipToPlay;
            }

            _shootSource.Play();
            _shootAnim.SetBool("isShooting", true);

            // Firing
            PlayerProjectile projectile = Instantiate(projectileToSpawn, _shootPosition.position, Quaternion.identity).GetComponent<PlayerProjectile>();
            projectile.Direction = transform.right;

            // Example of how to use OnDestroy callback
            if (3 < _combo) {
                projectile.OnDestroy = projectile.SplitOnDestroy;
            }

            // Update the text
            comboGuiTextGameobject.GetComponent<TextMeshProUGUI>().text = string.Concat("Combo ", _combo);
            if (comboTextAnimation != null) {
                comboTextAnimation.isAnimationPlaying = (_combo >= 3);
            }


            if (isTimed) {
                projectile.isTimed = true;
                _shootSpecialPart.Play();
                _combo++;
            } else {
                projectile.isTimed = false;
                _shootPart.Play();
                _combo = 0;
            }

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