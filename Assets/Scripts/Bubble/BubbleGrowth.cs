using System.Collections;
using Player;
using UnityEngine;

namespace Bubble {
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class BubbleGrowth : MonoBehaviour {
        [SerializeField] private float _initialSize;
        [SerializeField] private float _shrinkPerSecond;
        [SerializeField] private float _playerHitGrowthDuration;

        private bool _growing = false;

        private void Update() {
            if (_growing) {
                return;
            }

            Shrink(_shrinkPerSecond * Time.deltaTime);
        }

        public void Grow(float offset) {
            UpdateSize(offset);
        }

        public void Shrink(float offset) {
            UpdateSize(-offset);
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.TryGetComponent(out PlayerProjectile projectile)) {
                StartCoroutine(GrowTo(projectile.GrowthValue, _playerHitGrowthDuration));
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
