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
        player = GameObject.Find("player_test(Clone)");
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
            player = GameObject.Find("player_test(Clone)");
        transform.position = player.transform.position + new Vector3(0, 14f, -8f);
    }
}
