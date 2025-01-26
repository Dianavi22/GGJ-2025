using System.Collections;
using Assets.Scripts;
using Player;
using UnityEngine;

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
        private bool _growing = false;
        public bool isGameOver = false;
        private bool _isShrinked = false;
        private void Awake() {
            UpdateSize(initialSize - transform.localScale.x);
        }

        private GameManager _gameManager = GameManager.Instance;
        private void Start() {
            _gameManager.isPlaying = true;

        }

        private void Update() {

            if (_growing) {
                return;
            }

      
            if (transform.localScale.x > 13 && _gameManager.isPlaying && !_tuto.isInTuto) {
                _gameManager.Win();
            }

            if (transform.localScale.x < 0.4f ) {
                _isShrinked = true;
            }

            if (_gameManager.isPlaying && !_tuto.isInTuto) {
                Shrink(_shrinkPerSecond * Time.deltaTime);
            }

          
        }

        public void AnimationDeath() {
            _gameManager.isPlaying = false;
            _sc.ShakyCameCustom(0.2f, 0.5f);
            ScaleTo(this.transform, new Vector3(this.transform.localScale.x + 4, this.transform.localScale.y + 4, this.transform.localScale.z + 4), 0.3f);
            Invoke("PlayerFall", 1f);
            isGameOver = false;
            Invoke("CallGOCanvas", 0.5f);
        }

        private void PlayerFall() {
            _playerRb.useGravity = true;
            _sc.ShakyCameCustom(0.2f, 0.5f);

        }

        private void CallGOCanvas() {
            GameManager.Instance.GameOverCanvasFunc();
        }

        public void ScaleTo(Transform target, Vector3 targetScale, float duration) {
            StartCoroutine(ScaleLerpCoroutine(target, targetScale, duration));
        }

        private IEnumerator ScaleLerpCoroutine(Transform target, Vector3 targetScale, float duration) {
            Vector3 initialScale = target.localScale; 
            float elapsedTime = 0f;

            while (elapsedTime < duration) {

                target.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                
                yield return null;
            }
            target.localScale = targetScale;
            _goPart.Play();
            this.GetComponentInChildren<Renderer>().enabled = false;
        }

        public void Grow(float offset) {
            UpdateSize(offset);
        }

        public void Shrink(float offset) {
            UpdateSize(-offset);
        }

        public bool getIsShrinked() {
            return true;
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
        }

        private Vector3 GetLocalScaleWithOffset(float offset) {
            return new(transform.localScale.x + offset, transform.localScale.y + offset, 1);
        }

        private IEnumerator GrowTo(float offset, float growthDuration) {
            _growing = true;
            float elapsedTime = 0;
            Vector3 origin = transform.localScale;
            Vector3 target = GetLocalScaleWithOffset(offset);

            while (elapsedTime < growthDuration) {
                float k = elapsedTime / growthDuration;
                transform.localScale = Vector3.Lerp(origin, target, k);

                yield return null;

                elapsedTime += Time.deltaTime;
            }

            transform.localScale = target;
            _growing = false;
        }
    }
}
