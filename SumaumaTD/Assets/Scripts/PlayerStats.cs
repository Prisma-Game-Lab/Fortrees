﻿using UnityEngine;

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
        public int StartLives = 20;

        public static int Waves;

        public void Start ()
        {
            Waves = 0;
            //Money = StartMoney;
            Seeds = StartSeeds;
            Lives = StartLives;
        }

        public void Update ()
        {
	
        }
    }
}
