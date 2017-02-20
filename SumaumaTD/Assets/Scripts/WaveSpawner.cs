﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class WaveSpawner : MonoBehaviour
    {
        public static int EnemiesAlive = 0;
        
        [Tooltip("Ponto do qual o inimigo spawna")]
        public Transform SpawnPoint;
        public Text WaveCountdownText;
        public MovingSeedsManager SeedsManager;
		public float TimeBetweenWaves = 5f;
        
        [Header("UnityStuff")]
        public Image WaveCountdownBar;
        public GameManager GameManager;
        //public float TimeBetweenEnemySpawns = 0.5f;
		[HideInInspector]
		public static int NumberOfWaves;

		private Wave[] _waves;
        private float _countdown = 2f;
        private int _waveNumber = 0;
        private bool _seedsEarned = true;
        private float _countdownReset = 2f; // used to fill the bar

        public void Start()
        {
            EnemiesAlive = 0;
            SetWaves();
        }

        private void SetWaves()
        {
            NumberOfWaves = transform.childCount;
			_waves = new Wave[NumberOfWaves];
			for (var i = 0; i < NumberOfWaves; i++)
            {
                var wave = transform.GetChild(i).GetComponent<Wave>();
                _waves[i] = wave;
            }
        }

        // Update is called once per frame
        public void Update () {
            if(!GameManager.GameStarted)
                return;
            if (EnemiesAlive > 0)
            {
                return;
            }
            
            if(_countdown <= 0f) {
                StartCoroutine(SpawnWave());
                _countdown = TimeBetweenWaves;
                _countdownReset = _countdown;
                _seedsEarned = false;
                return;
            }

            EarnSeeds();

            _countdown -= Time.deltaTime;

            _countdown = Mathf.Clamp(_countdown, 0f, Mathf.Infinity);

            WaveCountdownText.text = string.Format("{0:00.00}", _countdown);

            WaveCountdownBar.fillAmount = _countdown / _countdownReset;

        }

        private void EarnSeeds()
        {
            if(_seedsEarned)
                return;
            //TODO VER PQ SEEDS
            SeedsManager.AddSeeds(_waves[_waveNumber].SeedsEarned);  //TODO: diminuir de acordo com quantos inimigos chegaram na base?
            _seedsEarned = true;

            _waveNumber++;

            if (_waveNumber == _waves.Length)
            {
                GameManager.WinLevel();
                this.enabled = false;
            }
        }

        //Chamada no começo da wave
        private IEnumerator SpawnWave()
        {
            
            PlayerStats.Waves ++;
            Wave wave = _waves[_waveNumber];

            for (var g = 0; g < wave.Groups.Length; g++)
            {
                Group group = wave.Groups[g];

                for (var i = 0; i < group.Count; i++)
                {
                    SpawnEnemy(group.EnemyPrefab);
                    yield return new WaitForSeconds(1f/ group.SpawnRate);
                }
            }
        }

        //Spawna inimigos
        private void SpawnEnemy(GameObject enemyPrefab)
        {
            Instantiate(enemyPrefab, SpawnPoint.position, SpawnPoint.rotation);
            EnemiesAlive++;
        }
    }
}
