using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheildHealthBarScript : MonoBehaviour
{
    private Transform bar;
    [SerializeField] SheildScript playerHealth;
    float healthValue;
    float healthValueClamped;
    private void Start()
    {
        bar = transform.Find("Bar");
    }
    void Update()
    {
        healthValue = (playerHealth.GetSheildHealth() / 1000.00f);
        //Debug.Log(healthValue);
        healthValueClamped = Mathf.Clamp(healthValue, 0f, 1.0f);
        bar.localScale = new Vector3(healthValueClamped, .9f);
    }

    public void DontSetActiveSheildHealthBar()
    {
        gameObject.SetActive(false);
    }

    public void SetActiveSheildHealthBar()
    {
        gameObject.SetActive(true);
    }


}
