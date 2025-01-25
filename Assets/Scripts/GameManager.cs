using Bubble;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts {
    internal class GameManager : MonoBehaviour{

        [Header("Public Attributes")]
        public bool isPlaying = false;

        [Header("Manager")]
        [SerializeField] SpawnerManager spawnerManager;


        [Header("Game Canvas")]
        [SerializeField] Canvas MainMenuCanvas;
        [SerializeField] Canvas PauseMenuCanvas;
        [SerializeField] Canvas GameOverCanvas;
        [SerializeField] Canvas UICanvas;

        [Header("End Game Text")]
        [SerializeField] GameObject WinText;
        [SerializeField] GameObject DeadText;

        [Header("Initialize Setup")]
        [SerializeField] GameObject player;
        [SerializeField] GameObject laBulle;

        // Save InitialePosition
        private Transform _initialeBubbleTransform;
        private float _initialeBubbleSize;
        private Transform _initialePlayerTransform;


        private int _level = 0;
        private int _score = 0;

        private float _precision_percentage = 100;
        private float _nb_shoots = 0;
        private float _nb_sucessful_shoots = 0;

        private WeaponTypeEnum _weaponLevelEnum = WeaponTypeEnum.Level1;
        private static GameManager _instance;

        private bool _isPaused = false;

        public static GameManager Instance {
            get {
                if (_instance == null) {
                    _instance = new GameManager();
                }
                return _instance;
            }
        }
        public void Awake() {

        }

        public void Start() {
            _initialeBubbleTransform = laBulle.transform;
            _initialePlayerTransform = player.transform;
            _initialeBubbleSize = laBulle.GetComponent<BubbleGrowth>().initialSize;

        }

        public void Update() {
            if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && isPlaying) {
                Pause();
            }
        }

        public int GetLevel() {
            return _level;
        }

        public void Score() {
            _score += _score;

            // This means that every 10 times that we score, we increase the level.
            _level = _score % 10;
        }

        /// <summary>
        /// Used for UI
        /// </summary>
        /// <returns></returns>
        public float GetPercentage() {
            // ex: 30,30921 x 100 => rounded = 3031 => rounded / 100 => 30,31% 
            return MathF.Round(_precision_percentage * 100) / 100;
        }

        public void ShootMissed() {
            _nb_shoots++;
            _precision_percentage = _nb_sucessful_shoots / _nb_shoots;
            UpdateWeaponLevel();
        }

        public void ShootSucessful() {
            _nb_shoots++;
            _nb_sucessful_shoots++;
            _precision_percentage = _nb_sucessful_shoots / _nb_shoots;
            UpdateWeaponLevel();
        }

        public void UpdateWeaponLevel() {
            if (_precision_percentage > 99) {
                _weaponLevelEnum = WeaponTypeEnum.Level4;
            } else if (_precision_percentage > 90) {
                _weaponLevelEnum = WeaponTypeEnum.Level3;
            } else if (_precision_percentage > 88) {
                _weaponLevelEnum = WeaponTypeEnum.Level2;
            } else {
                _weaponLevelEnum = WeaponTypeEnum.Level1;
            }
        }


        public WeaponTypeEnum GetWeaponLevelEnum() {
            return _weaponLevelEnum;
        }

        // Game Managing Canvas Display

        public void StartGame() {
            isPlaying = true;
            UICanvas.gameObject.SetActive(isPlaying);
        }
        public void GameOver() {
            isPlaying = false;
            GameOverCanvas.gameObject.SetActive(true);
            WinText.SetActive(false);
            DeadText.SetActive(true);
        }

        public void Win() {
            isPlaying = false;
            WinText.SetActive(true);
            DeadText.SetActive(false);
        }

        public void QuitGame() {
            Application.Quit();
        }

        public void BackToMainMen() {
            isPlaying = false;
            MainMenuCanvas.gameObject.SetActive(true);
            ResetGame();
        }

        public void Pause() {
            if (Input.GetKeyDown(KeyCode.P) && isPlaying) {
                _isPaused = !_isPaused;
                if (_isPaused) {
                    Time.timeScale = 0f;
                    PauseMenuCanvas.gameObject.SetActive(true);
                } else {
                    PauseMenuCanvas.gameObject.SetActive(false);
                    Time.timeScale = 1f;
                }
            }
        }

        public void ResetGame() {
            player.transform.position = _initialePlayerTransform.position;
            laBulle.transform.position = _initialeBubbleTransform.position;
            laBulle.transform.localScale = new Vector3(_initialeBubbleSize, _initialeBubbleSize, _initialeBubbleSize);

            spawnerManager._spawners.ForEach(spawner => {
                Destroy(spawner);
                spawnerManager._spawners.Remove(spawner);
            });
        }
    }
}
