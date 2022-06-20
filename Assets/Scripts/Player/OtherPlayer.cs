using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayer : Player
{

    NetworkManager networkManager;
    Rigidbody playerRigid;
    //private float dirH;
    //private float dirV;
    //float moveSpeed = 5f;
    Vector3 targetPos;

    void Start()
    {
        playerRigid = GetComponent<Rigidbody>();
        transform.position = new Vector3(0, 1.6f, 0);
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }
    /*
    public void move(float dirHor, float dirVer, float rotateY)
    {
        dirH = dirHor;
        dirV = dirVer;
        transform.rotation = Quaternion.Euler(0, rotateY, 0);
    }
    */
    public void SetTargetPos(Vector3 vec3)
    {
        targetPos = vec3;
    }

    private void Update()
    {
        //Vector3 moveVelocity = new Vector3(dirH, 0, dirV) * moveSpeed;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, 4f * Time.deltaTime);
        //playerRigid.MovePosition(playerRigid.position + moveVelocity * Time.deltaTime);
    }
}
