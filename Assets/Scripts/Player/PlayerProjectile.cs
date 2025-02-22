using System;
using Math;
using UnityEngine;

namespace Player {
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerProjectile : MonoBehaviour {
        [SerializeField, Tooltip("Speed of the projectile (in m/s)")] private float _speed;
        [SerializeField, Tooltip("Growth value when hitting the bubble (in m)")] private float _growthValue;
        [SerializeField] private bool _isPiercing;
        [SerializeField] ParticleSystem _trailPart;
        public bool isTimed;
        // Direction of this projectile. Need to be set when fired.
        public Vector2 Direction;

        private Rigidbody _rigidbody;
        private Plane[] _cameraFrustumPlanes;
        private Renderer _renderer;
        private Action _onDistroy;

        public float GrowthValue => _growthValue;
        public Action OnDestroy { set { _onDistroy = value; } }

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
            _renderer = GetComponentInChildren<Renderer>();
        }

        private void Start() {
            _cameraFrustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            _trailPart.transform.rotation = Quaternion.Euler(this.transform.localRotation.y * 90, this.transform.localRotation.x * -90, 0);
        }

        private void Update() {
            // Check if the renderer bounds isn't in any of the camera frustum's planes i.e.: if the object isn't visible to the camera
            // Technically useless, should be possible to be removed.
            if (!GeometryUtility.TestPlanesAABB(_cameraFrustumPlanes, _renderer.bounds)) {
                DestroyProjectile();
            }
        }

        private void FixedUpdate() {
            _rigidbody.MovePosition(_rigidbody.position + _speed * Time.fixedDeltaTime * (Vector3)Direction);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent(out SimpleMonster monster)) {
                monster.TakeDamage();

                if (!_isPiercing) {
                    DestroyProjectile();
                }
            }
        }

        private void DestroyProjectile() {
            _onDistroy?.Invoke();
            Destroy(gameObject);
        }

        #region Special Shot Callbacks
        public void SplitOnDestroy() {
            for (int i = 0; i < 2; i++) {
                PlayerProjectile projectile = Instantiate(gameObject).GetComponent<PlayerProjectile>();
                projectile.OnDestroy = null;
                projectile.Direction = CustomMath.RotateVectorAroundOrigin(Direction.normalized, 30 * (((i & 1) == 0) ? 1 : -1));
            }
        }
        #endregion
    }
}