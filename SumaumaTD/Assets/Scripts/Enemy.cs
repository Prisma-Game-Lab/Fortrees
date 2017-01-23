using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour
    {
        //TODO remove unnecessary things
        [HideInInspector]
        public float Speed;
        public float StartSpeed = 10f;
        public float StartHealth = 100;
        private float _health;

        [Header("Value Earned when enemy dies")]
        public int Value = 50;

        [Header("Unity Stuff")]
        public Image HealthBar;

        public void Start()
        {
            _health = StartHealth;
            Speed = StartSpeed;
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
            //PlayerStats.Money += Value;
            Destroy(gameObject);
        }

        public void Slow(float slowFactorPercentage)
        {
            Speed = StartSpeed*(1f - slowFactorPercentage);
        }
    }
}
