using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainManager
{
    //Dictionary<int, Train> dTrains = new Dictionary<int, Train>();

    public static TrainManager Instance { get; } = new TrainManager();

    Train train;
    public void Add()
    {
        Object obj = Resources.Load("Prefabs/train_mainmodule");
        GameObject go = Object.Instantiate(obj) as GameObject;
        train = go.AddComponent<Train>();
        go.AddComponent<TrainMainMoving>();
        train.transform.position = new Vector3(5, 1.6f, 5);
        
    }

    public void Move(S_BroadcastTrainMove packet)
    {
        train.transform.position = new Vector3(packet.posX, 1.6f, packet.posZ);
        train.transform.rotation = Quaternion.Euler(0, packet.rotateY * 180, 0);
        Debug.Log(train.transform);
    }
}
