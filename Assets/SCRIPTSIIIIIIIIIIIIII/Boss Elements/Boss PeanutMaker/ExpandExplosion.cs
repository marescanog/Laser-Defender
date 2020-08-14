using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandExplosion : MonoBehaviour
{
    [SerializeField] Color mycolor = Color.white;
    [SerializeField] Color transparent = Color.white;
    float timerScale = 0f;
    float colorScale = 0f;
    bool colorScaleOn = false;
    SpriteRenderer mySpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0, 0, 0);
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerScale <= 2)
        {
            timerScale += (Time.deltaTime * 10);

            Vector3 temp = transform.localScale;

            temp.x = timerScale;
            temp.y = timerScale;
            temp.z = timerScale;

            transform.localScale = temp;
            colorScaleOn = true;
        }

        
        if (colorScaleOn)
        {
            colorScale += (Time.deltaTime * 1);
            mySpriteRenderer.color = Color.Lerp(mycolor, transparent, colorScale);
            StartCoroutine(DestroyThisObjectDelayed());
        }        
    }

    private IEnumerator DestroyThisObjectDelayed()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
