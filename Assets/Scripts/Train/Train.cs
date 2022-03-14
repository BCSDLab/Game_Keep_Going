using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    NetworkManager _network;
    public int TrainId { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

    }

    // Update is called once per frame
    void Update()
    {
        C_TrainMove movePacket = new C_TrainMove();
        movePacket.posX = transform.position.x;
        movePacket.posY = transform.position.y;
        movePacket.posZ = transform.position.z;
        movePacket.rotateY = transform.rotation.y;
        _network.Send(movePacket.Write());

    }
}
