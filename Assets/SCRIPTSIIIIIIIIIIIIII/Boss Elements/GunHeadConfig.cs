using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHeadConfig : MonoBehaviour
{
    float yProjTraj;
    float xProjTraj;

    //Cached Componenet ref
    Transform myTransform;
    GunHeadConfig gunHeadConfig;
    //[SerializeField] GameObject gunHead;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();
        gunHeadConfig = GetComponent<GunHeadConfig>();
    }

    // Update is called once per frame
    void Update()
    {
        yProjTraj = myTransform.position.y;
        xProjTraj = myTransform.position.x;
    }

    public GunHeadConfig GetGunHead() { return gunHeadConfig; }

    public float GetYProjTrajGunHead() { return yProjTraj; }

    public float GetXProjTrajGunHead() { return xProjTraj; }
}
