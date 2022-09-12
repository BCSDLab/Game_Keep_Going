using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StationCollider : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    /// <summary>
    /// 해당 자료부분은 Store쪽으로 옮겨놓아야만 함.
    /// </summary>
    private Transform returnPosition; // 플레이어가 상점이 끝난 후에 돌아가야 하는 위치.

    private Camera camera;

    [SerializeField]
    Transform playerSpawnPoint;
    [SerializeField]
    Transform TrainSpawnPoint;

    private void Awake()
    {
        player = GameObject.Find("player");
        camera = Camera.main;
        playerSpawnPoint = GameObject.Find("PlayerSpawnPoint").transform;
        TrainSpawnPoint = GameObject.Find("TrainSpawnPoint").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        print("만남!");
        print(other.gameObject.name);
        if(other.gameObject.name == "train_mainmodule")
        {
            TeleportToShop();
            Destroy(this);
        }
    }

    public void TeleportToShop()
    {
        // 기차 데이터 불러오고 이를 기반으로 레이아웃 제작.
        // 플레이어 위치 옮기기 
        returnPosition = player.transform;
        // 들고 있는 물건 데이터가 옮겨지도록 만들기
        player.transform.position = playerSpawnPoint.position;
        // 카메라 위치 옮기기

        // 유경씨 작업 부탁한 부분.
        player.GetComponent<PickUpPutDown>().StoreLoadUnload();
        GameObject train2 = GameObject.Find("Train2");
        train2.GetComponent<LoadJsonData>().loadInStore = true;
    }
}
