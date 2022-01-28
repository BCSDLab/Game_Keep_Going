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
        Vector3 createPosition = transform.position;
        createPosition.y = transform.position.y + 0.8f;
        Instantiate(railResource, createPosition, transform.rotation);
    }
}
