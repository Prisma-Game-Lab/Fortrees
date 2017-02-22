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
			WavesSurvivedText.text = PlayerStats.Waves + "/" + WaveSpawner.NumberOfWaves + " waves";

			var wavecount =WaveCountImages.transform.childCount;
			for (; counter < 6; counter++)
				WaveCountImages.transform.GetChild (counter).gameObject.SetActive(true);
			for(; counter <0; counter++)
				WaveCountImages.transform.GetChild (counter).gameObject.SetActive(false);
			
        }
    }
}
