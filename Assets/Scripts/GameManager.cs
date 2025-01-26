using Bubble;
using System;
using System.Collections;
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
        [SerializeField] Canvas ShootingCanvas;

        [Header("Canvas Images")]
        [SerializeField] GameObject GameOver_GO;
        [SerializeField] GameObject Settings;

        [Header("Map Background")]
        [SerializeField] GameObject MapAndAssets;


        [Header("End Game Text")]
        [SerializeField] GameObject WinText;
        [SerializeField] GameObject DeadText;

        [Header("Initialize Setup")]
        [SerializeField] GameObject player;
        [SerializeField] GameObject laBulle;
        [SerializeField] GameObject tuto;

        [SerializeField] ParticleSystem _targetParticles;
        [SerializeField] ParticleSystem _startpart;

        [SerializeField] BubbleGrowth _bbg;

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
        [SerializeField] Animator _bubbleAnimator;    
        public static GameManager Instance {
            get {
                if (_instance == null) {
                    _instance = new GameManager();
                }
                return _instance;
            }
        }

        public void Start() {
            _initialeBubbleTransform = laBulle.transform;
            _initialePlayerTransform = player.transform;
            _initialeBubbleSize = laBulle.GetComponent<BubbleGrowth>().initialSize;
        }

        public void Update() {

            if(!isPlaying) {
                return;
            }

            if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && isPlaying) {
                Pause();
            }

            if(laBulle.GetComponent<BubbleGrowth>().GetIsShrinked() && isPlaying) {
                isPlaying = false;
                GameOver();
            }

            if (10 <= laBulle.transform.localScale.x) {
                Win();
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
            StartCoroutine(StartSetUp());

        }

        private IEnumerator StartSetUp() {
            
            MainMenuCanvas.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            _startpart.Play();
            yield return new WaitForSeconds(0.5f);
            _targetParticles.Play();
            laBulle.GetComponent<BubbleGrowth>().ResetValue();
           
            Time.timeScale = 1.0f;
            yield return new WaitForSeconds(0.2f);

            ResetGame();
            isPlaying = true;
            //Activate
            UICanvas.gameObject.SetActive(isPlaying);
            ShootingCanvas.gameObject.SetActive(isPlaying);
            player.gameObject.SetActive(isPlaying);
            tuto.gameObject.SetActive(isPlaying);
            laBulle.gameObject.SetActive(isPlaying);
            spawnerManager.gameObject.SetActive(isPlaying);
            MapAndAssets.gameObject.SetActive(isPlaying);

        }

        public void GameOver() {
            isPlaying = false;
            laBulle.GetComponent<BubbleGrowth>().AnimationDeath();
            Invoke("GameOverCanvasFunc", 2.5f);
        }

        public void GameOverCanvasFunc() {
            GameOverCanvas.gameObject.SetActive(true);
            laBulle.GetComponent<BubbleGrowth>().ResetValue();
            WinText.SetActive(false);
            DeadText.SetActive(true);
            Invoke("CallGameOverMenu", 2);
        }

        public void Win() {
            isPlaying = false;
            GameOverCanvas.gameObject.SetActive(true);
            print(GameOverCanvas.gameObject.activeInHierarchy);
            WinText.SetActive(true);
            DeadText.SetActive(false);
            Invoke("CallGameOverMenu", 3);
        }

        private void CallGameOverMenu() {
            _targetParticles.Stop();
            GameOver_GO.SetActive(true);
        }

        public void QuitGame() {
            Application.Quit();
        }

        public void BackToMainMen() {
            ResetGame();
            MainMenuCanvas.gameObject.SetActive(!isPlaying);
        }

        public void Pause() {
                _isPaused = !_isPaused;
                if (_isPaused) {
                    Time.timeScale = 0f;
                    PauseMenuCanvas.gameObject.SetActive(true);
                } else {
                    PauseMenuCanvas.gameObject.SetActive(false);
                    Time.timeScale = 1f;
            }
        }

        public void ResetGame() {
            isPlaying = false; // Should check everything needed when is playing or not is playing
            player.transform.position = _initialePlayerTransform.position;
            laBulle.transform.position = _initialeBubbleTransform.position;
            _isPaused = false;

            //Reset All UII and Canvas to False
            UICanvas.gameObject.SetActive(isPlaying);
            ShootingCanvas.gameObject.SetActive(isPlaying);
            PauseMenuCanvas.gameObject.SetActive(isPlaying);
            player.gameObject.SetActive(isPlaying);
            laBulle.gameObject.SetActive(isPlaying);
            //laBulle.gameObject.GetComponentInChildren<MeshRenderer>().is
            spawnerManager.gameObject.SetActive(isPlaying);
            GameOverCanvas.gameObject.SetActive(isPlaying);
            GameOver_GO.SetActive(isPlaying);
            MapAndAssets.gameObject.SetActive(isPlaying);

            //Reset Bulle Value
            laBulle.GetComponent<BubbleGrowth>().ResetValue();
        }

        public void DisplaySetting() {
            Settings.SetActive(true);
        }

        public void CloseSettings() {
            Settings.SetActive(false);
        }
    }
}
