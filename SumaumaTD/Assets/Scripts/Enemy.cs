using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour
    {
        [HideInInspector]
        public float Speed;
        public float StartSpeed = 10f;
        public float StartHealth = 100;

		private float _remainingPoisonTime = 0f;
		private float _currentPoisonCooldown = 0f;
		private float _poisonDamage;
		private float _totalPoisonCooldown;
        private float _health;
        private bool _isDead = false;

        [Header("Poison")]
        public GameObject PoisonEffect;

        [Header("Unity Stuff")]
        public Image HealthBar;
        public EnemySound sound;

        private bool IsPoisoned
        {
            get { return _remainingPoisonTime > 0f; }
        }

        public void Start()
        {
            _health = StartHealth;
            Speed = StartSpeed;
        }

		public void Update(){
			if (IsPoisoned)
				Poison();
		}

        public void TakeDamage(float amount)
        {
            _health -= amount;
            HealthBar.fillAmount = _health / StartHealth;
            if (_health <= 0)
                Die();
        }

        private void Die()
        {
            if (!_isDead)
            {
                sound.EnemyDied();
                _isDead = true;
                WaveSpawner.EnemiesAlive--;
                Destroy(gameObject);
            }
        }

        public void Slow(float slowFactorPercentage)
        {
            Speed = StartSpeed*(1f - slowFactorPercentage);
        }

        public void Poison()
        {
            _remainingPoisonTime -= Time.deltaTime;
            _currentPoisonCooldown -= Time.deltaTime;
            
            if (_currentPoisonCooldown <= 0f)
            {
                _currentPoisonCooldown = _totalPoisonCooldown;
                GameObject effect =
                    (GameObject)Instantiate(PoisonEffect, transform);
                var temp = transform.position;
                temp.y += 2f;
                effect.transform.position = temp;
                Destroy(effect, _totalPoisonCooldown);
                TakeDamage(_poisonDamage);

            }
        }

        public void TakePoison(float poisonTime, float poisonDamage, float poisonCooldown)
        {
            _remainingPoisonTime = poisonTime;
            _poisonDamage = poisonDamage;
            _totalPoisonCooldown = poisonCooldown;
        }
    }
}
