using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Enemy))]
    [RequireComponent(typeof(EnemySound))]
    public class EnemyMovement : MonoBehaviour {

        private Transform _target;
        private int _wavepointIndex = 0;

        //I'm using the same method in the "EndUI" script
        public Sprite EnemySpriteRight;
        public Sprite EnemySpriteLeft;
        public Sprite EnemySpriteUp;
        public Sprite EnemySpriteDown;
        public GameObject EnemySpriteGameObject;

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

                //TODO: remover ands depois do txt ficar pronto
                if (_target.position.x >= transform.position.x && _target.position.x - transform.position.x > 0.2f)
                {
                    EnemySpriteGameObject.GetComponent<SpriteRenderer>().sprite = EnemySpriteRight;
                    Debug.Log("Target: " + _target.position + " Transform: " + transform.position + " RIGHT");
                }

                else if (_target.position.x < transform.position.x && transform.position.x - _target.position.x > 0.2f)
                {
                    EnemySpriteGameObject.GetComponent<SpriteRenderer>().sprite = EnemySpriteLeft;
                    Debug.Log("Target: " + _target.position + " Transform: " + transform.position + " LEFT");
                }

                else if (_target.position.z >= transform.position.z && _target.position.z - transform.position.z > 0.2f)
                {
                    EnemySpriteGameObject.GetComponent<SpriteRenderer>().sprite = EnemySpriteUp;
                    Debug.Log("Target: " + _target.position + " Transform: " + transform.position + " UP");
                }

                else if (_target.position.z < transform.position.z && transform.position.z - _target.position.z > 0.2f)
                {
                    EnemySpriteGameObject.GetComponent<SpriteRenderer>().sprite = EnemySpriteDown;
                    Debug.Log("Target: " + _target.position + " Transform: " + transform.position + " DOWN");
                }
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
