using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update

    private Transform bar;
    [SerializeField] Player playerHealth;
    float healthValue;
    float healthValueClamped;
    private void Start()
    {
         bar = transform.Find("Bar");
    }
    void Update()
    {
        healthValue = (playerHealth.GetHealth() / 1000.00f);
        //Debug.Log(healthValue);
        healthValueClamped = Mathf.Clamp(healthValue, 0f, 1.0f);
        bar.localScale = new Vector3(healthValueClamped, .9f);
        //transform.Find("Bar").localScale = new Vector3(healthValue, 1f);
        //SetSize(healthValue);


    }

    // public void SetSize (float sizeNormalized)
    // {
    //     bar.localScale = new Vector3(sizeNormalized, 1f);
    // }

}
