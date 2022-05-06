using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PassengerController : MonoBehaviour
{
    private Vector3 firstFloorPos = new Vector3(5, 0, -9);
    private int floorheight = 3;
    private int fromfloor;
    private bool isEnd = false;
    public int Fromfloor
    {
        get
        {
            return this.fromfloor;
        }
        set
        {
            this.fromfloor = value;
        }
    }
    public int Tofloor
    {
        get
        {
            return this.tofloor;
        }
        set
        {
            this.tofloor = value;
        }
    }
    private int tofloor;
    public string frombuilding;
    public string tobuilding;
    public int personId=-1;
    public GameObject elevator;
    public ElevatorController elevatorController;
    public bool onElevator;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (onElevator)
        {
            Vector3 pos = transform.position;
            pos.y = elevatorController.transform.position.y;
            transform.position = pos;
        }
    }

    public int outorin()
    {
        if (personId != -1)
        {
            Vector3 pos = transform.position;
            //Debug.Log(elevatorController);
            if (elevatorController.floor==fromfloor && elevatorController.to == 0 && !onElevator && elevatorController.passengers.Count<=elevatorController.maxNum)
            {
                onElevator = true;
                pos += Vector3.right * (2+ elevatorController.personPosId);
                elevatorController.personPosId=(elevatorController.personPosId+1)%6+1;
                transform.position = pos;
                return 1;
            }
            else if(elevatorController.floor == tofloor && elevatorController.to == 0 && onElevator)
            {
                onElevator = false;
                Vector3 endpos = firstFloorPos;
                endpos += Vector3.up * (tofloor - 1) * floorheight;
                transform.position = endpos;
                personOut();
                return -1;
            }
        }
        return 0;
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
        elevatorController = GameObject.Find("Elevator").GetComponent<ElevatorController>();
    }

    private void personOut()
    {
        Destroy(gameObject);
    }
}
