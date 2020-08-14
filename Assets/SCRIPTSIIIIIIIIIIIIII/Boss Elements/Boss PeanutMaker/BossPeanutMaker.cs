using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPeanutMaker : MonoBehaviour
{
    public ParticleSystem particlelauncher;
    [SerializeField] ParticleSystem electricCharge;
    [SerializeField] ParticleSystem painCharge;
    [SerializeField] GameObject peanutCharge;
    [SerializeField] GameObject locationPeanutCharge;
    [SerializeField] GameObject expldPnut;
    [SerializeField] GameObject bossDeathVfX;
    [SerializeField] GameObject finalDeathBoom;
    [SerializeField] int collectedFrozenMeteors = 0; //serialized for debugging
    EnemyPathing myPathing;
    bool launchLaser = false;

    [SerializeField] float laserTimer = 0;//serialized for debugging
    [SerializeField] List<GameObject> icePeanut;
    GameObject thePlayer;
    List<PeanutScript> icePeanutScript;
    Player playerscript;
    Enemy enemyScript;
    SpriteRenderer mySpriteRenderer;
    [SerializeField] Animator eyeAnimations;

    AudioSource bossPeanutMakerAudiosource;
    bool touchedFirstPoint = false;
    [SerializeField] AudioClip hitPainSFX;
    [SerializeField] AudioClip freezeBeamSFX;
    [SerializeField] AudioClip bossChirpSFX;
    [SerializeField] AudioClip makepeanutSFX;
    bool isShootingLaser = false;
    bool delayNextPainAnimation = false;
    int peanutNumber = 0;
    int painCount = 0;
    bool isSpawningPeanut = false;
    bool isDying = false;
    List<GameObject> explodePeanutList = new List<GameObject>();

    GameObject meteorSpawner;
    MeteorSpawnerScript meteorSpawnerScript;



    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(-4.45f, 8.91f);
        meteorSpawner = FindObjectOfType<MeteorSpawnerScript>().gameObject;
        meteorSpawnerScript = meteorSpawner.GetComponent<MeteorSpawnerScript>();
        meteorSpawnerScript.MakeSpawnRateFaster();

        isDying = false;
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        enemyScript = GetComponent<Enemy>();
        thePlayer = FindObjectOfType<Player>().gameObject;
        playerscript = thePlayer.GetComponent<Player>();
        bossPeanutMakerAudiosource = GetComponent<AudioSource>();
        myPathing = gameObject.GetComponent<EnemyPathing>();
        launchLaser = false;

        //cache
        var chikenNuggets = new List<PeanutScript>();
        for (int i = 0; i < icePeanut.Count; i++)
        {
            chikenNuggets.Add(icePeanut[i].GetComponent<PeanutScript>());
        }
        icePeanutScript = chikenNuggets;
        mySpriteRenderer.enabled = false;
        StartCoroutine(DelayEnableSpriteRenderer());
    }

    private IEnumerator DelayEnableSpriteRenderer()
    {
        yield return new WaitForSeconds(0.75f);
        mySpriteRenderer.enabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(isDying)
        { eyeAnimations.SetBool("hit", true); StopCoroutine(Stop_EyePain_Anim()); }

        if (launchLaser)
        {
            particlelauncher.Emit(1);
            laserTimer += Time.deltaTime * 1f;
            LaserBeamSmoothEmission();
        }

        //BackUp In case object does not reach point, it will deactivate the laser
        //After 25 seconds so that the laser does not turn on forever
        if (laserTimer>=25)
        {
            launchLaser = false;
            ParticleSystem.MainModule m = particlelauncher.main;
            m.maxParticles = 200;
            touchedFirstPoint = false;
            laserTimer = 0;
            for (int i = 0; i < icePeanutScript.Count; i++)
            {
                icePeanutScript[i].DeActivate_Peanut_Second_PathLoop();
            }
        }
    }

    public void Start_EyePain_Anim() 
    { 
        if (!isDying)
        {
            if (!isShootingLaser)
            {
                if (!delayNextPainAnimation)
                {
                    isSpawningPeanut = true;
                    eyeAnimations.SetBool("hit", true); StartCoroutine(Stop_EyePain_Anim());
                    bossPeanutMakerAudiosource.clip = hitPainSFX;
                    bossPeanutMakerAudiosource.volume = .25f;
                    bossPeanutMakerAudiosource.Play();
                    delayNextPainAnimation = true;
                    painCharge.Emit(3);
                    painCount++;
                }
            }
        }

    }

    public IEnumerator Stop_EyePain_Anim() 
    { yield return new WaitForSeconds(2f); 
        eyeAnimations.SetBool("hit", false);
      yield return new WaitForSeconds (0.5f);
        if((painCount>=3)&(!isDying))
        {
            enemyScript.turnOnThisEnemiesInvincibility();
            eyeAnimations.SetBool("generatePeanut", true);
            yield return new WaitForSeconds(0.25f);
            bossPeanutMakerAudiosource.clip = bossChirpSFX;
            bossPeanutMakerAudiosource.volume = .75f;
            bossPeanutMakerAudiosource.Play();
            yield return new WaitForSeconds(0.5f);
            var peanutRingVFX = Instantiate(peanutCharge, locationPeanutCharge.transform.position, transform.rotation);
            peanutRingVFX.transform.parent = locationPeanutCharge.transform;
            Destroy(peanutRingVFX, 7f);
            yield return new WaitForSeconds(1f);
            bossPeanutMakerAudiosource.clip = makepeanutSFX;
            bossPeanutMakerAudiosource.volume = .75f;
            bossPeanutMakerAudiosource.Play();
            var expldPnutObj = Instantiate(expldPnut, locationPeanutCharge.transform.position, transform.rotation);
            expldPnutObj.name = "ExpldPnut" + peanutNumber.ToString();
            peanutNumber++;
            expldPnutObj.transform.parent = locationPeanutCharge.transform;
            yield return new WaitForSeconds(7f);
            eyeAnimations.SetBool("generatePeanut", false);
            delayNextPainAnimation = false;
            isSpawningPeanut = false;
            painCount = 0;
            enemyScript.turnOffThisEnemiesInvincibility();
            if (collectedFrozenMeteors >= 5)
            {
                if(!isDying)
                {
                    ActivateLaserSequence();
                }
                
            }
        }
        else
        {
            isSpawningPeanut = false;
            delayNextPainAnimation = false;
            enemyScript.turnOffThisEnemiesInvincibility();
        }

    }

    public void Stop_Freeze_Laser_Bream()
    {
        enemyScript.turnOffThisEnemiesInvincibility();
        launchLaser = false;
        ParticleSystem.MainModule m = particlelauncher.main;
        m.maxParticles = 200;
        touchedFirstPoint = false;
        laserTimer = 0;

        for (int i = 0; i < icePeanutScript.Count; i++)
        {
            icePeanutScript[i].DeActivate_Peanut_Second_PathLoop();
        }
    }

    public void Add_One_FTMeteor_to_Collection()
    {
        collectedFrozenMeteors++;

        electricCharge.Emit(30);

        if (!isSpawningPeanut)
        {
            if (collectedFrozenMeteors >= 5)
            {
                if(!isDying)
                {
                    ActivateLaserSequence();
                }

            }
        }
    }

    private void ActivateLaserSequence()
    {
        isShootingLaser = true;
        StartCoroutine(ActivateLaser());
        myPathing.PlaySecondPathLoop_Method();
        collectedFrozenMeteors = collectedFrozenMeteors - 5;
        for (int i = 0; i < icePeanutScript.Count; i++)
        {
            icePeanutScript[i].Activate_Peanut_Second_PathLoop();
        }
    }

    //This Coroutine doesn't activate Laser until it reaches Point 1.
    IEnumerator ActivateLaser()
    {
        yield return new WaitUntil(() => touchedFirstPoint == true);
        launchLaser = true;
        enemyScript.turnOnThisEnemiesInvincibility();
    }

    public void True_FirstPoint_Bool() {touchedFirstPoint = true;}
    private void LaserBeamSmoothEmission()
    {
        if ((laserTimer >= 0.5f) & (laserTimer <= 1))
        {
            ParticleSystem.MainModule m = particlelauncher.main;
            m.maxParticles = 300;
        }
        else if ((laserTimer >= 1) & (laserTimer <= 1.5f))
        {
            ParticleSystem.MainModule m = particlelauncher.main;
            m.maxParticles = 400;
        }
        else if ((laserTimer >= 2) & (laserTimer <= 2.5f))
        {
            ParticleSystem.MainModule m = particlelauncher.main;
            m.maxParticles = 550;
        }
        else if ((laserTimer >= 3) & (laserTimer <= 3.5f))
        {
            ParticleSystem.MainModule m = particlelauncher.main;
            m.maxParticles = 600;
        }
        else if ((laserTimer >= 4) & (laserTimer <= 4.5f))
        {
            ParticleSystem.MainModule m = particlelauncher.main;
            m.maxParticles = 575;
        }
        else if ((laserTimer >= 9) & (laserTimer <= 10))
        {
            ParticleSystem.MainModule m = particlelauncher.main;
            m.maxParticles = 550;
        }
        else if ((laserTimer >= 11) & (laserTimer <= 12))
        {
            ParticleSystem.MainModule m = particlelauncher.main;
            m.maxParticles = 525;
        }
        else if ((laserTimer >= 13) & (laserTimer <= 14))
        {
            ParticleSystem.MainModule m = particlelauncher.main;
            m.maxParticles = 500;
        }
        else if ((laserTimer >= 15) & (laserTimer <= 16))
        {
            ParticleSystem.MainModule m = particlelauncher.main;
            m.maxParticles = 475;
        }
        else if ((laserTimer >= 17) & (laserTimer <= 18))
        {
            ParticleSystem.MainModule m = particlelauncher.main;
            m.maxParticles = 455;
        }
        else if ((laserTimer >= 19) & (laserTimer <= 20))
        {
            ParticleSystem.MainModule m = particlelauncher.main;
            m.maxParticles = 425;
        }
        else if ((laserTimer >= 21) & (laserTimer <= 22))
        {
            ParticleSystem.MainModule m = particlelauncher.main;
            m.maxParticles = 400;
        }
    }

    public void Play_Laser_AudioSource()
    {
        bossPeanutMakerAudiosource.clip = freezeBeamSFX;
        bossPeanutMakerAudiosource.volume = 1f;
        bossPeanutMakerAudiosource.Play();
    }

    public void Stop_Laser_AudioSource()
    {
        bossPeanutMakerAudiosource.Stop();
        isShootingLaser = false;
    }

    public void Start_BossPeanut_Death_Sequence()
    {
        isDying = true;
        meteorSpawnerScript.StopSpawnRate();

        for (int i = 0; i < icePeanut.Count; i++)
        {
            icePeanutScript[i].Activate_Peanut_Death_Sequence();
        }
        eyeAnimations.SetBool("hit", true);

        StartCoroutine(Start_Series_Of_Explosions());

        playerscript.DestroyIceCubeDuetoWin();

        if(explodePeanutList.Count==0)
        { }
        else
        {
            for (int i = 0; i < explodePeanutList.Count; i++)
            {
                ExplPnutScript itsScript = explodePeanutList[0].GetComponent < ExplPnutScript > ();
                StartCoroutine(itsScript.ExplodeThisObject());
            }
        }
    }

    private IEnumerator Start_Series_Of_Explosions()
    {
        Instantiate_Explotion_thenDestroy();
        yield return new WaitForSeconds(Random.Range(.5f,1));
        Instantiate_Explotion_thenDestroy();
        yield return new WaitForSeconds(Random.Range(.5f, 1));
        Instantiate_Explotion_thenDestroy();
        yield return new WaitForSeconds(Random.Range(.5f, 1));
        Instantiate_Explotion_thenDestroy();
        yield return new WaitForSeconds(Random.Range(.5f, 1));
        Instantiate_Explotion_thenDestroy();
        yield return new WaitForSeconds(Random.Range(.5f, 1));
        yield return new WaitForSeconds(1);
        mySpriteRenderer.enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(10).gameObject.SetActive(false);
        GameObject explosion2 = Instantiate(finalDeathBoom, transform.position, transform.rotation) as GameObject;
        Destroy(explosion2, 5);
        yield return new WaitForSeconds(5);
        GameObject gameSession = FindObjectOfType<GameSession>().gameObject;
        GameSession gameSessionScript = gameSession.GetComponent<GameSession>();
        gameSessionScript.AddDestroyEnemies();
        gameObject.SetActive(false);
    }

    private void Instantiate_Explotion_thenDestroy()
    {
        float xPlus = Random.Range(-1.75f, 1.75f);
        float yPlus = Random.Range(-1.75f, 1.75f);
        var positionNew = new Vector3(transform.position.x+ xPlus, transform.position.y+ yPlus, transform.position.z);
        GameObject explosion1 = Instantiate(bossDeathVfX, positionNew, transform.rotation) as GameObject;
        float randomNumber = Random.Range(-1.5f, 1.5f);
        var scaleChange = new Vector3(1, 1, 1f);
        explosion1.transform.localScale = scaleChange * randomNumber;
        Destroy(explosion1, 2);
    }


    public void AddExplodePeanut(GameObject peanutObject)
    {
        
        if (explodePeanutList.Contains(peanutObject))
        { }
        else
        {
            explodePeanutList.Add(peanutObject);
        }
        
    }

    public void RemoveExplodePeanut(GameObject peanutObject)
    {
        if (explodePeanutList.Contains(peanutObject))
        { explodePeanutList.Remove(peanutObject); }

    }

    public void Enable_Peanut_Boss()
    {
        gameObject.SetActive(true);
        StopCoroutine(Stop_EyePain_Anim());
    }
}
