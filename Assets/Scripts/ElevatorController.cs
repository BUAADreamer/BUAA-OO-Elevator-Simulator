using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    private float speed = 1.2f;
    public int to;
    public int floor;
    private string building;
    private float delta = 0.00f;
    private float lowHeight = 0;
    private float bigHeight = 29;
    private int floorheight = 3;
    private int nextTo = 1;
    private float openandclosetime = 0.4f;
    private float eps = 0.01f;
    public int personPosId = 0;
    public int maxNum = 6;
    public PassengerController mainReq;
    // Start is called before the first frame update
    public List<PassengerController> passengers = new List<PassengerController>();
    ElevatorInput elevatorInput;
    private bool open = false;
    public int cnt = 0;
    void Start()
    {
        lowHeight += delta;
        bigHeight += delta;
        personPosId = 1;
        maxNum = 6;
        to = 1;
        floor = 1;
        building = "A";
        elevatorInput = GameObject.Find("ElevatorInput").GetComponent<ElevatorInput>();
        cnt = 0;
        //speed = 10;
        //openandclosetime = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        updateFloor();
        if (to != 0)
        {
            judgeNeedStop();
        }
        if(to!=0) decideTo();
        else
        {
            if(open)
                peopleInOrOut();
        }
        pos += speed * Time.deltaTime * Vector3.up*to;
        transform.position = pos;
        //Debug.Log(floor);
    }

    void decideTo()
    {
        if (mainReq == null)
        {
            //ALS Algorithm
            if (passengers.Count == 0)
            {
                if (elevatorInput.Passengers.Count > 0)
                {
                    PassengerController passenger = elevatorInput.Passengers[0];
                    mainReq = passenger;
                    //elevatorInput.Passengers.RemoveAt(0);
                }
            }
            else
            {
                PassengerController passenger = passengers[0];
                mainReq = passenger;
            }
        }
        if (mainReq != null)
        {
            if (!mainReq.onElevator)
            {
                if (mainReq.Fromfloor > floor) to = 1;
                else if (mainReq.Fromfloor < floor) to = -1;
                else
                {
                    to = 0;
                    Invoke("changeTo", openandclosetime);
                    open = true;
                }
            }
            else
            {
                if (mainReq.Tofloor > floor) to = 1;
                else if (mainReq.Tofloor < floor) to = -1;
                else
                {
                    to = 0;
                    Invoke("changeTo", openandclosetime);
                    open = true;
                }
            }

        }
        /*
        Vector3 pos = transform.position;
        if (to == 1 && pos.y >= bigHeight)
        {
            to = -1;
        }
        if (to == 1)
        {
            if (pos.y >= lowHeight + (floor) * floorheight)
            {
                floor++;
                nextTo = to;
                to = 0;
                Invoke("changeTo", openandclosetime);
            }
        }
        if (to == -1)
        {
            if (pos.y <= lowHeight + (floor - 2) * floorheight + eps)
            {
                floor--;
                nextTo = to;
                to = 0;
                Invoke("changeTo", openandclosetime);
            }
        }
        */
    }

    void peopleInOrOut()
    {
        int n = passengers.Count - 1;
        for(int i = n; i >= 0; i--)
        {
            if (passengers[i] != null)
            {
                int flag = passengers[i].outorin();
                if (passengers[i] == mainReq) mainReq = null;
                if (flag < 0)
                {
                    cnt++;
                    passengers.RemoveAt(i);
                    if(cnt==elevatorInput.reqStrList.Count)
                    {

                    }
                }
            }
        }
        n = elevatorInput.Passengers.Count - 1;
        for (int i = n; i >= 0; i--)
        {
            int flag = elevatorInput.Passengers[i].outorin();
            if (flag > 0)
            {
                passengers.Add(elevatorInput.Passengers[i]);
                elevatorInput.Passengers.RemoveAt(i);
            }
        }
    }

    void updateFloor()
    {
        Vector3 pos = transform.position;
        if (pos.y >= (lowHeight + (floor) * floorheight-eps) && pos.y <= (lowHeight + (floor) * floorheight + eps))
        {
            floor++;
        }
        else if (pos.y <= (lowHeight + (floor - 2) * floorheight + eps) && pos.y >= (lowHeight + (floor - 2) * floorheight - eps))
        {
            floor--;
        }
    }

    void judgeNeedStop()
    {
        int n = elevatorInput.Passengers.Count;
        int flag = 0;
        for (int i = 0; i < n ; i++)
        {
            if(elevatorInput.Passengers[i].Fromfloor == floor && (elevatorInput.Passengers[i].Tofloor-floor)*to==1)
            {
                to = 0;
                flag = 1;
            }
        }
        foreach (PassengerController passenger in passengers)
        {
            if (passenger.Tofloor == floor)
            {
                to = 0;
                flag = 1;
            }
        }
        if (flag == 1)
        {
            Invoke("changeTo", openandclosetime);
            open = true;
        }
    }

    void changeTo()
    {
        decideTo();
        open = false;
    }
    
}
