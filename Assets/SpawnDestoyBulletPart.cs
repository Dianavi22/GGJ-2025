using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDestoyBulletPart : MonoBehaviour
{

    [SerializeField] ParticleSystem _destroyBulletPart;
    [SerializeField] ParticleSystem _destroySpeBulletPart;


    public IEnumerator DestroyBulletVFX() {
        _destroyBulletPart.Play();
        yield return new WaitForSeconds(1f);
        //Destroy(this.gameObject);
    }

    public IEnumerator DestroySpeBulletVFX() {
        _destroySpeBulletPart.Play();
        yield return new WaitForSeconds(1f);
        Destroy(this.GetComponentInParent<GameObject>());

    }

}
