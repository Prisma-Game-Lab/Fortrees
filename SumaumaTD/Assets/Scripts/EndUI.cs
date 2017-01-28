using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class EndUI : MonoBehaviour {

        public GameObject BaseColor;
        private Color _startColor;

        [Header("UnityStuff")]
        public Image HealthBar;

        public void Start()
        {
            _startColor = BaseColor.GetComponent<Renderer>().material.color;
        }

        public void Update()
        {
            HealthBar.fillAmount = (float)PlayerStats.Lives / (float)PlayerStats.TotalLives; //pega a barra de vida do script PlayerStats e a diminui como no tutorial
            
            if (HealthBar.fillAmount <= 0.6f && HealthBar.fillAmount >= 0.3f)
            {
                BaseColor.GetComponent<Renderer>().material.color = Color.blue;
            }

            else if (HealthBar.fillAmount < 0.3f)
            {
                BaseColor.GetComponent<Renderer>().material.color = Color.grey;
            }

            else
            {
                BaseColor.GetComponent<Renderer>().material.color = _startColor;
            }
        }
    }
}
