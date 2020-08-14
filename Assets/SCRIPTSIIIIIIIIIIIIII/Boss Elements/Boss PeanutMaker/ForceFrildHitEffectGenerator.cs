using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFrildHitEffectGenerator : MonoBehaviour
{
    [SerializeField] GameObject feildHitVFX;
    [SerializeField] GameObject parentObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.gameObject.layer)
        {
            case 14: //Meteor Layer
                GameObject smoke = Instantiate(
                    feildHitVFX,
                    transform.position,
                    transform.rotation * Quaternion.Euler(0, -180f, 0f));
                smoke.transform.parent = parentObject.transform;
                Destroy(smoke, 2f);
                break;
            default:
                //do nothing
                break;
        }

    }
}
