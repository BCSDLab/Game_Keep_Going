using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public int ResourceId { get; set; }
    NetworkManager _network;

    // Start is called before the first frame update
    void Start()
    {
        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }
    /*
    // Update is called once per frame
    void Update()
    {
        C_Resource resourcePacket = new C_Resource();
        resourcePacket.posX = transform.position.x;
        resourcePacket.posY = transform.position.y;
        resourcePacket.posZ = transform.position.z;
        _network.Send(resourcePacket.Write());
    }
    */
}
