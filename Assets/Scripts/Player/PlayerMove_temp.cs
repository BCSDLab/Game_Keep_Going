using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MyPlayer스크립트 오류나서 플레이어 움직임 기능만 둔 임시 스크립트

public class PlayerMove_temp : MonoBehaviour
{
    NetworkManager networkManager;

    Camera viewCamera;
    public float moveSpeed = 5.0f;
    PlayerController controller;
    Rigidbody myRigidbody;
    private float dashForce = 10f;
    //[SerializeField]
    //GunController gunController;
    float dirH;
    float dirV;

    void Start()
    {
        gameObject.AddComponent<PickUpPutDown>();
        //        gameObject.AddComponent<PlayerMining>();
        controller = GetComponent<PlayerController>();
        myRigidbody = GetComponent<Rigidbody>();
        viewCamera = Camera.main;
        //networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        //gunController = GetComponent<GunController>();
    }

    void Update()
    {
        Plane groundPlane = new Plane(Vector3.up, -1.6f);
        float rayDistance;
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            point = new Vector3(point.x, point.y, point.z);
            LookAt(point);
        }

        if (Input.GetKeyDown("f"))
        {
            Debug.Log("Dash");
            //dash(moveInput);
        }
        if (Input.GetKeyDown("g"))
        {
            //gunController.Shoot();
        }

        //C_Move movePacket = new C_Move();
        //movePacket.posX = transform.position.x;
        //movePacket.posY = transform.position.y;
        //movePacket.posZ = transform.position.z;
        //movePacket.dirH = dirH;
        //movePacket.dirV = dirV;
        //movePacket.rotateY = transform.rotation.eulerAngles.y;
        //networkManager.Send(movePacket.Write());
    }

    void FixedUpdate()
    {
        dirH = Input.GetAxisRaw("Horizontal");
        dirV = Input.GetAxisRaw("Vertical");
        Vector3 moveInput = new Vector3(dirH, 0, dirV);
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        myRigidbody.MovePosition(myRigidbody.position + moveVelocity * Time.deltaTime);


    }
    public void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }

    void dash(Vector3 moveInput)
    {
        myRigidbody.AddForce(moveInput * dashForce, ForceMode.Impulse);
    }
}
