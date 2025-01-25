using UnityEngine;

namespace Math {
    public class CustomMath : MonoBehaviour {
        // Computing Second Order Dynamic coefficient from frequency, damping and reaction
        public static (float k1, float k2, float k3) ComputeSecondOrderConstants(float f, float z, float r) {
            float k1 = z / (Mathf.PI * f);
            float k2 = 1 / ((2 * Mathf.PI * f) * (2 * Mathf.PI * f));
            float k3 = r * z / (2 * Mathf.PI * f);

            return (k1, k2, k3);
        }

        // x is the target position
        // y is the modified X position
        // dx or dy are velocities (first position derivatives)
        // if dx is unknown, it is estimated using mean velocity computation
        public static (Vector3 y, Vector3 dy) SeconOrderDynamic(float k1, float k2, float k3, float t, Vector3 x, Vector3? dx, Vector3 y, Vector3 dy, Vector3 previousX) {
            if (dx == null) {
                dx = (x - previousX) / t;
            }

            Vector3 yRes = y + t * dy;
            Vector3 dyRes = dy + t * (x + k3 * (Vector3)dx - y - k1 * dy) / k2;

            return (yRes, dyRes);
        }

        /// <summary>
        /// This method returns the angle (related to the X axis) between two points.
        /// <example>For example:
        /// <code>
        /// Vector2 origin = new(0, 0);
        /// Vector2 target = new(0.5, 0.5);
        /// float angle = CustomMaths.GetXAngle(origin, target);
        /// </code>
        /// results in <c>angle</c> to be ~= 0.785398 radian (= 45°)
        /// </example>
        /// </summary>
        /// <param name="origin">The origin point</param>
        /// <param name="target">The targeted point</param>
        /// <returns>The angle between <paramref name="target"/> and <paramref name="origin"/> (related to the X angle) in radian</returns>
        public static float GetXAngle(Vector2 origin, Vector2 target) {
            return Mathf.Atan2(target.y - origin.y, target.x - origin.x);
        }

        public static Vector2 RotateVectorAroundOrigin(Vector2 vector, float angle) {
            angle *= Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angle) * vector.x - Mathf.Sin(angle) * vector.y, Mathf.Sin(angle) * vector.x + Mathf.Cos(angle) * vector.y).normalized;
        }
    }
}

