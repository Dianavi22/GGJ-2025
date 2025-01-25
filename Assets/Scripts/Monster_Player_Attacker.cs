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

}
