using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerPlayerAction : MonoBehaviour
{
    
    /// <summary>
    /// When player pick item, Send item information
    /// </summary>
    /// <param name="heldItem"></param>
    /// <param name="_type"></param>
    public void PickUpDownResource(GameObject heldItem, int _type)
    {
        C_PlayerStatus statusPacket = new C_PlayerStatus();
        statusPacket.posX = transform.position.x;
        statusPacket.posY = transform.position.y;
        statusPacket.posZ = transform.position.z;
        statusPacket.rotateY = transform.rotation.eulerAngles.y;
        statusPacket.status = _type;
        statusPacket.itemIdx = ResourceManager.Instance.GetResourceIdx(heldItem);
        //networkManager.Send(statusPacket.Write());
    }

    void Aiming()
    {

    }

}
