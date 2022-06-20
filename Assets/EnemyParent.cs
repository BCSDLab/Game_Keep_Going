using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParent : MonoBehaviour
{
    NetworkManager networkManager;
    // Start is called before the first frame update
    void Start()
    {

        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    private void SendEnemyList()
    {
        Transform[] transforms = transform.GetComponentsInChildren<Transform>();
        for (int i = 1; i < transforms.Length; i++)
        {
            C_EnemyMove enemyMovePacket = new C_EnemyMove();
            enemyMovePacket.enemyIdx = i;
            enemyMovePacket.posX = transforms[i].position.x;
            enemyMovePacket.posY = transforms[i].position.y;
            enemyMovePacket.posZ = transforms[i].position.z;
            networkManager.Send(enemyMovePacket.Write());
            Debug.Log(transforms[i].position);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(networkManager)
            SendEnemyList();

    }
}
