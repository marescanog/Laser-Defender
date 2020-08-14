using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandMinion : MonoBehaviour
{
    [Header("Pathing")]
    [SerializeField] float movementSpeed = 1;
    [SerializeField] List<GameObject> listOfWaypointGroups;
    [SerializeField] int scoreValue = 75;

    [Header("Transparency")]
    [SerializeField] Color transparentColor;
    [SerializeField] Color OpaqueColor;

    [Header("Shooting")]
    [SerializeField] bool enableShooting = false;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = -10f;
    [SerializeField] float projectileRotation = 250f;
    [SerializeField] float projectileOffsetX = 0f;
    [SerializeField] float projectileOffsetY = 0f;

    [Header("Health Color")]
    [SerializeField] float health = 5000;
    [SerializeField] float maxhealth = 5000;

    [Header("VisualFX/SoundFX")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOFExplosion = 1f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;

    List<Transform> waypointList1;
    List<Transform> waypointList2;
    int waypointList1Index = 0;
    int waypointList2Index = 0;

    GameObject bossBrainy;
    GameObject playerObject;
    GameObject orbRight;

    SpriteRenderer mySpriteRenderer;
    Transform myTransform;
    GameSession gameSessionScript;
    Player playerScript;

    float currentAngleValue = 0;
    float rotSpeed = 300;
    float lerpToRedorWhiteCount = 0f;
    float shotCounter;

    Vector3 slapPotsition;
    Vector3 playerPosition;

    bool rotPlayerPositionReaached = false;
    bool rotSlapPositionReaached = false;
    bool phaseInDone = false;
    bool detroyWhilePhasing = false;
    bool slapPositionReached = false;
    bool addOnce = true;
    bool phasingOngoing = true;

    [SerializeField] bool playerIsNotActive;

    ///For cached checks
    bool keep_looking_for_player = false;
    bool keep_looking_for_boss = false;

    bool fireUpwards = false;

    // Start is called before the first frame update
    void Start()
    {
        //Caching Refereneces
        gameSessionScript = FindObjectOfType<GameSession>();
        myTransform = GetComponent<Transform>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        orbRight = GameObject.Find("Orb Right");

        ///Check and Cache
        ///These Two if Statements checks if objects are null
        ///=========IF  PLAYER NULL
        if (FindObjectsOfType<Player>().Length == 0)
        {
            Debug.Log("no Player Object Found");
            keep_looking_for_player = true;
        }
        else
        {
            playerObject = GameObject.Find("Player");
            playerScript = playerObject.GetComponent<Player>();
        }

        ///=========IF  BOSS NULL
        if (GameObject.Find("Brain") == null)
        {
            Debug.Log("no Boss Object Found");
            keep_looking_for_boss = true;
        }
        else
        {
            bossBrainy = GameObject.Find("Boss Brainy(Clone)");
            Debug.Log("boss object found");
        }

        //Initializing Values
        mySpriteRenderer.color = transparentColor;
        enableShooting = false;
        gameSessionScript.Turn_ON_GamesessionBool_is_RightHandMinion_in_Scene();
        ///
        CachingTransformPositionReferences();
    }

    // Update is called once per frame
    void Update()
    {
        fireUpwards = gameSessionScript.Get_Value_TriggerBool_Fire_Upwards();

        if (keep_looking_for_player)
        {
            if (FindObjectsOfType<Player>().Length == 1)
            {
                Debug.Log("Player Object Found");
                playerObject = GameObject.Find("Player");
                playerScript = playerObject.GetComponent<Player>();
                keep_looking_for_player = false;
            }
        }

        playerIsNotActive = gameSessionScript.GetGameSessionPlayerIsDestroyedValue();

        PhaseingInMethod();

        InitiateSlap();

        Movement();

        if (!phasingOngoing)
        {
            ChangeColorOnHit();
        }


        if (enableShooting)
        {
            CountDownAndShoot();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (phasingOngoing)
        {
            //do nothing
        }
        else
        {
            //Gets the damage dealer of the thing it collided with
            //Second code is to protect from Null Instance where there is no Damage Dealer Value
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
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        //damageDealer.Hit();
        if (health <= 0)
        {
            if (gameObject.tag == "Enemy")
            {

                Die();
            }
            else
            {
                FindObjectOfType<ParentEnemy>().OnHitDestroyParent();
                Die();
            }


        }
    }

    private void Die()
    {
        //To call a method from anther script use FindObjectOfType
        //We are looking for the GameSession script
        //Calling the AddToScore method from that script
        //we are putting in the score value Delclared in the parameters above via "scoreValue"

        if (addOnce)
        {
            gameSessionScript.AddToScore(scoreValue);
            orbRight.GetComponent<OrbScript>().ResetRightHandSpawnerTimer();
            addOnce = false;
            gameSessionScript.Turn_OFF_GamesessionBool_is_RightHandMinion_in_Scene();
        }

        Instantiate_Explosion_On_Death_HandMinionRight();

        Destroy(gameObject);

    }

    public void Instantiate_Explosion_On_Death_HandMinionRight()
    {
        GameObject explosion = Instantiate(
            deathVFX,
            transform.position,
            transform.rotation);
        Destroy(explosion, durationOFExplosion);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }

    private void ChangeColorOnHit()
    {
        mySpriteRenderer.color = Color.Lerp(Color.red, Color.white, (health / maxhealth));
    }
    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        if (fireUpwards)
        {
            projectileSpeed = 3;
        }
        else { projectileSpeed = -3; }

        GameObject laser = Instantiate(
           projectile,
           new Vector2(myTransform.position.x + projectileOffsetX, myTransform.position.y + projectileOffsetY),
           Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
        laser.GetComponent<Rigidbody2D>().angularVelocity = projectileRotation;
        //AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position,
        //    shootSoundVolume);
    }

    private void InitiateSlap()
    {
        if (slapPositionReached)
        {
            currentAngleValue += Time.deltaTime * rotSpeed;
            myTransform.eulerAngles = new Vector3(0, 0, currentAngleValue);
            //Debug.Log(currentAngleValue);

            if (currentAngleValue >= 359)
            {
                slapPositionReached = false;
                rotPlayerPositionReaached = false;
                rotSlapPositionReaached = false;
                currentAngleValue = 0;
                waypointList2Index = 0;
            }

            if (!rotPlayerPositionReaached)
            {
                var targetPosition = playerPosition;
                var movementThisFrame = 3 * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);
                if (transform.position == targetPosition)
                {
                    //donothing
                    Debug.Log("PlayerPosition Resched");
                    rotPlayerPositionReaached = true;
                }
            }

            if ((rotPlayerPositionReaached) && (!rotSlapPositionReaached))
            {
                var targetPosition = slapPotsition;
                var movementThisFrame = 3 * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

                if (transform.position == targetPosition)
                {
                    //donothing
                    Debug.Log("SlapPosition Resched");
                    rotSlapPositionReaached = true;
                }
            }
        }
    }

    private void Movement()
    {
        if (phaseInDone)
        {
            ///<summary> Movement has 3 Phases
            ///<para> First Phase is the Entrance
            ///<para> Second Phase is Left To write
            ///<para> Third Phase is Approach Player
            ///</summary> Movement Loops back to second phase
            ///
            if (keep_looking_for_boss)
            {
                myTransform.position = orbRight.transform.position;
            }
            else if (waypointList1Index <= waypointList1.Count - 1)
            {
                var targetPosition = waypointList1[waypointList1Index].transform.position
                    + bossBrainy.transform.position;
                var movementThisFrame = movementSpeed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

                if (transform.position == targetPosition)
                {
                    waypointList1Index++;
                }


            }
            else
            {

                if (waypointList2Index <= waypointList2.Count - 1)
                {
                    var targetPosition = waypointList2[waypointList2Index].transform.position;
                    var movementThisFrame = movementSpeed * Time.deltaTime;

                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

                    if (transform.position == targetPosition)
                    {
                        if (waypointList2Index == 0)
                        {
                            enableShooting = true;
                            Debug.Log("Enable Shooting");
                        }

                        waypointList2Index++;


                        if (waypointList2Index > waypointList2.Count - 1)
                        {
                            enableShooting = false;
                            Debug.Log("Disable Shooting");
                        }

                    }
                }
                else
                {
                    if (keep_looking_for_player)
                    {
                        waypointList2Index = 0;
                        Debug.Log("No Player Found, restarting Loop");
                    }
                    else if (!slapPositionReached)
                    {
                        if (playerIsNotActive)
                        {
                            waypointList2Index = 0;
                            Debug.Log("Player is Not Active, Hand minion restart Loop");
                        }
                        else if (!playerIsNotActive)
                        {
                            var targetPosition = playerObject.transform.position + new Vector3(0, 1.5f, 0);
                            var movementThisFrame = movementSpeed * Time.deltaTime;

                            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

                            if (transform.position == targetPosition)
                            {
                                if (playerIsNotActive)
                                {
                                    waypointList2Index = 0;
                                    Debug.Log("Player is Not Active, Hand minion restart Loop");
                                }
                                else
                                {
                                    slapPotsition = playerObject.transform.position + new Vector3(0, 1.5f, 0);
                                    playerPosition = playerObject.transform.position;
                                    Debug.Log("Start Slap");
                                    slapPositionReached = true;
                                }

                            }
                        }

                    }

                }

            }
        }
    }

    private void PhaseingInMethod()
    {
        if (lerpToRedorWhiteCount <= 100.0f)
        {
            myTransform.position = orbRight.transform.position;
            lerpToRedorWhiteCount += Time.deltaTime * 25f;
            mySpriteRenderer.color = Color.Lerp(transparentColor, OpaqueColor, (lerpToRedorWhiteCount / 100));

            if (detroyWhilePhasing)
            {
                detroyWhilePhasing = false;
                Debug.Log("Destoryed While Phasing");
                gameSessionScript.Turn_OFF_GamesessionBool_is_RightHandMinion_in_Scene();

                Instantiate_Explosion_On_Death_HandMinionRight();

                Destroy(gameObject);
            }
        }
        else if (lerpToRedorWhiteCount >= 100f)
        {
            phaseInDone = true;
            phasingOngoing = false;
        }
    }

    public void DestroyRightHandMinion() { detroyWhilePhasing = true; }

    //Caching Methods used at Start Method
    private void CachingTransformPositionReferences()
    {
        for (int i = 0; i <= listOfWaypointGroups.Count - 1; i++)
        {
            if (i == 0)
            {
                waypointList1 = GetHandWayPoints(i);
            }
            else if (i == 1)
            {
                waypointList2 = GetHandWayPoints(i);
            }
        }
    }

    private List<Transform> GetHandWayPoints(int blue)
    {
        var pathWaypoints = new List<Transform>();
        foreach (Transform child in listOfWaypointGroups[blue].transform)
        {
            pathWaypoints.Add(child);
        }
        return pathWaypoints;
    }
}
