using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    //Go through list of pathloops

    [SerializeField] WaveConfig waveConfig; //remove serializefeild later for debugging purposes

    [SerializeField] List<PathLoop> movementLoop; 
    [SerializeField] int pathPointIndex = 0; //remove serializefeild later for debugging purposes
    [SerializeField] int pathLoopIndex = 0; //remove serializefeild later for debugging purposes

    [SerializeField] bool DestroyEndWLoop = true; //True if enemobject will not follow path after waveLoop

    [Header("Path Configuration")]
    [SerializeField] bool IncreaseIndexByOne = true;
    [SerializeField] bool playSecondPathLoop = false;

    [Header("Death Movement Config")]
    [SerializeField] float moveUpSpeed = .25f;
    bool activateDeathPathBossOnly = false;
    bool storeCurrentYPosOnce = false;
    float plusYvalueForMoveUP = 0f;
    float yPosStoreForDeath = 0f;

    BossPeanutMaker myBossPeanutMakerScript;

    private bool PathLoopPlayingAfterWaveLoop = false; //True when WaveLoop is done playing
    GameSession gameSession;

    List<Transform> waypoints;
    int waypointIndex = 0;

    List<Transform> pathPoints;
    [SerializeField] bool isBossPeanut = false;



    // Start is called before the first frame update
    void Start()
    {
       waypoints = waveConfig.GetWayPoints();
       transform.position = waypoints[waypointIndex].transform.position;
       gameSession = FindObjectOfType<GameSession>();

        if(isBossPeanut)
        {
            myBossPeanutMakerScript = gameObject.GetComponent<BossPeanutMaker>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!activateDeathPathBossOnly)
        {
            Move();
        }
        else
        {
            SlowlyMoveUP_StopAndExplode();

        }
    }

    private void SlowlyMoveUP_StopAndExplode()
    {
        if (!storeCurrentYPosOnce)
        {
            yPosStoreForDeath = transform.position.y;
            storeCurrentYPosOnce = true;
        }

        if (plusYvalueForMoveUP <= 1)
        {
            plusYvalueForMoveUP += Time.deltaTime * moveUpSpeed;
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(yPosStoreForDeath + plusYvalueForMoveUP, -3, 7.5f), transform.position.z); //values -3 and 7.5f are screen position
        }
    }

    public void Activate_Boss_Death_path()
    {
        activateDeathPathBossOnly = true;
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    public void PlaySecondPathLoop_Method()
    {
        playSecondPathLoop = true;
        pathLoopIndex = 1;
        pathPointIndex = 0;
        pathPoints = movementLoop[pathLoopIndex].GetPathPoints();
    }

    private void Move()
    {
        if(PathLoopPlayingAfterWaveLoop)
        {
            if (pathLoopIndex <= (movementLoop.Count - 1))
            {
                pathPoints = movementLoop[pathLoopIndex].GetPathPoints();

                if (pathPointIndex <= (pathPoints.Count - 1))
                {
                    
                    var targetPosition = pathPoints[pathPointIndex].transform.position;
                    var movementThisFrame = movementLoop[pathLoopIndex].GetPathMoveSpeed() * Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

                    if (transform.position == targetPosition)
                    {
                        Special_Movement_for_Boss_peanut();
                        pathPointIndex++;
                    }
                }
                else
                {
                    //SettingPathPointIndex to 0 loops the animation
                    if(IncreaseIndexByOne)
                    {
                        if (pathPointIndex > pathPoints.Count - 1)
                        {
                            pathPointIndex = 0;
                            pathLoopIndex++;
                        }
                    }
                    else if (playSecondPathLoop)
                    {
                        pathLoopIndex = 0;
                        pathPointIndex = 0;
                        playSecondPathLoop = false;
                    }
                    else
                    {
                        pathPointIndex = 0;
                    }
                }
            }
            else
            {
                    pathLoopIndex = 0;
                    pathPointIndex = 0;
            }
        }
        else if(!PathLoopPlayingAfterWaveLoop)
        {
            if (waypointIndex <= waypoints.Count - 1)
            {
                var targetPosition = waypoints[waypointIndex].transform.position;
                var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

                if (transform.position == targetPosition)
                {
                    waypointIndex++;
                }
            }
            else if (DestroyEndWLoop)
            {
                Destroy(gameObject);
                gameSession.AddDestroyEnemies();
            }
            else
            {
                PathLoopPlayingAfterWaveLoop = true;
            }
        }
        else 
        {
            Debug.Log("Check enemy Pathing script. Else option triggered. No code.");
        }
    }

    private void Special_Movement_for_Boss_peanut()
    {
        if (isBossPeanut)
        {
            if (pathLoopIndex == 1)//This is the second in list of pathloops which is the laser attack movement
            {
                if (pathPointIndex == 0)
                {
                    myBossPeanutMakerScript.True_FirstPoint_Bool();
                    myBossPeanutMakerScript.Play_Laser_AudioSource();
                }
                else if (pathPointIndex == pathPoints.Count - 1)
                {
                    myBossPeanutMakerScript.Stop_Freeze_Laser_Bream();
                    myBossPeanutMakerScript.Stop_Laser_AudioSource();
                }
            }
        }
    }
}
