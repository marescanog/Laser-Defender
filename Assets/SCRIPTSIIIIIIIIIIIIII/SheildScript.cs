using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheildScript : MonoBehaviour
{
    // config parameters
    [Header("Sheild")]
    [SerializeField] GameObject parentObject;
    [SerializeField] GameObject sheildHealthBar;
    [SerializeField] GameObject gameSession;
    [SerializeField] int health = 1000;
    [SerializeField] private GameObject deathVFX;
    [SerializeField] float durationOFHitExplosion = 1f;
    [SerializeField] GameObject rocketHead;

    [Header("Sounds")]
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;
    float getHitSoundVolume;

    Vector2 playerPos;
    GameObject hitVFX;
    SpriteRenderer mySpriteRenderer;
    AudioClip getHitSound;
    Player parentPlayer;
    AudioClip getHitLaserSound;
    AudioClip getHitBombSound;
    AudioClip getHitLaseBombSound;
    GameObject LaserombHitVFX;
    GameObject bombHitVFX;
    GameObject laserHitVFX;
    float otherObjTranPosX;
    SheildHealthBarScript sheildHealthBarScript;
    bool laserBomb = false;

    // Start is called before the first frame update
    void Start()
    {
        parentPlayer = parentObject.GetComponent<Player>();
        sheildHealthBarScript = sheildHealthBar.GetComponent<SheildHealthBarScript>();
        LaserombHitVFX = parentPlayer.GetLaserBombHitVFXFromPlayerParent();
        bombHitVFX = parentPlayer.GetBombHitVFXFromPlayerParent();
        laserHitVFX = parentPlayer.GetLaserHitVFXFromPlayerParent();
        getHitLaserSound = parentPlayer.GetLaserHitSoundSFXFromPlayerParent();
        getHitLaseBombSound = parentPlayer.GetLaserBombHitSoundSFXFromPlayerParent();
        getHitBombSound = parentPlayer.GetBombHitSFXFromPlayerParent();
        getHitSoundVolume = parentPlayer.GetHitSoundVolumeFromPlayerParent();

    }

    // Update is called once per frame
    void Update()
    {

        //Code to feed playr's current position for the HitPlayerVFX position
        playerPos = gameObject.transform.position;
        mySpriteRenderer = GetComponent<SpriteRenderer>();

    }
    void OnParticleCollision(GameObject other)
    {
        if (other.name == "Particle launcher")
        {
            health--;
            if (health <= 0)
            {
                Die();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Gets the damage dealer of the thing it collided with
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();

        //If collides with main player subtracts damage value from health
        //Depending on the Tag will Destroy other game object
        if (other.gameObject.tag == "EnemyLaser")
        {
            otherObjTranPosX = other.gameObject.transform.position.x;
            getHitSound = getHitLaserSound;
            hitVFX = laserHitVFX;
            ProcessHit(damageDealer);
            //Debug.Log("1");
        }
        else if (other.gameObject.tag == "BEnemyBomb")
        {
            otherObjTranPosX = other.gameObject.transform.position.x;
            getHitSound = getHitBombSound;
            hitVFX = bombHitVFX;
            ProcessHit(damageDealer);
            //Debug.Log("2");
           damageDealer.OnHitDestroyOtherObject();
        }
        else if (other.gameObject.tag == "BEnemyLaserBomb")
        {
            laserBomb = true;
            otherObjTranPosX = other.gameObject.transform.position.x;
            getHitSound = getHitLaseBombSound;
            hitVFX = LaserombHitVFX;
            ProcessHit(damageDealer);
            //Debug.Log("2");
           damageDealer.OnHitDestroyOtherObject();
        }
        else if (other.gameObject.tag == "PowerUP")
            {parentPlayer.PowerUpMethod(damageDealer);}

        else if (other.gameObject.tag == "HeavyMachineGun")
            {parentPlayer.HeavyMachineGunPowerUpMethod(damageDealer);}

        else if (other.gameObject.tag == "RocketLauncher")
            {parentPlayer.RocketLauncherPowerUPMethod(damageDealer);}

        else if (other.gameObject.tag == "Sheild")
            {parentPlayer.SheildPowerUpmethod(damageDealer); health = 1000; }

        else if (other.gameObject.tag == "ExtraLife")
            {parentPlayer.ExtraLifePowerUpMethod(damageDealer);}

        else
        {
            otherObjTranPosX = other.gameObject.transform.position.x;
            getHitSound = getHitLaserSound;
            hitVFX = laserHitVFX;
            ProcessHit(damageDealer);
            //Debug.Log("3");
        }
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        if (damageDealer == null)
        {
            Debug.Log("Object Collider with SheildScript DamageDealer is Null");
        }
        else
        {
            health -= damageDealer.GetDamage();

            PlayHitPlayerVFX();

            if (health <= 0)
            {
                Die();
            }
        }

    }

    private void PlayHitPlayerVFX()
    {
        if (laserBomb)
        {
            GameObject vfx = Instantiate(
                hitVFX,
                new Vector2(otherObjTranPosX, playerPos.y + 1f),
                //get Y position of the projectile. Thus create a return method for the projectile?
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
new Vector2(otherObjTranPosX, playerPos.y + 1f), //get Y position of the projectile. Thus create a return method for the projectile?
Quaternion.identity);
            vfx.transform.parent = gameObject.transform;
            Destroy(vfx, 1f);
            AudioSource.PlayClipAtPoint(getHitSound, Camera.main.transform.position, getHitSoundVolume);
        }

    }

    private void Die()
    {
        parentPlayer.TurnOffSheild();
        //Creates a Death VFX
        GameObject explosion = Instantiate(
           deathVFX,
           transform.position,
           transform.rotation);
        //Destroys the Death VFX
        Destroy(explosion, durationOFHitExplosion);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);

        DontSetActiveSheildHealth();
    }

    public void DontSetActiveSheildHealth()
    {
        sheildHealthBarScript.DontSetActiveSheildHealthBar();
        DontSetActiveSheildMethod();
        health = 0;
        parentPlayer.SheildUINotVisible();
    }

    public int GetSheildHealth() {return health;} //Accessed by Sheild health bar
    public void SetActiveSheildMethod() {gameObject.SetActive(true); health = 1000;}
    private void DontSetActiveSheildMethod() {gameObject.SetActive(false);}
}
