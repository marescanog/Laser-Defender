using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerByPlayerCrossChangeEnemyShotDirection : MonoBehaviour
{
    GameSession gameSessionScript;
    bool enemy_fireUpwards = false;

    private void Start()
    {
        gameSessionScript = FindObjectOfType<GameSession>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "MainPlayer")
        {
            if(!enemy_fireUpwards)
            {
                Debug.Log("Fire Upwards");
                gameSessionScript.Turn_On_TriggerBool_Fire_Upwards();
                enemy_fireUpwards = true;
            }
            else 
            {
                Debug.Log("Fire Downwards");
                gameSessionScript.Turn_Off_TriggerBool_Fire_Upwards();
                enemy_fireUpwards = false;
            }
        }
    }
}
