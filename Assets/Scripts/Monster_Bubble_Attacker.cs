using UnityEngine;

/// <summary>
/// Defines the behaviour of the monster that attacks the bubble.
/// </summary>
public class MonsterType1 : MonoBehaviour
{

    [SerializeField] private float _speed = 1.0f;

    private GameObject _bubble;

    // Start is called before the first frame update
    void Start()
    {
        _bubble = GameObject.Find("Bubble");
    }

    void LateUpdate()
    {
        // We do it here so we can change the bubble appearance before and run the move function.
        Move();
    }

    private void Move()
    {
        // 1) Identify the closest part of the bubble.


        // 2) Move to the identified part.

    }
}
