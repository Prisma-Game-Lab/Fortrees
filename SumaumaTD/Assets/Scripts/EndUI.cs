using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class EndUI : MonoBehaviour {

        public Sprite TreePlaceholderFullHealth;
        public Sprite TreePlaceholderMidHealth;
        public Sprite TreePlaceholderLowHealth;
        public GameObject TreeSpriteGameObject;
        private Color _startColor;

        [Header("UnityStuff")]
        public Image HealthBar;

        public void Start()
        {
            _startColor = TreeSpriteGameObject.GetComponent<SpriteRenderer>().color;
            TreeSpriteGameObject.GetComponent<SpriteRenderer>().sprite = TreePlaceholderFullHealth;

        }

        public void Update()
        {
            float h, s, v;

            HealthBar.fillAmount = (float)PlayerStats.Lives / (float)PlayerStats.TotalLives; //pega a barra de vida do script PlayerStats e a diminui como no tutorial

            Color.RGBToHSV(_startColor, out h, out s, out v);

            s = 1 - HealthBar.fillAmount;

            TreeSpriteGameObject.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(h, s, v);
            
            if (HealthBar.fillAmount <= 0.6f && HealthBar.fillAmount >= 0.3f)//mid
            {
                TreeSpriteGameObject.GetComponent<SpriteRenderer>().sprite = TreePlaceholderMidHealth;
            }

            else if (HealthBar.fillAmount < 0.3f)//low
            {
                TreeSpriteGameObject.GetComponent<SpriteRenderer>().sprite = TreePlaceholderLowHealth;
            }

            else//full
            {
                TreeSpriteGameObject.GetComponent<SpriteRenderer>().sprite = TreePlaceholderFullHealth;
            }
        }
    }
}
