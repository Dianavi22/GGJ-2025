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
        [SerializeField] SpawnVFX _spawnVFX;
        private bool _growing = false;

        private void Awake() {
            UpdateSize(initialSize - transform.localScale.x);
        }

        private GameManager _gameManager = GameManager.Instance;

        private void Update() {

            if (_growing) {
                return;
            }

            if (transform.localScale.x < 0.2 && _gameManager.isPlaying) {
                _gameManager.GameOver();
            }

            if (transform.localScale.x > 13 && _gameManager.isPlaying) {
                _gameManager.Win();
            }


            if (_gameManager.isPlaying) {
                Shrink(_shrinkPerSecond * Time.deltaTime);
            }
        }

        public void Grow(float offset) {
            UpdateSize(offset);
        }

        public void Shrink(float offset) {
            UpdateSize(-offset);
        }

        private void OnTriggerExit(Collider other) {
            if (other.TryGetComponent(out PlayerProjectile projectile)) {
                StartCoroutine(GrowTo(projectile.GrowthValue, _playerHitGrowthDuration));
                _sc.ShakyCameCustom(0.07f, 0.2f);
                _bubblesPart.transform.position = other.transform.position;
                Vector3 direction = projectile.Direction.normalized;
                _bubblesPart.transform.rotation = Quaternion.Euler(direction.y * -90, direction.x * 90, 0);
                _bubblesPart.Play();
                Transform transform = projectile.transform;
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
