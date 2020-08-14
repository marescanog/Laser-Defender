using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryRocketCollide : MonoBehaviour
{
    //This script is attached to the Player rocket

    //Laser VFX Config Params
    //This script is attached to the Player Laser
    [SerializeField] GameObject rocketHitVFX;
    [SerializeField] AudioClip getHitSound;
    [SerializeField] AudioClip laseGetHitSound;
    [SerializeField] [Range(0, 1)] float getHitSoundVolume = 0.1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "EnemyLaser")
        {
            Destroy(other.gameObject);
            TriggerRocketHitVFX();
            AudioSource.PlayClipAtPoint(laseGetHitSound, Camera.main.transform.position, getHitSoundVolume);
        }
        else if (other.gameObject.tag == "BEnemyBomb")
        {
            TriggerRocketHitVFX();
            Destroy(other.gameObject);
            AudioSource.PlayClipAtPoint(getHitSound, Camera.main.transform.position, getHitSoundVolume);
        }
        else if (other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            TriggerRocketHitVFX();
            AudioSource.PlayClipAtPoint(getHitSound, Camera.main.transform.position, getHitSoundVolume);
        }
        else if (other.gameObject.tag == "BossRocky")
        {
            Destroy(gameObject);
            TriggerRocketHitVFX();
            AudioSource.PlayClipAtPoint(getHitSound, Camera.main.transform.position, getHitSoundVolume);
        }
        else
        {
        }
    }

    private void TriggerRocketHitVFX()
    {
        GameObject vfx = Instantiate(
            rocketHitVFX,
            transform.position,
            Quaternion.identity);
        Destroy(vfx, 1f);
    }
}
