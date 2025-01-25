using UnityEngine;

public class Spawner : MonoBehaviour {
    private bool _isActive = false;

    [SerializeField]
    public GameObject bubbleAttacker;

    [SerializeField]
    public GameObject playerAttacker;

    [SerializeField] private LayerMask _activateSpawnerLayerMask;

    public void Spawn(MonsterType monsterType) {
        if (!_isActive) {
            return;
        }

        if (monsterType == MonsterType.BubbleAttacker ? bubbleAttacker : playerAttacker != null) {
            Instantiate(monsterType == MonsterType.BubbleAttacker ? bubbleAttacker : playerAttacker,
                transform.position, transform.rotation);
        }
    }

    private void OnTriggerEnter(Collider other) {
        PassThroughActivator(other.gameObject.layer);
    }

    private void OnTriggerExit(Collider other) {
        PassThroughActivator(other.gameObject.layer);
    }

    private void PassThroughActivator(int layer) {
        if (layer.IsInMask(_activateSpawnerLayerMask)) {
            _isActive = !_isActive;
        }
    }
}

public static class LayerMaskExtension {
    public static bool IsInMask(this int layer, LayerMask mask) {
        return (mask & (1 << layer)) > 0;
    }
}
