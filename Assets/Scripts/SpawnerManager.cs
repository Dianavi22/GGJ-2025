using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerManager : MonoBehaviour {
    [SerializeField] private float _timeTresholdToCreateMonsters = 1;

    private List<Spawner> _spawners = new();
    private float _elapsedTime = 0;

    private void Awake() {
        _spawners = GetComponentsInChildren<Spawner>().ToList();
    }

    private void Update() {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime > _timeTresholdToCreateMonsters) {
            _elapsedTime = 0;
            IEnumerable<Spawner> activatedSpawners = _spawners.Where(spawner => spawner.IsActive);
            activatedSpawners.ElementAt(Random.Range(0, activatedSpawners.Count())).Spawn(MonsterType.BubbleAttacker);
        }
    }
}
