using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]

public class Player : MonoBehaviour
{
    public int PlayerId { get; set; }
    Rigidbody playerRigid;
    private float dirH;
    private float dirV;
    float moveSpeed = 4f;
    void Start()
    {
        playerRigid = GetComponent<Rigidbody>();

    }
    public void move(float dirHor, float dirVer, float rotateY)
    {
        dirH = dirHor;
        dirV = dirVer;
        transform.rotation = Quaternion.Euler(0, rotateY, 0);
    }

    private void Update()
    {
        Vector3 moveVelocity = new Vector3(dirH, 0, dirV) * moveSpeed;
        playerRigid.MovePosition(playerRigid.position + moveVelocity * Time.deltaTime);

    }
}
