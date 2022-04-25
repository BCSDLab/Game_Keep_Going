using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager
{
    //Dictionary<int, Train> dTrains = new Dictionary<int, Train>();

    public static BulletManager Instance { get; } = new BulletManager();

    public void Add(S_BroadcastShot packet)
    {
        Debug.Log("Bullet");
        Object obj = Resources.Load("Prefabs/bullet");
        GameObject bullet = Object.Instantiate(obj) as GameObject;
        bullet.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
        bullet.transform.rotation = Quaternion.Euler(0, packet.rotateY, 0);
    }
}
