using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class EndUI : MonoBehaviour {

        public Sprite treePlaceholderFullHealth;
        public Sprite treePlaceholderMidHealth;
        public Sprite treePlaceholderLowHealth;
        public GameObject TreeSprites;
        public GameObject BaseColor;
        private Color _startColor;

        [Header("UnityStuff")]
        public Image HealthBar;

        public void Start()
        {
            _startColor = TreeSprites.GetComponent<SpriteRenderer>().color;
            TreeSprites.GetComponent<SpriteRenderer>().sprite = treePlaceholderFullHealth;

        }

        public void Update()
        {
            float h, s, v;

            HealthBar.fillAmount = (float)PlayerStats.Lives / (float)PlayerStats.TotalLives; //pega a barra de vida do script PlayerStats e a diminui como no tutorial

            Color.RGBToHSV(_startColor, out h, out s, out v);

            s = 1 - HealthBar.fillAmount;

            TreeSprites.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(h, s, v);
            
            if (HealthBar.fillAmount <= 0.6f && HealthBar.fillAmount >= 0.3f)//mid
            {
                TreeSprites.GetComponent<SpriteRenderer>().sprite = treePlaceholderMidHealth;
            }

            else if (HealthBar.fillAmount < 0.3f)//low
            {
                TreeSprites.GetComponent<SpriteRenderer>().sprite = treePlaceholderLowHealth;
            }

            else//full
            {
                TreeSprites.GetComponent<SpriteRenderer>().sprite = treePlaceholderFullHealth;
            }
        }
    }
}
