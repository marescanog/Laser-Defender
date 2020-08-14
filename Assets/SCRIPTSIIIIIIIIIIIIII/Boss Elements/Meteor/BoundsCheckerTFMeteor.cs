using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheckerTFMeteor : MonoBehaviour
{
    [SerializeField] GameObject meteorSpawnerObject;
    MeteorSpawnerScript meteorSpawnerScript;

    private void Start()
    {
        //cache some variables
        meteorSpawnerScript = meteorSpawnerObject.GetComponent<MeteorSpawnerScript>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch(other.gameObject.tag)
        {
            case "tinyFrozenMeteor":
                meteorSpawnerScript.Check_if_frozenTinyMeteor_isWithinBounds(other.gameObject);
                break;
            default:
                break;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "tinyFrozenMeteor":
                meteorSpawnerScript.Check_if_frozenTinyMeteor_isWithinBounds(other.gameObject);
                break;
            default:
                break;
        }
    }

}
