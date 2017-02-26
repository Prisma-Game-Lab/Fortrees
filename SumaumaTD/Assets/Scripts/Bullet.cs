using UnityEngine;

namespace Assets.Scripts
{
    public class Bullet : MonoBehaviour
    {
        #region Variables
        public float Speed = 50f;
        public float ExplosionRadius = 0f;
        public GameObject ImpactEffect;
        public int Damage = 50;
        [Tooltip("Time to play particle effect when it hits the enemy")]
        public float EffectLength = 0.5f;
        [HideInInspector]
        public bool IsPoisoned = false;

        [Header("Poison")]
        public float PoisonDamage;
        public float PoisonTime;
        public float PoisonCooldown;

        [Header("Bezier")]
        [Tooltip("Define a porcentagem do caminho da bala onde ocorre a altura máxima")]
        [Range(0, 1)]
        public float BezierPointX = 0.5f;

        [Tooltip("Define a altura máxima que a parábola da bala atinge")]
        public float MaxHeight = 5f;

        [Header("Audio")]
        [Tooltip("Volume do som de hit")]
        [Range(0f, 1f)]
        public float AudioVolume = 1f;
        public AudioSource SoundSource;

        private float _bezierCounter;
        private Transform _target;
        private Vector3 _newdir;
        private Vector3 _bezierPoint;
        private Vector3 _startPosition;
        private float _totalDistance;
        private float _dir;
        #endregion

        public void Awake()
        {
            _bezierCounter = 0;
            _startPosition = transform.position;
        }

        public void Seek(Transform target)
        {
            _target = target;
            _totalDistance = (_startPosition - _target.position).magnitude;
            _dir = _totalDistance;
        }

        // Update is called once per frame
        public void Update()
        {
            if (_target == null)
            {
                Destroy(gameObject);
                return;
            }

            //Vector3 dir = _target.position - transform.position;
            float distanceThisFrame = Speed * Time.deltaTime;
            _dir -= distanceThisFrame;

            if (_dir <= distanceThisFrame)
            {
                HitTarget();
                return;
            }


            _bezierPoint = _target.position;
            _bezierPoint.y += MaxHeight;
            _bezierPoint.x = (_bezierPoint.x + _startPosition.x) * BezierPointX;
            //_bezierPoint.z = (_bezierPoint.z + _startPosition.z) * BezierPointX;


            _newdir = GetPoint(_startPosition, _bezierPoint, _target.position, _bezierCounter);

            _bezierCounter += distanceThisFrame / _totalDistance;

            //transform.Translate (newdir.normalized*distanceThisFrame, Space.World);
            transform.position = _newdir;
            transform.LookAt(_target);
        }

        //instantiate effect and explode/hit enemy and destroy bullet
        public void HitTarget()
        {
            GameObject effectIns = (GameObject)Instantiate(ImpactEffect, transform.position, transform.rotation);
            Destroy(effectIns, EffectLength); //faz com que seja destruído só depois que o áudio já tocou... TODO: simplificar

            if (ExplosionRadius > 0f)
            {
                Explode();
            }
            else
            {
                DamageEnemy(_target);
            }

            Destroy(gameObject);
        }
        
        private void Explode()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);//pega todos os colliders dentro de um raio (de uma esfera com centro em position)
            foreach (var collid in colliders)
            {
                if (collid.tag == "Enemy")
                {
                    DamageEnemy(collid.transform);
                }
            }
        }

        //calls enemy function to handle damage
        private void DamageEnemy(Transform enemy)
        {
            Enemy e = enemy.GetComponent<Enemy>();
            if (e == null) return;

            e.TakeDamage(Damage);

            if (IsPoisoned)
                e.TakePoison(PoisonTime, PoisonDamage, PoisonCooldown);
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_newdir, 0.1f);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(_bezierPoint, 0.1f);
        }

        public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            var first = Vector3.Lerp(p0, p1, t);
            var second = Vector3.Lerp(p1, p2, t);
            var third = Vector3.Lerp(first, second, t);

            return third;
        }

    }
}