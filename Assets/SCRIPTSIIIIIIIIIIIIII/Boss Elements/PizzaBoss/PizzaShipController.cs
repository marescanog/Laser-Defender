using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaShipController : MonoBehaviour
{
    [SerializeField] GameObject door;
    doorMovement _doorMovement;

    // Start is called before the first frame update
    void Start()
    {
        _doorMovement = door.GetComponent<doorMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //ForDebugging
        if (Input.GetKeyDown(KeyCode.O))
        {
            _doorMovement.GoUP();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            _doorMovement.GoDown();
        }

    }


}
