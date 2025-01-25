using System;
using UnityEngine;

namespace Assets.Scripts
{
    internal class GameManager
    {
        public bool isPlaying  = true;
        private int _level = 0;
        private int _score = 0;

        private float _precision_percentage = 100;
        private float _nb_shoots = 0;
        private float _nb_sucessful_shoots = 0;

        private WeaponLevelEnum _weaponLevelEnum = WeaponLevelEnum.Level1;

        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameManager();
                }
                return _instance;
            }
        }

        private GameManager() { }

        public int GetLevel()
        {
            return _level;
        }

        public void Score()
        {
            _score += _score;

            // This means that every 10 times that we score, we increase the level.
            _level = _score % 10;
        }

        /// <summary>
        /// Used for UI
        /// </summary>
        /// <returns></returns>
        public float GetPercentage()
        {
            // ex: 30,30921 x 100 => rounded = 3031 => rounded / 100 => 30,31% 
            return MathF.Round(_precision_percentage * 100) / 100;
        }

        public void ShootMissed()
        {
            _nb_shoots++;
            _precision_percentage = _nb_sucessful_shoots / _nb_shoots;
            UpdateWeaponLevel();
        }

        public void ShootSucessful()
        {
            _nb_shoots++;
            _nb_sucessful_shoots++;
            _precision_percentage = _nb_sucessful_shoots / _nb_shoots;
            UpdateWeaponLevel();
        }

        public void UpdateWeaponLevel()
        {
            if (_precision_percentage > 99)
            {
                _weaponLevelEnum = WeaponLevelEnum.Level4;
            }
            else if (_precision_percentage > 90)
            {
                _weaponLevelEnum = WeaponLevelEnum.Level3;
            }
            else if (_precision_percentage > 88)
            {
                _weaponLevelEnum = WeaponLevelEnum.Level2;
            }
            else
            {
                _weaponLevelEnum = WeaponLevelEnum.Level1;
            }
        }

        public void GameOver() {
            isPlaying = false;
        }

        public WeaponLevelEnum GetWeaponLevelEnum()
        {
            return _weaponLevelEnum;
        }
    }
}
