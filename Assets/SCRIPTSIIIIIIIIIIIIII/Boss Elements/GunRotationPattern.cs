using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotationPattern : MonoBehaviour
{
    //Config Params
    float timeBetRotCycles = 5f; //Serialize for debug, Remove Later
    [Header("Reset Timer Settings")]
    [SerializeField] float minTimeBetRot = 5f;
    [SerializeField] float maxTimeBetRot = 5f;

    [Header("Rotation Settings")]
    [SerializeField] float rotSpeed = 10f;
    [SerializeField] bool startLeft = true;
    [SerializeField] List<float> angleValueForLeftRot;
    [SerializeField] List<float> angleValueForRightRot;

    [Header("Projectile Settings")]
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projAngularVelocity = 0f;
    [SerializeField] public GameObject parent;
    [SerializeField] AudioClip shootSound;
    [SerializeField] float shootSoundVolume;

    //Rotation Variables
    int numOfLeftRot;
    int numOfRightRot;
    int totalNumRot;
    int currentNumRot8 = 0;
    int leftRotIndex = 0;
    int rightRotIndex = 0;

    float currentAngleValue = 0;
    float targetRotVal = 0;

    bool countDownOn = true;
    bool rotateToLeft = false;
    bool playRotate = false;
    bool updateTargetRotVal = false;

    //Projectile variables
    float eularAngle;
    float eulerX;
    float eulerY;
    float xProjTraj;
    float yProjTraj;
    float shotCounter;

    //Cached Componenet ref
    GunHeadConfig gunHeadConfig;
    Transform myRotation;
    GameObject gunHead;

    // Start is called before the first frame update
    void Start()
    {
        myRotation = GetComponent<Transform>();

        //Initialize Game Object
        //Initialize Component
        gunHead = parent.transform.GetChild(0).gameObject;
        gunHeadConfig = gunHead.GetComponent<GunHeadConfig>();

        numOfLeftRot = angleValueForLeftRot.Count;
        numOfRightRot = angleValueForRightRot.Count;

        totalNumRot = numOfLeftRot + numOfRightRot;
        timeBetRotCycles = Random.Range(minTimeBetRot, maxTimeBetRot);

       // Debug.Log("Total Number of Rotations is " + (totalNumRot));
       // Debug.Log("Current angle value is " + currentAngleValue);
       // Debug.Log(playRotate);

        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()

    {
        if (countDownOn) { ActivateTimer(); }

        if (timeBetRotCycles <= 0) { DoTimerReachedZeroActions(); }

        if (playRotate)
        {
            CountDownAndShoot();

            if (rotateToLeft)
            {
                DoLeftRotationMethod();
            }
            else if (!rotateToLeft)
            {
                DoRightRotationMethod();
            }


            if (currentNumRot8 >= totalNumRot)
            {
              //  Debug.Log("Total Number of Rotations Complete");
                StartLeftTrueorFalse();
                leftRotIndex = 0;
                rightRotIndex = 0;
                targetRotVal = 0;
                currentNumRot8 = 0;
                playRotate = false;
                updateTargetRotVal = false;
                countDownOn = true;
            }

            myRotation.eulerAngles = new Vector3(0, 0, currentAngleValue);
        }
    }
    private void CountDownAndShoot()
    {
        //Grabs Updated transform values from Child Object Attached to it

        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }
    private void Fire()
    {
        eularAngle = myRotation.rotation.eulerAngles.z;
        yProjTraj = gunHeadConfig.GetYProjTrajGunHead();
        xProjTraj = gunHeadConfig.GetXProjTrajGunHead();

        GetCartesianValue();

        GameObject laser = Instantiate(
           projectile,
           new Vector3(xProjTraj, yProjTraj, -0.05f),
           transform.rotation
           ) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(eulerX * projectileSpeed, eulerY * projectileSpeed);
        laser.GetComponent<Rigidbody2D>().angularVelocity = projAngularVelocity;

        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position,
            shootSoundVolume);
    }

    private void GetCartesianValue()
    {
        if ((eularAngle >= 0) && (eularAngle <= 90))
        {
            if (eularAngle == 0)
            {
                eulerX = 0;
                eulerY = -1;
            }
            else
            {
                eulerX = eularAngle / 90;
                eulerY = eulerX - 1;
            }

            //Debug.Log("Projectile is fired with a EA of "
             //   + eularAngle + " and in Q4 (+,-) Vector(" +
              //  eulerX + "," + eulerY + ")");

        }
        else if ((eularAngle >= 90) && (eularAngle <= 180))
        {
            if (eularAngle == 90)
            {
                eulerY = 0;
                eulerX = 1;
            }
            else
            {
                eulerY = (eularAngle - 90) / 90;
                eulerX = 1 - eulerY;
            }

           // Debug.Log("Projectile is fired with a EA of "
              //  + eularAngle + " and in Q1 (+,+) Vector(" +
              //  eulerX + "," + eulerY + ")");

        }
        else if ((eularAngle >= 180) && (eularAngle <= 270))
        {
            if (eularAngle == 180)
            {
                eulerX = 0;
                eulerY = 1;
            }
            else
            {
                eulerX = ((eularAngle - 180) / 90) * (-1);
                eulerY = ((eulerX * -1) - 1) * (-1);
            }

           // Debug.Log("Projectile is fired with a EA of "
             //   + eularAngle + " and in Q2 (-,+) Vector(" +
             //   eulerX + "," + eulerY + ")");

        }
        else if ((eularAngle >= 270) && (eularAngle <= 360))
        {
            if (eularAngle == 270)
            {
                eulerY = 0;
                eulerX = -1;
            }
            else
            {
                eulerY = ((eularAngle - 270) / 90) * (-1);
                eulerX = ((eulerY * (-1)) - 1);
            }

           // Debug.Log("Projectile is fired with a EA of "
             //   + eularAngle + " and in Q3 (-,-) Vector(" +
              //  eulerX + "," + eulerY + ")");

        }
        else
        {
            Debug.Log("Method Fire() 5th if else statement tiggered");
        }
    }

    private void DoTimerReachedZeroActions()
            {
                countDownOn = true;
                StartLeftTrueorFalse();
                RestartRotateCounterAndTurnOff();
                playRotate = true;
                updateTargetRotVal = true;
            }

    private void DoLeftRotationMethod()
    {
        // 6 if statements
        // First 2 checks if leftRotIndex is invalid
        // 3rd statement checks if numOfLeftRot8 has a value
        // if Statement 1:Check if Index Value out of range
        // if Statement 2:Check if Index value is negative
        // if Statement 3:Check if numOfLeftRot8 is 0
        // if Statement 4:Check if All Left Rotations Complete
        // if Statement 5:Start Rotation when leftRotIndex <= numOfLeftRot8
        // if Statement 6:Trigger Last if statement

        //Statement 1
        if (leftRotIndex >= (numOfLeftRot + 1))
        {
            Debug.Log("Left Index Value Out of range");
        }

        //Statement 2
        else if (leftRotIndex < 0)
        {
            Debug.Log("leftRotIndexVal is negative");
        }

        //Statement 3
        else if (numOfLeftRot <= 0)
        {
            Debug.Log("No Left Rotation Value Assigned");
            LeftRotationsComplete();
        }

        //Statement 4
        else if (leftRotIndex == numOfLeftRot)
        {
            Debug.Log("All Left Rotations Complete");
            LeftRotationsComplete();
        }

        //Statement 5
        else if (leftRotIndex <= numOfLeftRot)
        {
            if (leftRotIndex == 0)
            {
                //leftRotIndex here cannot use the formula: prevLeftRotIndex = leftRotIndexVal - 1
                //There is no previous value since leftRotIndex Val is at 0
                //Jump straight to converting the value to negative to turn left
                //NameMethod RotateLeftatIndex0

                if (updateTargetRotVal)
                {
                    targetRotVal += (0 - angleValueForLeftRot[leftRotIndex]);

                   //Debug.Log("targetRotVal set at " + targetRotVal);

                   updateTargetRotVal = false;

                   // Debug.Log("Now Rotating to the left. Angle Value: " +
                   // (0 - angleValueForLeftRot[leftRotIndex]));
                }

                currentAngleValue -= Time.deltaTime * rotSpeed;

                if (currentAngleValue <= targetRotVal)
                {
                  //  Debug.Log("Left Rotation " + leftRotIndex +
                   //     ": Angle " + angleValueForLeftRot[leftRotIndex] + " complete");

                    StopLeftStartRight();

                   // Debug.Log("Number of New LeftIndex Value is " + leftRotIndex);
                }
            }

            else if (leftRotIndex > 0)
            {
                //Name this Method RotatetotheLeft()
                //get previous index
                //Since left rotation convert previous value to negative
                //targetRotVal is the cumulative value of previous values 
                //(including both previous left and right rotations)

                if (updateTargetRotVal)
                {
                    targetRotVal -= angleValueForLeftRot[leftRotIndex];

                   // Debug.Log("targetRotVal set at " + targetRotVal);
                 
                    updateTargetRotVal = false;

                   //Debug.Log("Now Rotating to the left. Angle Value: " +
                       // (0 - angleValueForLeftRot[leftRotIndex]));
                }

                currentAngleValue -= Time.deltaTime * rotSpeed;

                if (currentAngleValue <= targetRotVal)
                {
                  //  Debug.Log("Left Rotation " + leftRotIndex +
                     //   ": Angle " + angleValueForLeftRot[leftRotIndex] + " complete");

                    StopLeftStartRight();

                   // Debug.Log("Number of New LeftIndex Value is " + leftRotIndex);
                }
            }

        }

        //Statement 6
        else
        {
            Debug.Log("Triggered CheckForInvalidLeftIndexValue else (6th if statement)");
        }
    }

    private void DoRightRotationMethod()
            {
                //Statement 1
                if (rightRotIndex >= (numOfRightRot + 1))
                {
                    Debug.Log("Right Index Value Out of range");
                }

                //Statement 2
                else if (rightRotIndex < 0)
                {
                    Debug.Log("RightRotIndexVal is negative");
                }

                //Statement 3
                else if (numOfRightRot <= 0)
                {
                    Debug.Log("No Right Rotation Value Assigned");

                    //Since no numOfLeftRot8 is 0
                    //no need to rotate left, proceed to rotate right

                    rotateToLeft = true;
                }

                //Statement 4
                else if (rightRotIndex == numOfRightRot)
                {
                    Debug.Log("All Right Rotations Complete");
                    RightRotationsComplete();
                }

                //Statement 5
                else if (rightRotIndex <= numOfRightRot)
                {
                    if (rightRotIndex == 0)
                    {
                        //leftRotIndex here cannot use the formula: prevLeftRotIndex = leftRotIndexVal - 1
                        //There is no previous value since leftRotIndex Val is at 0
                        //Jump straight to converting the value to negative to turn left
                        //NameMethod RotateLeftatIndex0
                        if (updateTargetRotVal)
                        {
                            targetRotVal += angleValueForRightRot[rightRotIndex];
                            
                            updateTargetRotVal = false;

                          //  Debug.Log("targetRotVal set at " + targetRotVal);

                           // Debug.Log("Now Rotating to the Right. Angle Value: " +
                             //   angleValueForRightRot[rightRotIndex]);
                        }

                        currentAngleValue += Time.deltaTime * rotSpeed;

                        if (currentAngleValue >= targetRotVal)
                        {
                          //  Debug.Log("Right Rotation " + rightRotIndex +
                              //  ": Angle " + angleValueForRightRot[rightRotIndex] + " complete");

                            StartLeftStopRight();

                          //  Debug.Log("Number of New RightIndex Value is " + rightRotIndex);
                        }
                    }

                    else if (rightRotIndex > 0)
                    {
                        //Name this Method RotatetotheLeft()
                        //get previous index
                        //Since left rotation convert previous value to negative
                        //targetRotVal is the cumulative value of previous values 
                        //(including both previous left and right rotations)

                        if (updateTargetRotVal)
                        {
                            targetRotVal += angleValueForRightRot[rightRotIndex];
                            
                            updateTargetRotVal = false;

                          // Debug.Log("targetRotVal set at " + targetRotVal);

                           // Debug.Log("Now Rotating to the Right. Angle Value: " +
                             //    angleValueForRightRot[rightRotIndex]);
                        }

                        currentAngleValue += Time.deltaTime * rotSpeed;

                        if (currentAngleValue >= targetRotVal)
                        {
                           // Debug.Log("Right Rotation " + rightRotIndex +
                            //    ": Angle " + angleValueForRightRot[rightRotIndex] + " complete");

                            StartLeftStopRight();

                           // Debug.Log("Number of New RightIndex Value is " + rightRotIndex);
                        }
                    }

                }

                //Statement 6
                else
                {
                    Debug.Log("Triggered CheckForInvalidRightIndexValue else (6th if statement)");
                }
            }

    private void StopLeftStartRight()
            {
                leftRotIndex++;
                currentNumRot8++;
                rotateToLeft = false;
                updateTargetRotVal = true;
            }

    private void StartLeftStopRight()
            {
                rightRotIndex++;
                currentNumRot8++;
                rotateToLeft = true;
                updateTargetRotVal = true;
            }

    private void LeftRotationsComplete()
            {
                //This method is different from StopLeftStartRight/StartLeftStopRight
                //Since Rotations are complete, No need to ++indexes or currentNumRot
                rotateToLeft = false;
                updateTargetRotVal = true;
            }

    private void RightRotationsComplete()
            {
                //This method is different from StopLeftStartRight/StartLeftStopRight
                //Since Rotations are complete, No need to ++indexes or currentNumRot
                rotateToLeft = true;
                updateTargetRotVal = true;
            }

    private void RestartRotateCounterAndTurnOff()
            {
                timeBetRotCycles = Random.Range(minTimeBetRot, maxTimeBetRot);
                countDownOn = false;
            }

    private void StartLeftTrueorFalse()
            {
                if (startLeft)
                {
                    rotateToLeft = true;
                }
                else if (!startLeft)
                {
                    rotateToLeft = false;
                }
            }

    private void ActivateTimer()
            {
                timeBetRotCycles -= Time.deltaTime;
            }

    /*
    public void OnHitDestroyLeftLaser()
    {
        Destroy(gameObject);
    }
    */
}
