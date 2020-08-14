using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryGaugeScript : MonoBehaviour
{
    // Start is called before the first frame update

    private Transform bar;
    [SerializeField] OrbScript OrbHealth;
    [SerializeField] float maxHealth;
    [SerializeField] float healthValue;
    float healthValueClamped;
    [SerializeField] bool fillUphealth = false;

    [SerializeField] AudioClip rechargeSound;
    bool playSoundOnce = false;
    private void Start()
    {
        bar = transform.Find("Bar");
    }
    void Update()
    {
        if(!fillUphealth)
        {
            healthValue = (OrbHealth.GetHealth() / maxHealth);
            //Debug.Log(healthValue);
        }
        else if (fillUphealth)
        {
            if (healthValue >= 1)
            {
                healthValue = 2000;
                fillUphealth = false;
                playSoundOnce = false;
            }
            else if (healthValue<=1)
            {
                healthValue += Time.deltaTime * 1f;
                if(!playSoundOnce)
                {
                    AudioSource.PlayClipAtPoint(rechargeSound, Camera.main.transform.position, .3f);
                    playSoundOnce = true;
                }
            }
        }

        healthValueClamped = Mathf.Clamp(healthValue, 0f, 1f);
        bar.localScale = new Vector3(healthValueClamped, .9f);
    }

    public void FillUpHealth() { fillUphealth = true; healthValue = 0f; }

}
