using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : Player
{
	NetworkManager networkManager;

    Camera viewCamera;
    public float moveSpeed = 5.0f;
    PlayerController controller;
    Rigidbody myRigidbody;
    private float dashForce = 10f;
    [SerializeField]
    GunController gunController;

    void Start()
    {
        controller = GetComponent<PlayerController>();
        myRigidbody = GetComponent<Rigidbody>();
        viewCamera = Camera.main;
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        gunController = GetComponent<GunController>();
	}

    void Update()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        myRigidbody.MovePosition(myRigidbody.position + moveVelocity * Time.deltaTime);

        Plane groundPlane = new Plane(Vector3.up, -1.6f);
        float rayDistance;
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            point = new Vector3(point.x, point.y, point.z);
            controller.LookAt(point);
        }
        if (Input.GetKeyDown("f"))
        {
            Debug.Log("Dash");
            dash(moveInput);
        }
        if (Input.GetKeyDown("g"))
        {
            gunController.Shoot();
        }

        C_Move movePacket = new C_Move();
        movePacket.posX = transform.position.x;
        movePacket.posY = transform.position.y;
        movePacket.posZ = transform.position.z;
        movePacket.rotateY = transform.rotation.eulerAngles.y;
        networkManager.Send(movePacket.Write());
    }

    void dash(Vector3 moveInput)
    {
        myRigidbody.AddForce(moveInput * dashForce, ForceMode.Impulse);
    }
}
