using UnityEngine;

public class Spwaner : MonoBehaviour {
    public bool _isActive = false;

    [SerializeField]
    public GameObject bubbleAttacker;

    [SerializeField]
    public GameObject playerAttacker;

    void Start() {

    }

    public void Spawn(MonsterType monsterType) {
        if (!_isActive) {
            return;
        }

        if(monsterType == MonsterType.BubbleAttacker ? bubbleAttacker : playerAttacker != null) {
            Instantiate(monsterType == MonsterType.BubbleAttacker ? bubbleAttacker : playerAttacker,
                transform.position, transform.rotation);
        }
    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.name == "Bubble_Activator") {
            _isActive = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.name == "Bubble_Activator") {
            _isActive = false;
        }
    }
}
