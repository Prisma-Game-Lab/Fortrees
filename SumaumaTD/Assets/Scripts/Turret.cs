using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class Turret : MonoBehaviour {

#region Variables
        private Transform _target;
        private Enemy _targetEnemy;
        private static int _nextAttackAudio = 0; //usada para intercalar os sons de ataque
        private SurroundingExplorer _surroundings;
        private bool _poisonedBullet;
        private float _currentSeedGenerationTime = 0f;

        [Header("General")]
        public float Range = 15f;

		[Header("Bonus Specs")]
		public float SlowAmount = 0.2f;
        [Tooltip("Time in seconds to generate a single seed")]
		public float SeedGenerationTime= 30f;
        
        [Header("Unity Setup Fields")]
        public Animator SpriteAnimator;
        public AudioSource SoundSource;
        public Transform PartToRotate;
        public Transform FirePoint;
        public float TurnSpeed = 10f;
        public string EnemyTag = "Enemy";
        public MovingSeedsManager MovingSeedsManager;

        [Header("Use Bullets (default)")]
        public float FireRate = 1f;
        private float _fireCountdown = 0f;
        public GameObject BulletPrefab;
        [Tooltip("Lista os arquivos de som dos ataques")]
        public AudioClip[] AttackAudios;
        
        [Header("Use Range Damage")]
        [Tooltip("Every enemy that enters the range takes a damage")]
        public bool UseRangeDamage = false;
        private List<Enemy> _enemiesOnRange = new List<Enemy>();
        public int DamageOverTime = 20;

        //[Header("Use Laser")]
        //public bool UseLaser = false;
        //public LineRenderer LineRenderer;
        //public int DamageOverTime = 20;
        //public float SlowFactorPercentage = .5f;

        #endregion

        public void Start()
        {
            InvokeRepeating("UpdateTarget", 0f, 0.5f);
            SpriteAnimator = GetComponent<Animator>();
            _surroundings = new SurroundingExplorer();
        }
        
        public void Update()
        {
            _surroundings.GetSurroundings(1, this);

            if (!UseRangeDamage)
            {
                if (_target == null)
                {
                    //if (UseLaser)
                    //    if (LineRenderer.enabled)
                    //        LineRenderer.enabled = false;
                    return;
                }

                //target lock on
                LockOnTarget();

                //if (UseLaser)
                //    Laser();
                UseBullet();
            }
            else
                RangeDamage();
                

			ActivateAbility();
        }

        private void RangeDamage()
        {
            if(_enemiesOnRange.Count == 0)
                return;

            foreach (var enemy in _enemiesOnRange)
            {
                enemy.TakeDamage(DamageOverTime*Time.deltaTime);
            }
            CallShootingAnimation();
        }

        private void UseBullet()
        {
            if (_fireCountdown <= 0f)
            {
                CallShootingAnimation();
                _fireCountdown = 1f/FireRate;
            }
            _fireCountdown -= Time.deltaTime;
        }

        //private void Laser()
        //{
        //    _targetEnemy.TakeDamage(DamageOverTime*Time.deltaTime);
        //    _targetEnemy.Slow(SlowFactorPercentage);

        //    if (!LineRenderer.enabled)
        //        LineRenderer.enabled = true;
        //    LineRenderer.SetPosition(0, FirePoint.transform.position);
        //    LineRenderer.SetPosition(1, _target.position);
        //}

        private void LockOnTarget()
        {
            Vector3 dir = _target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(PartToRotate.rotation, lookRotation, Time.deltaTime*TurnSpeed).eulerAngles;
            PartToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }

        #region Shooting

        private void CallShootingAnimation()
        {
            SpriteAnimator.Play("Shooting");
        }
        
        private void PlayShootingSound()
        {
            if (_nextAttackAudio >= AttackAudios.Length) _nextAttackAudio = 0;
            SoundSource.PlayOneShot(AttackAudios[_nextAttackAudio]);
            _nextAttackAudio++;
        }

        public void Shoot()
        {
            GameObject bulletGO = (GameObject) Instantiate (BulletPrefab, FirePoint.position, FirePoint.rotation);
            PlayShootingSound();

            Bullet bullet = bulletGO.GetComponent<Bullet> ();

            bullet.IsPoisoned = _poisonedBullet;

            if (bullet != null)
                bullet.Seek (_target);
        }

        #endregion

        public void UpdateTarget()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(EnemyTag);
            if (UseRangeDamage)
            {
                GetAllEnemiesOnRange(enemies);
            }
            else
            {
                GetNearestEnemy(enemies);
            }
        }

        private void GetNearestEnemy(GameObject[] enemies)
        {
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

                if (distanceToEnemy < shortestDistance)
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

        private void GetAllEnemiesOnRange(GameObject[] enemies)
        {
            _enemiesOnRange.Clear();
            foreach (GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy <= Range)
                    _enemiesOnRange.Add(enemy.GetComponent<Enemy>());
            }
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Range);
        }

		#region Abilities

        public void ActivateAbility()
        {
            if (!_surroundings.Bonus)
                return;
            var type = _surroundings.BonusType;
            switch (type)
            {
                case SurroundingExplorer.BonusTypeEnum.Jaqueira:
                    Poison();
                    break;
                case SurroundingExplorer.BonusTypeEnum.Ipe:
                    GenerateSeed();
                    break;
                case SurroundingExplorer.BonusTypeEnum.Araucaria:
                    Slow();
                    break;
            }
        }

        private void Slow(){
			if(Vector3.Distance(transform.position, _target.position) <= Range)
			{
				_targetEnemy.Slow(SlowAmount);
			}
		}

        private void GenerateSeed()
        {
            _currentSeedGenerationTime -= Time.deltaTime;
            if (_currentSeedGenerationTime <= 0f)
            {
                _currentSeedGenerationTime = SeedGenerationTime;
                MovingSeedsManager.AddSeeds(1);
            }
        }
         
        private void Poison(){
			_poisonedBullet = true;
		}

		#endregion
    }
}
