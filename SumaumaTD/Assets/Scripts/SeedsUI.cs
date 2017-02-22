using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class SeedsUI : MonoBehaviour
    {
        //TODO change name
        public Text SeedsCountdownText;


        // Update is called once per frame
        public void Update () {
            SeedsCountdownText.text = "" + PlayerStats.Seeds;
        }
    }
}
