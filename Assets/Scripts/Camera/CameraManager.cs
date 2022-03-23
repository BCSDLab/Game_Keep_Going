using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.Find("player_test(Clone)");
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.Find("player_test(Clone)");
            if (playerObj.GetComponents<MyPlayer>() != null)
                player = playerObj;
        }
        transform.position = player.transform.position + new Vector3(0, 14f, -8f);
    }
}
