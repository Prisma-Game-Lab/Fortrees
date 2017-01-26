using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PlayerStats : MonoBehaviour
    {
        //TODO remove unnecessary things
        //public static int Money;
        //public int StartMoney = 400;
        public static int Seeds;
        public int StartSeeds = 3;
        public static int Lives;
        public static int TotalLives; //variavel criada para ser usada no script EnemyMovement
        public int StartLives = 20;

        public static int Waves;

        [Header("UnityStuff")] public Image HealthBar;

        public void Start ()
        {
            Waves = 0;
            //Money = StartMoney;
            Seeds = StartSeeds;
            Lives = StartLives;
            TotalLives = StartLives; //start
        }

        public void Update ()
        {
	
        }
    }
}
