using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MeteorSpawnerScript : MonoBehaviour
{
    List<Transform> meteorSpawnPoints;
    [SerializeField] GameObject spawnPoints;
    [SerializeField] float counter = 0;
    [SerializeField] int spawnPointIndex = 0; //serialized for debugging purposes

    float meteorSpeed = 1;
    [SerializeField] int meteorObjectListIndex = 0;
    float xVelpcity;
    float YVelocity;

    bool foundSpawner = false;

    [Header("Dummy")]
    [SerializeField] GameObject dummyGameObject;
    [Header("Original")]
    [SerializeField] List<GameObject> spawnMeteorObjectList = new List<GameObject>();
    [SerializeField] List<GameObject> splitBigMeteorObject;
    [SerializeField] List<GameObject> splitMedMeteorObject;
    [SerializeField] List<GameObject> splitTinyMeteorObject;
    [Header("Original Frozen")]
    [SerializeField] List<GameObject> spawnFrozenMeteorObjectList = new List<GameObject>();
    [SerializeField] List<GameObject> modListdeletedMeteorObject = new List<GameObject>();


    //Script For meteors
    List<NewMeteorScript> meteorScript = new List<NewMeteorScript>();
    List<NewMeteorScript> splitBigMeteorScript = new List<NewMeteorScript>();
    List<NewMeteorScript> splitMedMeteorScript = new List<NewMeteorScript>();
    List<NewMeteorScript> splitTinyMeteorScript = new List<NewMeteorScript>();

    //RigidBody for meteors
    List<Rigidbody2D> meteorRigidBody = new List<Rigidbody2D>();
    List<Rigidbody2D> splitBigMeteorRigidBody = new List<Rigidbody2D>();
    List<Rigidbody2D> splitMedMeteorRigidBody = new List<Rigidbody2D>();
    List<Rigidbody2D> splitTinyMeteorRigidBody = new List<Rigidbody2D>();

    ///Frozen
    List<NewMeteorScript> meteorFrozenScript = new List<NewMeteorScript>();
    List<Rigidbody2D> meteorFrozenRigidBody = new List<Rigidbody2D>();

    //transform for Meteors
    bool disableCounter = false;


    //[Header("babies")] //delete header after debug
    int smithBabies = 0; //serialized for debugging purposes
    int landonBabies = 0; //serialized for debugging purposes
    int priceBabies = 0; //serialized for debugging purposes
    int whiteBabies = 0; //serialized for debugging purposes
    int jacobsBabies = 0; //serialized for debugging purposes
    int morrisBabies = 0; //serialized for debugging purposes
    int johnsonBabies = 0; //serialized for debugging purposes

    //[Header("Frozen babies")] //delete header after debug
    int santosBabies = 0; //serialized for debugging purposes
    int rosalesBabies = 0; //serialized for debugging purposes
    int jimenezBabies = 0; //serialized for debugging purposes
    int taguptupBabies = 0; //serialized for debugging purposes
    int ramirezBabies = 0; //serialized for debugging purposes
    int lopezBabies = 0; //serialized for debugging purposes
    int villarealBabies = 0; //serialized for debugging purposes

    [Header("Player")] //delete header after debug
    [SerializeField] GameObject player;
    [SerializeField] GameObject bossPeanut;
    Vector3 playerPos;
    BossPeanutMaker bossPeanutScript;

    //Smith, Landon, Price, White, Jacobs, morris, Johnson
    bool smithFrozen = false;
    bool landonFrozen = false;
    bool priceFrozen = false;
    bool whiteFrozen = false;
    bool jacobsFrozen = false;
    bool morrisFrozen = false;
    bool johnsonFrozen = false;

    //Smith Line
    bool jackie_A_Smith_Frozen = false;
    bool jackie_B_Smith_Frozen = false;
    bool robert_A_Smith_Frozen = false;
    bool robert_B_Smith_Frozen = false;
    bool robert_D_Smith_Frozen = false;
    bool robert_E_Smith_Frozen = false;
    //Landon Line
    bool robert_A_Landon_Frozen = false;
    bool robert_B_Landon_Frozen = false;
    //Price Line
    bool robert_A_Price_Frozen = false;
    bool robert_B_Price_Frozen = false;
    //White Line
    bool robert_A_White_Frozen = false;
    bool robert_B_White_Frozen = false;

    int plusminusFrozenmeteor = 0; //Variable for debug delete later
    [SerializeField] List<GameObject> cycleThroughMeteorsForFrozen; //have to manually add meteors to cycle through
    [SerializeField] int trueOrFalse_OneMeansTrue = 1;
    [SerializeField] string meteorName = "Toby A Smith"; //delete later for debug purposes only delete once meteor launcher not needed
    [SerializeField] bool turnOnMeteorSpawnerCounter = false; //delete later for debug purposes only delete once meteor launcher not needed
    bool keypressSpawnMeteor = false;
    [SerializeField] string meteorNameforFrozen = "Toby A Smith";
    [SerializeField] bool disableFrozenSpawnRandomization = true;
    int plusminusmeteor = 0; //Variable for debug delete later

    //Tiny Peanuts coding
    [SerializeField] List<GameObject> icePeanuts;
    [SerializeField] List<GameObject> listofAll_tinyFrozenMeteors_inBounds_andNotAssigned = new List<GameObject>(); //serialized for debugging purposes. erase serialization later
    [SerializeField] List<GameObject> listofAll_tinyFrozenMeteors_Assigned_toPeanut = new List<GameObject>(); //serialized for debugging purposes. erase serialization later
    List<PeanutScript> icePeanutScript = new List<PeanutScript>();
    [SerializeField] int countAssignedPeanuts = 0;

    int meteorSpawnRate = 5;
    bool stopSpawn = false;


    // Start is called before the first frame update
    void Start()
    {            
        // 21 index max total 0-21 (22 points total)
        Cache_SpawnPoint_Transform();

        // 12 objects in list
        Cache_meteorScript_and_meteorRigidBody();

        // 2 objects in list
        Cache_splitBigMeteorScript_and_splitBigMeteorRigidBody();

        // 10 objects in list
        Cache_splitMedMeteorScript_and_splitMedMeteorRigidBody();

        // 26 objects in list
        Cache_splitTinyMeteorScript_and_splitTinyMeteorRigidBody();

        // 12 Objects in list  
        Cache_meteorFrozenScript_and_meteorFrozenRigidBody();

        trueOrFalse_OneMeansTrue = 1;

        Cache_PeanutScript();

        bossPeanutScript = bossPeanut.GetComponent<BossPeanutMaker>();
        //Decided not to use code below since this bug is not a big issue
        //StartCoroutine(UpdateListDeletedObjects());

        meteorSpawnRate = 5;
        stopSpawn = false;

    }

    public void Call_MeteorSpawner_To_Enable_Boss_and_Minions()
    {
        bossPeanutScript.Enable_Peanut_Boss();
        StartCoroutine(DelayEachPeanutSpawn());
    }

    private IEnumerator DelayEachPeanutSpawn()
    {
        icePeanutScript[0].Enable_Peanut_Object();
        yield return new WaitForSeconds(1.0f);
        icePeanutScript[1].Enable_Peanut_Object();
        yield return new WaitForSeconds(1.0f);
        icePeanutScript[2].Enable_Peanut_Object();
        yield return new WaitForSeconds(1.0f);
        icePeanutScript[3].Enable_Peanut_Object();
        yield return new WaitForSeconds(1.0f);
    }

    private void Cache_SpawnPoint_Transform()
    {
        var waveWaypoints = new List<Transform>();
        foreach (Transform child in spawnPoints.transform)
        {
            waveWaypoints.Add(child);
        }
        meteorSpawnPoints = waveWaypoints;
    }
    private void Cache_meteorScript_and_meteorRigidBody()
    {
        //Adds NewMeteorScript list for spawnMeteorObject Variables
        var chikenNuggets = new List<NewMeteorScript>();
        for (int i = 0; i < 12; i++)
        {
            chikenNuggets.Add(spawnMeteorObjectList[i].GetComponent<NewMeteorScript>());
        }
        meteorScript = chikenNuggets;

        //Add RigidBody list for spawnMeteorObject Variables
        var baconsandwhich = new List<Rigidbody2D>();
        for (int i = 0; i < 12; i++)
        {
            baconsandwhich.Add(spawnMeteorObjectList[i].GetComponent<Rigidbody2D>());
        }
        meteorRigidBody = baconsandwhich;
    }
    private void Cache_splitBigMeteorScript_and_splitBigMeteorRigidBody()
    {
        //Adds NewMeteorScript list for splitBigMeteorObject Variables
        var pizzapie = new List<NewMeteorScript>();
        for (int i = 0; i < 4; i++)
        {
            pizzapie.Add(splitBigMeteorObject[i].GetComponent<NewMeteorScript>());
        }
        splitBigMeteorScript = pizzapie;

        //Add RigidBody list for splitBigMeteorObject Variables
        var coffee = new List<Rigidbody2D>();
        for (int i = 0; i < 4; i++)
        {
            coffee.Add(splitBigMeteorObject[i].GetComponent<Rigidbody2D>());
        }
        splitBigMeteorRigidBody = coffee;
    }
    private void Cache_splitMedMeteorScript_and_splitMedMeteorRigidBody()
    {
        //Adds NewMeteorScript list for splitMedMeteorObject Variables
        var tunapie = new List<NewMeteorScript>();
        for (int i = 0; i < 20; i++)
        {
            tunapie.Add(splitMedMeteorObject[i].GetComponent<NewMeteorScript>());
        }
        splitMedMeteorScript = tunapie;

        //Add RigidBody list for splitMedMeteorObject Variables
        var energydrink = new List<Rigidbody2D>();
        for (int i = 0; i < 20; i++)
        {
            energydrink.Add(splitMedMeteorObject[i].GetComponent<Rigidbody2D>());
        }
        splitMedMeteorRigidBody = energydrink;
    }
    private void Cache_splitTinyMeteorScript_and_splitTinyMeteorRigidBody()
    {
        //Adds NewMeteorScript list for splitTinyMeteorObject Variables
        var baconpie = new List<NewMeteorScript>();
        for (int i = 0; i < 52; i++)
        {
            baconpie.Add(splitTinyMeteorObject[i].GetComponent<NewMeteorScript>());
        }
        splitTinyMeteorScript = baconpie;

        //Add RigidBody list for splitTinyMeteorObject Variables
        var orangejuice = new List<Rigidbody2D>();
        for (int i = 0; i < 52; i++)
        {
            orangejuice.Add(splitTinyMeteorObject[i].GetComponent<Rigidbody2D>());
        }
        splitTinyMeteorRigidBody = orangejuice;
    }
    private void Cache_meteorFrozenScript_and_meteorFrozenRigidBody()
    {
        //Adds meteorFrozenScript  list for spawnFrozenMeteorObjectList  Variables
        var pistacios = new List<NewMeteorScript>();
        for (int i = 0; i < 12; i++)
        {
            pistacios.Add(spawnFrozenMeteorObjectList[i].GetComponent<NewMeteorScript>());
        }
        meteorFrozenScript = pistacios;

        //Add RigidBody list for spawnFrozenMeteorObjectList  Variables
        var almonds = new List<Rigidbody2D>();
        for (int i = 0; i < 12; i++)
        {
            almonds.Add(spawnFrozenMeteorObjectList[i].GetComponent<Rigidbody2D>());
        }
        meteorFrozenRigidBody = almonds;
    }
    private void Cache_PeanutScript()
    {
        var babyNuggets = new List<PeanutScript>();
        for (int i = 0; i < icePeanuts.Count; i++)
        {
            babyNuggets.Add(icePeanuts[i].GetComponent<PeanutScript>());
        }
        icePeanutScript = babyNuggets;
    }

    public void Check_if_frozenTinyMeteor_isWithinBounds(GameObject thisMeteor)
    {
        //cache variables
        var thisMeteorX = thisMeteor.transform.localPosition.x;
        var thisMeteorY = thisMeteor.transform.localPosition.y;
        //Bounds
        float minX = -2.6f;
        float maxX = 2.6f;
        float minY = -4.7f;
        float maxY = 4.7f;

        if ((thisMeteorX<maxX) & (thisMeteorX > minX) & (thisMeteorY > minY) & (thisMeteorY < maxY))
        {
            ///If meteoriswithinBounds; assign meteor to peanut or add to notassignlist when all peanuts full
            ///Check NotAssigned List if it contains the meteor
            ///CheckAssigned List if contains meteor
            ///If Lists both do not contain meteor then proceed to assign to a meteor or add to a queue
            ///First 2 ifs does nothing when true
            if(listofAll_tinyFrozenMeteors_inBounds_andNotAssigned.Contains(thisMeteor)) 
            {
                //this satetement is added to ensure no duplicates in above list
                //so Do nothing
            }
            else if (listofAll_tinyFrozenMeteors_Assigned_toPeanut.Contains(thisMeteor)) 
            {
                ///start blocking below here (leaving this in just in case I need an additional check)
                /*
                int notAssigned = 0;
                int countDummyMeteors = 0;
                //this satetement is added to ensure no duplicates in above list
                //so Do nothing if check below is passed
                for (int w = 0; w < (icePeanuts.Count); w++)
                {
                    if(w< (icePeanuts.Count + 1))
                    {
                        GameObject assignedStatus = icePeanutScript[w].Get_thisPeanuts_AssignedTFM();

                        if (assignedStatus.name==thisMeteor.name)
                        {
                            //Do nothing
                            break;
                        }
                        else if (assignedStatus.name=="dummyMeteor")
                        {
                            countDummyMeteors++;
                            notAssigned++;
                        }
                        else
                        {
                            notAssigned++;
                        }
                    }
                }

                if (notAssigned==4)
                {
                    if(countDummyMeteors>0)
                    {
                        for (int w = 0; w < (icePeanuts.Count); w++)
                        {
                            GameObject assignedStatus = icePeanutScript[w].Get_thisPeanuts_AssignedTFM();

                            if (assignedStatus.name == "dummyMeteor")
                            {
                                countAssignedPeanuts++;
                                icePeanutScript[w].Assign_TFMeteor_to_thisPeanut(thisMeteor);
                                Debug.Log(icePeanuts[w].name + " is assigned " + thisMeteor.name);
                                Debug.Break();
                                break;
                            }
                        }
                    }
                    else if (countDummyMeteors == 0)
                    {
                        listofAll_tinyFrozenMeteors_inBounds_andNotAssigned.Add(thisMeteor);
                        listofAll_tinyFrozenMeteors_Assigned_toPeanut.Remove(thisMeteor);
                        Debug.Log("Moved to other list");
                        Debug.Break();
                    }
                }
                */
                ///end blocking above here
            }
            else //Proceed to assign the meteor to an empty peanut; or add to queue when empty peanuts not available;
            {
                //Extra check to make sure countAssignedPeanuts is up to date
                Check_if_CountAssigned_is_Equal_to_Assigned_Number_of_Peanuts();

                if (countAssignedPeanuts == icePeanuts.Count) //Peauts are full
                {
                    listofAll_tinyFrozenMeteors_inBounds_andNotAssigned.Add(thisMeteor);
                    Debug.Log("No more peanuts to assign to meteor. Adding to List");
                }
                else if (countAssignedPeanuts < icePeanuts.Count) // Peanuts are not full
                {
                    //Start Loop
                    for (int i = 0; i < icePeanuts.Count; i++) 
                    {
                        if (countAssignedPeanuts == icePeanuts.Count)//Peanuts are all full
                        {
                            bool tempBool = listofAll_tinyFrozenMeteors_inBounds_andNotAssigned.Contains(thisMeteor);

                            switch (tempBool)
                            {
                                case true:
                                    Debug.Log("No more peanuts to assign to meteor. Meteor Already In List");
                                    break;
                                case false:
                                    Debug.Log("No more peanuts to assign to meteor. Adding to List");
                                    listofAll_tinyFrozenMeteors_inBounds_andNotAssigned.Add(thisMeteor);
                                    break;
                            }
                            break;
                        }
                        else if (listofAll_tinyFrozenMeteors_Assigned_toPeanut.Contains(thisMeteor))
                        {
                            // Do nothing
                            break;
                        }
                        else //Peanuts are not full
                        {
                            GameObject assignedStatus = icePeanutScript[i].Get_thisPeanuts_AssignedTFM();

                            if (assignedStatus.name == "dummyMeteor")// if the peanuts assignedFTM is dummyMeteor then it counts as empty
                            {

                                if (listofAll_tinyFrozenMeteors_inBounds_andNotAssigned.Contains(thisMeteor))
                                {
                                    switch (listofAll_tinyFrozenMeteors_inBounds_andNotAssigned.Count)
                                    {
                                        case 0:
                                            countAssignedPeanuts++;
                                            icePeanutScript[i].Assign_TFMeteor_to_thisPeanut(thisMeteor);
                                            listofAll_tinyFrozenMeteors_Assigned_toPeanut.Add(thisMeteor);
                                            Debug.Log(icePeanuts[i].name + " is assigned " + thisMeteor.name);
                                            break;
                                        case 1:
                                            countAssignedPeanuts++;
                                            icePeanutScript[i].Assign_TFMeteor_to_thisPeanut(thisMeteor);
                                            listofAll_tinyFrozenMeteors_Assigned_toPeanut.Add(thisMeteor);
                                            listofAll_tinyFrozenMeteors_inBounds_andNotAssigned.Remove(thisMeteor);
                                            Debug.Log(icePeanuts[i].name + " is assigned " + thisMeteor.name);
                                            break;
                                        default:
                                            countAssignedPeanuts++;
                                            icePeanutScript[i].Assign_TFMeteor_to_thisPeanut(listofAll_tinyFrozenMeteors_inBounds_andNotAssigned[0]);
                                            listofAll_tinyFrozenMeteors_Assigned_toPeanut.Add(listofAll_tinyFrozenMeteors_inBounds_andNotAssigned[0]);
                                            listofAll_tinyFrozenMeteors_inBounds_andNotAssigned.Remove(listofAll_tinyFrozenMeteors_inBounds_andNotAssigned[0]);
                                            listofAll_tinyFrozenMeteors_inBounds_andNotAssigned.Add(thisMeteor);
                                            Debug.Log(icePeanuts[i].name + " is assigned " + thisMeteor.name);
                                            break;
                                    }
                                }
                                else
                                {
                                    if (listofAll_tinyFrozenMeteors_inBounds_andNotAssigned.Count == 0)
                                    {
                                        
                                        countAssignedPeanuts++;
                                        icePeanutScript[i].Assign_TFMeteor_to_thisPeanut(thisMeteor);
                                        listofAll_tinyFrozenMeteors_Assigned_toPeanut.Add(thisMeteor);
                                        Debug.Log(icePeanuts[i].name + " is assigned " + thisMeteor.name);
                                        break;
                                        
                                    }
                                    else
                                    {
                                        Debug.Log(icePeanuts[i].name + " is assigned " + listofAll_tinyFrozenMeteors_inBounds_andNotAssigned[0].name);
                                        countAssignedPeanuts++;
                                        icePeanutScript[i].Assign_TFMeteor_to_thisPeanut(listofAll_tinyFrozenMeteors_inBounds_andNotAssigned[0]);
                                        listofAll_tinyFrozenMeteors_Assigned_toPeanut.Add(listofAll_tinyFrozenMeteors_inBounds_andNotAssigned[0]);
                                        listofAll_tinyFrozenMeteors_inBounds_andNotAssigned.Remove(listofAll_tinyFrozenMeteors_inBounds_andNotAssigned[0]);
                                        listofAll_tinyFrozenMeteors_inBounds_andNotAssigned.Add(thisMeteor);

                                    }
                                }
                            }
                            else //Loop until to next peanut until no more peanuts, then exit loop
                            {
                                if (listofAll_tinyFrozenMeteors_inBounds_andNotAssigned.Contains(thisMeteor))
                                {
                                    //Do nothing
                                    Debug.Log(icePeanutScript[i] + " is assigned " + assignedStatus);
                                }
                                else
                                {
                                    listofAll_tinyFrozenMeteors_inBounds_andNotAssigned.Add(thisMeteor);
                                    Debug.Log(icePeanutScript[i] + " is assigned " + assignedStatus);
                                }

                            }
                        }
                    }
                }
                Check_if_CountAssigned_is_Equal_to_Assigned_Number_of_Peanuts();
            }


        }
        else//meteor is out of bounds
        {
            //meteorisoutofbounds

            if (listofAll_tinyFrozenMeteors_Assigned_toPeanut.Contains(thisMeteor))
            {
                for (int i = 0; i < icePeanuts.Count; i++) //Note to delete later: 4 for now since testing, but will change to 6?
                {
                    GameObject assignedStatus = icePeanutScript[i].Get_thisPeanuts_AssignedTFM();

                    if(assignedStatus==thisMeteor)
                    {
                        countAssignedPeanuts--;
                        thisMeteor.GetComponent<NewMeteorScript>().DoNotLockToPeanut_resetLayer();
                        listofAll_tinyFrozenMeteors_Assigned_toPeanut.Remove(thisMeteor);

                        //assign next meteor
                        if (listofAll_tinyFrozenMeteors_inBounds_andNotAssigned.Count==0)
                        {
                            icePeanutScript[i].Assign_RESET_to_thisPeanut();
                            break;
                        }
                        else 
                        {
                            countAssignedPeanuts++;
                            icePeanutScript[i].Assign_TFMeteor_to_thisPeanut(listofAll_tinyFrozenMeteors_inBounds_andNotAssigned[0]);
                            listofAll_tinyFrozenMeteors_Assigned_toPeanut.Add(listofAll_tinyFrozenMeteors_inBounds_andNotAssigned[0]);
                            listofAll_tinyFrozenMeteors_inBounds_andNotAssigned.Remove(listofAll_tinyFrozenMeteors_inBounds_andNotAssigned[0]);
                          
                            break;                            
                        }
                    }
                }
                    

            }
            else
            {
                if (listofAll_tinyFrozenMeteors_inBounds_andNotAssigned.Contains(thisMeteor))
                {
                    listofAll_tinyFrozenMeteors_inBounds_andNotAssigned.Remove(thisMeteor);
                }
                else //Neither Lists contain meteor
                {
                    //do nothing
                }

            }
        }
    }


    private void Check_if_CountAssigned_is_Equal_to_Assigned_Number_of_Peanuts()
    {
        int countFreePeanuts = 0;
        for (int q = 0; q < icePeanuts.Count; q++)
        {
            var currentPeanutsAssignedObject = icePeanutScript[q].Get_thisPeanuts_AssignedTFM();
            if (currentPeanutsAssignedObject.name == "dummyMeteor")
            {
                countFreePeanuts++;
            }
        }

        int totalNumberAssignedPeanuts = icePeanuts.Count- countFreePeanuts;
        //Extra check to make sure countAssignedPeanuts is up to date
        if (countAssignedPeanuts == totalNumberAssignedPeanuts)
        {
            //do nothing
        }
        else
        {
            countAssignedPeanuts = totalNumberAssignedPeanuts;
        }
    }

    public void Remove_meteor_from_List_withinBounds_due_to_death(GameObject thisMeteor)
    {
        if (listofAll_tinyFrozenMeteors_inBounds_andNotAssigned.Contains(thisMeteor))
        {
            listofAll_tinyFrozenMeteors_inBounds_andNotAssigned.Remove(thisMeteor);
        }
        else if (listofAll_tinyFrozenMeteors_Assigned_toPeanut.Contains(thisMeteor))
        {
            for (int i = 0; i < icePeanuts.Count; i++) //Note to delete later: 4 for now since testing, but will change to 6?
            {
                GameObject assignedStatus = icePeanutScript[i].Get_thisPeanuts_AssignedTFM();

                if (assignedStatus == thisMeteor)
                {
                    countAssignedPeanuts--;
                    listofAll_tinyFrozenMeteors_Assigned_toPeanut.Remove(thisMeteor);

                    //assign next meteor
                    if (listofAll_tinyFrozenMeteors_inBounds_andNotAssigned.Count == 0)
                    {
                        icePeanutScript[i].Assign_RESET_to_thisPeanut();
                        break;
                    }
                    else
                    {
                        countAssignedPeanuts++;
                        icePeanutScript[i].Assign_TFMeteor_to_thisPeanut(listofAll_tinyFrozenMeteors_inBounds_andNotAssigned[0]);
                        listofAll_tinyFrozenMeteors_Assigned_toPeanut.Add(listofAll_tinyFrozenMeteors_inBounds_andNotAssigned[0]);
                        listofAll_tinyFrozenMeteors_inBounds_andNotAssigned.Remove(listofAll_tinyFrozenMeteors_inBounds_andNotAssigned[0]);
                        break; //break since only one meteor is being freed
                    }
                }
            }
         
        }
        else //Neither Lists contain meteor
        {
            //do nothing
        }
    }


    // Update is called once per frame
    void Update()
    {
        /*
        //more code for testing
        Delete_this_Method_to_cycle_through_meteors_to_Freeze();
        Delete_this_Method_to_cycle_through_meteors_to_lauch(); 

        //code for keypress testing [delete later]
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            counter = 2;

            switch (trueOrFalse_OneMeansTrue)
            {
                case 1:
                    Spawn_The_Meteor_NOTFrozen_On_keyPress_Enter();
                    break;
                case 2:
                    Spawn_The_Meteor_Frozen_On_keyPress_Enter();
                    break;
                default:
                    Debug.Log("Not In cases");
                    break;
            }
        }
        */

        //disable if counter not needed
        if (turnOnMeteorSpawnerCounter == true)// Delete this since Enabling/Disabling is for debugging purposes.
        {
            if (disableCounter == false)
            {
                counter += Time.deltaTime;
            }
        }

        if (!stopSpawn)
        {
            SpawnARandomMeteorInRandomPoint();
        }

    }

    private void SpawnARandomMeteorInRandomPoint()
    {
        if (counter >= meteorSpawnRate)
        {
            disableCounter = true;
            counter = 0;
            spawnPointIndex = Random.Range(0, 22); //location of spawn
            AssignVelocity_to_SpawnPointIndex();

            meteorSpeed = Random.Range(1, 3);

            //Code for GameObjectIndex Randomization
            //AssignRandom_MeteorObjectIndex_WithProbabilityBias();

            //true = 1
            //false = 2 || Frozen
            if (!disableFrozenSpawnRandomization)
            {
                trueOrFalse_OneMeansTrue = Random.Range(1, 3);
            }


            //public static int Range(int min, int max); Return a random integer number between min [inclusive] 
            //and max [exclusive] (Read Only).
            //Note max is exclusive. Random.Range(0, 10) can return a value between 0 and 9.

            if (trueOrFalse_OneMeansTrue == 1) //There are 12 Elements in SpawnMeteorObject List, thus 0-13 since 13 is excluded
            {

                if (spawnMeteorObjectList.Count == 0)
                {
                    Debug.Log("No more meteors to spawn");
                    disableCounter = false; keypressSpawnMeteor = false;
                }
                else
                {
                    //Block if you don't want randomization for Debug
                    if (keypressSpawnMeteor == false)// delete kepress later; its for debugging purposes only.
                    {
                        meteorObjectListIndex = Random.Range(0, spawnMeteorObjectList.Count);
                    }


                    //we don't nest code below above as else int the if statement since if keypressSpawnMeteor is true then
                    // it means meteorObjectListIndex is assigned during keypress
                    if (meteorObjectListIndex == 100)//delete if statement once debug done
                    { //do nothing//do nothing, meteorObjectListIndex = 100 because it is assigned during keypress
                        //when you press enter to spawn and it is not in list it assigns a value of 100
                        disableCounter = false; keypressSpawnMeteor = false;
                    }
                    else
                    {
                        Debug.Log("Spawning Meteor " + meteorObjectListIndex + " with name of " + spawnMeteorObjectList[meteorObjectListIndex].name);

                        modListdeletedMeteorObject.Insert(0, spawnMeteorObjectList[meteorObjectListIndex]);

                        spawnMeteorObjectList[meteorObjectListIndex].transform.position = meteorSpawnPoints[spawnPointIndex].transform.position;

                        meteorScript[meteorObjectListIndex].EnableMeteor();

                        meteorRigidBody[meteorObjectListIndex].velocity = new Vector2(xVelpcity * meteorSpeed, YVelocity * meteorSpeed);

                        spawnMeteorObjectList.Remove(spawnMeteorObjectList[meteorObjectListIndex]);

                        meteorScript.Remove(meteorScript[meteorObjectListIndex]);

                        meteorRigidBody.Remove(meteorRigidBody[meteorObjectListIndex]);

                        disableCounter = false;

                        keypressSpawnMeteor = false;// for debug only delete later

                        Delete_This_is_just_toReset_Meteor_for_debug();

                    }
                }

            }
            else if (trueOrFalse_OneMeansTrue == 2)
            {

                //Block if you don't want randomization for Debug
                if (keypressSpawnMeteor == false)// delete kepress later; its for debugging purposes only.
                {
                    meteorObjectListIndex = Random.Range(0, spawnFrozenMeteorObjectList.Count);
                }


                if ((spawnFrozenMeteorObjectList.Count == 0) & (spawnFrozenMeteorObjectList.Count == 0))
                {
                    Debug.Log("No more FrozenMeteors to spawn");
                    disableCounter = false; keypressSpawnMeteor = false; //kepressSpawnmeteor for debug only delete later.
                }
                else if ((spawnFrozenMeteorObjectList.Count == 0) & (spawnFrozenMeteorObjectList.Count >= 0))
                {
                    Debug.Log("No more FrozenMeteors to spawn");
                    disableCounter = false; keypressSpawnMeteor = false; //kepressSpawnmeteor for debug only delete later.
                }
                else if ((spawnFrozenMeteorObjectList.Count >= 0) & (spawnFrozenMeteorObjectList.Count == 0))
                {
                    Debug.Log("No more FrozenMeteors to spawn");
                    disableCounter = false; keypressSpawnMeteor = false; //kepressSpawnmeteor for debug only delete later.
                }
                else
                {
                    if (meteorObjectListIndex == 100)//delete if statement once debug done
                    { //do nothing, meteorObjectListIndex = 100 because it is assigned during keypress
                      //when you press enter to spawn and it is not in list it assigns a value of 100
                        disableCounter = false; keypressSpawnMeteor = false;
                    }

                    //we don't nest code below above as else int the if statement since if keypressSpawnMeteor is true then
                    // it means meteorObjectListIndex is assigned during keypress
                    if (meteorObjectListIndex == 100)//delete if statement once debug done
                    { //do nothing//do nothing, meteorObjectListIndex = 100 because it is assigned during keypress
                      //when you press enter to spawn and it is not in list it assigns a value of 100
                        disableCounter = false; keypressSpawnMeteor = false;
                    }
                    else
                    {
                        Debug.Log("Spawning FrozenMeteor " + meteorObjectListIndex + " with name of " + spawnFrozenMeteorObjectList[meteorObjectListIndex].name);

                        modListdeletedMeteorObject.Insert(0, spawnFrozenMeteorObjectList[meteorObjectListIndex]);

                        spawnFrozenMeteorObjectList[meteorObjectListIndex].transform.position = meteorSpawnPoints[spawnPointIndex].transform.position;

                        meteorFrozenScript[meteorObjectListIndex].EnableMeteor();

                        meteorFrozenRigidBody[meteorObjectListIndex].velocity = new Vector2(xVelpcity * meteorSpeed, YVelocity * meteorSpeed);

                        spawnFrozenMeteorObjectList.Remove(spawnFrozenMeteorObjectList[meteorObjectListIndex]);

                        meteorFrozenScript.Remove(meteorFrozenScript[meteorObjectListIndex]);

                        meteorFrozenRigidBody.Remove(meteorFrozenRigidBody[meteorObjectListIndex]);

                        disableCounter = false;

                        keypressSpawnMeteor = false;// for debug only delete later.

                        Delete_This_is_just_toReset_Meteor_for_debug();


                    }
                }
            }
        }
    }

    private void Delete_This_is_just_toReset_Meteor_for_debug()
    {

        switch (trueOrFalse_OneMeansTrue)
        {
            case 1:
                if (spawnMeteorObjectList.Count == 0)
                {
                    Debug.Log("No more meteors to Spawn");
                }
                else
                {
                    plusminusmeteor = 0;
                    meteorName = spawnMeteorObjectList[plusminusmeteor].name;
                }
                break;
            case 2:
                if (spawnFrozenMeteorObjectList.Count == 0)
                {
                    Debug.Log("No more meteors to Spawn");
                }
                else
                {
                    plusminusmeteor = 0;
                    meteorName = spawnFrozenMeteorObjectList[plusminusmeteor].name;
                }

                break;
            default:
                Debug.Log("Not In cases");
                break;
        }
    }
    private void Spawn_The_Meteor_Frozen_On_keyPress_Enter()
    {
        if ((spawnFrozenMeteorObjectList.Count == 0)& (spawnMeteorObjectList.Count == 0))
        {
            // do nothing
        }
        else if ((spawnFrozenMeteorObjectList.Count == 0) & (spawnMeteorObjectList.Count > 0))
        {
            // do nothing
        }
        else if ((spawnFrozenMeteorObjectList.Count > 0) & (spawnMeteorObjectList.Count == 0))
        {
            // do nothing
        }
        else
        {
            //To delete later for debugging purposes. Code to search for specific game object to spawn
            // based on Numerical Kepress at the top of keyboard
            for (int i = 0; i < spawnFrozenMeteorObjectList.Count; i++)
            {


                if (spawnFrozenMeteorObjectList[i].name == meteorName)
                {
                    meteorObjectListIndex = i;
                    keypressSpawnMeteor = true;
                    break;
                }
                else if (i == spawnFrozenMeteorObjectList.Count)
                {
                    Debug.Log(meteorName + " Not in List");
                    keypressSpawnMeteor = true;
                    meteorObjectListIndex = 100;
                    break;
                }
            }
            
        }
    }

    private void Spawn_The_Meteor_NOTFrozen_On_keyPress_Enter()
    {
        if (spawnMeteorObjectList.Count == 0)
        {
            // do nothing
        }
        else
        {

            //To delete later for debugging purposes. Code to search for specific game object to spawn
            // based on Numerical Kepress at the top of keyboard
            for (int i = 0; i < spawnMeteorObjectList.Count; i++)
            {


                if (spawnMeteorObjectList[i].name == meteorName)
                {
                    meteorObjectListIndex = i;
                    keypressSpawnMeteor = true;
                    break;
                }
                else if (i == spawnMeteorObjectList.Count)
                {
                    Debug.Log(meteorName + " Not in List");
                    keypressSpawnMeteor = true;
                    meteorObjectListIndex = 100;
                    break;
                }
            }
        }
    }
    
    private void Delete_this_Method_to_cycle_through_meteors_to_lauch()
    {
          if (Input.GetKeyDown(KeyCode.Tab))
            {
                trueOrFalse_OneMeansTrue++;

            if (trueOrFalse_OneMeansTrue >=3)
            {
            trueOrFalse_OneMeansTrue = 1;
            }

            switch (trueOrFalse_OneMeansTrue)
            {
                case 1:
                    if (spawnMeteorObjectList.Count ==0)
                    {
                        meteorName = "";
                        Debug.Log("No more meteors to choose from");
                    }
                    else
                    {
                        meteorName = spawnMeteorObjectList[0].name;
                        plusminusmeteor = 0;
                    }
                    break;
                case 2:
                    if (spawnFrozenMeteorObjectList.Count == 0)
                    {
                        meteorName = "";
                        Debug.Log("No more Frozenmeteors to choose from");
                    }
                    else
                    {
                        meteorName = spawnFrozenMeteorObjectList[0].name;
                        plusminusmeteor = 0;
                    }
                    
                    break;
                default:
                    Debug.Log("Not In cases");
                    break;
            }
          }


            if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            switch (trueOrFalse_OneMeansTrue)
            {
                case 1:
                    plusminusmeteor--;
                    if (plusminusmeteor < 0)
                    {
                        plusminusmeteor = (spawnMeteorObjectList.Count - 1);
                    }
                    


                    if (spawnMeteorObjectList.Count <= 0)
                    {
                        meteorName = "";
                        Debug.Log("No more meteors to choose from");
                        plusminusmeteor = 0;
                    }
                    else
                    {
                        if (plusminusmeteor > spawnMeteorObjectList.Count)

                        { plusminusmeteor = 0; }

                        meteorName = spawnMeteorObjectList[plusminusmeteor].name;
                    }
                    break;
                case 2:
                    plusminusmeteor--;
                    if (plusminusmeteor < 0)
                    {
                        plusminusmeteor = (spawnFrozenMeteorObjectList.Count - 1);
                    }

                    if (spawnFrozenMeteorObjectList.Count <= 0)
                    {
                        meteorName = "";
                        Debug.Log("No more Frozen meteors to choose from");
                        plusminusmeteor = 0;
                    }
                    else
                    {
                        if (plusminusmeteor > spawnFrozenMeteorObjectList.Count)

                        { plusminusmeteor = 0; }

                        meteorName = spawnFrozenMeteorObjectList[plusminusmeteor].name;
                    }
                    break;
                default:
                    Debug.Log("Not In cases");
                    break;

                    
            }

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            switch (trueOrFalse_OneMeansTrue)
            {
                case 1:
                    plusminusmeteor++;
                    if (plusminusmeteor == spawnMeteorObjectList.Count)
                    {
                        plusminusmeteor = 0;
                    }

                    if (spawnMeteorObjectList.Count == 0)
                    {
                        meteorName = "";
                        Debug.Log("No more meteors to choose from");
                        plusminusmeteor = 0;
                    }
                    else
                    {
                        if (plusminusmeteor > spawnMeteorObjectList.Count)

                        { plusminusmeteor = 0; }
                        meteorName = spawnMeteorObjectList[plusminusmeteor].name;
                    }
                    break;
                case 2:
                    plusminusmeteor++;
                    if (plusminusmeteor == spawnFrozenMeteorObjectList.Count)
                    {
                        plusminusmeteor = 0;
                    }

                    if (spawnFrozenMeteorObjectList.Count == 0)
                    {
                        meteorName = "";
                        Debug.Log("No more Frozen meteors to choose from");
                        plusminusmeteor = 0;
                    }
                    else
                    {
                        if (plusminusmeteor > spawnFrozenMeteorObjectList.Count)

                        { plusminusmeteor = 0; }

                        meteorName = spawnFrozenMeteorObjectList[plusminusmeteor].name;
                    }
               break;
            }
        }
        
    }

    private void Delete_this_Method_to_cycle_through_meteors_to_Freeze()
    {

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            plusminusFrozenmeteor--;
            if (plusminusFrozenmeteor < 0)
            {
                plusminusFrozenmeteor = (cycleThroughMeteorsForFrozen.Count - 1);
            }
            meteorNameforFrozen = cycleThroughMeteorsForFrozen[plusminusFrozenmeteor].name;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            plusminusFrozenmeteor++;
            if (plusminusFrozenmeteor == cycleThroughMeteorsForFrozen.Count)
            {
                plusminusFrozenmeteor = 0;
            }
            meteorNameforFrozen = cycleThroughMeteorsForFrozen[plusminusFrozenmeteor].name;
        }
    }

    private void AssignVelocity_to_SpawnPointIndex()
    {
        if ((spawnPointIndex >= 0) & (spawnPointIndex <= 2)) //Top
        {
            if (spawnPointIndex == 0)
            {
                YVelocity = -1;
                xVelpcity = Random.Range(0.00f, 1.00f);
            }
            else if (spawnPointIndex == 2)
            {
                YVelocity = -1;
                xVelpcity = Random.Range(-1.00f, 0.00f);
            }
            else
            {
                YVelocity = -1;
                xVelpcity = Random.Range(-1.00f, 1.00f);
            }

        }
        else if ((spawnPointIndex >= 3) & (spawnPointIndex <= 5)) //Botttom
        {
            if (spawnPointIndex == 3)
            {
                YVelocity = 1;
                xVelpcity = Random.Range(0.00f, 1.00f);
            }
            else if (spawnPointIndex == 5)
            {
                YVelocity = 1;
                xVelpcity = Random.Range(-1.00f, 0.00f);
            }
            else
            {
                YVelocity = 1;
                xVelpcity = Random.Range(-1.00f, 1.00f);
            }
        }
        else if ((spawnPointIndex >= 6) & (spawnPointIndex <= 13)) //Botttom
        {
            if ((spawnPointIndex == 6) || (spawnPointIndex == 7))
            {
                xVelpcity = 1;
                YVelocity = Random.Range(-1.00f, 0.00f);
            }
            else if ((spawnPointIndex == 12) || (spawnPointIndex == 13))
            {
                xVelpcity = 1;
                YVelocity = Random.Range(0.00f, 1.00f);
            }
            else
            {
                xVelpcity = 1;
                YVelocity = Random.Range(-1.00f, 1.00f);
            }
        }
        else if ((spawnPointIndex >= 14) & (spawnPointIndex <= 21)) //Botttom
        {
            if ((spawnPointIndex == 14) || (spawnPointIndex == 15))
            {
                xVelpcity = -1;
                YVelocity = Random.Range(-1.00f, 0.00f);
            }
            else if ((spawnPointIndex == 20) || (spawnPointIndex == 21))
            {
                xVelpcity = -1;
                YVelocity = Random.Range(0.00f, 1.00f);
            }
            else
            {
                xVelpcity = -1;
                YVelocity = Random.Range(-1.00f, 1.00f);
            }
        }
    }

    public void Add_back_to_list(GameObject theOtherObject, string listType)
    {
        //string listType = theOtherObject.GetComponent<NewMeteorScript>().Get_List_Type();

        if (listType== "Spawn Meteor Object List")
        {
            if (spawnMeteorObjectList.Contains(theOtherObject))
            {
                theOtherObject.SetActive(false);
                theOtherObject.GetComponent<NewMeteorScript>().Back_To_Original_Position();
            }
            else
            {
                theOtherObject.GetComponent<NewMeteorScript>().Back_To_Original_Position();

                spawnMeteorObjectList.Add(theOtherObject);
                meteorScript.Add(theOtherObject.GetComponent<NewMeteorScript>());
                meteorRigidBody.Add(theOtherObject.GetComponent<Rigidbody2D>());

                modListdeletedMeteorObject.Remove(theOtherObject);

                theOtherObject.SetActive(false);
            }
        }
        else if (listType == "Spawn Frozen Meteor Object List")
        {
            if (spawnFrozenMeteorObjectList.Contains(theOtherObject))
            {
                theOtherObject.SetActive(false);
                theOtherObject.GetComponent<NewMeteorScript>().Back_To_Original_Position();
            }
            else
            {
                theOtherObject.GetComponent<NewMeteorScript>().Back_To_Original_Position();

                spawnFrozenMeteorObjectList.Add(theOtherObject);
                meteorFrozenScript.Add(theOtherObject.GetComponent<NewMeteorScript>());
                meteorFrozenRigidBody.Add(theOtherObject.GetComponent<Rigidbody2D>());

                modListdeletedMeteorObject.Remove(theOtherObject);

                theOtherObject.SetActive(false);
            }
        }   
    }

    /// <summary>
    /// NON FROZEN VARIABLES - Minus One Baby
    /// </summary>
    public void Minus_One_SmithBaby()
    {
            smithBabies--;

        if (smithBabies == 0)
        {
            for (int i = 0; i < modListdeletedMeteorObject.Count; i++)
            {
                if (modListdeletedMeteorObject[i].name == "Toby A Smith")
                {
                    Add_back_to_list(modListdeletedMeteorObject[i], "Spawn Meteor Object List");
                    //this bool below signals that all children should be frozen, thus when health is =0 and meteor splits, the split meteor gets this value from the public method below
                    //Thus since the parent object is being added to the main list, all variables must be reset to original, thus false
                    smithFrozen = false;
                    jackie_A_Smith_Frozen = false;
                    jackie_B_Smith_Frozen = false;
                    robert_A_Smith_Frozen = false;
                    robert_B_Smith_Frozen = false;
                    robert_D_Smith_Frozen = false;
                    robert_E_Smith_Frozen = false;
                    break;
                }
                else if (i > modListdeletedMeteorObject.Count)
                {
                    Debug.Log("Object Not Found");
                    break;

                }
            }
        }
    }
    public void Minus_One_LandonBaby()
    {
        landonBabies--;
        if (landonBabies == 0)
        {
            for (int i = 0; i < modListdeletedMeteorObject.Count; i++)
            {
                if (modListdeletedMeteorObject[i].name == "Jackie B Landon")
                {
                    Add_back_to_list(modListdeletedMeteorObject[i], "Spawn Meteor Object List");
                    landonFrozen = false;
                    robert_A_Landon_Frozen = false;
                    robert_B_Landon_Frozen = false;
                    break;
                }
                else if (i > modListdeletedMeteorObject.Count)
                {
                    Debug.Log("Object Not Found");
                    break;

                }
            }
        }
    } 
    public void Minus_One_PriceBaby()
    {
        priceBabies--;
        if (priceBabies == 0)
        {
            for (int i = 0; i < modListdeletedMeteorObject.Count; i++)
            {
                if (modListdeletedMeteorObject[i].name == "Jackie C Price")
                {
                    Add_back_to_list(modListdeletedMeteorObject[i], "Spawn Meteor Object List");
                    priceFrozen = false;
                    robert_A_Price_Frozen = false;
                    robert_B_Price_Frozen = false;
                    break;
                }
                else if (i > modListdeletedMeteorObject.Count)
                {
                    Debug.Log("Object Not Found");
                    break;

                }
            }
        }
    } 
    public void Minus_One_WhiteBaby()
    {
        whiteBabies--;
        if (whiteBabies == 0)
        {
            for (int i = 0; i < modListdeletedMeteorObject.Count; i++)
            {
                if (modListdeletedMeteorObject[i].name == "Jackie D White")
                {
                    Add_back_to_list(modListdeletedMeteorObject[i], "Spawn Meteor Object List");
                    robert_A_White_Frozen = false;
                    robert_B_White_Frozen = false;
                    whiteFrozen = false;
                    break;
                }
                else if (i > modListdeletedMeteorObject.Count)
                {
                    Debug.Log("Object Not Found");
                    break;

                }
            }
        }
    } 
    public void Minus_One_JacobBaby()
    {
        jacobsBabies--;
        if (jacobsBabies == 0)
        {
            for (int i = 0; i < modListdeletedMeteorObject.Count; i++)
            {
                if (modListdeletedMeteorObject[i].name == "Robert E Jacobs")
                {
                    Add_back_to_list(modListdeletedMeteorObject[i], "Spawn Meteor Object List");
                    jacobsFrozen = false;
                    break;
                }
                else if (i > modListdeletedMeteorObject.Count)
                {
                    Debug.Log("Object Not Found");
                    break;

                }
            }
        }
    } 
    public void Minus_One_MorrisBaby()
    {
        morrisBabies--;
        if (morrisBabies == 0)
        {
            for (int i = 0; i < modListdeletedMeteorObject.Count; i++)
            {
                if (modListdeletedMeteorObject[i].name == "Robert F morris")
                {
                    Add_back_to_list(modListdeletedMeteorObject[i], "Spawn Meteor Object List");
                    morrisFrozen = false;
                    break;
                }
                else if (i > modListdeletedMeteorObject.Count)
                {
                    Debug.Log("Object Not Found");
                    break;

                }
            }
        }
    } 
    public void Minus_One_JohnsonBaby()
    {
        johnsonBabies--;
        if (johnsonBabies == 0)
        {
            for (int i = 0; i < modListdeletedMeteorObject.Count; i++)
            {
                if (modListdeletedMeteorObject[i].name == "Robert G Johnson")
                {
                    Add_back_to_list(modListdeletedMeteorObject[i], "Spawn Meteor Object List");
                    johnsonFrozen = false;
                    break;
                }
                else if (i > modListdeletedMeteorObject.Count)
                {
                    Debug.Log("Object Not Found");
                    break;

                }
            }
        }
    }
    /// <summary>
    /// FROZEN VARIABLES - Minus One Baby
    /// </summary>
    public void Minus_One_SantosBaby()
    {
        santosBabies--;

        if (santosBabies == 0)
        {
            for (int i = 0; i < modListdeletedMeteorObject.Count; i++)
            {
                if (modListdeletedMeteorObject[i].name == "Diego Santos")
                {
                    Add_back_to_list(modListdeletedMeteorObject[i], "Spawn Frozen Meteor Object List"); 
                    break;
                }
                else if (i > modListdeletedMeteorObject.Count)
                {
                    Debug.Log("Object Not Found");
                    break;

                }
            }
        }
    }
    public void Minus_One_RosalesBaby()
    {
        rosalesBabies--;

        if (rosalesBabies == 0)
        {
            for (int i = 0; i < modListdeletedMeteorObject.Count; i++)
            {
                if (modListdeletedMeteorObject[i].name == "Yanna Rosales")
                {
                    Add_back_to_list(modListdeletedMeteorObject[i], "Spawn Frozen Meteor Object List");
                    break;
                }
                else if (i > modListdeletedMeteorObject.Count)
                {
                    Debug.Log("Object Not Found");
                    break;

                }
            }
        }
    }
    public void Minus_One_Jimenez()
    {
        jimenezBabies--;

        if (jimenezBabies == 0)
        {
            for (int i = 0; i < modListdeletedMeteorObject.Count; i++)
            {
                if (modListdeletedMeteorObject[i].name == "Yanna Jimenez")
                {
                    Add_back_to_list(modListdeletedMeteorObject[i], "Spawn Frozen Meteor Object List");
                    break;
                }
                else if (i > modListdeletedMeteorObject.Count)
                {
                    Debug.Log("Object Not Found");
                    break;

                }
            }
        }
    }
    public void Minus_One_Taguptup()
    {
        taguptupBabies--;

        if (taguptupBabies == 0)
        {
            for (int i = 0; i < modListdeletedMeteorObject.Count; i++)
            {
                if (modListdeletedMeteorObject[i].name == "Yanna Taguptup")
                {
                    Add_back_to_list(modListdeletedMeteorObject[i], "Spawn Frozen Meteor Object List");
                    break;
                }
                else if (i > modListdeletedMeteorObject.Count)
                {
                    Debug.Log("Object Not Found");
                    break;

                }
            }
        }
    }
    public void Minus_One_Ramirez()
    {
        ramirezBabies--;

        if (ramirezBabies == 0)
        {
            for (int i = 0; i < modListdeletedMeteorObject.Count; i++)
            {
                if (modListdeletedMeteorObject[i].name == "Hyacynth Ramirez")
                {
                    Add_back_to_list(modListdeletedMeteorObject[i], "Spawn Frozen Meteor Object List");
                    break;
                }
                else if (i > modListdeletedMeteorObject.Count)
                {
                    Debug.Log("Object Not Found");
                    break;

                }
            }
        }
    }
    public void Minus_One_Lopez()
    {
        lopezBabies--;

        if (lopezBabies == 0)
        {
            for (int i = 0; i < modListdeletedMeteorObject.Count; i++)
            {
                if (modListdeletedMeteorObject[i].name == "Hyacynth Lopez")
                {
                    Add_back_to_list(modListdeletedMeteorObject[i], "Spawn Frozen Meteor Object List");
                    break;
                }
                else if (i > modListdeletedMeteorObject.Count)
                {
                    Debug.Log("Object Not Found");
                    break;

                }
            }
        }
    }
    public void Minus_One_Villareal()
    {
        villarealBabies--;

        if (villarealBabies == 0)
        {
            for (int i = 0; i < modListdeletedMeteorObject.Count; i++)
            {
                if (modListdeletedMeteorObject[i].name == "Hyacynth Villareal")
                {
                    Add_back_to_list(modListdeletedMeteorObject[i], "Spawn Frozen Meteor Object List");
                    break;
                }
                else if (i > modListdeletedMeteorObject.Count)
                {
                    Debug.Log("Object Not Found");
                    break;

                }
            }
        }
    }
    /// <summary>
    /// NON FROZEN and FROZEN VARIABLES - Spawn Split Meteors
    /// </summary>
    public void Spawn_BigMeteorObjects(Transform parentsTransform, string gameObjectName)
    {
        int index1 = 0; int index2 = 0;

        if (gameObjectName == "Toby A Smith") { index1 = 0; index2 = 1; smithBabies = smithBabies + 2; } //Spawns Jackie A and Jackie B from SplitBigmeteorObject List
        else if (gameObjectName == "Diego Santos") { index1 = 2; index2 = 3; santosBabies = santosBabies + 2; }

        splitBigMeteorObject[index1].transform.position = new Vector3((parentsTransform.position.x + 0.5f), (parentsTransform.position.y + 0.5f));
        splitBigMeteorObject[index2].transform.position = parentsTransform.position;

        splitBigMeteorScript[index1].EnableMeteor();
        splitBigMeteorScript[index2].EnableMeteor();

        if ((smithFrozen)&(gameObjectName == "Toby A Smith"))
        {
            splitBigMeteorScript[index1].SpawnFrozen();
            splitBigMeteorScript[index2].SpawnFrozen();
        }

        splitBigMeteorRigidBody[index1].velocity = new Vector2(xVelpcity * (Random.Range(1, 3)), YVelocity * (Random.Range(1, 3)));
        splitBigMeteorRigidBody[index2].velocity = new Vector2(xVelpcity * -(Random.Range(1, 3)), YVelocity * -(Random.Range(1, 3)));

    }

    public void Spawn_Med_MeteorObjects(Transform parentsTransform, string gameObjectName)
    {
        int index1 = 0; int index2 = 0;

        switch(gameObjectName)
        {
            case "Jackie A Smith":
                index1 = 0; index2 = 1; smithBabies = smithBabies + 2;
                break;
            case "Jackie B Smith":
                index1 = 2; index2 = 3; smithBabies = smithBabies + 2;
                break;
            case "Jackie B Landon":
                index1 = 4; index2 = 5; landonBabies = landonBabies + 2;
                break;
            case "Jackie C Price":
                index1 = 6; index2 = 7; priceBabies = priceBabies + 2;
                break;
            case "Jackie D White":
                index1 = 8; index2 = 9; whiteBabies = whiteBabies + 2;
                break;
            case "Yanna A Santos":
                index1 = 10; index2 = 11; santosBabies = santosBabies + 2;
                break;
            case "Yanna B Santos":
                index1 = 12; index2 = 13; santosBabies = santosBabies + 2;
                break;
            case "Yanna Rosales":
                index1 = 14; index2 = 15; rosalesBabies = rosalesBabies + 2;
                break;
            case "Yanna Jimenez":
                index1 = 16; index2 = 17; jimenezBabies = jimenezBabies + 2;
                break;
            case "Yanna Taguptup":
                index1 = 18; index2 = 19; taguptupBabies = taguptupBabies + 2;
                break;
            default:
                print("No case for meteorName");
                break;
        }
                                 
        //Spawns splitmedMeteor from SplitMedMeteorObject List
        splitMedMeteorObject[index1].transform.position = new Vector3((parentsTransform.position.x + 0.5f), (parentsTransform.position.y + 0.5f));
        splitMedMeteorObject[index2].transform.position = parentsTransform.position;

        splitMedMeteorScript[index1].EnableMeteor();
        splitMedMeteorScript[index2].EnableMeteor();

        if ((gameObjectName == "Jackie A Smith") & ((smithFrozen)|(jackie_A_Smith_Frozen)))
        { Spawn_two_frozen_med_meteors(index1, index2); }
        else if ((gameObjectName == "Jackie B Smith") & ((smithFrozen)|(jackie_B_Smith_Frozen)))
        { Spawn_two_frozen_med_meteors(index1, index2); }
        else if ((gameObjectName == "Jackie B Landon") & (landonFrozen))
        { Spawn_two_frozen_med_meteors(index1, index2); }
        else if ((gameObjectName == "Jackie C Price") & (priceFrozen))
        { Spawn_two_frozen_med_meteors(index1, index2); }
        else if ((gameObjectName == "Jackie D White") & (whiteFrozen))
        { Spawn_two_frozen_med_meteors(index1, index2); }

        splitMedMeteorRigidBody[index1].velocity = new Vector2(xVelpcity * (Random.Range(1, 3)), YVelocity * (Random.Range(1, 3)));
        splitMedMeteorRigidBody[index2].velocity = new Vector2(xVelpcity * -(Random.Range(1, 3)), YVelocity * -(Random.Range(1, 3)));
    }

    private void Spawn_two_frozen_med_meteors(int index1, int index2)
    {
        splitMedMeteorScript[index1].SpawnFrozen();
        splitMedMeteorScript[index2].SpawnFrozen();
    } //Repeated code used in SpawnMedMeteorObject Method

    public void Spawn_tiny_MeteorObjects(Transform parentsTransform, string gameObjectName)
    {
        int index1 = 0; int index2 = 0;
        bool activateCheck = false;

        switch (gameObjectName)
        {
            case "Robert A Smith":
                index1 = 0; index2 = 1; smithBabies = smithBabies + 2;
                break;
            case "Robert B Smith":
                index1 = 2; index2 = 3; smithBabies = smithBabies + 2;
                break;
            case "Robert D Smith":
                index1 = 4; index2 = 5; smithBabies = smithBabies + 2;
                break;
            case "Robert E Smith":
                index1 = 6; index2 = 7; smithBabies = smithBabies + 2;
                break;
            case "Robert A Landon":
                index1 = 8; index2 = 9; landonBabies = landonBabies + 2;
                break;
            case "Robert B Landon":
                index1 = 10; index2 = 11; landonBabies = landonBabies + 2;
                break;
            case "Robert A Price":
                index1 = 12; index2 = 13; priceBabies = priceBabies + 2;
                break;
            case "Robert B Price":
                index1 = 14; index2 = 15; priceBabies = priceBabies + 2;
                break;
            case "Robert A White":
                index1 = 16; index2 = 17; whiteBabies = whiteBabies + 2;
                break;
            case "Robert B White":
                index1 = 18; index2 = 19; whiteBabies = whiteBabies + 2;
                break;
            case "Robert E Jacobs":
                index1 = 20; index2 = 21; jacobsBabies = jacobsBabies + 2;
                break;
            case "Robert F morris":
                index1 = 22; index2 = 23; morrisBabies = morrisBabies + 2;
                break;
            case "Robert G Johnson":
                index1 = 24; index2 = 25; johnsonBabies = johnsonBabies + 2;
                break;
            case "Hyacynth A Santos":
                index1 = 26; index2 = 27; santosBabies = santosBabies + 2; activateCheck = true;
                break;
            case "Hyacynth B Santos":
                index1 = 28; index2 = 29; santosBabies = santosBabies + 2; activateCheck = true;
                break;
            case "Hyacynth C Santos":
                index1 = 30; index2 = 31; santosBabies = santosBabies + 2; activateCheck = true;
                break;
            case "Hyacynth D Santos":
                index1 = 32; index2 = 33; santosBabies = santosBabies + 2; activateCheck = true;
                break;
            case "Hyacynth A Rosales":
                index1 = 34; index2 = 35; rosalesBabies = rosalesBabies + 2; activateCheck = true;
                break;
            case "Hyacynth B Rosales":
                index1 = 36; index2 = 37; rosalesBabies = rosalesBabies + 2; activateCheck = true;
                break;
            case "Hyacynth A Jimenez":
                index1 = 38; index2 = 39; jimenezBabies = jimenezBabies + 2; activateCheck = true;
                break;
            case "Hyacynth B Jimenez":
                index1 = 40; index2 = 41; jimenezBabies = jimenezBabies + 2; activateCheck = true;
                break;
            case "Hyacynth A Taguptup":
                index1 = 42; index2 = 43; taguptupBabies = taguptupBabies + 2; activateCheck = true;
                break;
            case "Hyacynth B Taguptup":
                index1 = 44; index2 = 45; taguptupBabies = taguptupBabies + 2; activateCheck = true;
                break;
            case "Hyacynth Ramirez":
                index1 = 46; index2 = 47; ramirezBabies = ramirezBabies + 2; activateCheck = true;
                break;
            case "Hyacynth Lopez":
                index1 = 48; index2 = 49; lopezBabies = lopezBabies + 2; activateCheck = true;
                break;
            case "Hyacynth Villareal":
                index1 = 50; index2 = 51; villarealBabies = villarealBabies + 2; activateCheck = true;
                break;
            default:
                print("No case for meteorName");
                break;
        }

        //Spawns splitTinyMeteor from SplitMedMeteorObject List
        splitTinyMeteorObject[index1].transform.position = new Vector3((parentsTransform.position.x + 0.2f), (parentsTransform.position.y + 0.2f));
        splitTinyMeteorObject[index2].transform.position = parentsTransform.position;

        splitTinyMeteorScript[index1].EnableMeteor();
        splitTinyMeteorScript[index2].EnableMeteor();

        if ((gameObjectName == "Robert A Smith") & ((smithFrozen) | (jackie_A_Smith_Frozen) | (robert_A_Smith_Frozen)))
        {Spawn_two_tiny_Frozen_meteors(index1, index2);}
        else if ((gameObjectName == "Robert B Smith") & ((smithFrozen) | (jackie_A_Smith_Frozen) | (robert_B_Smith_Frozen)))
        { Spawn_two_tiny_Frozen_meteors(index1, index2); }
        else if ((gameObjectName == "Robert D Smith") & ((smithFrozen) | (jackie_B_Smith_Frozen) | (robert_D_Smith_Frozen)))
        { Spawn_two_tiny_Frozen_meteors(index1, index2); }
        else if ((gameObjectName == "Robert E Smith") & ((smithFrozen) | (jackie_B_Smith_Frozen) | (robert_E_Smith_Frozen)))
        { Spawn_two_tiny_Frozen_meteors(index1, index2); }
        else if ((gameObjectName == "Robert A Landon") & ((landonFrozen)|(robert_A_Landon_Frozen)))
        { Spawn_two_tiny_Frozen_meteors(index1, index2); }
        else if ((gameObjectName == "Robert B Landon") & ((landonFrozen)|(robert_B_Landon_Frozen)))
        { Spawn_two_tiny_Frozen_meteors(index1, index2); }
        else if ((gameObjectName == "Robert A Price") & ((priceFrozen) | (robert_A_Price_Frozen)))
        { Spawn_two_tiny_Frozen_meteors(index1, index2); }
        else if ((gameObjectName == "Robert B Price") & ((priceFrozen) | (robert_B_Price_Frozen)))
        { Spawn_two_tiny_Frozen_meteors(index1, index2); }
        else if ((gameObjectName == "Robert A White") & ((whiteFrozen) | (robert_A_White_Frozen)))
        { Spawn_two_tiny_Frozen_meteors(index1, index2); }
        else if ((gameObjectName == "Robert B White") & ((whiteFrozen) | (robert_B_White_Frozen)))
        { Spawn_two_tiny_Frozen_meteors(index1, index2); }
        else if ((gameObjectName == "Robert G Johnson") & (johnsonFrozen))
        { Spawn_two_tiny_Frozen_meteors(index1, index2); }
        else if ((gameObjectName == "Robert F morris") & (morrisFrozen))
        { Spawn_two_tiny_Frozen_meteors(index1, index2); }
        else if ((gameObjectName == "Robert E Jacobs") & (jacobsFrozen))
        { Spawn_two_tiny_Frozen_meteors(index1, index2); }

        splitTinyMeteorRigidBody[index1].velocity = new Vector2(xVelpcity * (Random.Range(1, 3)), YVelocity * (Random.Range(1, 3)));
        splitTinyMeteorRigidBody[index2].velocity = new Vector2(xVelpcity * -(Random.Range(1, 3)), YVelocity * -(Random.Range(1, 3)));

        if (activateCheck)
        {
            switch (splitTinyMeteorScript[index1].gameObject.tag)
            {
                case "tinyFrozenMeteor":
                    Check_if_frozenTinyMeteor_isWithinBounds(splitTinyMeteorScript[index1].gameObject);
                    break;
                case "sfd_tinyFrozenMeteor":
                    Check_if_frozenTinyMeteor_isWithinBounds(splitTinyMeteorScript[index1].gameObject);
                    break;
                default:
                    break;
            }

            //If Frozen, This assigns the meteor to a peanut or assigns it to a list
            switch (splitTinyMeteorObject[index2].gameObject.tag)
            {
                case "tinyFrozenMeteor":
                    Check_if_frozenTinyMeteor_isWithinBounds(splitTinyMeteorObject[index2].gameObject);
                    break;
                case "sfd_tinyFrozenMeteor":
                    Check_if_frozenTinyMeteor_isWithinBounds(splitTinyMeteorScript[index2].gameObject);
                    break;
                //Do you do tag "sfd_tinyMeteor"?
                default:
                    break;
            }
        }
    }

    private void Spawn_two_tiny_Frozen_meteors(int index1, int index2)
    {
        splitTinyMeteorScript[index1].SpawnFrozen();
        splitTinyMeteorScript[index2].SpawnFrozen();

    } //Repeated code used in SpawnTinyMeteorObject Method


    public void Access_the_Script_to_kill_Meteor(GameObject thismeteor)
    {
        if ((thismeteor.tag == "bigMeteor")| (thismeteor.tag == "bigFrozenMeteor")) //this script loops through the list to find the object name in the bigMeteor.list
        {
            for (int i = 0; i < splitBigMeteorScript.Count; i++)
            {
                if (splitBigMeteorScript[i].name == thismeteor.name)
                {
                    splitBigMeteorScript[i].Kill_this_meteor_meaning_disable_backtoOriginal_pos_andresethealth();
                    break;
                }
                else if (splitBigMeteorScript.Count < i)
                {
                    Debug.Log("Meteor not in splitBigMeteorScript List");
                    break;
                }
            }
        }
        else if ((thismeteor.tag == "madeMeteor")| (thismeteor.tag == "medFrozenMeteor"))
        {
            for (int i = 0; i < splitMedMeteorScript.Count; i++)
            {
                if (splitMedMeteorScript[i].name == thismeteor.name)
                {
                    splitMedMeteorScript[i].Kill_this_meteor_meaning_disable_backtoOriginal_pos_andresethealth();
                    break;
                }
                else if (splitMedMeteorScript.Count < i)
                {
                    Debug.Log("Meteor not in splitMedMeteorScript List");
                    break;
                }
            }
        }
        else if ((thismeteor.tag == "tinyMeteor")| (thismeteor.tag == "tinyFrozenMeteor"))
        {
            for (int i = 0; i < splitTinyMeteorScript.Count; i++)
            {
                if (splitTinyMeteorScript[i].name == thismeteor.name)
                {
                    splitTinyMeteorScript[i].Kill_this_meteor_meaning_disable_backtoOriginal_pos_andresethealth();
                    break;
                }
                else if (splitTinyMeteorScript.Count < i)
                {
                    Debug.Log("Meteor not in splitTinyMeteorScript List");
                    break;
                }
            }
        }
    }

    public void Smith_Freeze_True() { smithFrozen = true; }
    public void Landon_Freeze_True() { landonFrozen = true; }
    public void Price_Freeze_True() { priceFrozen = true; }
    public void White_Freeze_True() { whiteFrozen = true; }
    public void Jacobs_Freeze_True() { jacobsFrozen = true; }
    public void Morris_Freeze_True() { morrisFrozen = true; }
    public void Johnson_Freeze_True() { johnsonFrozen = true; }
    /// For split meteors
    public void Jackie_A_Smith_Freeze_True() { jackie_A_Smith_Frozen = true; }
    public void Jackie_B_Smith_Freeze_True() { jackie_B_Smith_Frozen = true; }
    public void Robert_A_Smith_Freeze_True() { robert_A_Smith_Frozen = true; }
    public void Robert_B_Smith_Freeze_True() { robert_B_Smith_Frozen = true; }
    public void Robert_D_Smith_Freeze_True() { robert_D_Smith_Frozen = true; }
    public void Robert_E_Smith_Freeze_True() { robert_E_Smith_Frozen = true; }
    public void Robert_A_Landon_Freeze_True() { robert_A_Landon_Frozen = true; }
    public void Robert_B_Landon_Freeze_True() { robert_B_Landon_Frozen = true; }
    public void Robert_A_Price_Freeze_True() { robert_A_Price_Frozen = true; }
    public void Robert_B_Price_Freeze_True() { robert_B_Price_Frozen = true; }
    public void Robert_A_White_Freeze_True() { robert_A_White_Frozen = true; }
    public void Robert_B_White_Freeze_True() { robert_B_White_Frozen = true; }

    public bool GetSmithBoolValue() { return smithFrozen; }
    public bool GetLandonBoolValue() { return landonFrozen; }
    public bool GetPriceBoolValue() { return priceFrozen; }
    public bool GetWhiteBoolValue() { return whiteFrozen; }
    public bool GetJacobsBoolValue() { return jacobsFrozen; }
    public bool GetMorrisBoolValue() { return morrisFrozen; }
    public bool GetJohnsonBoolValue() { return johnsonFrozen; }
    /// For split meteors
    public bool Get_JackieA_SmithBoolValue() { return jackie_A_Smith_Frozen; }
    public bool Get_JackieB_SmithBoolValue() { return jackie_B_Smith_Frozen; }
    public bool Get_RobartA_SmithBoolValue() { return robert_A_Smith_Frozen; }
    public bool Get_RobertB_SmithBoolValue() { return robert_B_Smith_Frozen; }
    public bool Get_RobertD_SmithBoolValue() { return robert_D_Smith_Frozen; }
    public bool Get_RobertE_SmithBoolValue() { return robert_E_Smith_Frozen; }
    public bool Get_RobartA_LandonBoolValue() { return robert_A_Landon_Frozen; }
    public bool Get_RobertB_LandonBoolValue() { return robert_B_Landon_Frozen; }
    public bool Get_RobartA_PriceBoolValue() { return robert_A_Price_Frozen; }
    public bool Get_RobertB_PriceBoolValue() { return robert_B_Price_Frozen; }
    public bool Get_RobertA_WhiteBoolValue() { return robert_A_White_Frozen; }
    public bool Get_RobertB_WhiteBoolValue() { return robert_B_White_Frozen; }

    public string Get_FrozenMeteorName() { return meteorNameforFrozen; }

    public Vector3 Get_PlayerPos() { playerPos = player.transform.position; return playerPos; }

    public List<GameObject> Get_ListFromMeteorScript() { return listofAll_tinyFrozenMeteors_Assigned_toPeanut; }
    public void Remove_From_List_ofAll_TinyFrozen_blah_blah_blah(GameObject theMeteor)
    {
        listofAll_tinyFrozenMeteors_Assigned_toPeanut.Remove(theMeteor);
    }

    //Maybe will Use this code but not a big issue: Code not finished
    private IEnumerator UpdateListDeletedObjects()
    {
        yield return new WaitForSeconds(10);
        if (modListdeletedMeteorObject.Count == 0)
        {
            //Restart Coroutine
            Debug.Log("Restarting Coroutine");
            Debug.Break();
            StartCoroutine(UpdateListDeletedObjects());
        }
        else 
        {
            Debug.Log("There are "+ modListdeletedMeteorObject .Count + " Game Object/s is in Scene");

            for (int i=0; i< modListdeletedMeteorObject.Count; i++)
            {
                Debug.Log("Game Object: " + i);
                string objectName = modListdeletedMeteorObject[i].name;
                var isObjectInscene = GameObject.Find(objectName);

                if(isObjectInscene)
                {
                    //Do nothing
                    Debug.Log("Game Object is in Scene");
                }
                else
                {
                    Debug.Log("Game Object is in NOT in Scene");

                    if ((modListdeletedMeteorObject[i].tag=="tinyMeteor")|
                        (modListdeletedMeteorObject[i].tag == "sfd_tinyMeteor"))
                    {
                        //remove from deleted list
                        //add back to non frozen list
                    }
                    else if((modListdeletedMeteorObject[i].tag == "tinyFrozenMeteor") |
                        (modListdeletedMeteorObject[i].tag == "sfd_tinyFrozenMeteor"))
                    {
                        //remove from deleted list
                        //add back to frozen list
                    }
                    else 
                    {
                        string familyname = modListdeletedMeteorObject[i].GetComponent<NewMeteorScript>().GetFamilyNameOfMeteor();
                        Debug.Log("Game Object family name is " + familyname);

                        switch (familyname)
                        {
                            case "Smith":
                                if(smithBabies==0)
                                {
                                    Debug.Log("No Smith Babies Generated");
                                    spawnMeteorObjectList.Add(modListdeletedMeteorObject[i]);
                                    Debug.Log("Added back to spawnMeteorObjectList");
                                    modListdeletedMeteorObject.Remove(modListdeletedMeteorObject[i]);
                                    Debug.Log("Removed from modListdeletedMeteorObject");
                                }
                                else
                                {
                                    //Find each baby in scene by name
                                    //if smithbabies = number of counter objects in scene
                                    //then do nothing
                                    //otherwise
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                break;
                            case "Landon":
                                if (smithBabies == 0)
                                {
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                else
                                {
                                    //Find each baby in scene by name
                                    //if smithbabies = number of counter objects in scene
                                    //then do nothing
                                    //otherwise
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                break;
                            case "Price":
                                if (smithBabies == 0)
                                {
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                else
                                {
                                    //Find each baby in scene by name
                                    //if smithbabies = number of counter objects in scene
                                    //then do nothing
                                    //otherwise
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                break;
                            case "Jacobs":
                                if (smithBabies == 0)
                                {
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                else
                                {
                                    //Find each baby in scene by name
                                    //if smithbabies = number of counter objects in scene
                                    //then do nothing
                                    //otherwise
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                break;
                            case "Morris":
                                if (smithBabies == 0)
                                {
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                else
                                {
                                    //Find each baby in scene by name
                                    //if smithbabies = number of counter objects in scene
                                    //then do nothing
                                    //otherwise
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                break;
                            case "Johnson":
                                if (smithBabies == 0)
                                {
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                else
                                {
                                    //Find each baby in scene by name
                                    //if smithbabies = number of counter objects in scene
                                    //then do nothing
                                    //otherwise
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                break;
                            case "Santos":
                                if (smithBabies == 0)
                                {
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                else
                                {
                                    //Find each baby in scene by name
                                    //if smithbabies = number of counter objects in scene
                                    //then do nothing
                                    //otherwise
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                break;
                            case "Rosales":
                                if (smithBabies == 0)
                                {
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                else
                                {
                                    //Find each baby in scene by name
                                    //if smithbabies = number of counter objects in scene
                                    //then do nothing
                                    //otherwise
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                break;
                            case "Jimenez":
                                if (smithBabies == 0)
                                {
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                else
                                {
                                    //Find each baby in scene by name
                                    //if smithbabies = number of counter objects in scene
                                    //then do nothing
                                    //otherwise
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                break;
                            case "Taguptup":
                                if (smithBabies == 0)
                                {
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                else
                                {
                                    //Find each baby in scene by name
                                    //if smithbabies = number of counter objects in scene
                                    //then do nothing
                                    //otherwise
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                break;
                            case "Ramirez":
                                if (smithBabies == 0)
                                {
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                else
                                {
                                    //Find each baby in scene by name
                                    //if smithbabies = number of counter objects in scene
                                    //then do nothing
                                    //otherwise
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                break;
                            case "Lopez":
                                if (smithBabies == 0)
                                {
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                else
                                {
                                    //Find each baby in scene by name
                                    //if smithbabies = number of counter objects in scene
                                    //then do nothing
                                    //otherwise
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                break;
                            case "Villareal":
                                if (smithBabies == 0)
                                {
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                else
                                {
                                    //Find each baby in scene by name
                                    //if smithbabies = number of counter objects in scene
                                    //then do nothing
                                    //otherwise
                                    //remove from list
                                    //add back to SpawnMetObjList
                                }
                                break;
                            default:
                                Debug.Log("No Family Name assigned to this meteor " + modListdeletedMeteorObject[i].name);
                                break;
                        }
                            
                    }
                }
            }
            //restart Coroutine
            Debug.Log("Coroutine Done: Restarting Coroutine");
            Debug.Break();
            StartCoroutine(UpdateListDeletedObjects());
        }
    }

    public void Call_ThisMethod_Coroutine(GameObject meteorObjectThis)
    {
        StartCoroutine(TurnOffBoolOfMeteor(meteorObjectThis));
    }

    private IEnumerator TurnOffBoolOfMeteor(GameObject meteorObjectthisOne)
    {
        yield return new WaitForSeconds(1);
        meteorObjectthisOne.GetComponent<NewMeteorScript>().turnOffmyExplosionBoolPlease();
    }

    public void MakeSpawnRateFaster() { meteorSpawnRate = 1; }

    public void StopSpawnRate() { stopSpawn = true; }
}


