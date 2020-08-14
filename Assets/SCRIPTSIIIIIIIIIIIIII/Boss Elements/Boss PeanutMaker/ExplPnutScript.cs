using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplPnutScript : MonoBehaviour
{
    [SerializeField] Color myColor = Color.white;
    [SerializeField] Color beepColor = Color.white;
    [SerializeField] Color invisible = Color.white;
    [SerializeField] GameObject aoeCircle;
    [SerializeField] GameObject cartoonBoom;
    [SerializeField] GameObject explosionDamage;
    [SerializeField] AudioClip boomSFX;
    float timerScale = 0f;
    float timerColor = 0f;
    bool startMovement = false;
    private Vector3 target;
    float speed=2f;
    GameObject playerObject;
    GameObject aoeObject;
    SpriteRenderer mySpriteRenderer;
    AudioSource myAudioSource;
    bool aoeCircleInstantiated = false;
    bool startExplosionSequence = false;
    bool exploded = false;
    float beepSpeed = 3f;
    bool noPlayerInScene = false;

    BossPeanutMaker bossPeanutMakerScript;
    GameObject bossPeanutMaker;

    void Start()
    {
        bossPeanutMaker = FindObjectOfType<BossPeanutMaker>().gameObject;
        bossPeanutMakerScript = bossPeanutMaker.GetComponent<BossPeanutMaker>();
        transform.localScale = new Vector3(0,0,0);
        StartCoroutine(BreakFreeFromParentTransform());
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAudioSource = GetComponent<AudioSource>();
        mySpriteRenderer.color = myColor;
        myAudioSource.volume = 0.25f;

        Player playerInScene = (Player)FindObjectOfType(typeof(Player));
        if (playerInScene)
        {
            noPlayerInScene = false;
        }
        else
        { 
            noPlayerInScene = true;
        }

        bossPeanutMakerScript.AddExplodePeanut(gameObject);


    }

    void Update()
    {
        if(timerScale<=1)
        {
            timerScale += (Time.deltaTime * 1);

            Vector3 temp = transform.localScale;

            temp.x = timerScale;
            temp.y = timerScale;
            temp.z = timerScale;

            transform.localScale = temp;
        }

        if(noPlayerInScene)
        {
            //add loop movement or whatever
        }
        else 
        {
            if (startMovement)
            {
                if (!aoeCircleInstantiated)
                {
                    GameObject aoeCircObject = Instantiate(aoeCircle, playerObject.transform.position, transform.rotation) as GameObject;
                    aoeCircObject.name = gameObject.name + " AOECircle";
                    aoeCircObject.transform.parent = playerObject.transform;
                    aoeObject = aoeCircObject;
                    aoeCircleInstantiated = true;
                }

                if (!startExplosionSequence)
                {
                    float step = speed * Time.deltaTime;
                    target = playerObject.transform.position;
                    transform.position = Vector2.MoveTowards(transform.position, target, step);
                }

                if (!exploded)
                {
                    timerColor += (Time.deltaTime * beepSpeed); //5 makes it blink fast

                    if (timerColor <= 0.5f)
                    {
                        mySpriteRenderer.color = myColor;
                    }
                    else if (timerColor <= 1)
                    {
                        mySpriteRenderer.color = beepColor;
                        myAudioSource.Play();
                    }
                    else if (timerColor >= 1.5f)
                    {
                        timerColor = 0;
                    }
                }
            }
        }
    }

    private void Found_PlayerObject()
    {
        playerObject = FindObjectOfType<Player>().gameObject;
        target = playerObject.transform.position;
        startMovement = true;
        noPlayerInScene = false;
    }

    private IEnumerator FindPlayerAfterEvery2Secs()
    {
        yield return new WaitForSeconds(2);
        Player playerInScene = (Player)FindObjectOfType(typeof(Player));
        if (playerInScene)
        {
            Found_PlayerObject();
        }
        else
        {
            StartCoroutine(FindPlayerAfterEvery2Secs());
            noPlayerInScene = true;
        }
    }

    private IEnumerator BreakFreeFromParentTransform()
    {
        yield return new WaitForSeconds(7f);
        gameObject.transform.parent = null;

        if(noPlayerInScene)
        {
            StartCoroutine(FindPlayerAfterEvery2Secs());
            noPlayerInScene = true;
        }
        else 
        {
            playerObject = FindObjectOfType<Player>().gameObject;
            target = playerObject.transform.position;
            startMovement = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(aoeObject==null)
        {
            //Do nothing
        }
        else
        {
            if (other.name == aoeObject.name)
            {
                if(!startExplosionSequence)
                {
                    StartExplosionSequence();
                    startExplosionSequence = true;
                    Destroy(other.gameObject);
                }
            }
        }
    }

    private void StartExplosionSequence()
    {
        beepSpeed = 10f;
        bossPeanutMakerScript.RemoveExplodePeanut(gameObject);
        StartCoroutine(ExplodeThisObject());
    }

    public IEnumerator ExplodeThisObject()
    {
        
        yield return new WaitForSeconds(.75f);
        exploded = true;
        AudioSource.PlayClipAtPoint(boomSFX, Camera.main.transform.position, 1f);
        mySpriteRenderer.color = invisible;
        GameObject explosion = Instantiate(cartoonBoom, transform.position, transform.rotation) as GameObject;
        Instantiate(explosionDamage, transform.position, transform.rotation);
        Destroy(explosion, 2);
        Destroy(gameObject, 2);


    }
}
