using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] int scoreValue = 15;
    [SerializeField] bool spinAround = false;
    [SerializeField] bool spinRight = false;
    [SerializeField] float spinSpeed = 1;

    [Header("Shooting")]
    [SerializeField] bool enableShooting = true;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = -10f;
    [SerializeField] float projectileRotation = 250f;
    [SerializeField] float projectileOffsetX = 0f;
    [SerializeField] float projectileOffsetY = 0f;

    [Header("VisualFX/SoundFX")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOFExplosion = 1f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.75f;

    [Header("Health Color")]
    [SerializeField] bool ColorChangeActive = false;
    [SerializeField] Color myColor = Color.white;
    [SerializeField] Color myColorChange = Color.red;
    [SerializeField] float health = 100;
    [SerializeField] float maxhealth = 100;

    [Header("BossType")]
    [SerializeField] bool invincibilityOn = false;
    [SerializeField] bool bossBrainy = false;
    [SerializeField] bool bossPeanutMaker = false;

    // Variables
    Transform myTransform;
    float currentAngleValue;
    float shotCounter;
    SpriteRenderer mySpriteRenderer;
    Enemy myScriptEnemy;
    GameSession gameSession;
    bool addOnce = true;
    BossPeanutMaker bossPeanutScript;
    EnemyPathing mypathing;

    int countHitsForBossPeanut = 0;
    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myTransform = GetComponent<Transform>();
        myScriptEnemy = GetComponent<Enemy>();
        gameSession = FindObjectOfType<GameSession>();
        if (bossPeanutMaker)
        {
            bossPeanutScript = FindObjectOfType<BossPeanutMaker>().GetComponent<BossPeanutMaker>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpinningAnimation();
        ChangeColorOnHit();

        if(enableShooting)
        {
            if (gameObject.tag == "Enemy")
            {
                CountDownAndShoot();
            }
            else
            {
                //Debug.Log("Enemy.cs Script Line 42");
                //Do nothing if the Tagging is not "Enemy"
            }
        }

    }

    private void ChangeColorOnHit()
    {
        if (ColorChangeActive)
        {
            mySpriteRenderer.color = Color.Lerp(myScriptEnemy.myColorChange, myScriptEnemy.myColor, (health / maxhealth));
        }
    }

    private void SpinningAnimation()
    {
        if (spinAround)
        {
            if (spinRight)
            {
                currentAngleValue += Time.deltaTime * spinSpeed;
                myTransform.eulerAngles = new Vector3(0, 0, currentAngleValue);
            }
            if (!spinRight)
            {
                currentAngleValue -= Time.deltaTime * spinSpeed;
                myTransform.eulerAngles = new Vector3(0, 0, currentAngleValue);
            }
        }
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
        GameObject laser = Instantiate(
           projectile,
           new Vector2 (myTransform.position.x + projectileOffsetX, myTransform.position.y + projectileOffsetY),
           Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
        laser.GetComponent<Rigidbody2D>().angularVelocity = projectileRotation;
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, 
            shootSoundVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var otherGameObjectTag = other.gameObject.tag;
        
        //Gets the damage dealer of the thing it collided with
        //Second code is to protect from Null Instance where there is no Damage Dealer Value
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();

        if (!damageDealer) { return; }

        //If collides with main player then does not destroy other object, subtracts damage value from health
        if (bossPeanutMaker)
        {
            if (otherGameObjectTag == "Player Laser")
            {
                ProcessHit(damageDealer);

                if(other.gameObject.name== "Player Rocket(Clone)")
                {
                    countHitsForBossPeanut+=9 ;
                }
                countHitsForBossPeanut++;

                if(countHitsForBossPeanut>=10)
                {
                    //TriggerAnimation
                    bossPeanutScript.Start_EyePain_Anim(); countHitsForBossPeanut = 0;
                }

            }
        }
        else
        {
            if (otherGameObjectTag == "MainPlayer")
            {
                ProcessHit(damageDealer);
            }
            else if (otherGameObjectTag == "Player Laser")
            {
                ProcessHit(damageDealer);
            }
            else
            {
                //Debug.Log("hit");
            }
        }
            
    }

    
    private void ProcessHit(DamageDealer damageDealer)
    {
        if (bossBrainy)
        {
            health -= (damageDealer.GetDamage()*2);
        }
        else 
        {
            if(invincibilityOn)
            {
                //Nodamage
            }
            else 
            {
                health -= damageDealer.GetDamage();
            }
            
        }
            
        //damageDealer.Hit();
        if (health <= 0)
        {
            if (gameObject.tag == "Enemy")
            {
                Die();
            }
            else if (bossPeanutMaker)
            {
                bossPeanutScript.Start_BossPeanut_Death_Sequence();
                mypathing = GetComponent<EnemyPathing>();
                mypathing.Activate_Boss_Death_path();
                
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
        ///Has its own activation bool because if bool is activated then
        ///the minions it spawned will also die when it dies
        if (bossBrainy)
        {


            if (FindObjectsOfType<BrainSheildDome>().Length >= 1)
            {
                FindObjectOfType<BrainSheildDome>().StopCoRoutineDead();
                bossBrainy = false;
            }

            if (FindObjectsOfType<HandMinion>().Length >= 1)
            {
                gameSession.Turn_OFF_GamesessionBool_is_LeftHandMinion_in_Scene();
                var hand = GameObject.Find("Left handminion(Clone)");
                hand.GetComponent<HandMinion>().Instantiate_Explosion_On_Death_HandMinion_Left();
                Destroy(hand);
            }

            if (FindObjectsOfType<RightHandMinion>().Length >= 1)
            {
                gameSession.Turn_OFF_GamesessionBool_is_RightHandMinion_in_Scene();
                var hand = GameObject.Find("Right handminion(Clone)");
                hand.GetComponent<RightHandMinion>().Instantiate_Explosion_On_Death_HandMinionRight();
                Destroy(hand);
            }
        }

        //To call a method from anther script use FindObjectOfType
        //We are looking for the GameSession script
        //Calling the AddToScore method from that script
        //we are putting in the score value Delclared in the parameters above via "scoreValue"
        if (addOnce)
        {
            gameSession.AddToScore(scoreValue);
            gameSession.AddDestroyEnemies();
            addOnce = false;
        }

        //enemyValue
        Destroy(gameObject);
        GameObject explosion = Instantiate(
           deathVFX,
           transform.position,
           transform.rotation);
        Destroy(explosion, durationOFExplosion);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);

    }

    public void turnOnThisEnemiesInvincibility() { invincibilityOn = true; }

    public void turnOffThisEnemiesInvincibility() { invincibilityOn = false; }
}

