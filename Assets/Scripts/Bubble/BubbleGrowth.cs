using System.Collections;
using Player;
using UnityEngine;
using System.Collections.Generic;
using Extensions;
using Manager;

namespace Bubble {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    public class BubbleGrowth : MonoBehaviour {
        [SerializeField] public float initialSize;
        [SerializeField] private float _shrinkPerSecond;
        [SerializeField] private float _playerHitGrowthDuration;
        [SerializeField] private ShakyCame _sc;
        [SerializeField] private ParticleSystem _bubblesPart;
        [SerializeField] private ParticleSystem _destroyProj;
        [SerializeField] private ParticleSystem _destroySprProj;
        [SerializeField] private ParticleSystem _goPart;
        [SerializeField] private Rigidbody _playerRb;
        [SerializeField] private Tuto _tuto;
        public bool isGameOver = false;
        private bool _isGrowing = false;
        private bool _isShrinked = false;
        private bool _isFree = false;
        [SerializeField] private bool _isTest = false;

        [SerializeField] private List<Pylon> pylons = new();

        [SerializeField] private AudioSource _growthSource;
        [SerializeField] private AudioSource _warningSource;


        private float failSafeSeconds = 2;

        private void Awake() {
            UpdateSize(initialSize - transform.localScale.x);
        }

        private void Start() {
            AudioManager.Instance.OnSfxVolumeChanged.AddListener(_growthSource.UpdateVolume);
            AudioManager.Instance.OnSfxVolumeChanged.AddListener(_warningSource.UpdateVolume);
        }

        private void FixedUpdate() {

            if (failSafeSeconds > 0) {
                failSafeSeconds -= Time.deltaTime;
            }

            if (_isGrowing) {
                return;
            }

            if (transform.localScale.x < 0.4 && failSafeSeconds <= 0) {
                _isShrinked = true;
            }

            if (transform.localScale.x > 10) {
                _isFree = true;
            }


            if (!_isShrinked && !_tuto.isInTuto) {
                Shrink(_shrinkPerSecond * Time.deltaTime);
            }
        }

        public void AnimationDeath() {
            _sc.ShakyCameCustom(0.2f, 0.5f);
            _isShrinked = false;
            ScaleTo(new Vector3(this.transform.localScale.x + 4, this.transform.localScale.y + 4, this.transform.localScale.z + 4), 0.3f);
            Invoke("PlayerFall", 1f);
        }

        private void PlayerFall() {
            _playerRb.isKinematic = false;
            _playerRb.useGravity = true;
            _sc.ShakyCameCustom(0.2f, 0.5f);

        }

        public void ScaleTo(Vector3 targetScale, float duration) {
            StartCoroutine(ScaleLerpCoroutine(targetScale, duration));
        }

        private IEnumerator ScaleLerpCoroutine(Vector3 targetScale, float duration) {
            Vector3 initialScale = transform.localScale;
            float elapsedTime = 0f;

            while (elapsedTime < duration) {

                transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
                elapsedTime += Time.deltaTime;

                yield return null;
            }
            transform.localScale = targetScale;
            _goPart.Play();
            this.GetComponentInChildren<Renderer>().enabled = false;
        }

        public void Grow(float offset) {
            UpdateSize(offset);
        }

        public void Shrink(float offset) {
            UpdateSize(-offset);
        }

        public bool GetIsShrinked() {
            return _isShrinked;
        }
        public bool GetIsFree() {
            return _isFree;
        }



        public void ResetValue() {
            _isShrinked = false;
            _isFree = false;
            UpdateSize(initialSize - transform.localScale.x);
            //transform.localScale = new Vector3(initialSize, initialSize, 1);
            failSafeSeconds = 2;

            //playerRb
            _playerRb.gameObject.SetActive(false);
            _playerRb.useGravity = false;
            _playerRb.isKinematic = true;

            _playerRb.gameObject.transform.position = new Vector3(0, 0, 0);
            //Reset Active player
            _playerRb.gameObject.SetActive(true);

            this.GetComponentInChildren<Renderer>().enabled = true;
        }

       

       

        private void OnTriggerExit(Collider other) {
            if (other.TryGetComponent(out PlayerProjectile projectile)) {
                StartCoroutine(GrowTo(projectile.GrowthValue, _playerHitGrowthDuration));
                _sc.ShakyCameCustom(0.07f, 0.2f);
                _bubblesPart.transform.position = other.transform.position;
                Vector3 direction = projectile.Direction.normalized;
                _bubblesPart.transform.rotation = Quaternion.Euler(direction.y * -90, direction.x * 90, 0);
                _bubblesPart.Play();
                if (projectile.isTimed) {
                    _destroySprProj.transform.position = projectile.transform.position;
                    _destroySprProj.Play();
                } else {
                    _destroyProj.transform.position = projectile.transform.position;
                    _destroyProj.Play();
                }
                Destroy(other.gameObject);
            }
        }
        private void UpdateSize(float offset) {
            transform.localScale = GetLocalScaleWithOffset(offset);

            bool isSmallEnough = transform.localScale.x < 2;

            if (isSmallEnough && !_warningSource.isPlaying) {
                _warningSource.Play();
            } else if (!isSmallEnough && _warningSource.isPlaying) {
                _warningSource.Stop();
            }

        }

        private Vector3 GetLocalScaleWithOffset(float offset) {
            return new(transform.localScale.x + offset, transform.localScale.y + offset, 1);
        }

        private IEnumerator GrowTo(float offset, float growthDuration) {
            _isGrowing = true;
            float elapsedTime = 0;
            Vector3 origin = transform.localScale;
            Vector3 target = GetLocalScaleWithOffset(offset);

            _growthSource.Play();

            while (elapsedTime < growthDuration) {
                float k = elapsedTime / growthDuration;
                transform.localScale = Vector3.Lerp(origin, target, k);

                yield return null;

                elapsedTime += Time.deltaTime;
            }

            transform.localScale = target;
            _isGrowing = false;

        }
    }
}
