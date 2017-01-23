using UnityEngine;

namespace Assets.Scripts
{
    public class Turret : MonoBehaviour {

        private Transform _target;
        private Enemy _targetEnemy;

        [Header("General")]
        public float Range = 15f;

        [Header("Unity Setup Fields")]
        public Transform PartToRotate;
        public Transform FirePoint;
        public float TurnSpeed = 10f;
        public string EnemyTag = "Enemy";

        [Header("Use Bullets (default)")]
        public float FireRate = 1f;
        private float _fireCountdown = 0f;
        public GameObject BulletPrefab;

        [Header("Use Laser")]
        public bool UseLaser = false;
        public LineRenderer LineRenderer;
        public int DamageOverTime = 20;
        public float SlowFactorPercentage = .5f;

        // Use this for initialization
        public void Start()
        {
            InvokeRepeating("UpdateTarget", 0f, 0.5f);
        }

        // Update is called once per frame
        public void Update()
        {
            if (_target == null)
            {
                if (UseLaser)
                    if (LineRenderer.enabled)
                        LineRenderer.enabled = false;
                return;
            }

            //target lock on
            LockOnTarget();

            if (UseLaser)
                Laser();
            else
                UseBullet();
        }

        private void UseBullet()
        {
            if (_fireCountdown <= 0f)
            {
                Shoot();
                _fireCountdown = 1f/FireRate;
            }
            _fireCountdown -= Time.deltaTime;
        }

        private void Laser()
        {
            _targetEnemy.TakeDamage(DamageOverTime*Time.deltaTime);
            _targetEnemy.Slow(SlowFactorPercentage);

            if (!LineRenderer.enabled)
                LineRenderer.enabled = true;
            LineRenderer.SetPosition(0, FirePoint.transform.position);
            LineRenderer.SetPosition(1, _target.position);
        }

        private void LockOnTarget()
        {
            Vector3 dir = _target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(PartToRotate.rotation, lookRotation, Time.deltaTime*TurnSpeed).eulerAngles;
            PartToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }

        private void Shoot()
        {
            GameObject bulletGO = (GameObject) Instantiate (BulletPrefab, FirePoint.position, FirePoint.rotation);
            Bullet bullet = bulletGO.GetComponent<Bullet> ();

            if (bullet != null)
                bullet.Seek (_target);
        }

        public void UpdateTarget()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(EnemyTag);
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach(GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

                if(distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }

            if (nearestEnemy != null && shortestDistance <= Range)
            {
                _target = nearestEnemy.transform;
                _targetEnemy = nearestEnemy.GetComponent<Enemy>();
            }

            else _target = null;
        }

       public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Range);
        }
    }
}
