using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PlayerStats : MonoBehaviour
    {
        public static int Seeds;
        public int StartSeeds = 3;
        public static int Lives;
        public int StartLives = 20;
        public static float ForestSaturation =1 ;

        [HideInInspector]
        public static int TotalLives;
        public static int Waves;
        [HideInInspector]
        public static int PlantedTrees =0 ;
        public void Start ()
        {
            Waves = 0;
            Seeds = StartSeeds;
            Lives = StartLives;
            TotalLives = StartLives;
        }

		public void Update()
		{
			var healthFillAmount = PlayerStats.Lives/(float) PlayerStats.TotalLives;

			ForestSaturation = (0.5f*healthFillAmount + 0.5f);
		}
        
    }
}
