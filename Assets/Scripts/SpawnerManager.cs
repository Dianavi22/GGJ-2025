using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerManager : MonoBehaviour {

    public int spawnAvailable = 2;
    private int _initialeSpawnerNumber = 2;
    public List<Spawner> _spawners = new();
    public List<Spawner> _currentSpawnList = new(); // Reseted When the list as spawned

    private float _timeElapsed = 0;
    private float _timeTresholdToCreateMonsters = 1; // Varier.

    private void Awake() {
        _spawners = GetComponentsInChildren<Spawner>().ToList();
    }

    private void Start() {
        _initialeSpawnerNumber = spawnAvailable;
    }

    private void Update() {
        _timeElapsed += Time.deltaTime;

        // We Wait for Threshold of Monster
        if (_timeElapsed > _timeTresholdToCreateMonsters) {
            for (int i = 0; i < Random.Range(1, spawnAvailable + 1); i++) {
                _currentSpawnList.Add(_spawners[_spawners.Count() - 1]);
            }; 

            // Reset TimeElapsed
            _timeElapsed = 0;
            // Modify the Threshold time before next spawner
            _timeTresholdToCreateMonsters = Random.Range(2, 4);

            // Spawns Random Enemies on Random emplacements
            // The Spawner check if it can spawn enemies itself.
            _currentSpawnList.ForEach(spawner => {
                System.Random random = new();
                // active ones will spawns enemies.
                spawner.Spawn(random.Next(0, 2) == 0 ? MonsterType.PlayerAttacker : MonsterType.BubbleAttacker);
            });
            _currentSpawnList.Clear();
        }
    }
    public void UpgradeNumberOfSpawn() {
        spawnAvailable++;
    }

    public void ResetSpawnerManager() {
        _currentSpawnList.Clear();
        spawnAvailable = _initialeSpawnerNumber;
    }
}
