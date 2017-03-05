using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class HealBase : MonoBehaviour
    {
        public void Update () 
        {
            if (Input.GetButtonDown("HealButton") && PlayerStats.Seeds >= 5 && PlayerStats.Lives < PlayerStats.TotalLives)
            {
                PlayerStats.Seeds = PlayerStats.Seeds - 5;
                PlayerStats.Lives = PlayerStats.Lives + (int)Math.Round(0.2 * PlayerStats.TotalLives, MidpointRounding.AwayFromZero); //arredondando pra cima, já que estamos lidando com ints
            }

        }
    }
}
