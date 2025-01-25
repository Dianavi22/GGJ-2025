using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerManager : MonoBehaviour {

    public List<Spawner> _spawners = new();

    private float _timeElapsed = 0;
    private float _timeTresholdToCreateMonsters = 1;

    private void Awake() {
        _spawners = GetComponentsInChildren<Spawner>().ToList();
    }

    private void Update() {
        _timeElapsed += Time.deltaTime;

        if (_timeElapsed > _timeTresholdToCreateMonsters) {
            _timeElapsed = 0;
            _spawners.ForEach(spawner => {
                System.Random random = new();
                // active ones will spawns enemies.
                spawner.Spawn(random.Next(0, 2) == 0 ? MonsterType.PlayerAttacker : MonsterType.BubbleAttacker);
            });
        }
    }
}
