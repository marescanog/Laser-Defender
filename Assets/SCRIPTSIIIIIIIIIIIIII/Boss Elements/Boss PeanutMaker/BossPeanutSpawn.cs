using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPeanutSpawn : MonoBehaviour
{
    MeteorSpawnerScript meteorspawnerScript;
    GameObject meteorSpawnerObject;
    GameObject gameSessionObject;
    GameSession gameSessionScript;

    void Start()
    {
        meteorSpawnerObject = FindObjectOfType<MeteorSpawnerScript>().gameObject;
        meteorspawnerScript = meteorSpawnerObject.GetComponent<MeteorSpawnerScript>();
        Add_Boss_and_minions_to_Scene();
    }

    private void Add_Boss_and_minions_to_Scene()
    {
        meteorspawnerScript.Call_MeteorSpawner_To_Enable_Boss_and_Minions();
        Destroy(gameObject);
    }
}


