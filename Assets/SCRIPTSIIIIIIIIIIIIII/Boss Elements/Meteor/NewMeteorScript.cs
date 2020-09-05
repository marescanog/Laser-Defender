using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMeteorScript : MonoBehaviour
{

    [SerializeField] Color colorTransparent;
    [SerializeField] int healthMeteor = 100;
    [SerializeField] GameObject vfxExplosionSmoke;
    [SerializeField] float freezeTimer = 0; //for debugging purposes

    SpriteRenderer mySpriteRenderer;
    bool freezeOn = false;
    int damageDealer = 0;

    Rigidbody2D myRigidBody;

    [SerializeField] float immunityCounter = 0;
    [SerializeField] bool immunityOn = false;

    [SerializeField] GameObject meteorSpawnerObject;
    MeteorSpawnerScript meteorSpawnerScript;

    [Header("Split Toggle")]
    bool minusBabyOnce = false;

    [Header("FamilyName Children Toggle")]
    [SerializeField] bool isSmithChild = false;
    [SerializeField] bool isLandonChild = false;
    [SerializeField] bool isPriceChild = false;
    [SerializeField] bool isWhiteChild = false;
    [SerializeField] bool isjacobsChild = false;
    [SerializeField] bool isMorrisChild = false;
    [SerializeField] bool isJohnsonChild = false;

    [Header("ListSegragator")]
    [SerializeField] bool isSpawnedOriginal = false;
    [SerializeField] bool isSpawnedFrozen = false;

    [Header("FamilyName")]
    [SerializeField] string familyNameOfMeteor;

    [Header("Coordinates")]
    [SerializeField] float originalXPos;
    [SerializeField] float originalYPos;
    private Vector2 originalPosition;

    string listType;
    bool currentBoolForFamily = false;

    string myName;

    bool lockedToPeanut = false;
    Transform newTransform;
    Transform myTransformStored;

    bool inflictBlastDamageOnceBool = false;

    // Start is called before the first frame update
    public string GetName() { return myName; }


    void Start()
    {
        originalPosition = new Vector2(originalXPos, originalYPos);

        myTransformStored = transform;
        myName = gameObject.name;

        if (isSpawnedOriginal)
        {
            listType = "Spawn Meteor Object List";
        }
        else if (isSpawnedFrozen)
        {
            listType = "Spawn Frozen Meteor Object List";
        }

        mySpriteRenderer = GetComponent<SpriteRenderer>();
        mySpriteRenderer.color = colorTransparent;
        myRigidBody = GetComponent<Rigidbody2D>();
        meteorSpawnerScript = meteorSpawnerObject.GetComponent<MeteorSpawnerScript>();

        if ((gameObject.tag == "sfd_bigMeteor")
            | (gameObject.tag == "sfd_madeMeteor")
            | (gameObject.tag == "sfd_tinyMeteor")
            | (gameObject.tag == "sfd_bigFrozenMeteor")
            | (gameObject.tag == "sfd_madeFrozenMeteor")
            | (gameObject.tag == "sfd_tinyFrozenMeteor")
            )
        { immunityOn = true; };

    }
    
    // Update is called once per frame
    void Update()
    {
        if(lockedToPeanut)
        {
            transform.position = newTransform.position;
        }


        ImmunityToDamageOnAfterSpawnFromBrokenMeteor();

        FreezeMeteor();
        /*
        //Delete Later: For Debugging purposes
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (myName == meteorSpawnerScript.Get_FrozenMeteorName())
            {
                freezeOn = true;
            }
        }
        */

        //delete this stop motion kepress after testing. freezes meteor for testing
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (gameObject.tag == "hugeMeteor"| gameObject.tag == "bigMeteor" | 
                gameObject.tag == "madeMeteor" | gameObject.tag == "tinyMeteor"|
                gameObject.tag == "hugeFrozenMeteor" | gameObject.tag == "bigFrozenMeteor" | gameObject.tag == "sfd_bigFrozenMeteor" |
                gameObject.tag == "medFrozenMeteor" | gameObject.tag == "tinyFrozenMeteor" |
                gameObject.name == "Toby A Smith"|(isSmithChild))
            {
                myRigidBody.velocity = new Vector2(0, 0);
            }

        }
        
    }

    public void Stop_This_meteor()
    {
        myRigidBody.velocity = new Vector2(0, 0);
    }

    public void New_TransForm(GameObject peanut)
    {
        lockedToPeanut = true;
        newTransform = peanut.transform;
    }

    public void Change_MyLayer()
    {
        gameObject.layer = 16;
    }

    private void ImmunityToDamageOnAfterSpawnFromBrokenMeteor()
    {
        if (immunityOn)
        {
            immunityCounter += Time.deltaTime;

            if (immunityCounter >= 0.5f)
            {
                if(isSpawnedFrozen) //For Frozen Original
                { Assign_Tag_Frozen(); }


                if (isSmithChild){currentBoolForFamily = meteorSpawnerScript.GetSmithBoolValue();}
                else if (isLandonChild) { currentBoolForFamily = meteorSpawnerScript.GetLandonBoolValue(); }
                else if (isPriceChild) { currentBoolForFamily = meteorSpawnerScript.GetPriceBoolValue(); }
                else if (isWhiteChild) { currentBoolForFamily = meteorSpawnerScript.GetWhiteBoolValue(); }
                else if (isjacobsChild) { currentBoolForFamily = meteorSpawnerScript.GetJacobsBoolValue(); }
                else if (isMorrisChild) { currentBoolForFamily = meteorSpawnerScript.GetMorrisBoolValue(); }
                else if (isJohnsonChild) { currentBoolForFamily = meteorSpawnerScript.GetJohnsonBoolValue(); }

                if (!currentBoolForFamily)
                {
                    if ((gameObject.name == "Jackie A Smith")|
                        (gameObject.name == "Jackie B Smith")|
                        (gameObject.name == "Robert A Landon")|
                        (gameObject.name == "Robert B Landon")|
                        (gameObject.name == "Robert A Price")|
                        (gameObject.name == "Robert B Price")|
                        (gameObject.name == "Robert A White")|
                        (gameObject.name == "Robert B White")|
                        (gameObject.name == "tina Jacobs") |
                        (gameObject.name == "jacqueline Jacobs") |
                        (gameObject.name == "tina Morris") |
                        (gameObject.name == "jacqueline Morris") |
                        (gameObject.name == "tina Johnson") |
                        (gameObject.name == "jacqueline Johnson")){ Assign_Tag_NOTFrozen(); }
                    else if ((gameObject.name == "Robert A Smith")| (gameObject.name == "Robert B Smith"))
                    {
                        currentBoolForFamily = meteorSpawnerScript.Get_JackieA_SmithBoolValue();
                        CheckTag_IfFrozenOrNot_ThenAssignTag();
                    }
                    else if ((gameObject.name == "Robert D Smith") |(gameObject.name == "Robert E Smith"))
                    {
                        currentBoolForFamily = meteorSpawnerScript.Get_JackieB_SmithBoolValue();
                        CheckTag_IfFrozenOrNot_ThenAssignTag();
                    }
                    else if ((gameObject.name == "tina Smith") | (gameObject.name == "jacqueline Smith"))
                    {
                        currentBoolForFamily = meteorSpawnerScript.Get_RobartA_SmithBoolValue();
                        CheckTag_IfFrozenOrNot_ThenAssignTag();
                    }
                    else if ((gameObject.name == "herbert Smith") | (gameObject.name == "matty Smith"))
                    {
                        currentBoolForFamily = meteorSpawnerScript.Get_RobertB_SmithBoolValue();
                        CheckTag_IfFrozenOrNot_ThenAssignTag();
                    }
                    else if ((gameObject.name == "dennis Smith") | (gameObject.name == "jayson Smith"))
                    {
                        currentBoolForFamily = meteorSpawnerScript.Get_RobertD_SmithBoolValue();
                        CheckTag_IfFrozenOrNot_ThenAssignTag();
                    }
                    else if ((gameObject.name == "Paolo Smith") | (gameObject.name == "Cataract Smith"))
                    {
                        currentBoolForFamily = meteorSpawnerScript.Get_RobertE_SmithBoolValue();
                        CheckTag_IfFrozenOrNot_ThenAssignTag();
                    }
                    else if ((gameObject.name == "tina Landon") | (gameObject.name == "jacqueline Landon"))
                    {
                        currentBoolForFamily = meteorSpawnerScript.Get_RobartA_LandonBoolValue();
                        CheckTag_IfFrozenOrNot_ThenAssignTag();
                    }
                    //Landon
                    else if ((gameObject.name == "herbert Landon") | (gameObject.name == "matty Landon"))
                    {
                        currentBoolForFamily = meteorSpawnerScript.Get_RobertB_LandonBoolValue();
                        CheckTag_IfFrozenOrNot_ThenAssignTag();
                    }
                    //Price
                    else if ((gameObject.name == "tina Price") | (gameObject.name == "jacqueline Price"))
                    {
                        currentBoolForFamily = meteorSpawnerScript.Get_RobartA_PriceBoolValue();
                        CheckTag_IfFrozenOrNot_ThenAssignTag();
                    }
                    else if ((gameObject.name == "herbert Price") | (gameObject.name == "matty Price"))
                    {
                        currentBoolForFamily = meteorSpawnerScript.Get_RobertB_PriceBoolValue();
                        CheckTag_IfFrozenOrNot_ThenAssignTag();
                    }
                    //White
                    else if ((gameObject.name == "tina White") | (gameObject.name == "jacqueline White"))
                    {
                        currentBoolForFamily = meteorSpawnerScript.Get_RobertA_WhiteBoolValue();
                        CheckTag_IfFrozenOrNot_ThenAssignTag();
                    }
                    else if ((gameObject.name == "herbert White") | (gameObject.name == "matty White"))
                    {
                        currentBoolForFamily = meteorSpawnerScript.Get_RobertB_WhiteBoolValue();
                        CheckTag_IfFrozenOrNot_ThenAssignTag();
                    }
                }
                else if (currentBoolForFamily)
                {
                    Assign_Tag_Frozen();
                }

                immunityOn = false;
            }
        }
    }

    private void CheckTag_IfFrozenOrNot_ThenAssignTag()
    {
        if (!currentBoolForFamily) { Assign_Tag_NOTFrozen(); }
        else if (currentBoolForFamily) { Assign_Tag_Frozen(); }
    }
    private void Assign_Tag_Frozen()
    {
        if ((gameObject.tag == "sfd_bigMeteor")| (gameObject.tag == "sfd_bigFrozenMeteor"))
        {
            gameObject.tag = "bigFrozenMeteor";
        }
        else if ((gameObject.tag == "sfd_madeMeteor")| (gameObject.tag == "sfd_madeFrozenMeteor"))
        {
            gameObject.tag = "medFrozenMeteor";
        }
        else if ((gameObject.tag == "sfd_tinyMeteor")| (gameObject.tag == "sfd_tinyFrozenMeteor"))
        {
            gameObject.tag = "tinyFrozenMeteor";
        }
    }
    private void Assign_Tag_NOTFrozen()
    {
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

    private void FreezeMeteor()
    {
        if (freezeOn)
        {
            freezeTimer += Time.deltaTime * 0.5f;
            if (freezeTimer >= 1)
            {
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

                if(gameObject.name == "Toby A Smith"){ meteorSpawnerScript.Smith_Freeze_True(); }
                else if (gameObject.name == "Jackie B Landon"){meteorSpawnerScript.Landon_Freeze_True(); }
                else if (gameObject.name == "Jackie C Price"){ meteorSpawnerScript.Price_Freeze_True(); }
                else if (gameObject.name == "Jackie D White"){ meteorSpawnerScript.White_Freeze_True(); }
                else if (gameObject.name == "Robert E Jacobs") { meteorSpawnerScript.Jacobs_Freeze_True(); }
                else if (gameObject.name == "Robert F morris") { meteorSpawnerScript.Morris_Freeze_True(); }
                else if (gameObject.name == "Robert G Johnson") { meteorSpawnerScript.Johnson_Freeze_True(); }
                else if (gameObject.name == "Jackie A Smith") { if (!meteorSpawnerScript.GetSmithBoolValue()) { meteorSpawnerScript.Jackie_A_Smith_Freeze_True(); }}
                else if (gameObject.name == "Jackie B Smith") { if (!meteorSpawnerScript.GetSmithBoolValue()) { meteorSpawnerScript.Jackie_B_Smith_Freeze_True(); }}
                else if (gameObject.name == "Robert A Smith") { if (!meteorSpawnerScript.GetSmithBoolValue()) { if (!meteorSpawnerScript.Get_JackieA_SmithBoolValue()) { meteorSpawnerScript.Robert_A_Smith_Freeze_True(); } } }
                else if (gameObject.name == "Robert B Smith") { if (!meteorSpawnerScript.GetSmithBoolValue()) { if (!meteorSpawnerScript.Get_JackieA_SmithBoolValue()) { meteorSpawnerScript.Robert_B_Smith_Freeze_True(); } } }
                else if (gameObject.name == "Robert D Smith") { if (!meteorSpawnerScript.GetSmithBoolValue()) { if (!meteorSpawnerScript.Get_JackieB_SmithBoolValue()) { meteorSpawnerScript.Robert_D_Smith_Freeze_True(); } } }
                else if (gameObject.name == "Robert E Smith") { if (!meteorSpawnerScript.GetSmithBoolValue()) { if (!meteorSpawnerScript.Get_JackieB_SmithBoolValue()) { meteorSpawnerScript.Robert_E_Smith_Freeze_True(); } } }
                else if (gameObject.name == "Robert A Landon") { if (!meteorSpawnerScript.GetLandonBoolValue()) { meteorSpawnerScript.Robert_A_Landon_Freeze_True(); } }
                else if (gameObject.name == "Robert B Landon") { if (!meteorSpawnerScript.GetLandonBoolValue()) { meteorSpawnerScript.Robert_B_Landon_Freeze_True(); } }
                else if (gameObject.name == "Robert A Price") { if (!meteorSpawnerScript.GetPriceBoolValue()) { meteorSpawnerScript.Robert_A_Price_Freeze_True(); } }
                else if (gameObject.name == "Robert B Price") { if (!meteorSpawnerScript.GetPriceBoolValue()) { meteorSpawnerScript.Robert_B_Price_Freeze_True(); } }
                else if (gameObject.name == "Robert A White") { if (!meteorSpawnerScript.GetWhiteBoolValue()) { meteorSpawnerScript.Robert_A_White_Freeze_True(); } }
                else if (gameObject.name == "Robert B White") { if (!meteorSpawnerScript.GetWhiteBoolValue()) { meteorSpawnerScript.Robert_B_White_Freeze_True(); } }

                if((this.gameObject.tag== "tinyFrozenMeteor")| (this.gameObject.tag == "sfd_tinyFrozenMeteor"))
                {
                    meteorSpawnerScript.Check_if_frozenTinyMeteor_isWithinBounds(this.gameObject);
                }


                freezeOn = false;
            }

            mySpriteRenderer.color = Color.Lerp(colorTransparent, Color.white, freezeTimer);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //This section of code ensures that the meteor will not have a velocity = 0
        //when hitting the Main Player

        var collision_y = collision.gameObject.transform.position.y;
        var collision_x = collision.gameObject.transform.position.x;

        var y_playerPos = meteorSpawnerScript.Get_PlayerPos().y;
        var y_myPos = transform.position.y;

        var x_playerPos = meteorSpawnerScript.Get_PlayerPos().x;
        var x_myPos = transform.position.x;

        float xVel = 0;
        float yVel = 0;


        switch(collision.gameObject.tag)
        {
            case "MainPlayer":
                Deflect_On_Collision(y_playerPos, y_myPos, x_playerPos, x_myPos, ref xVel, ref yVel);
                break;
            case "meteorCollider":
                Deflect_On_Collision(collision_y, y_myPos, collision_x, x_myPos, ref xVel, ref yVel);
                break;
            case "meteor":
                break;
            default:
                //Do nothing
                break;
        }
    }

    private void Deflect_On_Collision(float y_playerPos, float y_myPos, float x_playerPos, float x_myPos, ref float xVel, ref float yVel)
    {
        if (y_myPos > y_playerPos)
        {
            //meteor is up
            yVel = Random.Range(1.0f, 3.0f);
        }
        else if (y_myPos < y_playerPos)
        {
            yVel = Random.Range(-1.0f, -3.0f);
        }
        else if (y_myPos == y_playerPos)
        {
            yVel = Random.Range(1.0f, 3.0f);
        }

        if (x_myPos > x_playerPos)
        {
            xVel = Random.Range(1.0f, 3.0f);
        }
        else if (x_myPos < x_playerPos)
        {
            xVel = Random.Range(-1.0f, -3.0f);
        }
        else if (x_myPos == x_playerPos)
        {
            xVel = Random.Range(1.0f, 3.0f);
        }

        myRigidBody.velocity = new Vector2(xVel, yVel);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        /// did not use this code due to multiple instances of GetComponent
        /// Freezebool still uses get Component
        /// 
        string mytag = gameObject.tag;
        string otherObjectsTag = other.gameObject.tag;

        //This Switch Assigns a damagedealer for Processhit in the end
        //based on the tag this object has VS. tag of other object
        switch (mytag)
        {
            case "hugeMeteor":
                switch(otherObjectsTag)
                {
                    case "MainPlayer":
                        damageDealer = 200;
                        break;
                    case "Player Laser":
                        damageDealer = 100;
                        break;
                    case "hugeMeteor":
                        damageDealer = 1000;
                        break;
                    case "hugeFrozenMeteor":
                        damageDealer = 1000;
                        freezeOn = true;
                        break;
                    case "ExplosionDamage":
                        InflictBlastDamageOnceMethod();
                        break;
                    default:
                        damageDealer = 0;
                        break;
                }
                break;
            case "bigMeteor":
                switch(otherObjectsTag)
                {
                    case "ExplosionDamage":
                        InflictBlastDamageOnceMethod();
                        break;
                    case "MainPlayer":
                        damageDealer = 200;
                        break;
                    case "Player Laser":
                        damageDealer = 100;
                        break;
                    case "hugeMeteor":
                        damageDealer = 1000;
                        break;
                    case "hugeFrozenMeteor":
                        damageDealer = 1000;
                        freezeOn = true;
                        break;
                    case "bigMeteor":
                        damageDealer = 500;
                        break;
                    case "sfd_bigMeteor":
                        damageDealer = 500;
                        break;
                    case "bigFrozenMeteor":
                        damageDealer = 500;
                        freezeOn = true;
                        break;
                    case "sfd_bigFrozenMeteor":
                        damageDealer = 500;
                        freezeOn = true;
                        break;
                    default:
                        damageDealer = 0;
                        break;

                }
                break;
            case "madeMeteor":
                switch(otherObjectsTag)
                {
                    case "ExplosionDamage":
                        InflictBlastDamageOnceMethod();
                        break;
                    case "MainPlayer":
                        damageDealer = 200;
                        break;
                    case "Player Laser":
                        damageDealer = 100;
                        break;
                    case "hugeMeteor":
                        damageDealer = 1000;
                        break;
                    case "hugeFrozenMeteor":
                        damageDealer = 1000;
                        freezeOn = true;
                        break;
                    case "bigMeteor":
                        damageDealer = 500;
                        break;
                    case "sfd_bigMeteor":
                        damageDealer = 500;
                        break;
                    case "bigFrozenMeteor":
                        damageDealer = 500;
                        freezeOn = true;
                        break;
                    case "sfd_bigFrozenMeteor":
                        damageDealer = 500;
                        freezeOn = true;
                        break;
                    case "madeMeteor":
                        damageDealer = 100;
                        break;
                    case "sfd_madeMeteor":
                        damageDealer = 100;
                        break;
                    case "medFrozenMeteor":
                        damageDealer = 100;
                        freezeOn = true;
                        break;
                    case "sfd_madeFrozenMeteor":
                        damageDealer = 100;
                        freezeOn = true;
                        break;
                    default:
                        damageDealer = 0;
                        break;
                }
                break;
            case "tinyMeteor":
                switch(otherObjectsTag)
                {
                    case "ExplosionDamage":
                        damageDealer = 0;
                        break;
                    case "MainPlayer":
                        damageDealer = 200;
                        break;
                    case "Player Laser":
                        damageDealer = 1000;
                        break;
                    case "hugeMeteor":
                        damageDealer = 1000;
                        break;
                    case "hugeFrozenMeteor":
                        damageDealer = 1000;
                        freezeOn = true;
                        break;
                    case "bigMeteor":
                        damageDealer = 500;
                        break;
                    case "sfd_bigMeteor":
                        damageDealer = 500;
                        break;
                    case "bigFrozenMeteor":
                        damageDealer = 500;
                        freezeOn = true;
                        break;
                    case "sfd_bigFrozenMeteor":
                        damageDealer = 500;
                        freezeOn = true;
                        break;
                    case "madeMeteor":
                        damageDealer = 100;
                        break;
                    case "sfd_madeMeteor":
                        damageDealer = 100;
                        break;
                    case "medFrozenMeteor":
                        damageDealer = 100;
                        freezeOn = true;
                        break;
                    case "sfd_madeFrozenMeteor":
                        damageDealer = 100;
                        freezeOn = true;
                        break;
                    case "tinyMeteor":
                        damageDealer = 0;
                        break;
                    case "sfd_tinyMeteor":
                        damageDealer = 0;
                        break;
                    case "tinyFrozenMeteor":
                        damageDealer = 0;
                        freezeOn = true;
                        break;
                    case "sfd_tinyFrozenMeteor":
                        damageDealer = 0;
                        freezeOn = true;
                        break;
                    default:
                        damageDealer = 0;
                        break;
                }
                break;
            case "hugeFrozenMeteor":
                switch(otherObjectsTag)
                {
                    case "ExplosionDamage":
                        InflictBlastDamageOnceMethod();
                        break;
                    case "MainPlayer":
                        damageDealer = 200;
                        break;
                    case "Player Laser":
                        damageDealer = 100;
                        break;
                    case "hugeMeteor":
                        damageDealer = 1000;
                        break;
                    case "hugeFrozenMeteor":
                        damageDealer = 1000;
                        break;
                    default:
                        damageDealer = 0;
                        break;
                }
                break;
            case "bigFrozenMeteor":
                switch (otherObjectsTag)
                {
                    case "ExplosionDamage":
                        InflictBlastDamageOnceMethod();
                        break;
                    case "MainPlayer":
                        damageDealer = 200;
                        break;
                    case "Player Laser":
                        damageDealer = 100;
                        break;
                    case "hugeMeteor":
                        damageDealer = 1000;
                        break;
                    case "hugeFrozenMeteor":
                        damageDealer = 1000;
                        break;
                    case "bigMeteor":
                        damageDealer = 500;
                        break;
                    case "sfd_bigMeteor":
                        damageDealer = 500;
                        break;
                    case "bigFrozenMeteor":
                        damageDealer = 500;
                        break;
                    case "sfd_bigFrozenMeteor":
                        damageDealer = 500;
                        break;
                    default:
                        damageDealer = 0;
                        break;
                }
                break;
            case "medFrozenMeteor":
                switch (otherObjectsTag)
                {
                    case "ExplosionDamage":
                        InflictBlastDamageOnceMethod();
                        break;
                    case "MainPlayer":
                        damageDealer = 200;
                        break;
                    case "Player Laser":
                        damageDealer = 100;
                        break;
                    case "hugeMeteor":
                        damageDealer = 1000;
                        break;
                    case "hugeFrozenMeteor":
                        damageDealer = 1000;
                        break;
                    case "bigMeteor":
                        damageDealer = 500;
                        break;
                    case "sfd_bigMeteor":
                        damageDealer = 500;
                        break;
                    case "bigFrozenMeteor":
                        damageDealer = 500;
                        break;
                    case "sfd_bigFrozenMeteor":
                        damageDealer = 500;
                        break;
                    case "madeMeteor":
                        damageDealer = 100;
                        break;
                    case "sfd_madeMeteor":
                        damageDealer = 100;
                        break;
                    case "medFrozenMeteor":
                        damageDealer = 100;
                        break;
                    case "sfd_madeFrozenMeteor":
                        damageDealer = 100;
                        break;
                    default:
                        damageDealer = 0;
                        break;
                }
                break;
            case "tinyFrozenMeteor":
                switch (otherObjectsTag)
                {
                    case "ExplosionDamage":
                        damageDealer = 0;
                        break;
                    case "MainPlayer":
                        damageDealer = 200;
                        break;
                    case "Player Laser":
                        damageDealer = 1000;
                        break;
                    case "hugeMeteor":
                        damageDealer = 1000;
                        break;
                    case "hugeFrozenMeteor":
                        damageDealer = 1000;
                        break;
                    case "bigMeteor":
                        damageDealer = 500;
                        break;
                    case "sfd_bigMeteor":
                        damageDealer = 500;
                        break;
                    case "bigFrozenMeteor":
                        damageDealer = 500;
                        break;
                    case "sfd_bigFrozenMeteor":
                        damageDealer = 500;
                        break;
                    case "madeMeteor":
                        damageDealer = 100;
                        break;
                    case "sfd_madeMeteor":
                        damageDealer = 100;
                        break;
                    case "medFrozenMeteor":
                        damageDealer = 100;
                        break;
                    case "sfd_madeFrozenMeteor":
                        damageDealer = 100;
                        break;
                    case "tinyMeteor":
                        damageDealer = 0;
                        break;
                    case "sfd_tinyMeteor":
                        damageDealer = 0;
                        break;
                    case "tinyFrozenMeteor":
                        damageDealer = 0;
                        break;
                    case "sfd_tinyFrozenMeteor":
                        damageDealer = 0;
                        break;
                    case "icePeanut":
                        damageDealer = 0;
                        //do nothing
                        break;
                    default:
                        damageDealer = 0;
                        break;
                }
                break;
            case "sfd_bigMeteor":
                switch(otherObjectsTag)
                {
                    case "hugeFrozenMeteor":
                        damageDealer = 0;
                        freezeOn = true;
                        break;
                    case "bigFrozenMeteor":
                        damageDealer = 0;
                        freezeOn = true;
                        break;
                    case "sfd_bigFrozenMeteor":
                        damageDealer = 0;
                        freezeOn = true;
                        break;
                    default:
                        damageDealer = 0;
                        break;
                }
                break;
            case "sfd_madeMeteor":
                switch (otherObjectsTag)
                {
                    case "hugeFrozenMeteor":
                        damageDealer = 0;
                        freezeOn = true;
                        break;
                    case "bigFrozenMeteor":
                        damageDealer = 0;
                        freezeOn = true;
                        break;
                    case "sfd_bigFrozenMeteor":
                        damageDealer = 0;
                        freezeOn = true;
                        break;
                    case "medFrozenMeteor":
                        damageDealer = 0;
                        freezeOn = true;
                        break;
                    case "sfd_madeFrozenMeteor":
                        damageDealer = 0;
                        freezeOn = true;
                        break;
                    default:
                        damageDealer = 0;
                        break;
                }
                break;
            case "sfd_tinyMeteor":
                switch (otherObjectsTag)
                {
                    case "hugeFrozenMeteor":
                        damageDealer = 0;
                        freezeOn = true;
                        break;
                    case "bigFrozenMeteor":
                        damageDealer = 0;
                        freezeOn = true;
                        break;
                    case "sfd_bigFrozenMeteor":
                        damageDealer = 0;
                        freezeOn = true;
                        break;
                    case "medFrozenMeteor":
                        damageDealer = 0;
                        freezeOn = true;
                        break;
                    case "sfd_madeFrozenMeteor":
                        damageDealer = 0;
                        freezeOn = true;
                        break;
                    case "tinyFrozenMeteor":
                        damageDealer = 0;
                        freezeOn = true;
                        break;
                    case "sfd_tinyFrozenMeteor":
                        damageDealer = 0;
                        freezeOn = true;
                        break;
                    default:
                        damageDealer = 0;
                        break;
                }
                break;
            case "sfd_bigFrozenMeteor":
                damageDealer = 0;
                break;
            case "sfd_madeFrozenMeteor":
                damageDealer = 0;
                break;
            case "sfd_tinyFrozenMeteor":
                damageDealer = 0;
                break;
            default:
                Debug.Log(gameObject.name + " has not been assigned tagging or Tagging for this meteor does not exist");
                Debug.Break();
                damageDealer = 0;
                break;
        }

        //After Damagedealer is assigned, processhit is called
        ProcessHit(damageDealer);
    }

    private void InflictBlastDamageOnceMethod()
    {
        if (!inflictBlastDamageOnceBool)
        {
            meteorSpawnerScript.Call_ThisMethod_Coroutine(gameObject);
            damageDealer = 3000;
            inflictBlastDamageOnceBool = true;
        }
        else
        {
            damageDealer = 0;
        }
    }

    public void turnOffmyExplosionBoolPlease()
    { inflictBlastDamageOnceBool = false; }

    private void ProcessHit(int damageDealer)
    {
        healthMeteor -= damageDealer;

        //PlayHitPlayerVFX();

        if (healthMeteor <= 0)
        {
            DoNotLockToPeanut_resetLayer();

            //Removes tracking from peanut for tinyfrozenmeteors
            if (gameObject.tag == "tinyFrozenMeteor")
            {
                meteorSpawnerScript.Remove_meteor_from_List_withinBounds_due_to_death(gameObject);
            }           

            //Executes Death code
            if (isSpawnedOriginal)
            {
                General_Death_Code("Spawn Meteor Object List");
            }
            else if (isSpawnedFrozen)
            {
                General_Death_Code("Spawn Frozen Meteor Object List");
            }
            Instantiate_and_Play_VFX_and_Disable_ThisObject();
        }
    }

    public void DoNotLockToPeanut_resetLayer()
    {
        newTransform = myTransformStored;
        lockedToPeanut = false;
        gameObject.layer = 14;
    }

    private void General_Death_Code(string listType)
    {
        if ((gameObject.tag == "hugeMeteor") | (gameObject.tag == "hugeFrozenMeteor"))
        {
            meteorSpawnerScript.Spawn_BigMeteorObjects(gameObject.transform, gameObject.name);
        }
        else if ((gameObject.tag == "bigMeteor") | (gameObject.tag == "bigFrozenMeteor") | (gameObject.tag == "sfd_bigMeteor") | (gameObject.tag == "sfd_bigFrozenMeteor"))
        {
            meteorSpawnerScript.Spawn_Med_MeteorObjects(gameObject.transform, gameObject.name);
        }
        else if ((gameObject.tag == "madeMeteor") | (gameObject.tag == "medFrozenMeteor") | (gameObject.tag == "sfd_madeMeteor") | (gameObject.tag == "sfd_madeFrozenMeteor"))
        {
            meteorSpawnerScript.Spawn_tiny_MeteorObjects(gameObject.transform, gameObject.name);
        }
        else if ((gameObject.tag == "tinyMeteor") | (gameObject.tag == "tinyFrozenMeteor") | (gameObject.tag == "sfd_tinyMeteor") | (gameObject.tag == "sfd_tinyFrozenMeteor"))
        {
            if ((gameObject.name == "dennis") | 
                (gameObject.name == "matty") | 
                (gameObject.name == "herbert") | 
                (gameObject.name == "jacqueline") | 
                (gameObject.name == "tina")|
                (gameObject.name == "totoy") |
                (gameObject.name == "opay") |
                (gameObject.name == "bantay") |
                (gameObject.name == "bulabog") |
                (gameObject.name == "cordapia"))
            {
                DoNotLockToPeanut_resetLayer();
                GameObject smoke = Instantiate(
                    vfxExplosionSmoke,
                    transform.position,
                    transform.rotation * Quaternion.Euler(0, -180f, 0f));
                Destroy(smoke, 2f);
                meteorSpawnerScript.Add_back_to_list(gameObject, listType);//VFX instantiation above since Meteor gets disabled before Instantiation could occur
            }
        }
    }

    private void Instantiate_and_Play_VFX_and_Disable_ThisObject()
    {
        GameObject smoke = Instantiate(
            vfxExplosionSmoke,
            transform.position,
            transform.rotation * Quaternion.Euler(0, -180f, 0f));
        Destroy(smoke, 2f);

        Kill_this_meteor_meaning_disable_backtoOriginal_pos_andresethealth();
    }
    public void Kill_this_meteor_meaning_disable_backtoOriginal_pos_andresethealth() // to not split, so do not merge with Instantiate and Play VFX and Disable ThisObject
    {
        Reset_Health_To_Full();//do I want to reset or keep the Damage of the meteors? or if health = 0 reset health

        Back_To_Original_Position();

        meteorSpawnerScript.Remove_meteor_from_List_withinBounds_due_to_death(gameObject);

        gameObject.SetActive(false);
    }

    private void Reset_Health_To_Full() // A seperate method just in case I want to delete this later
    {
        if ((gameObject.tag == "hugeMeteor")| (gameObject.tag == "hugeFrozenMeteor")) { healthMeteor = 5000; }
        else if ((gameObject.tag == "bigMeteor")| (gameObject.tag == "sfd_bigMeteor")| (gameObject.tag == "bigFrozenMeteor") | (gameObject.tag == "sfd_bigFrozenMeteor")) { healthMeteor = 2500; }
        else if ((gameObject.tag == "madeMeteor")| (gameObject.tag == "sfd_madeMeteor")| (gameObject.tag == "medFrozenMeteor") | (gameObject.tag == "sfd_madeFrozenMeteor")) { healthMeteor = 1250; }
        else if ((gameObject.tag == "tinyMeteor")| (gameObject.tag == "tinyMeteor")| (gameObject.tag == "tinyFrozenMeteor") | (gameObject.tag == "sfd_tinyFrozenMeteor")) { healthMeteor = 500; }
    }

    public void Back_To_Original_Position()
    {
        Debug.Log(myName);
        if (isSpawnedOriginal)
        {
            switch(myName)
            {
                case "Toby A Smith":
                    transform.localPosition = originalPosition; RestartSettingsfor_OriginalMeteor_to_NonFrozen();
                    break;
                case "Jackie B Landon":
                    transform.localPosition = originalPosition; RestartSettingsfor_OriginalMeteor_to_NonFrozen();
                    break;
                case "Jackie C Price":
                    transform.localPosition = originalPosition; RestartSettingsfor_OriginalMeteor_to_NonFrozen();
                    break;
                case "Jackie D White":
                    transform.localPosition = originalPosition; RestartSettingsfor_OriginalMeteor_to_NonFrozen();
                    break;
                case "Robert E Jacobs":
                    transform.localPosition = originalPosition; RestartSettingsfor_OriginalMeteor_to_NonFrozen();
                    break;
                case "Robert F morris":
                    transform.localPosition = originalPosition; RestartSettingsfor_OriginalMeteor_to_NonFrozen();
                    break;
                case "Robert G Johnson":
                    transform.localPosition = originalPosition; RestartSettingsfor_OriginalMeteor_to_NonFrozen();
                    break;
                case "tina":
                    transform.localPosition = originalPosition; RestartSettingsfor_OriginalMeteor_to_NonFrozen();
                    break;
                case "jacqueline":
                    transform.localPosition = originalPosition; RestartSettingsfor_OriginalMeteor_to_NonFrozen();
                    break;
                case "herbert":
                    transform.localPosition = originalPosition; RestartSettingsfor_OriginalMeteor_to_NonFrozen();
                    break;
                case "matty":
                    transform.localPosition = originalPosition; RestartSettingsfor_OriginalMeteor_to_NonFrozen();
                    break;
                case "dennis":
                    transform.localPosition = originalPosition; RestartSettingsfor_OriginalMeteor_to_NonFrozen(); //Here is where stops for RestartSettingsfor_OriginalMeteor_to_NonFrozen();
                    break;
                case "Jackie A Smith":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Smith_Baby();
                    break;
                case "Jackie B Smith":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Smith_Baby();
                    break;
                case "Robert A Smith":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Smith_Baby();
                    break;
                case "Robert B Smith":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Smith_Baby();
                    break;
                case "Robert D Smith":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Smith_Baby();
                    break;
                case "Robert E Smith":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Smith_Baby();
                    break;
                case "tina Smith":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Smith_Baby();
                    break;
                case "jacqueline Smith":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Smith_Baby();
                    break;
                case "herbert Smith":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Smith_Baby();
                    break;
                case "matty Smith":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Smith_Baby();
                    break;
                case "dennis Smith":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Smith_Baby();
                    break;
                case "jayson Smith":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Smith_Baby();
                    break;
                case "Paolo Smith":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Smith_Baby();
                    break;
                case "Cataract Smith":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Smith_Baby();
                    break;
                case "Robert A Landon": 
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Landon_Baby();
                    break;
                case "Robert B Landon":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Landon_Baby();
                    break;
                case "tina Landon":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Landon_Baby();
                    break;
                case "jacqueline Landon":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Landon_Baby();
                    break;
                case "herbert Landon":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Landon_Baby();
                    break;
                case "matty Landon":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Landon_Baby();
                    break;
                case "Robert A Price":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Price_Baby();
                    break;
                case "Robert B Price":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Price_Baby();
                    break;
                case "tina Price":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Price_Baby();
                    break;
                case "jacqueline Price":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Price_Baby();
                    break;
                case "herbert Price":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Price_Baby();
                    break;
                case "matty Price":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Price_Baby();
                    break;
                case "Robert A White":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_White_Baby();
                    break;
                case "Robert B White":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_White_Baby();
                    break;
                case "tina White":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_White_Baby();
                    break;
                case "jacqueline White":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_White_Baby();
                    break;
                case "herbert White":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_White_Baby();
                    break;
                case "matty White":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_White_Baby();
                    break;
                case "tina Jacobs":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Jacobs_Baby();
                    break;
                case "jacqueline Jacobs":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Jacobs_Baby();
                    break;
                case "tina Morris":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Morris_Baby();
                    break;
                case "jacqueline Morris":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Morris_Baby();
                    break;
                case "tina Johnson":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_johnson_Baby();
                    break;
                case "jacqueline Johnson":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_johnson_Baby();
                    break;
                default:
                    Debug.Log("MeteorName is Not Listed -NewmeteorScript>>BackToOriginalPos");
                    break;
            }

        }
        else if (isSpawnedFrozen)
        {
            switch(myName)
            {
                case "Diego Santos":
                    transform.localPosition = originalPosition;
                    break;
                case "Yanna Rosales":
                    transform.localPosition = originalPosition;
                    break;
                case "Yanna Jimenez":
                    transform.localPosition = originalPosition;
                    break;
                case "Yanna Taguptup":
                    transform.localPosition = originalPosition;
                    break;
                case "Hyacynth Ramirez":
                    transform.localPosition = originalPosition;
                    break;
                case "Hyacynth Lopez":
                    transform.localPosition = originalPosition;
                    break;
                case "Hyacynth Villareal":
                    transform.localPosition = originalPosition;
                    break;
                case "totoy":
                    transform.localPosition = originalPosition;
                    break;
                case "opay":
                    transform.localPosition = originalPosition;
                    break;
                case "bantay":
                    transform.localPosition = originalPosition;
                    break;
                case "bulabog":
                    transform.localPosition = originalPosition;
                    break;
                case "cordapia":
                    transform.localPosition = originalPosition;
                    break;
                case "Yanna A Santos":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Santos_Baby();
                    break;
                case "Yanna B Santos":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Santos_Baby();
                    break;
                case "Hyacynth A Santos":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Santos_Baby();
                    break;
                case "Hyacynth B Santos":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Santos_Baby();
                    break;
                case "Hyacynth C Santos":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Santos_Baby();
                    break;
                case "Hyacynth D Santos":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Santos_Baby();
                    break;
                case "totoy Santos":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Santos_Baby();
                    break;
                case "opay Santos":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Santos_Baby();
                    break;
                case "bantay Santos":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Santos_Baby();
                    break;
                case "bulabog Santos":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Santos_Baby();
                    break;
                case "cordapia Santos":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Santos_Baby();
                    break;
                case "felimon Santos":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Santos_Baby();
                    break;
                case "santino Santos":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Santos_Baby();
                    break;
                case "soliman Santos":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Santos_Baby();
                    break;
                case "Hyacynth A Rosales":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Rosales_Baby();
                    break;
                case "Hyacynth B Rosales":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Rosales_Baby();
                    break;
                case "totoy Rosales":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Rosales_Baby();
                    break;
                case "opay Rosales":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Rosales_Baby();
                    break;
                case "bantay Rosales":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Rosales_Baby();
                    break;
                case "bulabog Rosales":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Rosales_Baby();
                    break;
                case "Hyacynth A Jimenez":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Jimenez_Baby();
                    break;
                case "Hyacynth B Jimenez":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Jimenez_Baby();
                    break;
                case "totoy Jimenez":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Jimenez_Baby();
                    break;
                case "opay Jimenez":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Jimenez_Baby();
                    break;
                case "bantay Jimenez":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Jimenez_Baby();
                    break;
                case "bulabog Jimenez":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Jimenez_Baby();
                    break;
                case "Hyacynth A Taguptup":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Taguptup_Baby();
                    break;
                case "Hyacynth B Taguptup":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Taguptup_Baby();
                    break;
                case "totoy Taguptup":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Taguptup_Baby();
                    break;
                case "opay Taguptup":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Taguptup_Baby();
                    break;
                case "bantay Taguptup":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Taguptup_Baby();
                    break;
                case "bulabog Taguptup":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Taguptup_Baby();
                    break;
                case "totoy Ramirez":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Ramirez_Baby();
                    break;
                case "opay Ramirez":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Ramirez_Baby();
                    break;
                case "totoy Lopez":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Lopez_Baby();
                    break;
                case "opay Lopez":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Lopez_Baby();
                    break;
                case "totoy Villareal":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Villareal_Baby();
                    break;
                case "opay Villareal":
                    transform.localPosition = originalPosition; Run_With_Bool_Minus_One_Villareal_Baby();
                    break;
                default:
                    Debug.Log("MeteorName is Not Listed");
                    break;

            }
        }
        
    }

    private IEnumerator DelayActivationOf_inflicBlastDamageOnce_Bool()
    {
        yield return new WaitForSeconds(1);
        inflictBlastDamageOnceBool = false;
    }
    /// <summary>
    /// NORMAL NON FROZEN
    /// </summary>
    private void Run_With_Bool_Minus_One_Smith_Baby()
    {
        if (minusBabyOnce == false) { meteorSpawnerScript.Minus_One_SmithBaby(); RestartSettingsto_ImmuneWhenSplit_and_NonFrozen(); minusBabyOnce = true; } 
    }
    private void Run_With_Bool_Minus_One_Landon_Baby()
    {
        if (minusBabyOnce == false) { meteorSpawnerScript.Minus_One_LandonBaby(); RestartSettingsto_ImmuneWhenSplit_and_NonFrozen(); minusBabyOnce = true; }
    }
    private void Run_With_Bool_Minus_One_Price_Baby()
    {
        if (minusBabyOnce == false) { meteorSpawnerScript.Minus_One_PriceBaby(); RestartSettingsto_ImmuneWhenSplit_and_NonFrozen(); minusBabyOnce = true; }
    }
    private void Run_With_Bool_Minus_One_White_Baby()
    {
        if (minusBabyOnce == false) { meteorSpawnerScript.Minus_One_WhiteBaby(); RestartSettingsto_ImmuneWhenSplit_and_NonFrozen(); minusBabyOnce = true; }
    }
    private void Run_With_Bool_Minus_One_Jacobs_Baby()
    {
        if (minusBabyOnce == false) { meteorSpawnerScript.Minus_One_JacobBaby(); RestartSettingsto_ImmuneWhenSplit_and_NonFrozen(); minusBabyOnce = true; }
    }
    private void Run_With_Bool_Minus_One_Morris_Baby()
    {
        if (minusBabyOnce == false) { meteorSpawnerScript.Minus_One_MorrisBaby(); RestartSettingsto_ImmuneWhenSplit_and_NonFrozen(); minusBabyOnce = true; }
    }
    private void Run_With_Bool_Minus_One_johnson_Baby()
    {
        if (minusBabyOnce == false) { meteorSpawnerScript.Minus_One_JohnsonBaby(); RestartSettingsto_ImmuneWhenSplit_and_NonFrozen(); minusBabyOnce = true; }
    }
    /// <summary>
    /// FROZEN METEORS
    /// </summary>
    private void Run_With_Bool_Minus_One_Santos_Baby()
    {
        if (minusBabyOnce == false) { meteorSpawnerScript.Minus_One_SantosBaby(); RestartSettingsto_ImmuneWhenSplit_and_StillFrozen(); minusBabyOnce = true; }
    }

    private void Run_With_Bool_Minus_One_Rosales_Baby()
    {
        if (minusBabyOnce == false) { meteorSpawnerScript.Minus_One_RosalesBaby(); RestartSettingsto_ImmuneWhenSplit_and_StillFrozen(); minusBabyOnce = true; }
    }

    private void Run_With_Bool_Minus_One_Jimenez_Baby()
    {
        if (minusBabyOnce == false) { meteorSpawnerScript.Minus_One_Jimenez(); RestartSettingsto_ImmuneWhenSplit_and_StillFrozen(); minusBabyOnce = true; }
    }

    private void Run_With_Bool_Minus_One_Taguptup_Baby()
    {
        if (minusBabyOnce == false) { meteorSpawnerScript.Minus_One_Taguptup(); RestartSettingsto_ImmuneWhenSplit_and_StillFrozen(); minusBabyOnce = true; }
    }
    private void Run_With_Bool_Minus_One_Ramirez_Baby()
    {
        if (minusBabyOnce == false) { meteorSpawnerScript.Minus_One_Ramirez(); RestartSettingsto_ImmuneWhenSplit_and_StillFrozen(); minusBabyOnce = true; }
    }
    private void Run_With_Bool_Minus_One_Lopez_Baby()
    {
        if (minusBabyOnce == false) { meteorSpawnerScript.Minus_One_Lopez(); RestartSettingsto_ImmuneWhenSplit_and_StillFrozen(); minusBabyOnce = true; }
    }
    private void Run_With_Bool_Minus_One_Villareal_Baby()
    {
        if (minusBabyOnce == false) { meteorSpawnerScript.Minus_One_Villareal(); RestartSettingsto_ImmuneWhenSplit_and_StillFrozen(); minusBabyOnce = true; }
    }

    public void EnableMeteor()
    {
        gameObject.SetActive(true);
        minusBabyOnce = false;
    }

    public void SpawnFrozen()
    {
        freezeOn = true;
        freezeTimer = 1;

        switch(gameObject.tag)
        {
            case "hugeMeteor":
                gameObject.tag = "hugeFrozenMeteor";
                break;
            case "madeMeteor":
                gameObject.tag = "medFrozenMeteor";
                break;
            case "sfd_madeMeteor":
                gameObject.tag = "sfd_madeFrozenMeteor";
                break;
            case "bigMeteor":
                gameObject.tag = "bigFrozenMeteor";
                break;
            case "sfd_bigMeteor":
                gameObject.tag = "sfd_bigFrozenMeteor";
                break;
            case "sfd_tinyMeteor":
                gameObject.tag = "sfd_tinyFrozenMeteor";
                break;
            case "tinyMeteor":
                gameObject.tag = "tinyFrozenMeteor";
                break;
            default:
                break;
        }

    }

    public void RestartSettingsfor_OriginalMeteor_to_NonFrozen() //Separate Code for Original to not frozen since we dont want immunity when it spawns
    {
        freezeOn = false;
        freezeTimer = 0;
        mySpriteRenderer.color = colorTransparent;

        if (gameObject.tag == "hugeFrozenMeteor")
        {
            gameObject.tag = "hugeMeteor";
        }
        else if (gameObject.tag == "bigFrozenMeteor")
        {
            gameObject.tag = "bigMeteor";
        }
        else if (gameObject.tag == "medFrozenMeteor")
        {
            gameObject.tag = "madeMeteor";
        }
        else if (gameObject.tag == "tinyFrozenMeteor")
        {
            gameObject.tag = "tinyMeteor";
        }
    }

    public void RestartSettingsto_ImmuneWhenSplit_and_NonFrozen()
    {
        freezeOn = false;
        freezeTimer = 0;
        mySpriteRenderer.color = colorTransparent;

        if ((gameObject.tag == "bigFrozenMeteor")| (gameObject.tag == "bigMeteor"))
        {
            gameObject.tag = "sfd_bigMeteor";
        }
        else if ((gameObject.tag == "medFrozenMeteor")| (gameObject.tag == "madeMeteor"))
        {
            gameObject.tag = "sfd_madeMeteor";
        }
        else if ((gameObject.tag == "tinyFrozenMeteor")| (gameObject.tag == "tinyMeteor"))
        {
            gameObject.tag = "sfd_tinyMeteor";
        }

        immunityCounter = -1.5f;
        immunityOn = true;
    }

    public void RestartSettingsto_ImmuneWhenSplit_and_StillFrozen()
    {
        if ((gameObject.tag == "bigFrozenMeteor") | (gameObject.tag == "bigMeteor"))
        {
            gameObject.tag = "sfd_bigFrozenMeteor";
        }
        else if ((gameObject.tag == "medFrozenMeteor") | (gameObject.tag == "madeMeteor"))
        {
            gameObject.tag = "sfd_madeFrozenMeteor";
        }
        else if ((gameObject.tag == "tinyFrozenMeteor") | (gameObject.tag == "tinyMeteor"))
        {
            gameObject.tag = "sfd_tinyFrozenMeteor";
        }

        immunityCounter = -1.5f;
        immunityOn = true;
    }

    public string Get_List_Type() { return listType; }

    public void Flip_velocity()// for when in contact with meteorforcefeild
    {
            myRigidBody.velocity = -myRigidBody.velocity;
    }

    //FREEZE CODE BELOW
    void OnParticleCollision(GameObject other)
    {
        //Name of the Freeze Lazerbeam object is Particle launcher
        if (other.name == "Particle launcher")
        {
            freezeOn = true;
        }
    }

    public string GetFamilyNameOfMeteor() { return familyNameOfMeteor; }
}
