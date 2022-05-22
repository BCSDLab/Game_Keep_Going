using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform weaponHold;
    public Gun startingGun;
    Gun equipedGun;
    NetworkManager networkManager;

    void Start()
    {
        if(startingGun != null)
        {
            EquipGun(startingGun);
        }
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    public void EquipGun(Gun gunToEquip)
    {
        if (equipedGun != null)
        {
            Destroy(equipedGun.gameObject);
        }
        equipedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation);
        equipedGun.transform.parent = weaponHold;
    }
    public void Shoot()
    {
        if(equipedGun != null)
        {
            C_Shot shotPacket = new C_Shot();
            shotPacket.posX = equipedGun.transform.position.x;
            shotPacket.posY = equipedGun.transform.position.y;
            shotPacket.posZ = equipedGun.transform.position.z;
            shotPacket.rotateY = equipedGun.transform.rotation.eulerAngles.y;
            networkManager.Send(shotPacket.Write());
            equipedGun.Shoot();
        }
    }
}
