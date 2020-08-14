using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] bool delayCoroutineUntilAllEnemiesSpawned = false;
    [SerializeField] int stopDelayAfterThisWave = 0;
    [SerializeField] bool turnBackOndelayCoroutineUntilAllEnemiesSpawned = false;
    [SerializeField] int turnBackOnAfterThisWave = 0;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = false;
    [SerializeField] GameObject gameSessionObject;

    GameSession gameSessionScript;

    bool cachedelayCoroutineBool;
    bool cacheTurnOndelayCoroutineBool;
    int cacheStopWaveNumber;
    int cacheTurnOnWaveNumber;

    // need to store bool delayCoroutineUntilAllEnemiesSpawned and turnBackOndelayCoroutineUntilAllEnemiesSpawned 
    /*
     * In game session since the game will restart and you want the values to reset to what they were
     * or just cache it in here to save headache
     * then call a reset once final death sequence is called 
     */

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        //caching the shizz
        gameSessionScript = gameSessionObject.GetComponent<GameSession>();
        cachedelayCoroutineBool = delayCoroutineUntilAllEnemiesSpawned;
        cacheStopWaveNumber = stopDelayAfterThisWave;
        cacheTurnOndelayCoroutineBool = turnBackOndelayCoroutineUntilAllEnemiesSpawned;
        cacheTurnOnWaveNumber = turnBackOnAfterThisWave;

        do
        {
                yield return StartCoroutine(SpawnAllWaves());
        }
        while (looping);
    }

    public void ResetEnemySpawnerShizz()
    {
        delayCoroutineUntilAllEnemiesSpawned = cachedelayCoroutineBool;
        stopDelayAfterThisWave = cacheStopWaveNumber;
        turnBackOndelayCoroutineUntilAllEnemiesSpawned = cacheTurnOndelayCoroutineBool;
        turnBackOnAfterThisWave = cacheTurnOnWaveNumber;
    }

    public IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
            //This code turns of bool so the spawner doesn't have to wait until all
            //enemobjects are spawned before proceeding with next wave
            if(delayCoroutineUntilAllEnemiesSpawned)
            {
                //checks to make sure index is not null
                if (stopDelayAfterThisWave == 0)
                {
                    Debug.Log("No Value is Input for EnemySpawner/stopDelayAfterThisWave");
                }
                else if (stopDelayAfterThisWave < 0)
                {
                    Debug.Log("Value Null for EnemySpawner/stopDelayAfterThisWave");
                    Debug.Log("Value must be greater than zero");
                }
                else if (waveIndex > stopDelayAfterThisWave)
                {
                    Debug.Log("Value Null for EnemySpawner/stopDelayAfterThisWave");
                    Debug.Log("Value must be less than total number of waves");
                }
                else if (waveIndex == (stopDelayAfterThisWave - 1))
                {
                    delayCoroutineUntilAllEnemiesSpawned = false;
                }
            }

            //This code turns bool back on after certain number of waves
            //check in pace to make sure that there is a value for stop delay
            //and that value for turnbackOn is not less than value for stopdelay
            // to avoud conflict in code
            if((turnBackOnAfterThisWave> stopDelayAfterThisWave) &&(!(stopDelayAfterThisWave==0)))
            {
                if (waveIndex ==(turnBackOnAfterThisWave-1))
                {
                    delayCoroutineUntilAllEnemiesSpawned = true;
                }
            }

            var currentWave = waveConfigs[waveIndex];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
            //checkforcondition if number of Enemis is 0
        }
       
        if (gameObject.tag == "EnemySpawner")
        {
            //check what happens in build mode if this is deactivated
            Debug.Log("Enemy Spawner Coroutine Done");
            StartCoroutine(DelayTurnOnBoolEnemySpawnerCoroutine());
        }
        else if (gameObject.tag == "BossSpawner")
        {
            Debug.Log("Boss Spawner Coroutine Done");
            StartCoroutine(DelayTurnOnBoolBossSpawnerCoroutine());
        }

        StopCoroutine(SpawnAllWaves());


    }

    private IEnumerator DelayTurnOnBoolBossSpawnerCoroutine()
    {
        yield return new WaitForSeconds(2);
        gameSessionScript.TurnOnIsBossCoroutineDone();
        StopCoroutine(DelayTurnOnBoolBossSpawnerCoroutine());
    }

    private IEnumerator DelayTurnOnBoolEnemySpawnerCoroutine()
    {
        yield return new WaitForSeconds(2);
        gameSessionScript.TurnOnBoolIsEnemySpawnerCouRoutineDone();
        StopCoroutine(DelayTurnOnBoolEnemySpawnerCoroutine());
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
            var newEnemy = Instantiate(
                waveConfig.GetEnemyPrefab(),
                waveConfig.GetWayPoints()[0].transform.position,
                Quaternion.identity);
            gameSessionScript.AddEnemies();
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());

            if( (enemyCount == waveConfig.GetNumberOfEnemies()-1) && (delayCoroutineUntilAllEnemiesSpawned))
            {
                yield return new WaitUntil(() => gameSessionScript.ReturnEnemies() 
                    <= gameSessionScript.ReturnDestroyedEnemies());
            }
 
        }
    }

    public IEnumerator SpawnAllWavesDelayed()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(SpawnAllWaves());
    }


}
