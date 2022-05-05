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
    private float eps = 0.00001f;
    public int personPosId = 0;
    public int maxNum = 6;
    // Start is called before the first frame update
    void Start()
    {
        lowHeight += delta;
        bigHeight += delta;
        personPosId = 1;
        maxNum = 6;
        to = 1;
        floor = 1;
        building = "A";
        //speed = 10;
        //openandclosetime = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        //if (to==-1 && pos.y<= lowHeight)
        //{
         //   to = 1;
       // }
        if(to==1 && pos.y>= bigHeight)
        {
            to = -1;
        }
        if (to == 1)
        {
            if (pos.y >= lowHeight+(floor) * floorheight)
            {
                floor++;
                nextTo = to;
                to = 0;
                Invoke("changeTo", openandclosetime);
            }
        }
        if (to == -1)
        {
            if (pos.y <= lowHeight + (floor-2) * floorheight+ eps)
            {
                floor--;
                nextTo = to;
                to = 0;
                Invoke("changeTo", openandclosetime);
            }
        }
        pos += speed * Time.deltaTime * Vector3.up*to;
        transform.position = pos;
    }

    void changeTo()
    {
        to = nextTo;
        if (floor == 10)
        {
            to = -1;
        }
        if (floor == 1)
        {
            to = 1;
        }
    }
}
