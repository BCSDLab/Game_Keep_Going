using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDownBlock : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;
    private void Awake()
    {
        Player = this.transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Block")
        {
            Player.GetComponent<PickUpPutDown>().SetPutDownPosition(other.transform.position);
        }
    }
}
