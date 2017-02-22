using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class EndUI : MonoBehaviour {

        public Sprite TreePlaceholderFullHealth;
        public Sprite TreePlaceholderMidHealth;
        public Sprite TreePlaceholderLowHealth;
        public GameObject TreeSpriteGameObject;
        
        public void Start()
        {
            TreeSpriteGameObject.GetComponent<SpriteRenderer>().sprite = TreePlaceholderFullHealth;
        }

        public void Update()
        {
            
			float healthFillAmount = 1 - PlayerStats.ForestSaturation;

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
