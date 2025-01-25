using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerfectGlow : MonoBehaviour {

    [SerializeField] Slider slider;

    [SerializeField] Image image; 

    // Start is called before the first frame update
    private void Start() {
        slider.onValueChanged.AddListener(PerfectUpdateColor);
    }

    private void PerfectUpdateColor(float value) {
        if (value > 0.65 && value < 0.75) {
            image.color = new Color(1, 1, 0, 1);
        } else{
            image.color = new Color(0.4f, 0.4f, 0.4f, 1);
        }
    }
}