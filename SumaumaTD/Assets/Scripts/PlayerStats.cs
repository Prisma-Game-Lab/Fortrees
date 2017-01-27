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
		public GameObject BaseColor;
		private Color StartColor;

        public static int Waves;

        [Header("UnityStuff")] public Image HealthBar;

        public void Start ()
        {
            Waves = 0;
            //Money = StartMoney;
            Seeds = StartSeeds;
            Lives = StartLives;
            TotalLives = StartLives; //start
			StartColor = BaseColor.GetComponent<Renderer> ().material.color;
        }

        public void Update ()
        {
			if (HealthBar.fillAmount <= 0.6f && HealthBar.fillAmount >= 0.3f) 
			{
				BaseColor.GetComponent<Renderer> ().material.color = Color.blue;
			} 

			else if (HealthBar.fillAmount < 0.3f) 
			{
				BaseColor.GetComponent<Renderer> ().material.color = Color.grey;
			}

			else 
			{
				BaseColor.GetComponent<Renderer> ().material.color = StartColor;
			}
        }
    }
}
