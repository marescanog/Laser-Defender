using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbLoader : MonoBehaviour
{

    private Transform bar;
    [SerializeField] float orbHealth;
    [SerializeField] float maxHealth;
    float healthValue;
    float healthValueClamped;
    [SerializeField] float rechargeSpeed = 50f;
    bool startRecharge = false;
    [SerializeField] GameObject orbObject;
    OrbScript theOrbScript;

    private void Start()
    {
        bar = transform.Find("Bar");
        theOrbScript = orbObject.GetComponent<OrbScript>();
    }

    void Update()
    {
        if (startRecharge)
        {
            orbHealth += rechargeSpeed * Time.deltaTime;
            healthValue = (orbHealth / maxHealth);
            healthValueClamped = Mathf.Clamp(healthValue, 0f, 1f);
            bar.localScale = new Vector3(healthValueClamped, .9f);
            if (orbHealth >= maxHealth)
            {
                orbHealth = maxHealth;
                startRecharge = false;
                theOrbScript.SetActiveOrb();
            }
        }
    }

    public void StartRechargingOrb(){startRecharge = true;}
    public void SetOrbloaderHealthToZero() { orbHealth = 0; }

}
