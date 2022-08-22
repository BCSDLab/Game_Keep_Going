using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainRailSaving : MonoBehaviour
{
    int rail_Num;
    GameObject railResource;
    void Start()
    {
        railResource = Resources.Load("Prefabs/rail") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRails()
    {
        if (transform.childCount == 1)
        {
            GameObject newRail = Instantiate(railResource, transform);
            newRail.transform.position = newRail.transform.position + new Vector3(0f, 0.8f, 0f);

        }
        else
        {
            Transform rail = transform.GetChild(1);
            GameObject newRail = Instantiate(railResource, rail);
            newRail.transform.position = rail.position + new Vector3(0, 0.3f, 0) * rail.GetComponent<Rail>().GetInt();
        }
    }
}
