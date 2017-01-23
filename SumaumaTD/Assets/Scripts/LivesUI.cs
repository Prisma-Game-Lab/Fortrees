using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts
{
    public class LivesUI : MonoBehaviour {

        public Text LivesText;

        // Update is called once per frame
        public void Update()
        {
            LivesText.text = PlayerStats.Lives + "Hp";

        }
    }
}
