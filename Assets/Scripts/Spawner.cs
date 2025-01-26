using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField]
    private bool _isActive = false;

    [SerializeField]
    public GameObject bubbleAttacker;

    [SerializeField]
    public GameObject playerAttacker;

    [SerializeField]
    public GameObject bubbleParticle;

    [SerializeField] private LayerMask _activateSpawnerLayerMask;

    public bool IsActive => _isActive;

    public void Spawn(MonsterType monsterType) {
        if (!_isActive) {
            return;
        }

        StartCoroutine(CreateSpawnAnimation());
    }

    public IEnumerator CreateSpawnAnimation() {
        bubbleParticle.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(3f);
        bubbleParticle.GetComponent<ParticleSystem>().Stop();
        Instantiate(bubbleAttacker, transform.position, Quaternion.Euler(0, 90, -90));
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
