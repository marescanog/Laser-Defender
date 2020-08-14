using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainSheildDome : MonoBehaviour
{
    [SerializeField] bool leftOrbDown = false;
    [SerializeField] bool rightOrbDown = false; 
    bool checkOrbBools = false;
    bool blinkingCoroutineOn = false;
    private SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (checkOrbBools)
        {
            if (leftOrbDown && rightOrbDown)
            {
                StartCoroutine(BlinkingSheildDown());
            }
            else { gameObject.SetActive(true); }

            checkOrbBools = false;
        }
    }

    private IEnumerator BlinkingSheildDown()
    {
        blinkingCoroutineOn = true;
        this.spriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.25f);
        this.spriteRenderer.enabled = true;
        yield return new WaitForSeconds(0.25f);
        this.spriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.15f);
        this.spriteRenderer.enabled = true;
        yield return new WaitForSeconds(0.15f);
        this.spriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.15f);
        this.spriteRenderer.enabled = true;
        yield return new WaitForSeconds(0.15f);
        this.spriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.15f);
        this.spriteRenderer.enabled = true;
        if (leftOrbDown && rightOrbDown)
        {
            gameObject.SetActive(false);
            blinkingCoroutineOn = false;
        }
        StopCoroutine(BlinkingSheildDown());
    }

    public void StopCoRoutineDead()
    {
        if (blinkingCoroutineOn)
        {
            StopCoroutine(BlinkingSheildDown());
        }
    }
    public void ActivateOrbDome() 
    { 
        if (blinkingCoroutineOn)
        {
            StopCoroutine(BlinkingSheildDown());
            blinkingCoroutineOn = false;
        }
        gameObject.SetActive(true);
    }

    public void turnOffLeftOrb() { leftOrbDown = true; }
    public void turnOffRightOrb() { rightOrbDown = true; }
    public void turnOnLeftOrb() { leftOrbDown = false; }
    public void turnOnRightOrb() { rightOrbDown = false; }
    public void OrbActiavteCheck() { checkOrbBools = true; }


}
