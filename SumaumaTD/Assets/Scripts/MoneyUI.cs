using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts
{
    public class MoneyUI : MonoBehaviour
    {
        //TODO change name
        public Text MoneyText;

        // Update is called once per frame
        public void Update () {
            MoneyText.text = "Seeds:" + PlayerStats.Seeds.ToString();

        }
    }
}
