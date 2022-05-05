using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PassengerController : MonoBehaviour
{
    private Vector3 firstFloorPos = new Vector3(5, 0, -9);
    private int floorheight = 3;
    private int fromfloor;
    private int tofloor;
    private string frombuilding;
    private string tobuilding;
    private int personId=-1;
    public GameObject elevator;
    public ElevatorController elevatorController;
    private bool onElevator;
    // Start is called before the first frame update
    void Start()
    {
        elevatorController = GameObject.Find("Elevator").GetComponent<ElevatorController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (personId != -1)
        {
            Vector3 pos = transform.position;
            if (elevatorController.floor==fromfloor && elevatorController.to == 0 && !onElevator && elevatorController.personPosId<=elevatorController.maxNum)
            {
                onElevator = true;
                pos += Vector3.right * (2+ elevatorController.personPosId);
                elevatorController.personPosId=(elevatorController.personPosId+1)%6+1;
                transform.position = pos;
            }
            else if(elevatorController.floor == tofloor && elevatorController.to == 0 && onElevator)
            {
                onElevator = false;
                Vector3 endpos = firstFloorPos;
                endpos += Vector3.up * (tofloor - 1) * floorheight;
                transform.position = endpos;
                Invoke("personOut", 1);
            }
            if (onElevator)
            {
                pos.y = elevatorController.transform.position.y;
                transform.position = pos;
            }
        }
    }

    public void setInitData(string reqStr)
    {
        string[] args = reqStr.Split('-');
        fromfloor = Convert.ToInt32(args[3]);
        tofloor= Convert.ToInt32(args[6]);
        frombuilding = args[2];
        tobuilding = args[5];
        personId = Convert.ToInt32(args[0]);
        Vector3 pos = firstFloorPos;
        pos += Vector3.up * (fromfloor - 1) * floorheight;
        transform.position = pos;
    }

    private void personOut()
    {
        Destroy(gameObject);
    }
}
