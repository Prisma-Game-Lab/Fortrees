using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Scripts
{
    public class RoundsSurvived : MonoBehaviour
    {

        public Text WavesSurvivedText;

        public void OnEnable()
        {
            WavesSurvivedText.text = PlayerStats.Waves.ToString();
        }
    }
}
