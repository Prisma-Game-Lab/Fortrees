using UnityEngine;

namespace Assets.Scripts
{
    public class ShowHealUI : MonoBehaviour
    {

        public GameObject HealUI;

        public void Start()
        {
            HealUI.SetActive(false);
        }

        public void Update () {
		    if(PlayerStats.Lives < PlayerStats.TotalLives)
                HealUI.SetActive(true);
		    else
		        HealUI.SetActive(false);
        }
    }
}
