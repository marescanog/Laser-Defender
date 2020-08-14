using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentEnemy : MonoBehaviour
{
    Transform myTransform;

    private void Start()
    {
        myTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        //locks rotation
        myTransform.eulerAngles = new Vector3(0, 0, 0);


    }
    public void OnHitDestroyParent()
    {
        Destroy(gameObject);
    }
}
