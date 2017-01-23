using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts
{
    public class MoneyUI : MonoBehaviour
    {
        public Text MoneyText;

        // Update is called once per frame
        public void Update () {
            MoneyText.text = "$" + PlayerStats.Money.ToString();

        }
    }
}
