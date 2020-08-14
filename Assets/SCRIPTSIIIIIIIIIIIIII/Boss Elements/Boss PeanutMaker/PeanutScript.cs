using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeanutScript : MonoBehaviour
{
    [Header("Serialized For Debugging")]
    [SerializeField] GameObject assignedTinyMeteor;//not serialized for debugging purposes anymore. Assign dummy gameobject so it will not return null
    [SerializeField] int pathPointIndex = 0; //remove serializefeild later for debugging purposes
    [SerializeField] int pathLoopIndex = 0; //remove serializefeild later for debugging purposes
    [SerializeField] int indexPointStart = 0;//remove serializefeild later for debugging purposes

    [Header("GameObjects")]
    [SerializeField] GameObject meteorSpawner;
    [SerializeField] GameObject dummyMeteor;
    [SerializeField] GameObject myParent;
    [SerializeField] GameObject leftChute;
    [SerializeField] GameObject rightChute;
    [SerializeField] GameObject leftChuteEntrance;
    [SerializeField] GameObject rightChuteEntrance;
    [SerializeField] List<GameObject> leftFlightPath;
    [SerializeField] List<GameObject> rightFlightPath;
    [SerializeField] List<PathLoop> movementLoop;
    [SerializeField] PathLoop deathMovement;

    [Header("Speed")]
    [SerializeField] float normalSpeed =5.0f;
    [SerializeField] float slowSpeed =2.5f;
    //GetPathMoveSpeed on PathConfig Also Assigns Speed to Peanut
    //Pathconfig is the List of Pathloop> movement Loop

    float speed;
    private Vector3 target;
    bool targetLocked = false;
    bool targetCaptured = false;
    bool isLeftchute = true;
    bool bossLaserMovement = false;
    List<Transform> pathPoints;
    MeteorSpawnerScript meteorSpawnerScript;
    BossPeanutMaker bossPeanutMakerScript;
    Vector3 lastTargetSaved;
    int lastPathPointIndexSaved = 0;
    [SerializeField] bool bossDefeated = false; //serialized for debug
    Vector3 currentTargetPos;
    [SerializeField] Color transparency;
    [SerializeField] GameObject bOOMVFX;
    float colorTime = 0;
    SpriteRenderer mySpriteRenderer;

    // Start is called before the first frame update


    void Start()
    {
        target = new Vector2(0.0f, 0.0f);
        meteorSpawnerScript = meteorSpawner.GetComponent<MeteorSpawnerScript>();
        bossPeanutMakerScript = myParent.GetComponent<BossPeanutMaker>();
        speed = normalSpeed;
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float step = speed * Time.deltaTime;

        if (!bossDefeated)
        {
            // move sprite towards the target location if target is assigned to peanut
            if (bossLaserMovement)
            {
                if (pathLoopIndex <= (movementLoop.Count - 1))
                {
                    pathLoopIndex = 1;
                    pathPoints = movementLoop[pathLoopIndex].GetPathPoints();

                    var targetPosition = pathPoints[pathPointIndex].transform.position;

                    var movementThisFrame = movementLoop[pathLoopIndex].GetPathMoveSpeed() * Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);
                    if (transform.position == targetPosition)
                    {
                        pathPointIndex++;
                        if (pathPointIndex == pathPoints.Count - 1)
                        {
                            pathPointIndex = 0;

                        }
                    }


                }
            }
            else if (targetLocked)
            {
                // if target isn't captured yet, it moves the peanut toward target
                //if captured it starts to move meteor to hopper
                if (!targetCaptured)
                {
                    //this check below is to prevent the peanut from being assigned a dead meteor
                    //In the event the meteor was in the notassignedbutinbounds list
                    //then dies siultaneously being assigned to this peanut
                    if (!assignedTinyMeteor.activeSelf)
                    {
                        assignedTinyMeteor = dummyMeteor;
                        targetLocked = false;
                        indexPointStart = 0;
                        speed = normalSpeed;
                        //should we put a check to see if the meteor is still in ht eassigned list?
                        //should we put a check to see if the total number of peanuts assigned variable was affected?
                    }

                    target = assignedTinyMeteor.transform.position;

                }
                else if (targetCaptured)
                {
                    //when the meteor is captured, this changes the indexPointStart
                    //when indexpointstart is reached, another is assigned
                    //switch case since indexpoint start can be anywhere bet 0-6
                    switch (indexPointStart)
                    {
                        case 0:
                            Check_if_On_Target();
                            break;
                        case 1:
                            Check_if_On_Target();
                            break;
                        case 2:
                            Check_if_On_Target();
                            break;
                        case 3:
                            Check_if_On_Target();
                            break;
                        case 4:
                            Check_if_On_Target();
                            break;
                        case 5:
                            Check_if_On_Target();
                            break;
                        case 6:
                            Check_if_On_Target();
                            break;
                        default:
                            //This removes tiny meteor from scene without playing the VFX
                            //Restarts variables
                            assignedTinyMeteor.GetComponent<NewMeteorScript>().DoNotLockToPeanut_resetLayer();
                            switch (assignedTinyMeteor.name)
                            {
                                case "tina":
                                    meteorSpawnerScript.Add_back_to_list(assignedTinyMeteor, "Spawn Meteor Object List");
                                    break;
                                case "jacqueline":
                                    meteorSpawnerScript.Add_back_to_list(assignedTinyMeteor, "Spawn Meteor Object List");
                                    break;
                                case "herbert":
                                    meteorSpawnerScript.Add_back_to_list(assignedTinyMeteor, "Spawn Meteor Object List");
                                    break;
                                case "matty":
                                    meteorSpawnerScript.Add_back_to_list(assignedTinyMeteor, "Spawn Meteor Object List");
                                    break;
                                case "dennis":
                                    meteorSpawnerScript.Add_back_to_list(assignedTinyMeteor, "Spawn Meteor Object List");
                                    break;
                                case "totoy":
                                    meteorSpawnerScript.Add_back_to_list(assignedTinyMeteor, "Spawn Frozen Meteor Object List");
                                    break;
                                case "opay":
                                    meteorSpawnerScript.Add_back_to_list(assignedTinyMeteor, "Spawn Frozen Meteor Object List");
                                    break;
                                case "bantay":
                                    meteorSpawnerScript.Add_back_to_list(assignedTinyMeteor, "Spawn Frozen Meteor Object List");
                                    break;
                                case "bulabog":
                                    meteorSpawnerScript.Add_back_to_list(assignedTinyMeteor, "Spawn Frozen Meteor Object List");
                                    break;
                                case "cordapia":
                                    meteorSpawnerScript.Add_back_to_list(assignedTinyMeteor, "Spawn Frozen Meteor Object List");
                                    break;
                                case "totoy Villareal":
                                    meteorSpawnerScript.Access_the_Script_to_kill_Meteor(assignedTinyMeteor);
                                    break;
                                case "opay Villareal":
                                    meteorSpawnerScript.Access_the_Script_to_kill_Meteor(assignedTinyMeteor);
                                    break;
                                default:
                                    switch (assignedTinyMeteor.tag)
                                    {
                                        case "tinyMeteor":
                                            meteorSpawnerScript.Access_the_Script_to_kill_Meteor(assignedTinyMeteor);
                                            break;
                                        case "tinyFrozenMeteor":
                                            meteorSpawnerScript.Access_the_Script_to_kill_Meteor(assignedTinyMeteor);
                                            break;
                                        case "sfd_tinyMeteor":
                                            meteorSpawnerScript.Access_the_Script_to_kill_Meteor(assignedTinyMeteor);
                                            break;
                                        case "sfd_tinyFrozenMeteor":
                                            meteorSpawnerScript.Access_the_Script_to_kill_Meteor(assignedTinyMeteor);
                                            break;
                                        default:
                                            Debug.Log("Name not found. Check peanutScript Update indexPointstart witch case default");
                                            break;
                                    }
                                    break;

                            }
                            targetLocked = true;
                            targetCaptured = false;
                            indexPointStart = 0;
                            speed = normalSpeed;

                            //This is a check to place the peanut back into the movement loop to
                            //circle around the boss, since no meteor is assigned to it
                            if (assignedTinyMeteor.name == "dummyMeteor")
                            {
                                targetLocked = false;
                                indexPointStart = 0;
                                speed = normalSpeed;
                            }

                            //add script here to count Peanuts Deposited
                            bossPeanutMakerScript.Add_One_FTMeteor_to_Collection();

                            List<GameObject> listofAll_tinyFrozenMeteors_Assigned_toPeanut = meteorSpawnerScript.Get_ListFromMeteorScript();
                            if (listofAll_tinyFrozenMeteors_Assigned_toPeanut.Contains(assignedTinyMeteor))
                            {
                                meteorSpawnerScript.Remove_From_List_ofAll_TinyFrozen_blah_blah_blah(assignedTinyMeteor);
                            }
                            break;
                    }

                    //check in case peanut was assigned dead meteor
                    if (!assignedTinyMeteor.activeSelf)
                    {
                        assignedTinyMeteor = dummyMeteor;
                        targetLocked = false;
                        indexPointStart = 0;
                        speed = normalSpeed;
                    }
                }

                transform.position = Vector2.MoveTowards(transform.position, target, step);//this script moves the peanut towards the target
            }


            if (!targetLocked)
            {
                if (!bossLaserMovement)
                {
                    Movement_Basedon_PathLoopIndex();
                }

            }

        }
        else
        {
            if (colorTime<=1)
            {
                colorTime += Time.deltaTime * 0.5f;
                mySpriteRenderer.color = Color.Lerp(Color.white, Color.red, Mathf.Clamp(colorTime, 0, 1));
            }

            if (pathLoopIndex <= (movementLoop.Count - 1))
            {
                pathPoints = deathMovement.GetPathPoints();

                var targetPosition = pathPoints[pathPointIndex].transform.position+ currentTargetPos;

                var movementThisFrame = movementLoop[pathLoopIndex].GetPathMoveSpeed() * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);
                if (transform.position == targetPosition)
                {
                    pathPointIndex++;
                    if (pathPointIndex == pathPoints.Count - 1)
                    {
                        pathPointIndex = 0;

                    }
                }
            }
        }
    }
    private void Check_if_On_Target()
    {
        if (gameObject.transform.position == target)
        {
            indexPointStart++;
            speed = slowSpeed;
        }

        if (indexPointStart < 5)
        {
            if(isLeftchute)
            {
                target = leftFlightPath[indexPointStart].transform.position;
            }
            else 
            {
                target = rightFlightPath[indexPointStart].transform.position;
            }

        }
        else if (indexPointStart==5)
        {
            if(isLeftchute)
            {
                target = leftChuteEntrance.transform.position;
            }
            else
            {
                target = rightChuteEntrance.transform.position;
            }
        }
        else if (indexPointStart==6)
        {
            if(isLeftchute)
            {
                target = leftChute.transform.position;
            }
            else
            {
                target = rightChute.transform.position;
            }

        }
        else
        {
            if (isLeftchute)
            {
                target = leftChuteEntrance.transform.position;
            }
            else
            {
                target = rightChuteEntrance.transform.position;
            }
        }
    }

    private void Movement_Basedon_PathLoopIndex()
    {
        if (pathLoopIndex <= (movementLoop.Count - 1))
        {
            pathPoints = movementLoop[pathLoopIndex].GetPathPoints();

            if(pathLoopIndex == 1) //Second Path
            {

                var targetPosition = pathPoints[pathPointIndex].transform.position + myParent.transform.position;

                var movementThisFrame = movementLoop[pathLoopIndex].GetPathMoveSpeed() * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

                if (transform.position == targetPosition)
                {
                    if(pathPointIndex==pathPoints.Count-1)
                    {
                        pathPointIndex = 0;

                    }
                    else
                    {
                        pathPointIndex++;
                    }

                }

            }
            else if (pathPointIndex <= (pathPoints.Count - 1)) //Default First Path
            {

                var targetPosition = pathPoints[pathPointIndex].transform.position + myParent.transform.position;
                var movementThisFrame = movementLoop[pathLoopIndex].GetPathMoveSpeed() * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

                if (transform.position == targetPosition)
                {
                    pathPointIndex++;
                }
            }
            else
            {
                //SettingPathPointIndex to 0 loops the animation
                //Debug.Log("Nothing Happens");
                if (pathPointIndex > pathPoints.Count - 1)
                {
                    pathPointIndex = 0;
                    //pathLoopIndex++;
                }
            }
        }
        else
        {
            pathLoopIndex = 0;
            pathPointIndex = 0;
            // Debug.Log("triggered");
        }
    }

    public void Assign_TFMeteor_to_thisPeanut(GameObject thismeteor)
    {
        assignedTinyMeteor = thismeteor;
        targetLocked = true;
    }

    public void Assign_RESET_to_thisPeanut()
    {
        assignedTinyMeteor = dummyMeteor;
        targetLocked = false;
        targetCaptured = false;
        indexPointStart = 0;
        speed = normalSpeed;
        //target = position;
    }

    public GameObject Get_thisPeanuts_AssignedTFM() { return assignedTinyMeteor; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject ChildGameObject1 = gameObject.transform.GetChild(0).gameObject;

        //this is a check if meteor has already been clamped
        //if clamped, the layer is 16
        if (collision.gameObject.layer==14)
        {
            //check if the meteor is the assigned meteor
            if (collision.gameObject.name == assignedTinyMeteor.name)
            {
                NewMeteorScript collisionMetScript = assignedTinyMeteor.GetComponent<NewMeteorScript>();
                collisionMetScript.Stop_This_meteor();
                collisionMetScript.New_TransForm(ChildGameObject1);
                collisionMetScript.Change_MyLayer();
                targetCaptured = true;

                //The next two lines compute which chute is nearest
                var leftChuteDistance = Vector2.Distance(gameObject.transform.position, leftChute.transform.position);
                var rightChuteDistance = Vector2.Distance(gameObject.transform.position, rightChute.transform.position);
                
                //Using Variables above checks to see which one is the smallest distance
                //Smallest distance gets to be the assigned chute
                if (leftChuteDistance >= rightChuteDistance)
                {
                    isLeftchute = false;
                    float minValue = 10000;

                    //Second check is to see which amonth the 5 points along the path is closest
                    //This loops computes all distances to points from the peanut and adds to new list
                    var RightChutePointDistance = new List<float>();
                    for (int i = 0; i < rightFlightPath.Count; i++)
                    {
                        RightChutePointDistance.Add(Vector2.Distance(gameObject.transform.position, rightFlightPath[i].transform.position));
                        if (RightChutePointDistance[i] < minValue)
                        {
                            minValue = RightChutePointDistance[i];
                        }
                    }

                    //After computing all points distances and finding the minimum dist among points
                    //This loop finds the index of the point which has the smallest distance
                    for (int x = 0; x < RightChutePointDistance.Count; x++)
                    {
                        if (RightChutePointDistance[x] == minValue)
                        {
                            indexPointStart = x;
                            break;
                        }
                    }


                }
                else
                {
                    isLeftchute = true;
                    float minValue = 10000;

                    //Second check is to see which amonth the 5 points along the path is closest
                    //This loops computes all distances to points from the peanut and adds to new list
                    var LeftChutePointDistance = new List<float>();
                    for (int i = 0; i < leftFlightPath.Count; i++)
                    {
                        LeftChutePointDistance.Add(Vector2.Distance(gameObject.transform.position, leftFlightPath[i].transform.position));
                        if (LeftChutePointDistance[i]< minValue)
                        {
                            minValue = LeftChutePointDistance[i];
                        }

                    }

                    //After computing all points distances and finding the minimum dist among points
                    //This loop finds the index of the point which has the smallest distance
                    for (int x = 0; x < LeftChutePointDistance.Count; x++)
                    {
                        if (LeftChutePointDistance[x] == minValue)
                        {
                            indexPointStart = x;
                            break;
                        }
                    }

                }

                //Add a debug break here if break needed for testing
            }
        }

    }

    public void Enable_Peanut_Object()
    {
        gameObject.SetActive(true);
    }

    public void Activate_Peanut_Second_PathLoop()
    {
        bossLaserMovement = true;
        lastTargetSaved = target;
        lastPathPointIndexSaved = pathPointIndex;
        pathPointIndex = 1;
    }

    public void DeActivate_Peanut_Second_PathLoop()
    {
        bossLaserMovement = false;
        target = lastTargetSaved;
        pathPointIndex = lastPathPointIndexSaved;
        pathLoopIndex = 0;
    }

    public void Activate_Peanut_Death_Sequence()
    {
        bossDefeated = true;
        pathPointIndex = 0;
        currentTargetPos = gameObject.transform.position;
        StartCoroutine(PlayVFX_DisablePeanut());
    }

    private IEnumerator PlayVFX_DisablePeanut()
    {
        yield return new WaitForSeconds(Random.Range(4, 4.5f));
        mySpriteRenderer.color = transparency;
        GameObject icePeanutBoom = Instantiate(bOOMVFX, transform.position, transform.rotation) as GameObject;
        Destroy(icePeanutBoom, 2);
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }

}
