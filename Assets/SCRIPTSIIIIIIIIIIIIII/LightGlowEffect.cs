using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGlowEffect : MonoBehaviour
{
    [SerializeField] float minRange = 1.5f;
    [SerializeField] float MinRandomMinRange = 0f;
    [SerializeField] float maxRange = -1.5f;
    [SerializeField] float MinRandomMaxRange;
    [SerializeField] [Range(0,1)] float phaseSpeed = 1f;
    [SerializeField] float rangeCount = 1f;

    bool increaseRangeCount = true;
    float randomMaxRange;
    float randomMinRange;

    Light mylight;

    // Start is called before the first frame update
    void Start()
    {
        mylight = GetComponent<Light>();
        randomMaxRange = Random.Range(MinRandomMaxRange, maxRange);
        randomMinRange = Random.Range(MinRandomMinRange, minRange);
    }

    // Update is called once per frame
    void Update()
    {

        if (increaseRangeCount)
        {
            rangeCount += Time.deltaTime*phaseSpeed;

            if (rangeCount >= randomMaxRange)
            {
                increaseRangeCount = false;
                randomMaxRange = Random.Range(MinRandomMaxRange, maxRange);
            }
        }
        else if (!increaseRangeCount)
        {
            rangeCount -= Time.deltaTime * phaseSpeed;

            if (rangeCount <= randomMinRange)
            {
                increaseRangeCount = true;
                randomMinRange = Random.Range(MinRandomMinRange, minRange);
            }
        }

        mylight.range = rangeCount;
    }
}
