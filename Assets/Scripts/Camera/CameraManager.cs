using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Train;

    private void Start()
    {
        Train = Train.transform.GetChild(0).gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3 (Train.transform.position.x - 5.0f, transform.position.y, transform.position.z);
    }
}
