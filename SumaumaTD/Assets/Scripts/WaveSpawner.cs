using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class WaveSpawner : MonoBehaviour
    {
        public static int EnemiesAlive = 0;

        //public Transform EnemyPrefab;
        public Wave[] Waves;
        public Transform SpawnPoint;
        public Text WaveCountdownText;

        public float TimeBetweenWaves = 5f;
        //public float TimeBetweenEnemySpawns = 0.5f;

        private float _countdown = 2f;
        private int _waveNumber = 0;


        public void Start()
        {
            EnemiesAlive = 0;
        }

        // Update is called once per frame
        public void Update () {
            if (EnemiesAlive > 0)
            {
                return;
            }

            if(_countdown <= 0f) {
                StartCoroutine(SpawnWave());
                _countdown = TimeBetweenWaves;
                return;
            }

            _countdown -= Time.deltaTime;

            _countdown = Mathf.Clamp(_countdown, 0f, Mathf.Infinity);

            WaveCountdownText.text = string.Format("{0:00.00}", _countdown);

        }

        //Chamada no começo da wave
        private IEnumerator SpawnWave()
        {
            
            PlayerStats.Waves ++;
            Wave wave = Waves[_waveNumber];
            for (var i = 0; i < wave.Count; i++)
            {
                SpawnEnemy(wave.EnemyPrefab);
                yield return new WaitForSeconds(1f/wave.SpawnRate);
            }
            _waveNumber++;

            if (_waveNumber == Waves.Length)
            {
                Debug.Log("Level won!");
                this.enabled = false;
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
