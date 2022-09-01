using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickRail : MonoBehaviour
{
    [SerializeField]
    private GameObject playerTeleportPos;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private CameraManager cameraManager;

    private void Start()
    {
        cameraManager = GameObject.FindObjectOfType<CameraManager>();
        playerTeleportPos = GameObject.Find("PlayerSpawnPoint");
        player = GameObject.Find("player");
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Train")
        {
            MapManager.instance.playerReturnPos = player.transform.position;
            player.transform.SetPositionAndRotation(playerTeleportPos.transform.position , playerTeleportPos.transform.rotation);
            cameraManager.ChangeTargetToPlayer();
        }
    }
}
