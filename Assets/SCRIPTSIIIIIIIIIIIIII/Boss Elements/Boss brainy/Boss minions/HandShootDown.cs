using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandShootDown : MonoBehaviour
{
    GameSession gameSessionScript;

    private void Start()
    {
        gameSessionScript = FindObjectOfType<GameSession>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "MainPlayer")
        {
                Debug.Log("Fire Downwards");
                gameSessionScript.Turn_Off_TriggerBool_Fire_Upwards();
        }
    }
}
