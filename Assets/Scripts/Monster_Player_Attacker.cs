using UnityEngine;

public class Monster_Player_Attacker : SimpleMonster {

    public GameObject player;


    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindWithTag("Player");

        transform.LookAt(player.transform.position);
    }

    void FixedUpdate() {
        if (player == null) {
            player = GameObject.FindWithTag("Player");
        }

        DoBasicMove();
    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.tag == "Player") {
            // TODO do damage.
            Destroy(gameObject);
        }
    }
}
