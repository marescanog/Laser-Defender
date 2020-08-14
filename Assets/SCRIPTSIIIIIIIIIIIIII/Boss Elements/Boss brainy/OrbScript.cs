using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbScript : MonoBehaviour
{
    [Header("Orb Stats")]
    [SerializeField] int health = 500;
    [SerializeField] int maxHealth = 500;
    [SerializeField] GameObject brainSheildDomeObejct;
    [SerializeField] GameObject orbLoaderObject;
    [SerializeField] GameObject batteryGaugeObject;
    [SerializeField] bool leftOrb;
    [SerializeField] bool rightOrb;

    [Header("VisualFX/SoundFX")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOFExplosion = 1f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;
    [SerializeField] GameObject rechardParticleSystem;

    [Header("Transparency")]
    [SerializeField] Color transparentColor;
    [SerializeField] Color OpaqueColor;

    [Header("Minions")]
    [SerializeField] GameObject leftHandminion;
    [SerializeField] GameObject rightHandminion;

    BrainSheildDome brainSheildDomeScript;
    OrbLoader orbLoaderScript;
    SpriteRenderer mySpriteRenderer;
    BatteryGaugeScript batteryGaugeScript;
    GameSession gameSessionScript;

    float lerpToRedorWhiteCount= 100f;
    [SerializeField] float countToLeftMinionSpawn = 0f;
    [SerializeField] bool countDownForLeftSpawnerOn = true;
    bool delayInstantiateLeftHandMinionOn = false;
    bool is_LeftHandMinion_in_scene = false;

    [SerializeField] float countToRightMinionSpawn = 0f;
    [SerializeField] bool countDownForRightSpawnerOn = true;
    bool delayInstantiateRightHandMinionOn = false;
    bool is_RightHandMinion_in_scene = false;

    // Start is called before the first frame update
    void Start()
    {
        brainSheildDomeScript = brainSheildDomeObejct.GetComponent<BrainSheildDome>();
        orbLoaderScript = orbLoaderObject.GetComponent<OrbLoader>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        batteryGaugeScript = batteryGaugeObject.GetComponent<BatteryGaugeScript>();
        gameSessionScript = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor_On_Hit();

        is_LeftHandMinion_in_scene = gameSessionScript.GetValue_GamesessionBool_is_LeftHandMinion_in_Scene();

        is_RightHandMinion_in_scene = gameSessionScript.GetValue_GamesessionBool_is_RightHandMinion_in_Scene();

        if (leftOrb)
        {
            Spawn_LeftHandMinion();
        }
        else if (rightOrb)
        {
            Spawn_RightHandMinion();
        }

    }

    private void Spawn_RightHandMinion()
    {
        if (!is_RightHandMinion_in_scene)
        {
            if (countDownForRightSpawnerOn)
            {
                if (rightOrb)
                {
                    if (countToRightMinionSpawn <= 5.0f)
                    {
                        countToRightMinionSpawn += Time.deltaTime * 1f;
                        //Debug.Log(countToMinionSpawn);
                        if (countToRightMinionSpawn >= 5.0f)
                        {
                            GameObject particleFX = Instantiate(
                               rechardParticleSystem,
                               transform.position,
                               transform.rotation * Quaternion.Euler(0, -180f, 0f));
                            particleFX.transform.parent = gameObject.transform;
                            Destroy(particleFX, 6.5f);
                            delayInstantiateLeftHandMinionOn = true;
                            StartCoroutine(delayInstantiateRightHandMinion());

                            countDownForRightSpawnerOn = false;
                            is_RightHandMinion_in_scene = true;

                        }
                    }
                }
            }
        }
    }

    private void Spawn_LeftHandMinion()
    {
        if (!is_LeftHandMinion_in_scene)
        {
            if (countDownForLeftSpawnerOn)
            {
                if (leftOrb)
                {
                    if (countToLeftMinionSpawn <= 5.0f)
                    {
                        countToLeftMinionSpawn += Time.deltaTime * 1f;
                        //Debug.Log(countToMinionSpawn);
                        if (countToLeftMinionSpawn >= 5.0f)
                        {
                            GameObject particleFX = Instantiate(
                               rechardParticleSystem,
                               transform.position,
                               transform.rotation * Quaternion.Euler(0, -180f, 0f));
                            particleFX.transform.parent = gameObject.transform;
                            Destroy(particleFX, 6.5f);
                            delayInstantiateLeftHandMinionOn = true;
                            StartCoroutine(delayInstantiateLeftHandMinion());

                            countDownForLeftSpawnerOn = false;
                            is_LeftHandMinion_in_scene = true;
                            /*
                            var newHand = Instantiate(leftHandminion,
                            transform.position,
                            transform.rotation);
                            */

                            //makes the intantiated object a child o the parent gameobject
                            //newHand.transform.parent = gameObject.transform;
                        }
                    }
                }
            }
        }
    }

    private void ChangeColor_On_Hit()
    {
        if (lerpToRedorWhiteCount <= 100.0f)
        {
            lerpToRedorWhiteCount += Time.deltaTime * 50f;
            mySpriteRenderer.color = Color.Lerp(transparentColor, OpaqueColor, (lerpToRedorWhiteCount / 100));
        }
    }

    private IEnumerator delayInstantiateLeftHandMinion()
    {
        yield return new WaitForSeconds(2);
        Instantiate(leftHandminion,
            transform.position,
            transform.rotation);
            delayInstantiateLeftHandMinionOn = false;
        StopCoroutine(delayInstantiateLeftHandMinion());
    }

    private IEnumerator delayInstantiateRightHandMinion()
    {
        yield return new WaitForSeconds(2);
        Instantiate(rightHandminion,
        transform.position,
        transform.rotation);
        delayInstantiateLeftHandMinionOn = false;
        StopCoroutine(delayInstantiateRightHandMinion());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }

        //If collides with main player then does not destroy other object, subtracts damage value from health
        if (other.gameObject.tag == "MainPlayer")
        {
            ProcessHit(damageDealer);
        }
        else if (other.gameObject.tag == "Player Laser")
        {
            ProcessHit(damageDealer);
        }
        else
        {
            Debug.Log("hit");
        }

    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        health = Mathf.Clamp(health, 0, maxHealth);
        //damageDealer.Hit();
        if (health <= 0)
        {

            if (leftOrb)
            {
                brainSheildDomeScript.turnOffLeftOrb();
                brainSheildDomeScript.OrbActiavteCheck();
                orbLoaderScript.SetOrbloaderHealthToZero();
                orbLoaderScript.StartRechargingOrb();

                if(delayInstantiateLeftHandMinionOn)
                {
                    StopCoroutine(delayInstantiateLeftHandMinion());
                    delayInstantiateLeftHandMinionOn = false;
                }

                ///cache this reference?
                if (FindObjectsOfType<HandMinion>().Length == 1)
                {
                    FindObjectOfType<HandMinion>().DestroyLeftHandMinion();
                }
            }

            if (rightOrb)
            {
                brainSheildDomeScript.turnOffRightOrb();
                brainSheildDomeScript.OrbActiavteCheck();
                orbLoaderScript.SetOrbloaderHealthToZero();
                orbLoaderScript.SetOrbloaderHealthToZero();
                orbLoaderScript.StartRechargingOrb();

                if (delayInstantiateRightHandMinionOn)
                {
                    StopCoroutine(delayInstantiateRightHandMinion());
                    delayInstantiateRightHandMinionOn = false;
                }

                ///cache this reference?
                if (FindObjectsOfType<RightHandMinion>().Length == 1)
                {
                    FindObjectOfType<RightHandMinion>().DestroyRightHandMinion();
                }
            }

            Die();
        }
    }

    private void Die()
    {
        GameObject explosion = Instantiate(
           deathVFX,
           transform.position,
           transform.rotation * Quaternion.Euler(180f, 0, 0f));
        Destroy(explosion, durationOFExplosion);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        mySpriteRenderer.color = transparentColor;
        gameObject.SetActive(false);
    }

    public int GetHealth() { return health; }
    public void SetActiveOrb()
    {
        batteryGaugeScript.FillUpHealth();
        health = 2000;
        gameObject.SetActive(true);

        //if Coroutine is on then stop it

        brainSheildDomeScript.ActivateOrbDome();

        if (leftOrb)
        {
            brainSheildDomeScript.turnOnLeftOrb();
            ResetLeftHandSpawnerTimer();
        }

        if (rightOrb)
        {
            brainSheildDomeScript.turnOnRightOrb();
            ResetRightHandSpawnerTimer();
        }

        brainSheildDomeScript.OrbActiavteCheck();

        lerpToRedorWhiteCount = 0f;
    }

    public void ResetLeftHandSpawnerTimer() { countToLeftMinionSpawn = 0; countDownForLeftSpawnerOn = true; }

    public void ResetRightHandSpawnerTimer() { countToRightMinionSpawn = 0; countDownForRightSpawnerOn = true; }
}
