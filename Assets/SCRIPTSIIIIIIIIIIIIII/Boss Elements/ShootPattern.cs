using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPattern : MonoBehaviour
{
    [Header("Shooting")]
    float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = -10f;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
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
           transform.position,
           Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
        //laser.GetComponent<Rigidbody2D>().angularVelocity = 250f;
        //AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position,
        //    shootSoundVolume);
    }
}
