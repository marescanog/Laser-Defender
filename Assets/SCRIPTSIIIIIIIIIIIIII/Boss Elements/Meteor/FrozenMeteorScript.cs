using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenMeteorScript : MonoBehaviour
{
    [SerializeField] int healthMeteor = 100;
    [SerializeField] GameObject vfxExplosionSmoke;
    bool meteorSplit = false;
    int damageDealer = 0;

    [SerializeField] float immunityCounter = 0;
    [SerializeField] bool immunityOn = false;

    [SerializeField] GameObject meteorSpawnerObject;
    MeteorSpawnerScript meteorSpawnerScript;
    Rigidbody2D myRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        meteorSpawnerScript = meteorSpawnerObject.GetComponent<MeteorSpawnerScript>();

        if ((gameObject.tag == "sfd_bigFrozenMeteor") 
            || (gameObject.tag == "sfd_madeFrozenMeteor") 
            || (gameObject.tag == "sfd_tinyFrozenMeteor"))
        { immunityOn = true; };
    }

    void Update()
    {
        ImmunityOnFromNewSplitFrozenMeteors();
    }

    private void ImmunityOnFromNewSplitFrozenMeteors()
    {
        if (immunityOn == true)
        {
            immunityCounter += Time.deltaTime;

            if (immunityCounter >= 0.5f)
            {
                immunityOn = false;
                if (gameObject.tag == "sfd_bigFrozenMeteor")
                {
                    gameObject.tag = "bigFrozenMeteor";
                }
                else if (gameObject.tag == "sfd_madeFrozenMeteor")
                {
                    gameObject.tag = "medFrozenMeteor";
                }
                else if (gameObject.tag == "sfd_tinyFrozenMeteor")
                {
                    gameObject.tag = "tinyFrozenMeteor";
                }

            }
        }
    }

    public void StopMeteor()
    {
        myRigidBody.velocity = new Vector2(0, 0);
        myRigidBody.constraints = RigidbodyConstraints2D.FreezePositionX & RigidbodyConstraints2D.FreezePositionY;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        /// did not use this code due to multiple instances of GetComponent
        /// Freezebool still uses get Component

        ////=============================HUGE METEOR TYPE - CODE==========================
        /// <summary>
        /// If this gameObject has the tag "hugeMeteor" then
        ///========= MeteorHP decreases when hit by
        ///==============o Player, Player Laser, hugeMeteor
        ///========= MeteorHP does not decreas when hit by 
        ///==============o bigMeteor, madeMeteor, tinyMeteor, enemy
        /// </summary>
        if (gameObject.tag == "hugeFrozenMeteor")
        {
            if ((other.gameObject.tag == "hugeMeteor") || (other.gameObject.tag == "hugeFrozenMeteor"))
            {
                damageDealer = 1000;
            }
            else if ((other.gameObject.tag == "MainPlayer"))
            {
                damageDealer = 200;
            }
            else if ((other.gameObject.tag == "Player Laser") || (other.gameObject.tag == "Player Laser(Clone)"))
            {
                damageDealer = 100;
            }
            else //bigMeteor, madeMeteor, tinyMeteor, EnemyObject or other object
            {
                damageDealer = 0;
            }
        }
        ////=============================BIG METEOR TYPE - CODE==========================
        /// <summary>
        /// If this gameObject has the tag "hugeMeteor" then
        ///========= MeteorHP decreases when hit by
        ///==============o Player, Player Laser, hugeMeteor, bigMeteor
        ///========= MeteorHP does not decreas when hit by
        ///==============o enemy, madeMeteor, tinyMeteor
        /// </summary>
        else if (gameObject.tag == "bigFrozenMeteor")
        {
            //if I'm a big meteor and I collide with a hugemeteor
            if ((other.gameObject.tag == "hugeMeteor") || (other.gameObject.tag == "hugeFrozenMeteor"))
            {
                damageDealer = 1000;
            }
            else if ((other.gameObject.tag == "bigMeteor")
                || (other.gameObject.tag == "bigFrozenMeteor")
                || (other.gameObject.tag == "sfd_bigMeteor")
                || (other.gameObject.tag == "sfd_bigFrozenMeteor"))
            {
                damageDealer = 500;
            }
            else if ((other.gameObject.tag == "MainPlayer"))
            {
                damageDealer = 200;
            }
            else if ((other.gameObject.tag == "Player Laser") || (other.gameObject.tag == "Player Laser(Clone)"))
            {
                damageDealer = 100;
            }
            else
            {
                damageDealer = 0;
            }
        }
        ////=============================MED METEOR TYPE - CODE==========================
        /// <summary>
        /// If this gameObject has the tag "hugeMeteor" then
        ///========= MeteorHP decreases when hit by 
        ///==============o Player, Player Laser, hugeMeteor, bigMeteor, madeMeteor
        ///========= MeteorHP does not decreas when hit by
        ///==============o enemy, tinyMeteor
        /// </summary>
        else if (gameObject.tag == "medFrozenMeteor")
        {
            if ((other.gameObject.tag == "hugeMeteor") || (other.gameObject.tag == "hugeFrozenMeteor"))
            {
                damageDealer = 1000;
            }
            else if ((other.gameObject.tag == "bigMeteor")
                || (other.gameObject.tag == "bigFrozenMeteor")
                || (other.gameObject.tag == "sfd_bigMeteor")
                || (other.gameObject.tag == "sfd_bigFrozenMeteor"))
            {
                damageDealer = 500;
            }
            else if ((other.gameObject.tag == "madeMeteor")
                || (other.gameObject.tag == "medFrozenMeteor")
                || (other.gameObject.tag == "sfd_madeMeteor")
                || (other.gameObject.tag == "sfd_madeFrozenMeteor"))
            {
                damageDealer = 100;
            }
            else if ((other.gameObject.tag == "MainPlayer"))
            {
                damageDealer = 200;
            }
            else if ((other.gameObject.tag == "Player Laser") || (other.gameObject.tag == "Player Laser(Clone)"))
            {
                damageDealer = 100;
            }
            else
            {
                damageDealer = 0;
            }
        }
        ////=============================MED METEOR TYPE - CODE==========================
        /// <summary>
        /// If this gameObject has the tag "hugeMeteor" then
        ///========= MeteorHP decreases when hit by 
        ///==============o Player, Player Laser, hugeMeteor, bigMeteor, madeMeteor
        ///========= MeteorHP does not decreas when hit by
        ///==============o enemy, tinyMeteor
        /// </summary>
        else if (gameObject.tag == "tinyFrozenMeteor")
        {
            if ((other.gameObject.tag == "hugeMeteor") || (other.gameObject.tag == "hugeFrozenMeteor"))
            {
                damageDealer = 1000;
            }
            else if ((other.gameObject.tag == "bigMeteor")
                || (other.gameObject.tag == "bigFrozenMeteor")
                || (other.gameObject.tag == "sfd_bigMeteor")
                || (other.gameObject.tag == "sfd_bigFrozenMeteor"))
            {
                damageDealer = 500;
            }
            else if ((other.gameObject.tag == "madeMeteor")
                || (other.gameObject.tag == "medFrozenMeteor")
                || (other.gameObject.tag == "sfd_madeMeteor")
                || (other.gameObject.tag == "sfd_madeFrozenMeteor"))
            {
                damageDealer = 100;
            }
            else if ((other.gameObject.tag == "tinyMeteor")
                || (other.gameObject.tag == "tinyFrozenMeteor")
                || (other.gameObject.tag == "sfd_tinyMeteor")
                || (other.gameObject.tag == "sfd_tinyFrozenMeteor"))
            {
                damageDealer = 0;
            }
            else if ((other.gameObject.tag == "MainPlayer"))
            {
                damageDealer = 200;
            }
            else if (other.gameObject.tag == "Player Laser")
            {
                damageDealer = 1000;
                Destroy(other.gameObject);
            }
            else if (other.gameObject.tag == "icePeanut")
            {
                var parent = other.gameObject;
                Debug.Log("Collision between meteor and peanut");
                //move public method here so getcomponent would not be used

                //transform.parent = other.gameObject.transform;
                // turn on lock bool

                transform.SetParent(parent.transform);
            }
            else
            {
                damageDealer = 0;
                Debug.Log("Else");
            }
        }
        else if ((gameObject.tag == "sfd_bigFrozenMeteor")
            || (gameObject.tag == "sfd_madeFrozenMeteor")
            || (gameObject.tag == "sfd_tinyFrozenMeteor"))
             {
                if (other.gameObject.tag == "Player Laser")
                {
                    Destroy(other.gameObject);
                }
             }
        else { Debug.Log("no"); }

        //END OF IF-ELSE STATEMENTS

        ProcessHit(damageDealer);
    }


    private void ProcessHit(int damageDealer)
    {
        healthMeteor -= damageDealer;

        //PlayHitPlayerVFX();

        if (healthMeteor <= 0)
        {
            // edit code game freezes when theres too many meteors
            if (gameObject.tag == "hugeFrozenMeteor")
            {
                if (!meteorSplit)
                {
                    FrozenMeteorSplit();
                }
            }
            else if (gameObject.tag == "bigFrozenMeteor")
            {
                if (!meteorSplit) //we need this bool so it will only execute this code once
                {
                    FrozenMeteorSplit();
                }
            }
            else if (gameObject.tag == "medFrozenMeteor")
            {
                if (!meteorSplit)
                {
                    FrozenMeteorSplit();
                }
            }
            else if (gameObject.tag == "tinyFrozenMeteor")
            {
                GameObject smoke = Instantiate(
                    vfxExplosionSmoke,
                    transform.position,
                    transform.rotation * Quaternion.Euler(0, -180f, 0f));
                Destroy(smoke, 2);
            }
            else
            {

            }

            Destroy(gameObject);
        }
    }

    private void FrozenMeteorSplit()
    {
        //Replace with Enable/Disable Code

        GameObject smoke = Instantiate(
            vfxExplosionSmoke,
            transform.position,
            transform.rotation * Quaternion.Euler(0, -180f, 0f));
        Destroy(smoke, 2);

        meteorSplit = true;
    }
}
