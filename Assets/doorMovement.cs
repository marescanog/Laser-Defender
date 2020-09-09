using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorMovement : MonoBehaviour
{
    readonly Vector3 openedDoorPos = new Vector3(0, 0.8f,0);
    Vector3 doorState;
    Vector3 doorOpen;
    Vector3 doorClosed;
    [SerializeField] float doorSpeed = 1;

    bool isDoorActivated = false;
    bool isDoorOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        doorClosed = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDoorActivated)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, doorState, doorSpeed * Time.deltaTime);
            if (transform.localPosition == doorState)
            {
                isDoorActivated = false;
            }
        }
        doorOpen = transform.localPosition + openedDoorPos;
        doorClosed = transform.localPosition - openedDoorPos;
    }

    public void GoUP()
    {
        if(!isDoorActivated & !isDoorOpen)
        {
            doorState = doorOpen;
            isDoorActivated = true;
            isDoorOpen = true;
        }
    }

    public void GoDown()
    {
        if (!isDoorActivated & isDoorOpen)
        {
            doorState = doorClosed;
            isDoorActivated = true;
            isDoorOpen = false;
        }
    }

}
