using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{

    [SerializeField] List<GameObject> _bubblesCredits = new List<GameObject>();
    [SerializeField] List<GameObject> _buttons = new List<GameObject>();
    [SerializeField] ParticleSystem _creditsPart;
    public void LoadsCredits() {
        _creditsPart.Play();
        for (int i = 0; i < _buttons.Count; i++) {
            _buttons[i].SetActive(false);
        }
        StartCoroutine(CreditsBubbles());
    }

    private IEnumerator CreditsBubbles() {
        yield return new WaitForSeconds(2);

        for (int i = 0; i < _bubblesCredits.Count; i++) {
            _bubblesCredits[i].SetActive(true);

            yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(7);
        DisableCredits();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            DisableCredits();
            StopAllCoroutines();
        }
    }

    private void DisableCredits() {
        _creditsPart.Stop();

        for (int i = 0; i < _buttons.Count; i++) {
            _buttons[i].SetActive(true);
        }
        for (int i = 0; i < _bubblesCredits.Count; i++) {
            _bubblesCredits[i].SetActive(false);
        }
    }
}
