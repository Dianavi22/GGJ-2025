using Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Manager {
    public class AudioManager : MonoBehaviour {
        private static AudioManager _instance;

        public static AudioManager Instance {
            get { return _instance; }
        }

        [SerializeField, Range(0, 1)] private float _sfxVolume = 0.5f;
        [SerializeField, Range(0, 1)] private float _musicVolume = 0.5f;
        [SerializeField] private Slider _volumeSlider;
        [SerializeField] private Slider _sfxSlider;

        [SerializeField] private AudioSource _uiButtonClick;
        [SerializeField] private AudioSource _mainLoop;

        private AudioSource _musicPlayer;

        [HideInInspector] public UnityEvent<float> OnSfxVolumeChanged;

        public float SfxVolume => _sfxVolume;

        private void Awake() {
            _musicPlayer = GetComponent<AudioSource>();
            _instance = this;

            OnSfxVolumeChanged.AddListener(_uiButtonClick.UpdateVolume);
            Invoke(nameof(ChangeToMainLoop), _musicPlayer.clip.length - .75f);
        }

        private void Start() {
            _sfxSlider.value = _sfxVolume;
            _volumeSlider.value = _musicVolume;

            _sfxSlider.onValueChanged.AddListener(SetSfxVolume);
            _volumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        public void SetSfxVolume(float value) {
            _sfxVolume = Mathf.Clamp01(value);
            OnSfxVolumeChanged.Invoke(_sfxVolume);
        }

        public void SetMusicVolume(float value) {
            _musicVolume = Mathf.Clamp01(value);
            _musicPlayer.volume = _musicVolume;
            _mainLoop.volume = _musicVolume;
        }

        public void OnButtonClick() {
            _uiButtonClick.Play();
        }

        private void ChangeToMainLoop() {
            _mainLoop.Play();
        }
    }
}
