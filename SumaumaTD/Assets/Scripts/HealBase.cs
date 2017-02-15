using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class HealBase : MonoBehaviour
{

    private PlayerStats PlayerStats;

    void Awake()
    {
        PlayerStats = GetComponent<PlayerStats>();
    }
	
	void Start () {
	
	}
	
	
	void Update () 
    {
	    if (Input.GetKeyDown("h") && PlayerStats.Seeds >= 5)
	    {
	        PlayerStats.Seeds = PlayerStats.Seeds - 5;
            PlayerStats.Lives = PlayerStats.Lives + (int)Math.Round(0.2 * PlayerStats.StartLives, MidpointRounding.AwayFromZero); //arredondando pra cima, já que estamos lidando com ints
            //PlayerStats.Lives = PlayerStats.Lives + (int)Math.Round(0.2 * PlayerStats.StartLives); //arredondando pra baixo
	    }

    }
}
