using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{

    [SerializeField] ParticleSystem _playerFallPart;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("DeadZone")) {
            _playerFallPart.Play();
        }
    }
}
