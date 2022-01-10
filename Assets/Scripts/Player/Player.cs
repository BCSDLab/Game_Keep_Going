using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerMining))]

public class Player : MonoBehaviour
{
    Camera viewCamera;
    public float moveSpeed = 5.0f;
    PlayerController controller;
    Rigidbody myRigidbody;
    public PlayerMining playerMining;

    void Start()
    {
        controller = GetComponent<PlayerController>();
        myRigidbody = GetComponent<Rigidbody>();
        viewCamera = Camera.main;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Resources")
        {
            GameObject resourceObject = collision.gameObject;
            Debug.Log(collision.gameObject.name);
            playerMining.StartMining(resourceObject);
        }
        else
        {
            return;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Resources")
        {
            Debug.Log("stopMining");
            playerMining.StopMining();
        }
    }
    void Update()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        myRigidbody.MovePosition(myRigidbody.position + moveVelocity * Time.deltaTime);

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            point = new Vector3(point.x, point.y, point.z);
            controller.LookAt(point);
        }
    }
}
