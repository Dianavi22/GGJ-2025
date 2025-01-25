using Math;
using UnityEngine;

namespace Player {
    [RequireComponent(typeof(Rigidbody))]
    public class Aim : MonoBehaviour {
        private Rigidbody _rigidbody;

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate() {
            Vector3 offsetRot = transform.forward * GetMouseAngle() * Mathf.Rad2Deg;
            Quaternion targetRotQuat = Quaternion.Euler(offsetRot);

            _rigidbody.MoveRotation(targetRotQuat);
        }

        private float GetMouseAngle() {
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldMousePos.z = 0;
            return CustomMath.GetXAngle(transform.position, worldMousePos);
        }
    }
}
