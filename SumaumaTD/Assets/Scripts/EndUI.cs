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
        
        public void Start()
        {
            _startColor = TreeSpriteGameObject.GetComponent<SpriteRenderer>().color;
            TreeSpriteGameObject.GetComponent<SpriteRenderer>().sprite = TreePlaceholderFullHealth;
        }

        public void Update()
        {
            float h, s, v;

            var healthFillAmount = PlayerStats.Lives/(float) PlayerStats.TotalLives;

            Color.RGBToHSV(_startColor, out h, out s, out v);

            s = 1 - healthFillAmount;

            TreeSpriteGameObject.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(h, s, v);
            
            if (healthFillAmount <= 0.6f && healthFillAmount >= 0.3f)//mid
            {
                TreeSpriteGameObject.GetComponent<SpriteRenderer>().sprite = TreePlaceholderMidHealth;
            }

            else if (healthFillAmount < 0.3f)//low
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
