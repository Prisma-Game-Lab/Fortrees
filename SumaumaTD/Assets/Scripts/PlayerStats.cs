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

        [HideInInspector]
        public static int TotalLives;

        public static int Waves;

        public void Start ()
        {
            Waves = 0;
            Seeds = StartSeeds;
            Lives = StartLives;
            TotalLives = StartLives;
        }

        
    }
}
