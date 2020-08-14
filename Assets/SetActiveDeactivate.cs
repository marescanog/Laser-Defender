using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveDeactivate : MonoBehaviour
{
    public void SetActiveObject()
    {
        gameObject.SetActive(true);
    }

    public void DontSetActiveObject()
    {
        gameObject.SetActive(false);
    }
}
