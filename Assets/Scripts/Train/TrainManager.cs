using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainManager
{
    //Dictionary<int, Train> dTrains = new Dictionary<int, Train>();

    public static TrainManager Instance { get; } = new TrainManager();
    bool isCreated = false;
    Train train;
    [SerializeField]
    ArrayList trainList = new ArrayList();
    public void AddTrains()
    {
        GameObject trainParant = GameObject.Find("Train");
        for (int i = 0; i < trainParant.transform.childCount; i++)
        {
            trainList.Add(trainParant.transform.GetChild(i).gameObject);
        }
    }
    
    public void Add()
    {
        if (!isCreated)
        {
            Object obj = Resources.Load("Prefabs/train_mainmodule");
            GameObject go = Object.Instantiate(obj) as GameObject;
            train = go.AddComponent<Train>();
            go.AddComponent<TrainMainMoving>();
            train.transform.position = new Vector3(5, 1.6f, 5);
            isCreated = true;
        }
        
    }
    

    //public void CreateTrain(S_BroadcastTrainMove packet)
    //{
    //    OtherPlayer player = go.AddComponent<OtherPlayer>();
    //    player.PlayerId = p.playerId;
    //    player.transform.position = new Vector3(p.posX, 1.6f, p.posZ);
    //    _players.Add(p.playerId, player);
    //}

    public void Move(S_BroadcastTrainMove packet)
    {
        train.transform.position = new Vector3(packet.posX, 1.6f, packet.posZ);
        train.transform.rotation = Quaternion.Euler(0, packet.rotateY * 180, 0);
    }

}
