using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class GradientSlider : MonoBehaviour {
        [SerializeField] private Gradient _gradient;

        private Image _targetImage;

        private void Start() {
            Slider slider = GetComponent<Slider>();
            _targetImage = slider.fillRect.GetComponentInChildren<Image>();

            slider.onValueChanged.AddListener(SliderUpdate);
        }

        private void SliderUpdate(float value) {
            _targetImage.color = _gradient.Evaluate(value);
        }
    }

}

