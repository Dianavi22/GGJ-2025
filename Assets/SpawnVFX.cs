using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnVFX : MonoBehaviour
{
    [SerializeField] GameObject _destroyPart;

    public void SpawnerVFX(bool isTimed, Transform transform) {
        if (isTimed) {
            print("HEEEEEERE");
            SpawnDestoyBulletPart _spb = Instantiate(_destroyPart, transform).GetComponent<SpawnDestoyBulletPart>();
            StartCoroutine(_spb.DestroySpeBulletVFX());
        } else {
            print("HEEEEEERE");
            Instantiate(_destroyPart, transform).GetComponent<SpawnDestoyBulletPart>();
           // StartCoroutine(_spb.DestroyBulletVFX());
        }
    }
}
