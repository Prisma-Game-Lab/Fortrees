using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class BaseHealthBar : MonoBehaviour {

        public Image HealthBar;

        public void Update()
        {
            HealthBar.fillAmount = (float) PlayerStats.Lives/(float) PlayerStats.TotalLives;
        }
    }
}
