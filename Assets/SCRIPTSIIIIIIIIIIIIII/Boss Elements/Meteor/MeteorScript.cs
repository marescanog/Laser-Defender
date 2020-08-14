using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorScript : MonoBehaviour
{

    [SerializeField] Color colorTransparent;
    [SerializeField] int healthMeteor = 100;
    [SerializeField] GameObject vfxExplosionSmoke;
    [SerializeField] float freezeTimer = 0; //for debugging purposes

    SpriteRenderer mySpriteRenderer;
    bool freezeOn = false;
    bool meteorSplit = false;
    int damageDealer = 0;

    [SerializeField] List<GameObject> meteorGameObjects;

    Rigidbody2D myRigidBody;

    [SerializeField] float immunityCounter = 0;
    [SerializeField] bool immunityOn = false;

    // Start is called before the first frame update
    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        mySpriteRenderer.color = colorTransparent;
        myRigidBody = GetComponent<Rigidbody2D>();

        if ((gameObject.tag == "sfd_bigMeteor")
            || (gameObject.tag == "sfd_madeMeteor") 
            || (gameObject.tag == "sfd_tinyMeteor")
            )
        { immunityOn = true; };
    }

    // Update is called once per frame
    void Update()
    {
        //This code is a timer that provides 1 second immunity to spawned meteor
        ImmunityToDamageOnAfterSpawnFromBrokenMeteor();

        FreezeMeteor();

        //Delete Later: For Debugging purposes
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (gameObject.tag == "hugeMeteor")
            {
                freezeOn = true;
            }
        }

    }

    private void ImmunityToDamageOnAfterSpawnFromBrokenMeteor()
    {
        if (immunityOn == true)
        {
            immunityCounter += Time.deltaTime;

            if (immunityCounter >= 0.5f)
            {
                immunityOn = false;
                if (gameObject.tag == "sfd_bigMeteor")
                {
                    gameObject.tag = "bigMeteor";
                }
                else if (gameObject.tag == "sfd_madeMeteor")
                {
                    gameObject.tag = "madeMeteor";
                }
                else if (gameObject.tag == "sfd_tinyMeteor")
                {
                    gameObject.tag = "tinyMeteor";
                }
            }
        }
    }

    public void StopMeteor()
    {
        myRigidBody.velocity = new Vector2(0, 0);
        myRigidBody.constraints = RigidbodyConstraints2D.FreezePositionX & RigidbodyConstraints2D.FreezePositionY;
    }

    private void FreezeMeteor()
    {
        if (freezeOn) 
        {
            freezeTimer += Time.deltaTime * 0.5f;
            if (freezeTimer >= 1)
            {
                freezeOn = false;

                if (gameObject.tag == "hugeMeteor")
                {
                    gameObject.tag = "hugeFrozenMeteor";
                }
                else if (gameObject.tag == "bigMeteor")
                {
                    gameObject.tag = "bigFrozenMeteor";
                }
                else if (gameObject.tag == "madeMeteor")
                {
                    gameObject.tag = "medFrozenMeteor";
                }
                else if (gameObject.tag == "tinyMeteor")
                {
                    gameObject.tag = "tinyFrozenMeteor";
                }

            }

            mySpriteRenderer.color = Color.Lerp(colorTransparent, Color.white, freezeTimer);
        }
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
        if (gameObject.tag == "hugeMeteor")
        {
            if (other.gameObject.tag == "hugeMeteor")
            {
                damageDealer = 1000;
            }
            else if (other.gameObject.tag == "hugeFrozenMeteor")
            {
                damageDealer = 1000;
                freezeOn = true;
            }
            else if ((other.gameObject.tag == "MainPlayer"))
            {
                damageDealer = 200;
            }
            else if (other.gameObject.tag == "Player Laser")
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
        else if (gameObject.tag == "bigMeteor")
        {
            //if I'm a big meteor and I collide with a hugemeteor
            if (other.gameObject.tag == "hugeMeteor")
            {
                damageDealer = 1000;
            }
            else if (other.gameObject.tag == "hugeFrozenMeteor")
            {
                damageDealer = 1000;
                freezeOn = true;
            }
            else if ((other.gameObject.tag == "bigMeteor")|| (other.gameObject.tag == "sfd_bigMeteor"))
            {
                damageDealer = 500;
            }
            else if ((other.gameObject.tag == "bigFrozenMeteor")|| (other.gameObject.tag == "sfd_bigFrozenMeteor"))
            {
                damageDealer = 500;
                freezeOn = true;
            }
            else if ((other.gameObject.tag == "MainPlayer"))
            {
                damageDealer = 200;
            }
            else if (other.gameObject.tag == "Player Laser")
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
        else if (gameObject.tag == "madeMeteor")
        {
            if (other.gameObject.tag == "hugeMeteor")
            {
                damageDealer = 1000;
            }
            else if (other.gameObject.tag == "hugeFrozenMeteor")
            {
                damageDealer = 1000;
                freezeOn = true;
            }
            else if ((other.gameObject.tag == "bigMeteor") || (other.gameObject.tag == "sfd_bigMeteor"))
            {
                damageDealer = 500;
            }
            else if ((other.gameObject.tag == "bigFrozenMeteor") || (other.gameObject.tag == "sfd_bigFrozenMeteor"))
            {
                damageDealer = 500;
                freezeOn = true;
            }
            else if ((other.gameObject.tag == "madeMeteor")|| (other.gameObject.tag == "sfd_madeMeteor"))
            {
                damageDealer = 100;
            }
            else if ((other.gameObject.tag == "medFrozenMeteor")|| (other.gameObject.tag == "sfd_madeFrozenMeteor"))
            {
                damageDealer = 100;
                freezeOn = true;
            }
            else if ((other.gameObject.tag == "MainPlayer"))
            {
                damageDealer = 200;
            }
            else if (other.gameObject.tag == "Player Laser")
            {
                damageDealer = 100;
            }
            else
            {
                damageDealer = 0;
            }
        }
        ////=============================TINY METEOR TYPE - CODE==========================
        /// <summary>
        /// If this gameObject has the tag "hugeMeteor" then
        ///========= MeteorHP decreases when hit by 
        ///==============o Player, Player Laser, hugeMeteor, bigMeteor, madeMeteor
        ///========= MeteorHP does not decreas when hit by
        ///==============o enemy, tinyMeteor
        /// </summary>
        else if (gameObject.tag == "tinyMeteor")
        {
            if (other.gameObject.tag == "hugeMeteor")
            {
                damageDealer = 1000;
            }
            else if (other.gameObject.tag == "hugeFrozenMeteor")
            {
                damageDealer = 1000;
                freezeOn = true;
            }
            else if ((other.gameObject.tag == "bigMeteor") || (other.gameObject.tag == "sfd_bigMeteor"))
            {
                damageDealer = 500;
            }
            else if ((other.gameObject.tag == "bigFrozenMeteor") || (other.gameObject.tag == "sfd_bigFrozenMeteor"))
            {
                damageDealer = 500;
                freezeOn = true;
            }
            else if ((other.gameObject.tag == "madeMeteor") || (other.gameObject.tag == "sfd_madeMeteor"))
            {
                damageDealer = 100;
            }
            else if ((other.gameObject.tag == "medFrozenMeteor") || (other.gameObject.tag == "sfd_madeFrozenMeteor"))
            {
                damageDealer = 100;
                freezeOn = true;
            }
            else if ((other.gameObject.tag == "tinyMeteor")|| (other.gameObject.tag == "sfd_tinyMeteor"))
            {
                damageDealer = 200;
            }
            else if ((other.gameObject.tag == "tinyFrozenMeteor")|| (other.gameObject.tag == "sfd_tinyFrozenMeteor"))
            {
                damageDealer = 200;
                freezeOn = true;
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
            else
            {
                damageDealer = 0;
            }
        }
        ////=============================HUGE FROZEN METEOR TYPE - CODE==========================
        /// <summary> This has the Frozen Tagging
        /// If this gameObject has the tag "FrozenhugeMeteor" then
        ///========= MeteorHP decreases when hit by 
        ///==============o Player, Player Laser, hugeMeteor, bigMeteor, madeMeteor
        ///========= MeteorHP does not decreas when hit by
        ///==============o enemy, tinyMeteor
        /// </summary>
        else if (gameObject.tag == "hugeFrozenMeteor")
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
        ////=============================BIG FROZEN METEOR TYPE - CODE==========================
        /// <summary> This has the Frozen Tagging
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
        ////=============================MED FROZEN METEOR TYPE - CODE==========================
        /// <summary> This has the Frozen Tagging
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
        ////=============================TINY FROZEN METEOR TYPE - CODE==========================
        /// <summary> This has the Frozen Tagging
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
        /// 
        //Code grants imunity to meteors with any of the 3 taggings, playerlaser gets destroyed.
        else if ((gameObject.tag == "sfd_bigMeteor")
            || (gameObject.tag == "sfd_madeMeteor") 
            || (gameObject.tag == "sfd_tinyMeteor"))
             {
                if (other.gameObject.tag == "Player Laser")
                {
                    Destroy(other.gameObject);
                }
                else if ((other.gameObject.tag == "hugeFrozenMeteor")
                    ||(other.gameObject.tag == "bigFrozenMeteor") 
                    || (other.gameObject.tag == "sfd_bigFrozenMeteor")
                    || (other.gameObject.tag == "medFrozenMeteor") 
                    || (other.gameObject.tag == "sfd_madeFrozenMeteor") 
                    || (other.gameObject.tag == "tinyFrozenMeteor") 
                    || (other.gameObject.tag == "sfd_tinyFrozenMeteor"))
                {
                    freezeOn = true;
                }
                else
                {
                    damageDealer = 0;
                }
             }


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
            if (gameObject.tag == "hugeMeteor")
            {
                if (!meteorSplit)
                {
                    Regular_Meteor_Split();
                }
            }
            if (gameObject.tag == "hugeFrozenMeteor")
            {
                if (!meteorSplit)
                {
                    Frozen_meteorSplit();
                }
            }
            else if (gameObject.tag == "bigMeteor")
            {
                if (!meteorSplit)
                {
                    Regular_Meteor_Split();
                }
            }
            else if (gameObject.tag == "bigFrozenMeteor")
            {
                if (!meteorSplit)
                {
                    Frozen_meteorSplit();
                }
            }
            else if (gameObject.tag == "madeMeteor")
            {
                if (!meteorSplit)
                {
                    Regular_Meteor_Split();
                }
            }
            else if (gameObject.tag == "medFrozenMeteor")
            {
                if (!meteorSplit)
                {
                    Frozen_meteorSplit();
                }
            }
            else if ((gameObject.tag == "tinyMeteor") || (gameObject.tag == "tinyFrozenMeteor"))
            {
                GameObject smoke = Instantiate(
                    vfxExplosionSmoke,
                    transform.position,
                    transform.rotation * Quaternion.Euler(0, -180f, 0f));
                Destroy(smoke, 2f);
            }
            else
            {

            }

            Destroy(gameObject);
        }
    }

    private void Frozen_meteorSplit()
    {
        GameObject meteor = Instantiate(
            meteorGameObjects[Random.Range(3, 6)],
            (transform.position + new Vector3(1, 1, 0)),
            Quaternion.identity) as GameObject;
        meteor.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));

        GameObject meteor2 = Instantiate(
                    meteorGameObjects[Random.Range(3, 6)],
                    transform.position,
                    Quaternion.identity) as GameObject;
        meteor2.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));

        GameObject smoke = Instantiate(
            vfxExplosionSmoke,
            transform.position,
            transform.rotation * Quaternion.Euler(0, -180f, 0f));
        Destroy(smoke, 2f);

        meteorSplit = true;
    }

    private void Regular_Meteor_Split()
    {
        GameObject meteor = Instantiate(
            meteorGameObjects[Random.Range(0, 2)],
            (transform.position + new Vector3(1, 1, 0)),
            Quaternion.identity) as GameObject;
        meteor.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));

        GameObject meteor2 = Instantiate(
                    meteorGameObjects[Random.Range(0, 2)],
                    transform.position,
                    Quaternion.identity) as GameObject;
        meteor2.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));

        GameObject smoke = Instantiate(
            vfxExplosionSmoke,
            transform.position,
            transform.rotation * Quaternion.Euler(0, -180f, 0f));
        Destroy(smoke, 2f);

        meteorSplit = true;
    }
}
