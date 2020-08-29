using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    //Params for Delay Death Courotine
    [SerializeField] float delayInSeconds = 2f;

    MusicPlayer musicPlayerScript;
    GameSession gameSessionScript;
    //This is because there is a Null reference error when LoadMainGame is used(seperate method is called
    //without FindObjectOfType<GameSession>().ResetGame(); 
    //Seperate start scene is created as well Current Seperate start scene is at Index number so string name is used

    private void Start()
    {
        musicPlayerScript = FindObjectOfType<MusicPlayer>();
        gameSessionScript = FindObjectOfType<GameSession>();
    }

    //This is because there is a Null reference error when LoadMainGame is used(seperate method is called
    //without FindObjectOfType<GameSession>().ResetGame(); 
    //First Scene is Start menu
    //Loads from first scene
    public void StartLoadMainGameFromStart()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadGameOver()
    {
        StartCoroutine(DelayDeath());
    }

    private IEnumerator DelayDeath()
    {
        yield return new WaitForSeconds(delayInSeconds);
        //musicPlayerScript.PlayBGMGameOver();
        SceneManager.LoadScene("Game Over");
    }

    //Second Menu scene is Game Over
    //Restart Button Accesses this script
    public void LoadMainGameFromContinue()//should just go through start menu replay so that sceneObejcts (music player & gamesession) can be destroyed
    {
        SceneManager.LoadScene(1);
        gameSessionScript.ResetGame();
        gameSessionScript.Level1Start();
        //musicPlayerScript.PlayBGMMusicLevelOne(); Level.CS should not play music since musicloader will be destroyed
    }

    //In Game Over Menu -Main Menu Button Accesses this script
    //Third menu Scene is loaded
    public void LoadStartMenuReplay()
    {
        SceneManager.LoadScene("Raply Start Menu Replay");
    }

    //Third Menu Start Button Accesses this script
    public void LoadMainGame()
    {
        SceneManager.LoadScene(1);
        gameSessionScript.ResetGame();
        gameSessionScript.Level1Start();
        musicPlayerScript.PlayBGMMusicLevelOne();
    }


    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level 3");
    }

    public void LoadLevel4()
    {
        SceneManager.LoadScene("Level 4");
    }

    public void LoadLevel5()
    {
        SceneManager.LoadScene("Level 5");
    }

    public void LoadWinScreen()
    {
        SceneManager.LoadScene("You Win");
    }


    public void QuitGame()
    {
        Application.Quit();
    }

}
