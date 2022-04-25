using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Vector3 velocity;
    Rigidbody myRigidbody;
    bool isOnSnow = false;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();   
    }


    public void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }
    // Update is called once per frame
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void FixedUpdate()
    {
        // 눈에 있고 없고의 이동속도 차이
        if (isOnSnow)
        {
            myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);
        }
        else
        {
            myRigidbody.MovePosition(myRigidbody.position + velocity * 0.8f * Time.fixedDeltaTime);
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Snow")
        {
            isOnSnow = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Snow")
        {
            isOnSnow = false;
        }
    }
}
