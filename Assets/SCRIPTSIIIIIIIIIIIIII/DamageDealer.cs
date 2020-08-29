using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    //This script returns the damage of the gameObject it is attached to
    //This script Destroys the gameObject it is attached to when called

    //Damage passed on to the Object it Collides with
    [SerializeField] int damage = 100;

    public int GetDamage()
    {
        return damage;
    }

    public void OnHitDestroyOtherObject()
    {
        try
        {
            Destroy(gameObject);
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
}
