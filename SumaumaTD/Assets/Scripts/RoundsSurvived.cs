using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Scripts
{
    public class RoundsSurvived : MonoBehaviour
    {

        public Text WavesSurvivedText;
		public GameObject WaveCountImages;

        public void OnEnable()
        {
			var counter =0;
			WavesSurvivedText.text = PlayerStats.Waves + "/" + WaveSpawner.NumberOfWaves + " waves survived";

			var wavecount =WaveCountImages.transform.childCount;
			for (; counter < PlayerStats.Waves; counter++)
				WaveCountImages.transform.GetChild (counter).gameObject.SetActive(true);
			for(; counter <wavecount; counter++)
				WaveCountImages.transform.GetChild (counter).gameObject.SetActive(false);
        }
    }
}
