using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerManager : MonoBehaviour {

    private int _initialeSpawnerNumber = 2;

    public List<Spawner> _spawners = new();

    private float _timeElapsed = 0;
    private float _timeTresholdToCreateMonsters = 1; // Varier.

    private void Awake() {
        _spawners = GetComponentsInChildren<Spawner>().ToList();
    }
    private void Update() {
        _timeElapsed += Time.deltaTime;

        // We Wait for Threshold of Monster
        if (_timeElapsed > _timeTresholdToCreateMonsters) {

            // Reset TimeElapsed
            _timeElapsed = 0;

            _spawners.ForEach(spawner => {
                System.Random random = new();
                // active ones will spawns enemies.
                spawner.Spawn(random.Next(0, 2) == 0 ? MonsterType.PlayerAttacker : MonsterType.BubbleAttacker);
            });
        }
    }
}
