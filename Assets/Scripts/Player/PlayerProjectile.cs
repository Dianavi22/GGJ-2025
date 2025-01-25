using UnityEngine;

namespace Player {
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerProjectile : MonoBehaviour {
        [SerializeField, Tooltip("Speed of the projectile (in m/s)")] private float _speed;
        [SerializeField, Tooltip("Growth value when hitting the bubble (in m)")] private float _growthValue;

        // Direction of this projectile. Need to be set when fired.
        public Vector2 Direction;

        private Rigidbody2D _rigidbody;
        private Plane[] _cameraFrustumPlanes;
        private Renderer _renderer;

        public float GrowthValue => _growthValue;

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody2D>();
            _renderer = GetComponentInChildren<Renderer>();
        }

        private void Start() {
            _cameraFrustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        }

        private void Update() {
            // Check if the renderer bounds isn't in any of the camera frustum's planes i.e.: if the object isn't visible to the camera
            // Technically useless, should be possible to be removed.
            if (!GeometryUtility.TestPlanesAABB(_cameraFrustumPlanes, _renderer.bounds)) {
                Destroy(gameObject);
            }
        }

        private void FixedUpdate() {
            _rigidbody.MovePosition(_rigidbody.position + _speed * Time.fixedDeltaTime * Direction);
        }
    }
}