using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioSource bGM;

    [Header("Background Music")]
    [SerializeField] AudioClip levelOneMusic;
    [SerializeField] AudioClip levelTwoMusic;
    [SerializeField] AudioClip levelThreeMusic;
    [SerializeField] AudioClip levelFourMusic;
    [SerializeField] AudioClip levelFiveMusic;
    [SerializeField] AudioClip gameOverBGM;

    [Header("Boss Fight Music")]
    [SerializeField] AudioClip bossMusic;
    [SerializeField] AudioClip bossMusicLevel2;

    [Header("Sound effects")]
    [SerializeField] AudioClip gameOverSound;
    [SerializeField] AudioClip levelOneSound;
    [SerializeField] AudioClip levelTwoSound;
    [SerializeField] AudioClip levelThreeSound;
    [SerializeField] AudioClip levelFourSound;
    [SerializeField] AudioClip FinalRoundSound;
    [SerializeField] AudioClip MissionCompleteSound;

    bool gamesessionDestroyed = false;
    //float currentTime = 0;
    //float start;

    // Start is called before the first frame update
    void Awake()
    {
        SetUpSingleton();
    }

    public void RestBGMVolume() { bGM.volume = 1f; }
    public IEnumerator StartFade( float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = bGM.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            bGM.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            if (gamesessionDestroyed)
            {

            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }

    }

    /// <summary>
    /// =====BOSS FIGHT MUSIC
    /// </summary>
    public void ChangeBGMtoBossFightMusic()
    {
        bGM.Stop();
        bGM.volume = 0.3f;
        bGM.clip = bossMusic;
        bGM.Play();
    }

    public void ChangeBGMtoBossFightMusicLevelTwo()
    {
        bGM.Stop();
        bGM.volume = 0.3f;
        bGM.clip = bossMusicLevel2;
        bGM.Play();
    }

    /// <summary>
    /// ====BGM
    /// </summary>
    public void PlayBGMMusicLevelOne()
    {
        bGM.Stop();
        bGM.clip = levelOneMusic;
        bGM.Play();
    }

    public void PlayBGMMusicLevelTwo()
    {
        bGM.Stop();
        bGM.clip = levelTwoMusic;
        bGM.Play();
    }

    public void PlayBGMMusicLevelThree()
    {
        bGM.Stop();
        bGM.clip = levelThreeMusic;
        bGM.Play();
    }

    public void PlayBGMMusicLevelFour()
    {
        bGM.Stop();
        bGM.clip = levelFourMusic;
        bGM.Play();
    }

    public void PlayBGMMusicLevelFive()
    {
        bGM.Stop();
        bGM.clip = levelFiveMusic;
        bGM.Play();
    }

    public void PlayGameOver()
    {AudioSource.PlayClipAtPoint(gameOverSound, Camera.main.transform.position, 1);}
    public void PlayLevelOneSound()
    { AudioSource.PlayClipAtPoint(levelOneSound, Camera.main.transform.position, 1); }
    public void PlayLevelTwoSound()
    { AudioSource.PlayClipAtPoint(levelTwoSound, Camera.main.transform.position, 1); }
    public void PlayLevelThreeSound()
    { AudioSource.PlayClipAtPoint(levelThreeSound, Camera.main.transform.position, 1); }
    public void PlayLevelFourSound()
    { AudioSource.PlayClipAtPoint(levelFourSound, Camera.main.transform.position, 1); }
    public void PlayFinalRoundSound()
    { AudioSource.PlayClipAtPoint(FinalRoundSound, Camera.main.transform.position, 1); }
    public void PlayMissionCompleted()
    { AudioSource.PlayClipAtPoint(MissionCompleteSound, Camera.main.transform.position, 1); }

    public void PlayBGMGameOver()
    {
        bGM.Stop();
        bGM.clip = gameOverBGM;
        bGM.Play();
    }

    public void DestroyPair()
    {
        gamesessionDestroyed = true;
    }

    public void EnablePair()
    {
        gamesessionDestroyed = false;
    }
}
