using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorMovement : MonoBehaviour
{
    Vector2 origDoorPos = new Vector2(0, 0);
    Vector2 openedDoorPos = new Vector2(0, 0.8f);
    Vector2 doorState;
    [SerializeField] float doorSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        doorState = origDoorPos;

    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector2.MoveTowards(transform.position, doorState, doorSpeed * Time.deltaTime);
    }

    public void GoUP()
    {
        doorState = openedDoorPos;
    }

    public void GoDown()
    {
        doorState = origDoorPos;
    }

}
