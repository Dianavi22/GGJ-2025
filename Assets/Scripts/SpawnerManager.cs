using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour {
    [SerializeField]
    private List<Spwaner> _spawners = new List<Spwaner>();

    private float _timeElapsed = 0;
    private float _timeTresholdToCreateMonsters = 3;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        _timeElapsed += Time.deltaTime;

        if (_timeElapsed > _timeTresholdToCreateMonsters) {
            _timeElapsed = 0;
            _spawners.ForEach(spwaner => {
                System.Random random = new System.Random();
                int rng = random.Next(0, 1);
                // active ones will spawns enemies.
                spwaner.Spawn(rng == 0 ? MonsterType.PlayerAttacker : MonsterType.BubbleAttacker); 
            });
        }
    }
}
