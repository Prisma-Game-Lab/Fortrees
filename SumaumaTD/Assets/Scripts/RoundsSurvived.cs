using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Scripts
{
    public class RoundsSurvived : MonoBehaviour
    {
		public GameObject WaveCountImages;

        public void Update()
        {
			int counter ;
			var wavecount =WaveCountImages.transform.childCount;
			for (counter = 0; counter < PlayerStats.Waves; counter++)
				SetImageAlpha(counter, true);
			for(; counter < wavecount; counter++)
                SetImageAlpha(counter, false);
        }

        private void SetImageAlpha(int counter, bool visible)
        {
            var temp = WaveCountImages.transform.GetChild(counter).gameObject.GetComponent<Image>();
            temp.color = new Color(temp.color.r, temp.color.g, temp.color.b, visible ? 1 : 0);
        }
    }
}
