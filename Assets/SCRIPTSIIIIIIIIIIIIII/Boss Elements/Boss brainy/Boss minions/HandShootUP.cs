using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandShootUP : MonoBehaviour
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
                Debug.Log("Fire Upwards");
                gameSessionScript.Turn_On_TriggerBool_Fire_Upwards();
        }
    }
}
