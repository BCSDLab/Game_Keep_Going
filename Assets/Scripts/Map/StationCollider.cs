using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StationCollider : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    /// <summary>
    /// �ش� �ڷ�κ��� Store������ �Űܳ��ƾ߸� ��.
    /// </summary>
    private Transform returnPosition; // �÷��̾ ������ ���� �Ŀ� ���ư��� �ϴ� ��ġ.

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
        print("����!");
        print(other.gameObject.name);
        if(other.gameObject.name == "train_mainmodule")
        {
            TeleportToShop();
            Destroy(this);
        }
    }

    public void TeleportToShop()
    {
        // ���� ������ �ҷ����� �̸� ������� ���̾ƿ� ����.
        // �÷��̾� ��ġ �ű�� 
        returnPosition = player.transform;
        // ��� �ִ� ���� �����Ͱ� �Ű������� �����
        player.transform.position = playerSpawnPoint.position;
        // ī�޶� ��ġ �ű��

        // ���澾 �۾� ��Ź�� �κ�.
        player.GetComponent<PickUpPutDown>().StoreLoadUnload();
        GameObject train2 = GameObject.Find("Train2");
        train2.GetComponent<LoadJsonData>().loadInStore = true;
    }
}
