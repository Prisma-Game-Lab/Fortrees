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
		[HideInInspector] public float RemainingPoisonTime = 0f;
		[HideInInspector] public float CurrentPoisonCooldown = 0f;
		[HideInInspector] public float PoisonDamage;
		[HideInInspector] public float TotalPoisonCooldown;

        private float _health;
        
        [Header("Unity Stuff")]
        public Image HealthBar;

        public void Start()
        {
            _health = StartHealth;
            Speed = StartSpeed;
        }

		public void Update(){
			if (RemainingPoisonTime > 0f)
				Poison ();
		}

        public void TakeDamage(float amount)
        {
            _health -= amount;
            HealthBar.fillAmount = _health / StartHealth;
            if (_health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            WaveSpawner.EnemiesAlive--;
            Destroy(gameObject);
        }

        public void Slow(float slowFactorPercentage)
        {
            Speed = StartSpeed*(1f - slowFactorPercentage);
        }

		public void Poison()
		{
			if(RemainingPoisonTime >= 0f)
			{
				RemainingPoisonTime = RemainingPoisonTime - Time.deltaTime;
				CurrentPoisonCooldown -= Time.deltaTime;

				if (CurrentPoisonCooldown <= 0f) {
					CurrentPoisonCooldown = TotalPoisonCooldown;
					Debug.Log ("POISON");
					TakeDamage(PoisonDamage);
				}
			}
		}
    }
}
