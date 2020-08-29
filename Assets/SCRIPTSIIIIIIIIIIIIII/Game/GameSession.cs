using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameSession : MonoBehaviour
{
    int score = 0;
    int powerUpScore = 0;
    [SerializeField] int levelNumber = 1; //Serialized for Debugging Purposes
    [SerializeField] int numberOfEnemiesInScene; //Serialized for Debugging Purposes
    [SerializeField] int numberOfDestroyedEnemies; //Serialized for Debugging Purposes

    [Header("GameObjects")] //To intstantiate
    [SerializeField] GameObject playerObject;
    [SerializeField] GameObject powerUpObject;
    [SerializeField] GameObject HeavyMachineGunObject;
    [SerializeField] GameObject ExtraLifeObject;
    [SerializeField] GameObject rocketLauncherObject;
    [SerializeField] GameObject SheildObject;

    [Header("UI")]
    [SerializeField] GameObject canvasScoreTextObject;
    [SerializeField] TextMeshProUGUI pressCText;
    [SerializeField] TextMeshProUGUI LevelIntroductionText;

    [Header("Lives UI")]
    [SerializeField] GameObject rocketUICollection;
    [SerializeField] GameObject liveUI1;
    [SerializeField] GameObject liveUI2;
    [SerializeField] GameObject liveUI3;

    [Header("Rocket UI")]
    [SerializeField] GameObject livesUICollection;
    [SerializeField] GameObject rocketUI1;
    [SerializeField] GameObject rocketUI2;
    [SerializeField] GameObject rocketUI3;

    [Header("Scene Handlers")]
    [SerializeField] GameObject enemySpawnerObjectLevelOne;
    [SerializeField] GameObject bossSpawnerObjectLevelOne;
    [SerializeField] bool levelOneOn = false;
    [SerializeField] GameObject enemySpawnerObjectLevelTwo;
    [SerializeField] GameObject bossSpawnerObjectLevelTwo;
    [SerializeField] bool levelTwoOn = false;
    [SerializeField] GameObject enemySpawnerObjectLevelThree;
    [SerializeField] GameObject bossSpawnerObjectLevelThree;
    [SerializeField] bool levelThreeOn = false;
    [SerializeField] GameObject enemySpawnerObjectLevelFour;
    [SerializeField] GameObject bossSpawnerObjectLevelFour;
    [SerializeField] bool levelFourOn = false;
    [SerializeField] GameObject enemySpawnerObjectLevelFive;
    [SerializeField] GameObject bossSpawnerObjectLevelFive;
    [SerializeField] bool levelFiveOn = false;
    [SerializeField] bool levelSixOn = false;

    [Header(" Object bools serialized for Debug Purposes")]
    //ObjectDetectionbools
    [SerializeField] bool playerIsDestroyed = false;
    [SerializeField] bool leftHandInScene = false;
    [SerializeField] bool is_LeftHandMinion_in_scene = false;
    [SerializeField] bool leftRightInScene = false;
    [SerializeField] bool is_RightHandMinion_in_scene = false;

    [Header(" Enemy Spawner bools serialized for Debug Purposes")]
    //Do Once Corotines
    //This bool when true Manually activates the EnemySpawnerCoroutine when second time loading scene
    [SerializeField] bool isNotFirstTime = false;
    [SerializeField] bool isNotFirstTimeForLevelTwo = false;
    [SerializeField] bool isNotFirstTimeForLevelThree = false;
    [SerializeField] bool isNotFirstTimeForLevelFour = false;
    [SerializeField] bool isNotFirstTimeForLevelFive = false;


    //BossSpwner
    //This bool when true Manually activates the BossSpawnerCoroutine when second time loading scene
    [SerializeField] bool isNotFirstTimeBossSpawner = false;
    [SerializeField] bool isNotFirstTimeForLevelTwoBossSpawner = false;
    [SerializeField] bool isNotFirstTimeForLevelThreeBossSpawner = false;
    [SerializeField] bool isNotFirstTimeForLevelFourBossSpawner = false;
    [SerializeField] bool isNotFirstTimeForLevelFiveBossSpawner = false;

    [Header("Level 3 Objects")]
    [SerializeField] GameObject mMeteors;
    [SerializeField] GameObject mMeteorSpawner;
    [SerializeField] GameObject mPeanuts;
    [SerializeField] GameObject mBounds;
    [SerializeField] GameObject mBossPeanut;

    int numberOfLives = 3;
    int numberOfRockets = 0;
    int randomNumberForSpawner = 0;
    bool isPlayerDestroyed = false;


    //Do Once Corotines
    bool isEnemySpawnerCourotineDone = false;
    bool isBossSpawnerCoroutineDone = false;
    //used by Objects who are dependent on player, instead of usding FindObjectOfType<Player>

    //Do Once Corotines
    bool doLevelOneCourotineOnce = true;
    bool doLevelTwoCourotineOnce = true;
    bool doLevelThreeCourotineOnce = true;
    bool doLevelFourCourotineOnce = true;
    bool doLevelFiveCourotineOnce = true;
    bool doLevelSixCourotineOnce = true;

    bool hand_enemy_fireUpwards = false;

    //cached Scripts Access
    SetActiveDeactivate onOffRocketGroup;
    SetActiveDeactivate onOFFRocketUI1;
    SetActiveDeactivate onOFFRocketUI2;
    SetActiveDeactivate onOFFRocketUI3;
    //
    SetActiveDeactivate onOffLivesGroup;
    SetActiveDeactivate onOFFLiveUI1;
    SetActiveDeactivate onOFFLiveUI2;
    SetActiveDeactivate onOFFLiveUI3;
    //
    EnemySpawner enemySpawnerScriptLevelOne;
    EnemySpawner bossSpawnerScriptLevelOne;
    EnemySpawner enemySpawnerScriptLevelTwo;
    EnemySpawner bossSpawnerScriptLevelTwo;
    EnemySpawner enemySpawnerScriptLevelThree;
    EnemySpawner bossSpawnerScriptLevelThree;
    EnemySpawner enemySpawnerScriptLevelFour;
    EnemySpawner bossSpawnerScriptLevelFour;
    EnemySpawner enemySpawnerScriptLevelFive;
    EnemySpawner bossSpawnerScriptLevelFive;
    //
    SetActiveDeactivate enemySpawnerSetActiveScriptLevelOne;
    SetActiveDeactivate bossSpawnerSetActiveScriptOne;
    SetActiveDeactivate enemySpawnerSetActiveScriptLevelTwo;
    SetActiveDeactivate bossSpawnerSetActiveScriptTwo;
    SetActiveDeactivate enemySpawnerSetActiveScriptLevelThree;
    SetActiveDeactivate bossSpawnerSetActiveScriptThree;
    SetActiveDeactivate enemySpawnerSetActiveScriptLevelFour;
    SetActiveDeactivate bossSpawnerSetActiveScriptFour;
    SetActiveDeactivate enemySpawnerSetActiveScriptLevelFive;
    SetActiveDeactivate bossSpawnerSetActiveScriptFive;
    //
    Player playerObjectScript;
    MusicPlayer musicPlayerScript;
    SetActiveDeactivate accessPressCSetActiveScript;
    SetActiveDeactivate introTextSetActiveDeactivate;
    SetActiveDeactivate canvasScoreActivateDeactivate;
    bool findLivesdone = false;

    // Start is called before the first frame update
    void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            if(findLivesdone)
            {

            }
            else 
            {
                DontDestroyOnLoad(gameObject);
            }
            

        }
    }

    private void Start()
    {
        findLivesdone = false;
        //UI Cache
        onOffRocketGroup = rocketUICollection.GetComponent<SetActiveDeactivate>();
        onOFFRocketUI1 = rocketUI1.GetComponent<SetActiveDeactivate>();
        onOFFRocketUI2 = rocketUI2.GetComponent<SetActiveDeactivate>();
        onOFFRocketUI3 = rocketUI3.GetComponent<SetActiveDeactivate>();
        onOffLivesGroup = livesUICollection.GetComponent<SetActiveDeactivate>();
        onOFFLiveUI1 = liveUI1.GetComponent<SetActiveDeactivate>();
        onOFFLiveUI2 = liveUI2.GetComponent<SetActiveDeactivate>();
        onOFFLiveUI3 = liveUI3.GetComponent<SetActiveDeactivate>();

        //Cache some spawners to access thier script
        enemySpawnerScriptLevelOne = enemySpawnerObjectLevelOne.GetComponent<EnemySpawner>();
        bossSpawnerScriptLevelOne = bossSpawnerObjectLevelOne.GetComponent<EnemySpawner>();
        enemySpawnerScriptLevelTwo = enemySpawnerObjectLevelTwo.GetComponent<EnemySpawner>();
        bossSpawnerScriptLevelTwo = bossSpawnerObjectLevelTwo.GetComponent<EnemySpawner>();
        enemySpawnerScriptLevelThree = enemySpawnerObjectLevelThree.GetComponent<EnemySpawner>();
        bossSpawnerScriptLevelThree = bossSpawnerObjectLevelThree.GetComponent<EnemySpawner>();
        enemySpawnerScriptLevelFour = enemySpawnerObjectLevelFour.GetComponent<EnemySpawner>();
        bossSpawnerScriptLevelFour = bossSpawnerObjectLevelFour.GetComponent<EnemySpawner>();
        enemySpawnerScriptLevelFive = enemySpawnerObjectLevelFive.GetComponent<EnemySpawner>();
        bossSpawnerScriptLevelFive = bossSpawnerObjectLevelFive.GetComponent<EnemySpawner>();

        //cache some spaners to access thier SetActiveProperty
        enemySpawnerSetActiveScriptLevelOne = enemySpawnerObjectLevelOne.GetComponent<SetActiveDeactivate>();
        bossSpawnerSetActiveScriptOne = bossSpawnerObjectLevelOne.GetComponent<SetActiveDeactivate>();
        enemySpawnerSetActiveScriptLevelTwo = enemySpawnerObjectLevelTwo.GetComponent<SetActiveDeactivate>();
        bossSpawnerSetActiveScriptTwo = bossSpawnerObjectLevelTwo.GetComponent<SetActiveDeactivate>();
        enemySpawnerSetActiveScriptLevelThree = enemySpawnerObjectLevelThree.GetComponent<SetActiveDeactivate>();
        bossSpawnerSetActiveScriptThree = bossSpawnerObjectLevelThree.GetComponent<SetActiveDeactivate>();
        enemySpawnerSetActiveScriptLevelFour = enemySpawnerObjectLevelFour.GetComponent<SetActiveDeactivate>();
        bossSpawnerSetActiveScriptFour = bossSpawnerObjectLevelFour.GetComponent<SetActiveDeactivate>();
        enemySpawnerSetActiveScriptLevelFive = enemySpawnerObjectLevelFive.GetComponent<SetActiveDeactivate>();
        bossSpawnerSetActiveScriptFive = bossSpawnerObjectLevelFive.GetComponent<SetActiveDeactivate>();

        //Cache some more
        playerObjectScript = playerObject.GetComponent<Player>();
        accessPressCSetActiveScript = pressCText.GetComponent<SetActiveDeactivate>();
        introTextSetActiveDeactivate = LevelIntroductionText.GetComponent<SetActiveDeactivate>();
        canvasScoreActivateDeactivate = canvasScoreTextObject.GetComponent<SetActiveDeactivate>();

        //FindObjectsoftype
        musicPlayerScript = FindObjectOfType<MusicPlayer>();

        //Initializing Values on Start
        RPGUpdate0UI();
        CheckNumberOfLivesAndUpdate();
        numberOfLives = 3;
        //StartCoroutine(LevelOneIntroduction());
    }

    private void Update()
    {
        CheckIFInspectorSetLevelforSpawnerThenSetLevelAccordingly();

        if (!findLivesdone)
        {
            LoadToNextLevelIfNoMoreLives();
        }

        GenerateRandomPowerUpAfterXPoints();

        /*
        ////==== Delete later, for debugging purposes
        ///============================================
        ///============================================        
        if (Input.GetKeyDown(KeyCode.X))
        {
            playerObjectScript.Public_Access_Trigger_PlayerDeath();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            playerObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            playerObjectScript.SetPlayerInactive();
        }
        */
    }

    private void CheckIFInspectorSetLevelforSpawnerThenSetLevelAccordingly()
    {
        if (levelSixOn)
        {
            if (doLevelSixCourotineOnce)
            {
                Debug.Log("level Six Initializing");
                levelOneOn = false;
                levelTwoOn = false;
                levelThreeOn = false;
                levelFourOn = false;
                levelFiveOn = false;
                Debug.Log("You Win");
                doLevelFiveCourotineOnce = false;
            }
        }
        else if (levelFiveOn)
        {
            if (doLevelFiveCourotineOnce)
            {
                Debug.Log("level Five Initializing");
                TurnOnLevelFive();
                StartCoroutine(LevelFiveIntroduction());
                doLevelFiveCourotineOnce = false;
            }
            LevelFiveEnemySpawnerBossSpawnerMethod();
        }
        else if (levelFourOn)
        {
            if (doLevelFourCourotineOnce)
            {
                Debug.Log("level Four Initializing");
                TurnOnLevelFour();
                StartCoroutine(LevelFourIntroduction());
                doLevelFourCourotineOnce = false;
            }
            LevelFourEnemySpawnerBossSpawnerMethod();
        }
        else if (levelThreeOn)
        {

            if (doLevelThreeCourotineOnce)
            {
                Debug.Log("level Three Initializing");
                TurnOnLevelThree();
                StartCoroutine(LevelThreeIntroduction());
                doLevelThreeCourotineOnce = false;
            }
            LevelThreeEnemySpawnerBossSpawnerMethod();
        }
        else if (levelTwoOn)
        {
            if (doLevelTwoCourotineOnce)
            {
                Debug.Log("level Two Initializing");
                TurnOnLevelTwo();
                playerObjectScript.SetToLevelTwo();
                StartCoroutine(LevelTwoIntroduction());
                doLevelTwoCourotineOnce = false;
                //TurnOffLater this script is already in the playerScript
                //musicPlayerScript.RestBGMVolume(); error because musicplayer was deactivated
                //musicPlayerScript.ChangeBGMtoBossFightMusicLevelTwo();
            }

            LevelTwoEnemySpawnerBossSpawnerMethod();

        }
        else if (levelOneOn)
        {
            if (doLevelOneCourotineOnce)
            {
                Debug.Log("level One Initializing");
                TurnOnLevelOne();
                playerObjectScript.SetToLevelOne();
                StartCoroutine(LevelOneIntroduction());
                doLevelOneCourotineOnce = false;
            }

            LevelOneEnemySpawnerBossSpawnerMethod();
        }
        else { Debug.Log("No level is on"); }
    }

    private void LoadToNextLevelIfNoMoreLives()
    {
        if (isPlayerDestroyed)
        {
            if (numberOfLives >= 0)
            {
                StartCoroutine(InstantiatePlayer());
                Debug.Log("Start Instantiate Player Courotine");
                isPlayerDestroyed = false;
            }
            else
            {
                //Insert Game Over here
                Debug.Log("Game Over - No More lives");
                if (isEnemySpawnerCourotineDone)
                {
                    isEnemySpawnerCourotineDone = false;
                }

                if (isBossSpawnerCoroutineDone)
                {
                    isBossSpawnerCoroutineDone = false;
                }

                //////=============
                //Code to reset EnemySpawnerCached Refs to Start level values
                ResetTheEnemySpawnersBoolToOriginalSettings();

                //Also put code when player sucessfully transitions into next level
                //Edit this code to be a method




                //this code is not placed in the loadlevel code since it tracks if first time
                //if placed in loadlevelcode it will trigger the even in enemyspawner despite being first time
                //paced in death event instead
                ActivateBoolIsitFirstTimeForEnemySpawnerForEachLevel();
                musicPlayerScript.DestroyPair();
                findLivesdone = true;
                isPlayerDestroyed = false;
                musicPlayerScript.PlayGameOver();
                enemySpawnerSetActiveScriptLevelOne.DontSetActiveObject();
                FindObjectOfType<Level>().LoadGameOver();
                musicPlayerScript.PlayBGMGameOver();
                canvasScoreActivateDeactivate.DontSetActiveObject();
            }
        }
    }

    private void ResetTheEnemySpawnersBoolToOriginalSettings()
    {
        if (levelSixOn)
        {
            //enemySpawnerScriptLevelFive.ResetEnemySpawnerShizz();
        }
        else if (levelFiveOn)
        {
            enemySpawnerScriptLevelFive.ResetEnemySpawnerShizz();
        }
        else if (levelFourOn)
        {
            enemySpawnerScriptLevelFour.ResetEnemySpawnerShizz();
        }
        else if (levelThreeOn)
        {
            enemySpawnerScriptLevelThree.ResetEnemySpawnerShizz();
        }
        else if (levelTwoOn)
        {
            enemySpawnerScriptLevelTwo.ResetEnemySpawnerShizz();
        }
        else if (levelOneOn)
        {
            enemySpawnerScriptLevelOne.ResetEnemySpawnerShizz();
        }
    }

    private void ActivateBoolIsitFirstTimeForEnemySpawnerForEachLevel()
    {
        if (levelNumber == 5)
        {
            if (!isNotFirstTimeForLevelFive) { isNotFirstTimeForLevelFive = true; }
            else if (isNotFirstTimeForLevelFive)
            { //do nothing 
            }

            if (!isNotFirstTimeForLevelTwo) { isNotFirstTimeForLevelTwo = true; }
            else if (isNotFirstTimeForLevelTwo)
            { //do nothing 
            }

            if (!isNotFirstTimeForLevelThree) { isNotFirstTimeForLevelThree = true; }
            else if (isNotFirstTimeForLevelThree)
            { //do nothing 
            }

            if (!isNotFirstTimeForLevelFour) { isNotFirstTimeForLevelFour = true; }
            else if (isNotFirstTimeForLevelFour)
            { //do nothing 
            }
        }
        else if (levelNumber == 4)
        {
            if (!isNotFirstTimeForLevelFour) { isNotFirstTimeForLevelFour = true; }
            else if (isNotFirstTimeForLevelFour)
            { //do nothing 
            }

            if (!isNotFirstTimeForLevelTwo) { isNotFirstTimeForLevelTwo = true; }
            else if (isNotFirstTimeForLevelTwo)
            { //do nothing 
            }

            if (!isNotFirstTimeForLevelThree) { isNotFirstTimeForLevelThree = true; }
            else if (isNotFirstTimeForLevelThree)
            { //do nothing 
            }
        }
        else if (levelNumber == 3)
        {
            if (!isNotFirstTimeForLevelThree) { isNotFirstTimeForLevelThree = true; }
            else if (isNotFirstTimeForLevelThree)
            { //do nothing 
            }

            if (!isNotFirstTimeForLevelTwo) { isNotFirstTimeForLevelTwo = true; }
            else if (isNotFirstTimeForLevelTwo)
            { //do nothing 
            }
        }
        else if (levelNumber == 2)
        {
            if (!isNotFirstTimeForLevelTwo) { isNotFirstTimeForLevelTwo = true; }
            else if (isNotFirstTimeForLevelTwo)
            { //do nothing 
            }
        }
        else { }
    }

    // Instantiates Objects in Scene / calls SetActive for obj in scene
    public IEnumerator InstantiatePlayer() //Toggles Player SetActive
    {
        yield return new WaitForSeconds(3);
        playerObjectScript.SetPlayerActive();
        StartCoroutine(playerObjectScript.DelayInvincibility());
        TurnOFFGamesessionBoolPlayerisDestroyed();
    }
    private void GenerateRandomPowerUpAfterXPoints()
    {
        if (powerUpScore >= 150)
        {
            randomNumberForSpawner = Random.Range(0, 12);

            Debug.Log("Generate Power UP");
            if (randomNumberForSpawner == 0 || randomNumberForSpawner == 8 || randomNumberForSpawner == 1)
            {
                GameObject powerUpInstantiatedObject = Instantiate(
                    powerUpObject,
                    new Vector2(Random.Range(-5, 5), 9),
                    transform.rotation);
                powerUpInstantiatedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1f);
            }
            if (randomNumberForSpawner == 9)
            {
                GameObject powerUpInstantiatedObject = Instantiate(
                    HeavyMachineGunObject,
                    new Vector2(Random.Range(-5, 5), 9),
                    transform.rotation);
                powerUpInstantiatedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1f);
            }
            if (randomNumberForSpawner == 2 || randomNumberForSpawner == 10)
            {
                GameObject powerUpInstantiatedObject = Instantiate(
                        ExtraLifeObject,
                    new Vector2(Random.Range(-5, 5), 9),
                    transform.rotation);
                powerUpInstantiatedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1f);
            }
            if (randomNumberForSpawner == 3 || randomNumberForSpawner == 11)
            {
                GameObject powerUpInstantiatedObject = Instantiate(
                    rocketLauncherObject,
                    new Vector2(Random.Range(-5, 5), 9),
                    transform.rotation);
                powerUpInstantiatedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1f);
            }
            if (randomNumberForSpawner == 4 || randomNumberForSpawner == 12)
            {
                GameObject powerUpInstantiatedObject = Instantiate(
                    SheildObject,
                    new Vector2(Random.Range(-5, 5), 9),
                    transform.rotation);
                powerUpInstantiatedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1f);
            }
            else
            {
                Debug.Log("None");
            }
            powerUpScore = 0;
        }
    } //Instantiates PowerUps


    //UI Handlers (Updates UI After a specific event has been fulfilled
    public void CheckNumberOfLivesAndUpdate()
    {
        if (numberOfLives == 0)
        {
            onOFFLiveUI1.DontSetActiveObject();
            onOFFLiveUI2.DontSetActiveObject();
            onOFFLiveUI3.DontSetActiveObject();
        }
        else if (numberOfLives == 1)
        {
            onOFFLiveUI1.SetActiveObject();
            onOFFLiveUI2.DontSetActiveObject();
            onOFFLiveUI3.DontSetActiveObject();
        }
        else if (numberOfLives == 2)
        {
            onOFFLiveUI1.SetActiveObject();
            onOFFLiveUI2.SetActiveObject();
            onOFFLiveUI3.DontSetActiveObject();
        }
        else if (numberOfLives == 3)
        {
            onOFFLiveUI1.SetActiveObject();
            onOFFLiveUI2.SetActiveObject();
            onOFFLiveUI3.SetActiveObject();
        }
        else if (numberOfLives == -1)
        {
            Debug.Log("Game Over");
            //FindObjectOfType<Level>().LoadGameOver();
        }
    }
    public void RPGUpdate0UI()
    {
        onOFFRocketUI1.DontSetActiveObject();
        onOFFRocketUI2.DontSetActiveObject();
        onOFFRocketUI3.DontSetActiveObject();
        numberOfRockets = 0;
    }
    public void RPGUpdate1UI()
    {
        onOFFRocketUI1.SetActiveObject();
        onOFFRocketUI2.DontSetActiveObject();
        onOFFRocketUI3.DontSetActiveObject();
        numberOfRockets = 1;
    }
    public void RPGUpdate2UI()
    {
        onOFFRocketUI1.SetActiveObject();
        onOFFRocketUI2.SetActiveObject();
        onOFFRocketUI3.DontSetActiveObject();
        numberOfRockets = 2;
    }
    public void RPGUpdate3UI()
    {
        onOFFRocketUI1.SetActiveObject();
        onOFFRocketUI2.SetActiveObject();
        onOFFRocketUI3.SetActiveObject();
        numberOfRockets = 3;
    }

    //EDIT THIS SINCE IT DOES NOT DEACTIIVATE WHEN PROCEED TO NEXT LEVEL
    //SHOWGUIDE FOR PRESS C METHOD
    public IEnumerator ShowGuideforPressC()
    {
        //
        accessPressCSetActiveScript.SetActiveObject();
        yield return new WaitForSeconds(2);
        accessPressCSetActiveScript.DontSetActiveObject();
        StopCoroutine(ShowGuideforPressC());
    }

    public void ResetGame()
    {
        musicPlayerScript.EnablePair();
        findLivesdone = false;
        isPlayerDestroyed = false;
        score = 0;
        powerUpScore = 0;
        numberOfEnemiesInScene = 0;
        numberOfDestroyedEnemies = 0;
        numberOfRockets = 0;
        RPGUpdate0UI();
        onOffLivesGroup.SetActiveObject();
        onOffRocketGroup.SetActiveObject();
        playerObjectScript.DisableRocketHeadShow();
        playerObjectScript.RestartHeavyMachineGun();
        //playerObjectScript.SheildRestart();
        playerObjectScript.TurnOffLockPlayerMovement();
        canvasScoreActivateDeactivate.SetActiveObject();
        TurnOnLevelOne();
        playerObjectScript.RestartLevelNumber();
        doLevelOneCourotineOnce = true;
        doLevelTwoCourotineOnce = true;
        doLevelThreeCourotineOnce = true;
        doLevelFourCourotineOnce = true;
        doLevelFiveCourotineOnce = true;
        TurnOFFGamesessionBoolPlayerisDestroyed();
        StopAllCoroutines();
        ResetTheEnemySpawnersBoolToOriginalSettings();

        enemySpawnerObjectLevelTwo.SetActive(false);
        enemySpawnerObjectLevelThree.SetActive(false);
        enemySpawnerObjectLevelFour.SetActive(false);
        enemySpawnerObjectLevelFive.SetActive(false);
        bossSpawnerObjectLevelTwo.SetActive(false);
        bossSpawnerObjectLevelThree.SetActive(false);
        bossSpawnerObjectLevelFour.SetActive(false);
        bossSpawnerObjectLevelFive.SetActive(false);

        //Level3Objects
        mMeteors.SetActive(false);
        mMeteorSpawner.SetActive(false);
        mPeanuts.SetActive(false);
        mBounds.SetActive(false);
        mBossPeanut.SetActive(false);
    }

    //Called to Returns Values or Change values
    public void TurnOnIsBossCoroutineDone() { isBossSpawnerCoroutineDone = true; }
    public void TurnOffIsBossCoroutineDone() { isBossSpawnerCoroutineDone = false; }
    public void TurnOnBoolIsEnemySpawnerCouRoutineDone() { isEnemySpawnerCourotineDone = true; }
    public void TurnOffBoolIsEnemySpawnerCouRoutineDone() { isEnemySpawnerCourotineDone = false; }
    public void TurnOnCheckIfPlayerIsDestroyed() { isPlayerDestroyed = true; } //PlayerScipt Activates on Die
    public void AddToScore(int scoreValue) { score += scoreValue; powerUpScore += scoreValue; }
    public void Addlives() { numberOfLives = Mathf.Clamp(numberOfLives + 1, -1, 3); }
    public void Subtractlives() { numberOfLives = Mathf.Clamp(numberOfLives - 1, -1, 3); }
    public void AddDestroyEnemies() { numberOfDestroyedEnemies++; }
    public void AddEnemies() { numberOfEnemiesInScene++; }
    public int ReturnEnemies() { return numberOfEnemiesInScene; }
    public int ReturnDestroyedEnemies() { return numberOfDestroyedEnemies; }
    public int GetNumberOfRockets() { return numberOfRockets; }
    public int GetScore() { return score; }

    public IEnumerator MissonCompletePrompt()
    {
        StartCoroutine(musicPlayerScript.StartFade(2, 0));
        LevelIntroductionText.text = "Mission Complete";
        introTextSetActiveDeactivate.SetActiveObject();
        musicPlayerScript.PlayMissionCompleted();
        yield return new WaitForSeconds(2);
        introTextSetActiveDeactivate.DontSetActiveObject();
        StopCoroutine(musicPlayerScript.StartFade(2, 0));
        playerObjectScript.TurnOnLockPlayerMovement();
        playerObjectScript.TurnOffPlayerMovementForward();
        isBossSpawnerCoroutineDone = false;

        mMeteors.SetActive(false);
        mMeteorSpawner.SetActive(false);
        mPeanuts.SetActive(false);
        mBounds.SetActive(false);
        //The rest of the code to transition to another level is in player object script
        //this is because the level will initiate once player moves to point offscreen
        //Too lazy to make coroutine
    }

    //=================SPAWNER METHODS=======
    private void LevelOneEnemySpawnerBossSpawnerMethod()
    {
        if (isBossSpawnerCoroutineDone)
        {
            if (numberOfDestroyedEnemies == numberOfEnemiesInScene)
            {
                Debug.Log("Initializing Mission complete");
                StartCoroutine(MissonCompletePrompt());
                isBossSpawnerCoroutineDone = false;
            }
        }

        if (isEnemySpawnerCourotineDone) //also checks if all enemies have been destroyed
        { // returns false when enemies destroyed and enemyspawner not set active

            ; if (numberOfDestroyedEnemies == numberOfEnemiesInScene)
            {//SetActive Spawner does not restart Coroutine
                if (isNotFirstTimeBossSpawner)
                {//If not activate first time then manually start coroutine
                    Debug.Log("Activating Boss Spawnere");
                    bossSpawnerSetActiveScriptOne.SetActiveObject();
                    StartCoroutine(bossSpawnerScriptLevelOne.SpawnAllWaves());
                    musicPlayerScript.ChangeBGMtoBossFightMusic();
                    enemySpawnerSetActiveScriptLevelOne.DontSetActiveObject();
                    isEnemySpawnerCourotineDone = false;
                }
                else
                {
                    Debug.Log("Activating Boss Spawnere");
                    bossSpawnerSetActiveScriptOne.SetActiveObject();
                    musicPlayerScript.ChangeBGMtoBossFightMusic();
                    enemySpawnerSetActiveScriptLevelOne.DontSetActiveObject();
                    isEnemySpawnerCourotineDone = false;

                    //code to activate bool should be here instead of bosspawner coroutine
                    // because what it player kills boss before boss spawner coroutine finishes?
                    if (!isNotFirstTimeBossSpawner)
                    {
                        isNotFirstTimeBossSpawner = true;
                    }
                }
            }
        }
    }

    private void LevelTwoEnemySpawnerBossSpawnerMethod()
    {
        if (isBossSpawnerCoroutineDone)
        {
            if (numberOfDestroyedEnemies == numberOfEnemiesInScene)
            {
                Debug.Log("Initializing Mission complete");
                StartCoroutine(MissonCompletePrompt());
                isBossSpawnerCoroutineDone = false;
            }
        }

        if (isEnemySpawnerCourotineDone) //also checks if all enemies have been destroyed
        { // returns false when enemies destroyed and enemyspawner not set active

            if (numberOfDestroyedEnemies == numberOfEnemiesInScene)
            {//SetActive Spawner does not restart Coroutine
                if (isNotFirstTimeForLevelTwoBossSpawner)
                {//If not activate first time then manually start coroutine
                    Debug.Log("Activating Boss Spawnere");
                    bossSpawnerSetActiveScriptTwo.SetActiveObject();
                    StartCoroutine(bossSpawnerScriptLevelTwo.SpawnAllWaves());
                    musicPlayerScript.ChangeBGMtoBossFightMusicLevelTwo();
                    enemySpawnerSetActiveScriptLevelTwo.DontSetActiveObject();
                    isEnemySpawnerCourotineDone = false;
                }
                else
                {
                    Debug.Log("Activating Boss Spawnere");
                    bossSpawnerSetActiveScriptTwo.SetActiveObject();
                    musicPlayerScript.ChangeBGMtoBossFightMusicLevelTwo();
                    enemySpawnerSetActiveScriptLevelTwo.DontSetActiveObject();
                    isEnemySpawnerCourotineDone = false;

                    if (!isNotFirstTimeForLevelTwoBossSpawner)
                    {
                        isNotFirstTimeForLevelTwoBossSpawner = true;
                    }
                }
            }
        }
    }


    private void LevelThreeEnemySpawnerBossSpawnerMethod()
    {
        if (isBossSpawnerCoroutineDone)
        {
            if (numberOfDestroyedEnemies == numberOfEnemiesInScene)
            {
                Debug.Log("Initializing Mission complete");
                StartCoroutine(MissonCompletePrompt());
                isBossSpawnerCoroutineDone = false;
                //levelTwoOn = false;
            }
        }

        if (isEnemySpawnerCourotineDone) //also checks if all enemies have been destroyed
        { // returns false when enemies destroyed and enemyspawner not set active

            if (numberOfDestroyedEnemies == numberOfEnemiesInScene)
            {//SetActive Spawner does not restart Coroutine
                if (isNotFirstTimeForLevelThreeBossSpawner)
                {//If not activate first time then manually start coroutine
                    Debug.Log("Activating Boss Spawnere");
                    bossSpawnerSetActiveScriptThree.SetActiveObject();
                    StartCoroutine(bossSpawnerScriptLevelThree.SpawnAllWaves());
                    musicPlayerScript.ChangeBGMtoBossFightMusic();
                    enemySpawnerSetActiveScriptLevelThree.DontSetActiveObject();
                    isEnemySpawnerCourotineDone = false;
                }
                else
                {
                    Debug.Log("Activating Boss Spawnere");
                    bossSpawnerSetActiveScriptThree.SetActiveObject();
                    musicPlayerScript.ChangeBGMtoBossFightMusic();
                    enemySpawnerSetActiveScriptLevelThree.DontSetActiveObject();
                    isEnemySpawnerCourotineDone = false;

                    if (!isNotFirstTimeForLevelThreeBossSpawner)
                    {
                        isNotFirstTimeForLevelThreeBossSpawner = true;
                    }
                }
            }
        }
    }


    private void LevelFourEnemySpawnerBossSpawnerMethod()
    {
        if (isBossSpawnerCoroutineDone)
        {
            if (numberOfDestroyedEnemies == numberOfEnemiesInScene)
            {
                Debug.Log("Initializing Mission complete");
                StartCoroutine(MissonCompletePrompt());
                isBossSpawnerCoroutineDone = false;
                //levelTwoOn = false;
            }
        }

        if (isEnemySpawnerCourotineDone) //also checks if all enemies have been destroyed
        { // returns false when enemies destroyed and enemyspawner not set active

            if (numberOfDestroyedEnemies == numberOfEnemiesInScene)
            {//SetActive Spawner does not restart Coroutine
                if (isNotFirstTimeForLevelFourBossSpawner)
                {//If not activate first time then manually start coroutine
                    Debug.Log("Activating Boss Spawnere");
                    bossSpawnerSetActiveScriptFour.SetActiveObject();
                    StartCoroutine(bossSpawnerScriptLevelFour.SpawnAllWaves());
                    musicPlayerScript.ChangeBGMtoBossFightMusic();
                    enemySpawnerSetActiveScriptLevelFour.DontSetActiveObject();
                    isEnemySpawnerCourotineDone = false;
                }
                else
                {
                    //Debug.Log("Activating Boss Spawnere");
                    bossSpawnerSetActiveScriptFour.SetActiveObject();
                    musicPlayerScript.ChangeBGMtoBossFightMusic();
                    enemySpawnerSetActiveScriptLevelFour.DontSetActiveObject();
                    isEnemySpawnerCourotineDone = false;

                    if (!isNotFirstTimeForLevelFourBossSpawner)
                    {
                        isNotFirstTimeForLevelFourBossSpawner = true;
                    }
                }
            }
        }
    }


    private void LevelFiveEnemySpawnerBossSpawnerMethod()
    {
        if (isBossSpawnerCoroutineDone)
        {
            if (numberOfDestroyedEnemies == numberOfEnemiesInScene)
            {
                Debug.Log("Initializing Mission complete");
                StartCoroutine(MissonCompletePrompt());
                isBossSpawnerCoroutineDone = false;
                //levelTwoOn = false;
            }
        }

        if (isEnemySpawnerCourotineDone) //also checks if all enemies have been destroyed
        { // returns false when enemies destroyed and enemyspawner not set active

            if (numberOfDestroyedEnemies == numberOfEnemiesInScene)
            {//SetActive Spawner does not restart Coroutine
                if (isNotFirstTimeForLevelFiveBossSpawner)
                {//If not activate first time then manually start coroutine
                    Debug.Log("Activating Boss Spawnere");
                    bossSpawnerSetActiveScriptFive.SetActiveObject();
                    StartCoroutine(bossSpawnerScriptLevelFive.SpawnAllWaves());
                    musicPlayerScript.ChangeBGMtoBossFightMusic();
                    enemySpawnerSetActiveScriptLevelFive.DontSetActiveObject();
                    isEnemySpawnerCourotineDone = false;
                }
                else
                {
                    //Debug.Log("Activating Boss Spawnere");
                    bossSpawnerSetActiveScriptFive.SetActiveObject();
                    musicPlayerScript.ChangeBGMtoBossFightMusic();
                    enemySpawnerSetActiveScriptLevelFive.DontSetActiveObject();
                    isEnemySpawnerCourotineDone = false;

                    if (!isNotFirstTimeForLevelFiveBossSpawner)
                    {
                        isNotFirstTimeForLevelFiveBossSpawner = true;
                    }
                }
            }
        }
    }



    public void Level1Start()
    {
        if (isNotFirstTime)
        {
            StopCoroutine(enemySpawnerScriptLevelOne.SpawnAllWavesDelayed());
        }
        numberOfLives = 3;
        CheckNumberOfLivesAndUpdate();
        playerObjectScript.SetPlayerActive();
        StartCoroutine(LevelOneIntroduction());
        StartCoroutine(enemySpawnerScriptLevelOne.SpawnAllWavesDelayed());
        isNotFirstTime = true;
    }

    //==========INTRODUCTION COROUTINES===============
    public IEnumerator LevelOneIntroduction()
    {
        //enemySpawnerSetActiveScript.DontSetActiveObject();
        LevelIntroductionText.text = "Level One";
        introTextSetActiveDeactivate.SetActiveObject();
        musicPlayerScript.PlayLevelOneSound();
        yield return new WaitForSeconds(2);
        introTextSetActiveDeactivate.DontSetActiveObject();
        enemySpawnerSetActiveScriptLevelOne.SetActiveObject();
        StopCoroutine(LevelOneIntroduction());
    }
    public IEnumerator LevelTwoIntroduction()
    {
        LevelIntroductionText.text = "Level Two";
        introTextSetActiveDeactivate.SetActiveObject();
        musicPlayerScript.PlayLevelTwoSound();
        yield return new WaitForSeconds(2);
        introTextSetActiveDeactivate.DontSetActiveObject();
        enemySpawnerSetActiveScriptLevelTwo.SetActiveObject();

        if (isNotFirstTimeForLevelTwo)
        {
            StartCoroutine(enemySpawnerScriptLevelTwo.SpawnAllWaves());
        }

        StopCoroutine(LevelTwoIntroduction());
    }
    public IEnumerator LevelThreeIntroduction()
    {
        mMeteors.SetActive(true);
        mMeteorSpawner.SetActive(true);
        mPeanuts.SetActive(true);
        mBounds.SetActive(true);
        LevelIntroductionText.text = "Level Three";
        introTextSetActiveDeactivate.SetActiveObject();
        musicPlayerScript.PlayLevelThreeSound();
        yield return new WaitForSeconds(2);
        introTextSetActiveDeactivate.DontSetActiveObject();
        enemySpawnerSetActiveScriptLevelThree.SetActiveObject();

        if (isNotFirstTimeForLevelThree)
        {
            StartCoroutine(enemySpawnerScriptLevelThree.SpawnAllWaves());
        }

        StopCoroutine(LevelTwoIntroduction());
    }
    public IEnumerator LevelFourIntroduction()
    {
        LevelIntroductionText.text = "Level Four";
        introTextSetActiveDeactivate.SetActiveObject();
        musicPlayerScript.PlayLevelFourSound();
        yield return new WaitForSeconds(2);
        introTextSetActiveDeactivate.DontSetActiveObject();
        enemySpawnerSetActiveScriptLevelFour.SetActiveObject();

        if (isNotFirstTimeForLevelFour)
        {
            StartCoroutine(enemySpawnerScriptLevelFour.SpawnAllWaves());
        }

        StopCoroutine(LevelFourIntroduction());
    }
    public IEnumerator LevelFiveIntroduction()
    {
        LevelIntroductionText.text = "Level Five";
        introTextSetActiveDeactivate.SetActiveObject();
        musicPlayerScript.PlayFinalRoundSound();
        yield return new WaitForSeconds(2);
        introTextSetActiveDeactivate.DontSetActiveObject();
        enemySpawnerSetActiveScriptLevelFive.SetActiveObject();

        if (isNotFirstTimeForLevelFive)
        {
            StartCoroutine(enemySpawnerScriptLevelFive.SpawnAllWaves());
        }

        StopCoroutine(LevelFiveIntroduction());
    }


    //====PUBLIC BOOLS=====
    public void TurnOnLevelOne()
    {
        levelOneOn = true;
        levelTwoOn = false;
        levelThreeOn = false;
        levelFourOn = false;
        levelFiveOn = false;
    }

    public void TurnOnLevelTwo()
    {
        levelOneOn = false;
        levelTwoOn = true;
        levelThreeOn = false;
        levelFourOn = false;
        levelFiveOn = false;
    }

    public void TurnOnLevelThree()
    {
        levelOneOn = false;
        levelTwoOn = false;
        levelThreeOn = true;
        levelFourOn = false;
        levelFiveOn = false;
    }

    public void TurnOnLevelFour()
    {
        levelOneOn = false;
        levelTwoOn = false;
        levelThreeOn = false;
        levelFourOn = true;
        levelFiveOn = false;
    }

    public void TurnOnLevelFive()
    {
        levelOneOn = false;
        levelTwoOn = false;
        levelThreeOn = false;
        levelFourOn = false;
        levelFiveOn = true;
    }

    public void TurnOnLevelSix()
    {
        levelOneOn = false;
        levelTwoOn = false;
        levelThreeOn = false;
        levelFourOn = false;
        levelFiveOn = true;
    }

    //===PUBLIC COMMANDS===
    public void GameSessionSetToLevelTwo() { levelNumber = 2; }
    public void GameSessionSetToLevelThree() { levelNumber = 3; }
    public void GameSessionSetToLevelFour() { levelNumber = 4; }
    public void GameSessionSetToLevelFive() { levelNumber = 5; }

    public void TurnOFFGamesessionBoolPlayerisDestroyed() { playerIsDestroyed = false; }
    public void TurnONGamesessionBoolPlayerisDestroyed() { playerIsDestroyed = true; }
    public bool GetGameSessionPlayerIsDestroyedValue() { return playerIsDestroyed; }

    public void Turn_ON_GamesessionBool_is_LeftHandMinion_in_Scene() { is_LeftHandMinion_in_scene = true; }
    public void Turn_OFF_GamesessionBool_is_LeftHandMinion_in_Scene() { is_LeftHandMinion_in_scene = false; }
    public bool GetValue_GamesessionBool_is_LeftHandMinion_in_Scene() { return is_LeftHandMinion_in_scene; }

    public void Turn_ON_GamesessionBool_is_RightHandMinion_in_Scene() { is_RightHandMinion_in_scene = true; }
    public void Turn_OFF_GamesessionBool_is_RightHandMinion_in_Scene() { is_RightHandMinion_in_scene = false; }
    public bool GetValue_GamesessionBool_is_RightHandMinion_in_Scene() { return is_RightHandMinion_in_scene; }

    public void Turn_On_TriggerBool_Fire_Upwards() { hand_enemy_fireUpwards = true; }
    public void Turn_Off_TriggerBool_Fire_Upwards() { hand_enemy_fireUpwards = false; }
    public bool Get_Value_TriggerBool_Fire_Upwards() { return hand_enemy_fireUpwards; }

    public GameObject Get_Player_GameObject() { return playerObject; }

}


