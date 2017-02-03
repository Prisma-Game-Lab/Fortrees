using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Enemy))]
    [RequireComponent(typeof(EnemySound))]
    public class EnemyMovement : MonoBehaviour {

        private Transform _target;
        private int _wavepointIndex = 0;

        private Enemy _enemy;
        private EnemySound _sound;

        public void Start()
        {
            _enemy = GetComponent<Enemy>();
            _sound = GetComponent<EnemySound>();
            _target = Waypoints.Points[0];
        }

        public void Update()
        {
            Vector3 dir = _target.position - transform.position;
            transform.Translate(dir.normalized * _enemy.Speed * Time.deltaTime, Space.World);

            if (Vector3.Distance(transform.position, _target.position) <= 0.2f)
            {
                GetNextWaypoint();
            }

            _enemy.Speed = _enemy.StartSpeed;
        }

        //Muda o wavepoint atual para o próximo e atualiza o target
        private void GetNextWaypoint()
        {
            if (_wavepointIndex >= Waypoints.Points.Length - 1)
            { //Acabou os waypoints
                EndPath();
                return;
            }

            _wavepointIndex++;
            _target = Waypoints.Points[_wavepointIndex];
        }

        private void EndPath()
        {
            WaveSpawner.EnemiesAlive--;
            PlayerStats.Lives--;
            _sound.EnemyReachedEnd();
            Destroy(gameObject);
        }
    }
}
