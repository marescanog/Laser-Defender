using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // config parameters
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [SerializeField] int health = 200;
    [SerializeField] private GameObject deathVFX;
    [SerializeField] float durationOFHitExplosion = 1f;
    [SerializeField] GameObject laserBombHitVFX;
    [SerializeField] GameObject bombHitVFX;
    [SerializeField] GameObject laserHitVFX;
    [SerializeField] Color semiTransparent;

    [Header("Projectile/VFX")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.1f;
    [SerializeField] GameObject rocketPrefab;
    [SerializeField] GameObject iceCube;
    [SerializeField] GameObject iceSmokeVFX;
    [SerializeField] GameObject iceBlastShardEffect;
    [SerializeField] GameObject iceBlastShardEffect2;

    [Header("Sounds")]
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.75f;
    [SerializeField] AudioClip getHitLaserSound;
    [SerializeField] AudioClip getHitBombSound;
    [SerializeField] AudioClip getHitLaserBombSound;
    [SerializeField] [Range(0, 1)] float getHitSoundVolume = 0.75f;
    [SerializeField] AudioClip sheildHealthUPSFX;
    [SerializeField] AudioClip heavyMachineGunSFX;
    [SerializeField] AudioClip rocketLauncherSFX;
    [SerializeField] AudioClip rocketpressSFX;
    [SerializeField] AudioClip CountDownSFX;
    [SerializeField] AudioClip iceBlastSound;
    [SerializeField] AudioClip iceBlastShatter;
    [SerializeField] List<AudioClip> IceCrack;

    [Header("GameSession")]
    [SerializeField] GameObject sheild;
    [SerializeField] GameObject sheildHealthBar;
    [SerializeField] GameObject rocketHead;
    [SerializeField] GameObject gameSession;
    [SerializeField] GameObject heavyMachineGunUI;

    [Header("Tracker")]//For Debugging Purposes
    [SerializeField] int particlehit = 0;

    int numberofRockets = 0;
    [SerializeField] int levelNumber = 1;

    bool isFrozenFromLaser = false;
    bool lockPlayerMovement;
    bool playCountDownSound = false;
    bool sheildOn = false;
    bool immunityOn = false;
    bool isPlayerObjectMoveFoward;
    bool machineGunCountDownOn;
    [SerializeField] bool isPlayerActive = true; //serialized for debugging purposes

    float xMin;
    float xMax;
    float yMin;
    float yMax;
    float lerpToRedorWhiteCount = 100f;
    float rocketTime = 0f;

    SheildHealthBarScript sheildHealthBarScript;
    SheildScript sheildScript;
    SetActiveDeactivate rocketHeadScript;
    SetActiveDeactivate setActiveDeactiveHeavyMachineGunUI;
    GameSession gameSessionScript;
    Vector2 playerPos;
    GameObject hitVFX;
    Coroutine firingCourotine;
    SpriteRenderer mySpriteRenderer;
    AudioClip getHitSound;
    MusicPlayer musicPlayer;
    Color myColor = Color.red;
    bool laserBomb = false;
    [SerializeField] float valueNeededToFreezePlayer = 150;

    bool freezePlayerInPosAfterFreezeLazerHit = false;
    bool stopAbsorbingParticles = false;
    [SerializeField] float secondsFrozen = 0; //serialized for debugging purposes
    Vector3 myposition;
    bool lockItUpToJitter = false;
    [SerializeField] bool fireButtonIsHeldDown = false; //serialized for debugging purposes

    GameObject myNewIceCubeFreezeObject, smokeVFXFreezeObject, VFX1FreezeObject;
    DamageDealer potato = new DamageDealer();
    bool bActivatedSheild = false;


    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
        //Initializing Cached References
        sheildHealthBarScript = sheildHealthBar.GetComponent<SheildHealthBarScript>();
        sheildScript = sheild.GetComponent<SheildScript>();
        rocketHeadScript = rocketHead.GetComponent<SetActiveDeactivate>();
        gameSessionScript = gameSession.GetComponent<GameSession>();
        setActiveDeactiveHeavyMachineGunUI = heavyMachineGunUI.GetComponent<SetActiveDeactivate>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        musicPlayer = FindObjectOfType<MusicPlayer>();
        //Declaring Values
        machineGunCountDownOn = false;
        sheildOn = false;
        lockPlayerMovement = false;
        bActivatedSheild = false;
    }

    // Update is called once per frame
    void Update()
    {
        //This code detects if the player is holding down the Fire button
        if (Input.GetButton("Fire1"))
        {
            fireButtonIsHeldDown = true;
        }
        else { fireButtonIsHeldDown = false; }
        //Using the bool status above, this code makes sure that the coroutine stops 
        //when the buttton is not pressed
        if(!fireButtonIsHeldDown)
        {
            if(firingCourotine==null)
            { }
            else
            { StopCoroutine(firingCourotine); }
        }

        if (lockPlayerMovement)
        {
            if (isPlayerObjectMoveFoward)
            {
                PlayerObjectmoveForward();
            }
            else if (freezePlayerInPosAfterFreezeLazerHit)
            {
                PlayerFreezesAndCanBreakFree();

                myposition = gameObject.transform.position;

                JitterEffect();

            }
        }
        else
        {
            Move();
            Fire();
        }


        numberofRockets = gameSessionScript.GetNumberOfRockets();

        HeavyMachineGunMethod();

        RocektFireMethod();


        //Code to feed playr's current position for the HitPlayerVFX position
        playerPos = gameObject.transform.position;


        if (immunityOn)
        {
            mySpriteRenderer.color = semiTransparent;
        }
        else if (lerpToRedorWhiteCount <= 100.0f)
        {
            lerpToRedorWhiteCount += Time.deltaTime * 100f;
            mySpriteRenderer.color = Color.Lerp(myColor, Color.white, (lerpToRedorWhiteCount / 100));
        }
        else
        {
            mySpriteRenderer.color = Color.white;
        }

        //Debug Delete later, delete also in start
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Debug.Log("Debug Key to activate Machine gun");   
            HeavyMachineGunPowerUpMethod(potato);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Debug Key to activate death");
            Disable_ThePlayer();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            DamageDealer dummyDamagedealer = new DamageDealer();
            Debug.Log("Debug Key to activate sheild");
            sheildHealthBarScript.SetActiveSheildHealthBar();
            sheildScript.SetActiveSheildMethod();
            bActivatedSheild = true;
        }
    }

    public void PlayerObjectmoveForward()
    {
        sheildOn = true;
        var targetPosition1 = new Vector3(0f, 12f, -0.853f);
        var movementThisFrame = 5f * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition1, movementThisFrame);
        if (transform.position == targetPosition1)
        {
            isPlayerObjectMoveFoward = false;
            SetPlayerInactive();
            transform.position = new Vector2(0, -5);

            if (levelNumber == 1)
            {
                Debug.Log("transition to Level 2");
                levelNumber = 2;
                gameSessionScript.TurnOnLevelTwo();
                gameSessionScript.GameSessionSetToLevelTwo();
                FindObjectOfType<Level>().LoadLevel2();
                musicPlayer.RestBGMVolume();
                musicPlayer.PlayBGMMusicLevelTwo();
            }
            else if (levelNumber == 2)
            {
                levelNumber = 3;
                gameSessionScript.TurnOnLevelThree();
                gameSessionScript.GameSessionSetToLevelThree();
                Debug.Log("load level3");
                FindObjectOfType<Level>().LoadLevel3();
                musicPlayer.RestBGMVolume();
                musicPlayer.PlayBGMMusicLevelThree();

            }
            else if (levelNumber == 3)
            {
                levelNumber = 4;
                gameSessionScript.TurnOnLevelFour();
                gameSessionScript.GameSessionSetToLevelFour();
                Debug.Log("load level4");
                gameSessionScript.TurnOnLevelFour();
                FindObjectOfType<Level>().LoadLevel4();
                musicPlayer.RestBGMVolume();
                musicPlayer.PlayBGMMusicLevelFour();
            }
            else if (levelNumber == 4)
            {
                levelNumber = 5;
                gameSessionScript.TurnOnLevelFive();
                gameSessionScript.GameSessionSetToLevelFive();
                Debug.Log("load level5");
                FindObjectOfType<Level>().LoadLevel5();
                musicPlayer.RestBGMVolume();
                musicPlayer.PlayBGMMusicLevelFive();
            }
            else if (levelNumber == 5)
            {
                levelNumber = 6;
                gameSessionScript.TurnOnLevelSix();
                Debug.Log("load winning Game Screen");
                FindObjectOfType<Level>().LoadWinScreen();
            }

            SetPlayerActiveLoadNextLevel();
            lockPlayerMovement = false;
        }
    }

    private void RocektFireMethod()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (numberofRockets == 0)
            {
            }
            else if (numberofRockets == 1)
            {
                FireRocket();
                gameSessionScript.RPGUpdate0UI();
                rocketHeadScript.DontSetActiveObject();
            }
            else if (numberofRockets == 2)
            {
                FireRocket();
                gameSessionScript.RPGUpdate1UI();
            }
            else if (numberofRockets == 3)
            {
                FireRocket();
                gameSessionScript.RPGUpdate2UI();
            }
        }
    }

    private void HeavyMachineGunMethod()
    {
        if (machineGunCountDownOn)
        {
            rocketTime += Time.deltaTime;

            if (rocketTime >= 20)
            {
                RestartHeavyMachineGun();
            }
            else if (rocketTime >= 19) { setActiveDeactiveHeavyMachineGunUI.DontSetActiveObject(); }
            else if (rocketTime >= 18) { setActiveDeactiveHeavyMachineGunUI.SetActiveObject(); }
            else if (rocketTime >= 17) { setActiveDeactiveHeavyMachineGunUI.DontSetActiveObject(); }
            else if (rocketTime >= 16) { setActiveDeactiveHeavyMachineGunUI.SetActiveObject(); }
            else if (rocketTime >= 15) { setActiveDeactiveHeavyMachineGunUI.DontSetActiveObject();
                if (playCountDownSound)
                {
                    AudioSource.PlayClipAtPoint(CountDownSFX, Camera.main.transform.position, 1f);
                    playCountDownSound = false;
                }
            }
        }
    }
    public void RestartHeavyMachineGun()
    {
        projectileFiringPeriod = 0.2f;
        machineGunCountDownOn = false;
        setActiveDeactiveHeavyMachineGunUI.DontSetActiveObject();
        rocketTime = 0;
    }

    private void FireRocket()
    {
        GameObject laser = Instantiate(
            rocketPrefab,
            transform.position,
            Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
        AudioSource.PlayClipAtPoint(rocketpressSFX, Camera.main.transform.position, shootSoundVolume);
    }
    public void SheildUINotVisible() { bActivatedSheild = false; }
    public bool GetIfPlayerSheildUI_isActive() { return bActivatedSheild; }
    public void TurnOffSheild() { sheildOn = false; }

    public void DeactivateSheildduetoDeath() { sheildScript.DontSetActiveSheildHealth(); }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Gets the damage dealer of the thing it collided with
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();

        if (sheildOn == true || immunityOn == true)
        {
            //do nothing when hit OtherObjects but still get Powerups

            if (other.gameObject.tag == "PowerUP")
            { PowerUpMethod(damageDealer); }

            else if (other.gameObject.tag == "HeavyMachineGun")
            { HeavyMachineGunPowerUpMethod(damageDealer); }

            else if (other.gameObject.tag == "RocketLauncher")
            { RocketLauncherPowerUPMethod(damageDealer); }

            else if (other.gameObject.tag == "Sheild")
            {
                SheildPowerUpmethod(damageDealer);
                sheildHealthBarScript.SetActiveSheildHealthBar();
                sheildScript.SetActiveSheildMethod();
                bActivatedSheild = true;
            }
            else if (other.gameObject.tag == "ExtraLife")
            {
                ExtraLifePowerUpMethod(damageDealer);
            }
            else { }
        }
        else
        {
            //If collides with main player subtracts damage value from health
            //Depending on the Tag will Destroy other game object
            if (other.gameObject.tag == "EnemyLaser")
            {
                getHitSound = getHitLaserSound;
                hitVFX = laserHitVFX;
                ProcessHit(damageDealer);
                //Debug.Log("1");
                damageDealer.OnHitDestroyOtherObject();
            }
            else if (other.gameObject.tag == "BEnemyBomb")
            {

                if (!isPlayerActive)
                {
                    Debug.Log("No bomb lerp Coroutine Will be played");
                }
                else if (isPlayerActive)
                {
                    getHitSound = getHitBombSound;
                    hitVFX = bombHitVFX;
                    myColor = Color.red;
                    lerpToRedorWhiteCount = 0f;
                    ProcessHit(damageDealer);
                    //Debug.Log("2");
                    damageDealer.OnHitDestroyOtherObject();
                }

            }
            else if (other.gameObject.tag == "BEnemyLaserBomb")
            {

                if (!isPlayerActive)
                {
                    Debug.Log("No bomb lerp Coroutine Will be played");
                }
                else if (isPlayerActive)
                {
                    laserBomb = true;
                    getHitSound = getHitLaserBombSound;
                    hitVFX = laserBombHitVFX;
                    myColor = Color.green;
                    lerpToRedorWhiteCount = 0f;
                    ProcessHit(damageDealer);
                    //Debug.Log("2");
                    damageDealer.OnHitDestroyOtherObject();
                }

            }
            else if (other.gameObject.tag == "PowerUP")
            { PowerUpMethod(damageDealer); }

            else if (other.gameObject.tag == "HeavyMachineGun")
            { HeavyMachineGunPowerUpMethod(damageDealer); }

            else if (other.gameObject.tag == "RocketLauncher")
            { RocketLauncherPowerUPMethod(damageDealer); }

            else if (other.gameObject.tag == "Sheild")
            {
                SheildPowerUpmethod(damageDealer);
                sheildHealthBarScript.SetActiveSheildHealthBar();
                sheildScript.SetActiveSheildMethod();
            }
            else if (other.gameObject.tag == "ExtraLife")
            { ExtraLifePowerUpMethod(damageDealer); }
            else
            {
                getHitSound = getHitLaserSound;
                hitVFX = laserHitVFX;
                ProcessHit(damageDealer);
            }
        }

    }

    public void ExtraLifePowerUpMethod(DamageDealer damageDealer)
    {
        Debug.Log("Touched Extralife");
        PlayPowerUpSound();
        gameSessionScript.Addlives();
        gameSessionScript.CheckNumberOfLivesAndUpdate();
        damageDealer.OnHitDestroyOtherObject();
    }

    public void SheildPowerUpmethod(DamageDealer damageDealer)
    {
        Debug.Log("Touched Sheild");
        PlayPowerUpSound();
        sheildOn = true;
        damageDealer.OnHitDestroyOtherObject();
    }

    public void RocketLauncherPowerUPMethod(DamageDealer damageDealer)
    {
        Debug.Log("Touched RocketLauncher");
        PlayRocketLauncherSound();
        rocketHeadScript.SetActiveObject();
        damageDealer.OnHitDestroyOtherObject();
        StartCoroutine(gameSessionScript.ShowGuideforPressC());
        gameSessionScript.RPGUpdate3UI();
    }

    public void DisableRocketHeadShow()
    {
        rocketHeadScript.DontSetActiveObject();
    }

    public void HeavyMachineGunPowerUpMethod(DamageDealer damageDealer)
    {
        Debug.Log("Touched HeavyMachineGUn");
        PlayHeavyMachineGunSound();
        ActivateHeavyMachineGun();
        damageDealer.OnHitDestroyOtherObject();
    }

    public void PowerUpMethod(DamageDealer damageDealer)
    {
        Debug.Log("Touched Power Up");
        PlayPowerUpSound();
        health = Mathf.Clamp(health + 500, 0, 1000);
        damageDealer.OnHitDestroyOtherObject();
    }

    

    public IEnumerator DelayInvincibility()
    {
        Debug.Log("Immunity On");
        immunityOn = true;
        yield return new WaitForSeconds(3);
        immunityOn = false;
        StopCoroutine(DelayInvincibility());
    }

    public void ActivateHeavyMachineGun()
    { machineGunCountDownOn = true;
        playCountDownSound = true;
        projectileFiringPeriod = 0.04f;
        setActiveDeactiveHeavyMachineGunUI.SetActiveObject();
        rocketTime = 0;
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        if (damageDealer == null)
        {
            //Debug.Log("No Damage Dealer Assigned to Object in Collision");
        }
        else
        {
            health -= damageDealer.GetDamage();

            PlayHitPlayerVFX();

            if (health <= 0)
            {
                if (isFrozenFromLaser)
                {
                    StopCoroutine(FreezePlayer_ifFreezeValueReached());

                    Unfreeze_The_Player();

                    DestrotCubeVFXDuetoDeath();
                }
                else 
                {
                    sheildOn = false;
                    Disable_ThePlayer();
                }
            }
        }

    }

    private void DestrotCubeVFXDuetoDeath()
    {
        if (myNewIceCubeFreezeObject == null) { }
        else { Destroy(myNewIceCubeFreezeObject); }

        if (smokeVFXFreezeObject == null) { }
        else { Destroy(smokeVFXFreezeObject); }

        if (VFX1FreezeObject == null) { }
        else { Destroy(VFX1FreezeObject); }
    }
    public void DestroyIceCubeDuetoWin()
    {
        if (isFrozenFromLaser)
        {
            StopCoroutine(FreezePlayer_ifFreezeValueReached());

            Unfreeze_The_Player();

            DestrotCubeVFXDuetoDeath();
        }
    }
    private void PlayHitPlayerVFX()
    {
        if (laserBomb)
        {
            GameObject vfx = Instantiate(
            hitVFX,
            new Vector2(playerPos.x, playerPos.y + .3f),
            transform.rotation * Quaternion.Euler(0, -180f, 0f));
            vfx.transform.parent = gameObject.transform;
            Destroy(vfx, 1f);
            AudioSource.PlayClipAtPoint(getHitSound, Camera.main.transform.position, getHitSoundVolume);
            laserBomb = false;
        }
        else
        {
            GameObject vfx = Instantiate(
            hitVFX,
            new Vector2(playerPos.x, playerPos.y + .3f),
            Quaternion.identity);
            vfx.transform.parent = gameObject.transform;
            Destroy(vfx, 1f);
            AudioSource.PlayClipAtPoint(getHitSound, Camera.main.transform.position, getHitSoundVolume);
        }

    }

    private void PlayPowerUpSound() { AudioSource.PlayClipAtPoint(sheildHealthUPSFX,
          Camera.main.transform.position, deathSoundVolume); }

    private void PlayHeavyMachineGunSound() { AudioSource.PlayClipAtPoint(heavyMachineGunSFX,
          Camera.main.transform.position, 1); }

    private void PlayRocketLauncherSound() { AudioSource.PlayClipAtPoint(rocketLauncherSFX,
          Camera.main.transform.position, 1); }

    private void Disable_ThePlayer()
    {
        StopCoroutine(gameSessionScript.InstantiatePlayer());
        gameSessionScript.Subtractlives();
        gameSessionScript.CheckNumberOfLivesAndUpdate();
        gameSessionScript.TurnONGamesessionBoolPlayerisDestroyed();

        //This Turns on Method in GameSession to start SpawnPlayer Courotine
        gameSessionScript.TurnOnCheckIfPlayerIsDestroyed();


        //Creates a Death VFX
        GameObject explosion = Instantiate(
           deathVFX,
           transform.position,
           transform.rotation);

        //Destroys the Death VFX
        Destroy(explosion, durationOFHitExplosion);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        SetPlayerInactive();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCourotine = StartCoroutine(FireContinously());
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCourotine);
        }
    }


    IEnumerator FireContinously()
    {
        while (true)
        {
            GameObject laser = Instantiate(
                laserPrefab,
                transform.position,
                Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin + 1, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0.04f, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    //===PUBLIC COMMANDS===
    public void SetToLevelOne() { levelNumber = 1; }
    public void SetToLevelTwo() { levelNumber = 2; }
    public void SetToLevelThree() { levelNumber = 3; }
    public void SetToLevelFour() { levelNumber = 4; }
    public void SetToLevelFive() { levelNumber = 5; }

    public void SetPlayerActiveLoadNextLevel()
    {
        lerpToRedorWhiteCount = 100f;
        gameObject.SetActive(true);
        transform.position = new Vector2(0, -5);
        sheildOn = false;
        isPlayerActive = true;
        gameSessionScript.TurnOFFGamesessionBoolPlayerisDestroyed();
    }
    public void SetPlayerActive()
    {
        lerpToRedorWhiteCount = 100f;
        gameObject.SetActive(true);
        health = 1000;
        transform.position = new Vector2(0, -5);
        isPlayerActive = true;
        gameSessionScript.TurnOFFGamesessionBoolPlayerisDestroyed();
        //
        isFrozenFromLaser = false;
        secondsFrozen = 0;
        lockItUpToJitter = false;
        freezePlayerInPosAfterFreezeLazerHit = false;
       
    }
    public void SetPlayerInactive()
    {
        isPlayerActive = false;
        gameObject.SetActive(false);
    }

    public void TurnOnLockPlayerMovement() { lockPlayerMovement = true; }

    public void TurnOffLockPlayerMovement() { lockPlayerMovement = false; }
    //public void TurnOnPlayerMovementForward() { isPlayerObjectMoveFoward = true; }

    public void TurnOffPlayerMovementForward() { isPlayerObjectMoveFoward = true; }

    //===RETURN VALUES TO PUBLIC===
    public int GetHealth() { return health; }
    //public void AddHealth(){ health = Mathf.Clamp(health + 500, 0, 1000); }

    public GameObject GetLaserBombHitVFXFromPlayerParent() { return laserBombHitVFX; }

    public GameObject GetBombHitVFXFromPlayerParent() { return bombHitVFX; }

    public AudioClip GetLaserBombHitSoundSFXFromPlayerParent() { return getHitLaserBombSound; }

    public AudioClip GetLaserHitSoundSFXFromPlayerParent() { return getHitLaserSound; }

    public GameObject GetLaserHitVFXFromPlayerParent() { return laserHitVFX; }

    public AudioClip GetBombHitSFXFromPlayerParent() { return getHitBombSound; }

    public float GetHitSoundVolumeFromPlayerParent() { return getHitSoundVolume; }

    public void RestartLevelNumber() { levelNumber = 1; }

    /// <summary>
    /// //Delete Later -for Debugging purposes only
    /// </summary>
    public void Public_Access_Trigger_PlayerDeath()
    {
        StopCoroutine(gameSessionScript.InstantiatePlayer());
        gameSessionScript.Subtractlives();
        gameSessionScript.CheckNumberOfLivesAndUpdate();
        gameSessionScript.TurnONGamesessionBoolPlayerisDestroyed();

        //This Turns on Method in GameSession to start SpawnPlayer Courotine
        gameSessionScript.TurnOnCheckIfPlayerIsDestroyed();


        //Creates a Death VFX
        GameObject explosion = Instantiate(
           deathVFX,
           transform.position,
           transform.rotation);
        //Destroys the Death VFX
        Destroy(explosion, durationOFHitExplosion);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        SetPlayerInactive();
    }

    //FREEZE CODE BELOW
    void OnParticleCollision(GameObject other)
    {
        //Name of the Freeze Lazerbeam object is Particle launcher
        if(other.name== "Particle launcher")
        {
            if(!sheildOn)
            {
                if (!stopAbsorbingParticles)
                {
                    if (!immunityOn)
                    {
                        particlehit++;
                        StartCoroutine(FreezePlayer_ifFreezeValueReached());
                    }
                }
            }
            
        }
    }

    private IEnumerator FreezePlayer_ifFreezeValueReached()
    {
        yield return new WaitForSeconds(0.01f);
        if (particlehit > valueNeededToFreezePlayer)
        {
            stopAbsorbingParticles = true;
            particlehit = 0;
            lockPlayerMovement = true;
            freezePlayerInPosAfterFreezeLazerHit = true;

            AudioSource.PlayClipAtPoint(iceBlastSound, Camera.main.transform.position, deathSoundVolume);

            myNewIceCubeFreezeObject = Instantiate(iceCube, gameObject.transform.position, Quaternion.identity);
            myNewIceCubeFreezeObject.transform.parent = gameObject.transform;
            Destroy(myNewIceCubeFreezeObject, 5);

            smokeVFXFreezeObject = Instantiate(iceSmokeVFX, gameObject.transform.position, Quaternion.identity);
            smokeVFXFreezeObject.transform.parent = gameObject.transform;
            Destroy(smokeVFXFreezeObject, 5);

            VFX1FreezeObject = Instantiate(laserHitVFX, gameObject.transform.position, Quaternion.identity);
            Destroy(VFX1FreezeObject, 2);

            if (firingCourotine == null)
            { }
            else { StopCoroutine(firingCourotine); }

        }
    }

    //ALL ADDITIONAL FEAUTURES FOR FREEZE CODE BELOW
    private void PlayerFreezesAndCanBreakFree()
    {
        isFrozenFromLaser = true;
        secondsFrozen = secondsFrozen + (Time.deltaTime * 1);

        if (secondsFrozen >= 5)
        {
            Unfreeze_The_Player();
        }
    }
    private void Unfreeze_The_Player()
    {
        AudioSource.PlayClipAtPoint(iceBlastShatter, Camera.main.transform.position, 1f);
        GameObject iceExplosion = Instantiate(
    iceBlastShardEffect,
    transform.position,
    transform.rotation);
        Destroy(iceExplosion, 2);

        GameObject iceExplosion2 = Instantiate(
    iceBlastShardEffect2,
    transform.position,
    transform.rotation);
        Destroy(iceExplosion2, 2);

        stopAbsorbingParticles = false;
        lockPlayerMovement = false;
        freezePlayerInPosAfterFreezeLazerHit = false;
        particlehit = 0;
        secondsFrozen = 0;
        health = health - 200;
        PlayHitPlayerVFX();
        StartCoroutine(DelayMinusHealth());
    }
    private IEnumerator DelayMinusHealth()
    {
        yield return new WaitForSeconds(0.5f);       
        if (health <= 0)
        {
            Disable_ThePlayer();
        }
        isFrozenFromLaser = false;
    }

    private void JitterEffect()
    {
        if (!lockItUpToJitter)
        {
            if ((Input.GetKeyDown(KeyCode.UpArrow)) | (Input.GetKeyDown(KeyCode.W)))
            {
                lockItUpToJitter = true;
                AudioSource.PlayClipAtPoint(IceCrack[UnityEngine.Random.Range(0, IceCrack.Count)], Camera.main.transform.position, 1f);
                transform.position = new Vector3(myposition.x, myposition.y + 0.25f, myposition.z);
                StartCoroutine(BacktoPosYUp());
            }
            else if ((Input.GetKeyDown(KeyCode.DownArrow)) | (Input.GetKeyDown(KeyCode.S)))
            {
                lockItUpToJitter = true;
                AudioSource.PlayClipAtPoint(IceCrack[UnityEngine.Random.Range(0, IceCrack.Count)], Camera.main.transform.position, 1f);
                transform.position = new Vector3(myposition.x, myposition.y - 0.25f, myposition.z);
                StartCoroutine(BacktoPosYDown());
            }
            else if ((Input.GetKeyDown(KeyCode.RightArrow)) | (Input.GetKeyDown(KeyCode.A)))
            {
                lockItUpToJitter = true;
                AudioSource.PlayClipAtPoint(IceCrack[UnityEngine.Random.Range(0, IceCrack.Count)], Camera.main.transform.position, 1f);
                transform.position = new Vector3(myposition.x + 0.25f, myposition.y, myposition.z);
                StartCoroutine(BacktoPosXRight());
            }
            else if ((Input.GetKeyDown(KeyCode.LeftArrow)) | (Input.GetKeyDown(KeyCode.D)))
            {
                lockItUpToJitter = true;
                AudioSource.PlayClipAtPoint(IceCrack[UnityEngine.Random.Range(0, IceCrack.Count)], Camera.main.transform.position, 1f);
                transform.position = new Vector3(myposition.x - 0.25f, myposition.y, myposition.z);
                StartCoroutine(BacktoPosXLeft());
            }
        }
    }
    private IEnumerator BacktoPosXRight()
    {
        yield return new WaitForSeconds(0.1f);
        transform.position = new Vector3(myposition.x - 0.25f, myposition.y, myposition.z);
        lockItUpToJitter = false;
    }
    private IEnumerator BacktoPosXLeft()
    {
        yield return new WaitForSeconds(0.1f);
        transform.position = new Vector3(myposition.x + 0.25f, myposition.y, myposition.z);
        lockItUpToJitter = false;
    }
    private IEnumerator BacktoPosYDown()
    {
        yield return new WaitForSeconds(0.1f);
        transform.position = new Vector3(myposition.x, myposition.y + 0.25f, myposition.z);
        lockItUpToJitter = false;
    }
    private IEnumerator BacktoPosYUp()
    {
        yield return new WaitForSeconds(0.1f);
        transform.position = new Vector3(myposition.x, myposition.y - 0.25f, myposition.z);
        lockItUpToJitter = false;
    }


}
