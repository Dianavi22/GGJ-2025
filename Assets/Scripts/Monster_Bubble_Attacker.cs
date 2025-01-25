using UnityEngine;

/// <summary>
/// Defines the behaviour of the monster that attacks the bubble.
/// </summary>
public class Monster_Bubble_Attacker : SimpleMonster {

    private GameObject _bubble;
    private BubbleTargetsGenerator bubbleTargetsGenerator;

    // Start is called before the first frame update
    void Start() {
        _bubble = GameObject.Find("Bubble");
        bubbleTargetsGenerator = _bubble.GetComponent<BubbleTargetsGenerator>();
    }


    void FixedUpdate() {
        Move();
    }

    private void Move() {
        Vector3 targetPosition = Vector3.zero;
        float closestDistance = Mathf.Infinity;

        // 1) Identify the closest part of the bubble.
        bubbleTargetsGenerator.bubbleTargetPoints.ForEach(target => {
            float currentMinDistance = Vector3.Distance(transform.position, target.transform.position);
            if (closestDistance > currentMinDistance) {
                closestDistance = currentMinDistance;
                targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
            }
        });

        // 3) Change orientation to target the closest point of the bubble.
        transform.LookAt(targetPosition);

        DoBasicMove();
    }
}
