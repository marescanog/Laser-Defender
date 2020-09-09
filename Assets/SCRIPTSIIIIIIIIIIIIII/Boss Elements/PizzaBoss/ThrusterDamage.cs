using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterDamage : MonoBehaviour
{
    [SerializeField] ThrusterMonitoring thrusterMonitor;
    [SerializeField] GameObject origThrusterSprite;
    [SerializeField] GameObject myParticleSystem;
    [SerializeField] int health = 25;
    PolygonCollider2D myCollider;
    GunRotationPattern myRotationScript;
    int maxHealth;
    bool isDamaged = false;

// Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<PolygonCollider2D>();
        myRotationScript = GetComponent<GunRotationPattern>();
        maxHealth = health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player Laser")
        {
            Debug.Log("Hit");
            health -= 1;
            CheckHealth();
        }
          
    }

    private void CheckHealth()
    {
        if (health <= 0)
        {
            ShowDamagedThruster();
        }
    }

    private void ShowDamagedThruster()
    {
        origThrusterSprite.SetActive(false);
        myParticleSystem.SetActive(false);
        myCollider.enabled = false;
        myRotationScript.Disabled();
        isDamaged = true;
    }

    private void ShowRepaiedThruster()
    {
        origThrusterSprite.SetActive(true);
        myParticleSystem.SetActive(true);
        myCollider.enabled = true;
        myRotationScript.Enabled();
        health = maxHealth;
        isDamaged = false;
    }

    public bool GetDamageStatus()
    {
        return isDamaged;
    }
}
