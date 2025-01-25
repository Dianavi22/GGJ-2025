using Math;
using UnityEngine;

namespace Player {
    [RequireComponent(typeof(Rigidbody2D))]
    public class Aim : MonoBehaviour {
        private Rigidbody2D _rigidbody;

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate() {
            _rigidbody.SetRotation(GetMouseAngle() * Mathf.Rad2Deg);
        }
        
        private float GetMouseAngle() {
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldMousePos.z = 0;
            return CustomMath.GetXAngle(transform.position, worldMousePos);
        }
    }
}
