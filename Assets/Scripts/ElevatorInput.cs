using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ElevatorInput : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject passengerPrefab;
    private List<string> reqStrList= new List<string>();
    private int reqIndex=0;
    void Start()
    {
        StreamReader sr = new StreamReader("Assets/Scripts/data.txt");
        string requestStr;
        DateTime beginTime = DateTime.Now;
        while ((requestStr = sr.ReadLine()) != null)
        {
            requestStr = requestStr.Trim();
            if (requestStr==null) continue;
            double reqTime = Convert.ToDouble(requestStr.Trim('[').Split(']')[0]);
            if (requestStr.Contains("]"))
            {
                reqStrList.Add(requestStr.Split(']')[1]);
                Invoke("addPerson", (float)reqTime);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void addPerson()
    {
        string reqStr = reqStrList[reqIndex];
        Debug.Log(reqStr);
        reqIndex++;
        GameObject person = Instantiate(passengerPrefab);
        PassengerController passenger = person.GetComponent<PassengerController>();
        passenger.setInitData(reqStr);
    }
}
