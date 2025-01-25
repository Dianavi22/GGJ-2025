using UnityEngine;

public class MonsterType0_Attacker : SimpleMonster {

    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");

        transform.LookAt(player.transform.position);
    }

    void FixedUpdate()
    {
        DoBasicMove();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO do damage.
        Destroy(gameObject);
    }

    /// <summary>
    /// This method returns the angle (related to the X axis) between two points.
    /// <example>For example:
    /// <code>
    /// Vector2 origin = new(0, 0);
    /// Vector2 target = new(0.5, 0.5);
    /// float angle = CustomMaths.GetXAngle(origin, target);
    /// </code>
    /// results in <c>angle</c> to be ~= 0.785398 radian (= 45�)
    /// </example>
    /// </summary>
    /// <param name="origin">The origin point</param>
    /// <param name="target">The targeted point</param>
    /// <returns>The angle between <paramref name="target"/> and <paramref name="origin"/> (related to the X angle) in radian</returns>
    public static float GetXAngle(Vector2 origin, Vector2 target)
    {
        return Mathf.Atan2(target.y - origin.y, target.x - origin.x);
    }
}
