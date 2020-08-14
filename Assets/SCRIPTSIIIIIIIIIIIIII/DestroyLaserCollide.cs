using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyLaserCollide : MonoBehaviour
{
    //This script is attached to the Player laser

    //Laser VFX Config Params
    //This script is attached to the Player Laser
    [SerializeField] GameObject laserHitVFX;
    [SerializeField] GameObject BrainHitVFX;
    [SerializeField] AudioClip getHitSound;
    [SerializeField] AudioClip BrainGetHitSound;
    [SerializeField] AudioClip getHitBombSound;
    [SerializeField] [Range(0, 1)] float getHitSoundVolume = 0.1f;
    [SerializeField] [Range(0, 1)] float getHitBombSoundVolume = 0.1f;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.name);
        string collisionName = other.gameObject.name;

        if((collisionName== "Player") | (collisionName == "Ice Cube(Clone)"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "EnemyLaser")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            TriggerLaserHitVFX();
            AudioSource.PlayClipAtPoint(getHitSound, Camera.main.transform.position, getHitSoundVolume);
        }
        else if (other.gameObject.tag == "BEnemyBomb")
        {
            //Destroy(gameObject); Object will not destroy itself if enemy bomb is hit
            TriggerLaserHitVFX();
            AudioSource.PlayClipAtPoint(getHitBombSound, Camera.main.transform.position, getHitBombSoundVolume);
        }
        else if (other.gameObject.tag == "Enemy")
        {
            //Destroy(other.gameObject); Diabled:does not destoy Enemy, Scipt within Enemy destoys itself when Health=0

            //Laser destroys itself
            Destroy(gameObject);

            TriggerLaserHitVFX();

            //change sound when enemy hit, enable once you find sound
            //AudioSource.PlayClipAtPoint(getHitSound, Camera.main.transform.position, getHitSoundVolume);
        }
        else if (other.gameObject.tag == "BossRocky")
        {
            //Destroy(other.gameObject); Diabled:does not destoy Enemy, Scipt within Enemy destoys itself when Health=0

            //Laser destroys itself
            if(other.gameObject.name == "Brain")
            {
                TriggerBrainHitVFX();
                AudioSource.PlayClipAtPoint(BrainGetHitSound, Camera.main.transform.position, getHitSoundVolume);
                Destroy(gameObject);
                Debug.Log("hit brain");
            }
            else 
            {
                TriggerLaserHitVFX();
                Destroy(gameObject);
            }

            //change sound when enemy hit, enable once you find sound
            //AudioSource.PlayClipAtPoint(BrainGetHitSound, Camera.main.transform.position, getHitSoundVolume);
        }
        else if ((other.gameObject.tag == "hugeMeteor") | (other.gameObject.tag == "tinyMeteor") |
                (other.gameObject.tag == "madeMeteor") | (other.gameObject.tag == "bigMeteor") |
                (other.gameObject.tag == "hugeFrozenMeteor") |
                (other.gameObject.tag == "bigFrozenMeteor") | 
                (other.gameObject.tag == "medFrozenMeteor") | 
                (other.gameObject.tag == "tinyFrozenMeteor") | 
                (other.gameObject.tag == "sfd_bigFrozenMeteor") | 
                (other.gameObject.tag == "sfd_madeFrozenMeteor") | 
                (other.gameObject.tag == "sfd_tinyFrozenMeteor") | 
                (other.gameObject.tag == "sfd_bigFrozenMeteor") | 
                (other.gameObject.tag == "sfd_medFrozenMeteor") | 
                (other.gameObject.tag == "sfd_tinyFrozenMeteor") 
                )
        {
            Destroy(gameObject);
            TriggerLaserHitVFX(); 
        }
        else if (other.gameObject.tag == "icePeanut")
        {
            Destroy(gameObject);
            TriggerLaserHitVFX();
        }
        else
        {
        }
    }

    private void TriggerBrainHitVFX()
    {
        GameObject vfx = Instantiate(
            BrainHitVFX,
            transform.position,
            transform.rotation * Quaternion.Euler(180f, 0, 0f));
        Destroy(vfx, 1f);
    }

    private void TriggerLaserHitVFX()
    {
        GameObject vfx = Instantiate(
            laserHitVFX,
            transform.position,
            Quaternion.identity);
        Destroy(vfx, 1f);
    }
}
