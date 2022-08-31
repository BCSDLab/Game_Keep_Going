using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField]
    private GameObject train;
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private bool isTargetTrain = true;

    public void ChangeTargetToTrain()
    {
        isTargetTrain = true;
    }
    
    public void ChangeTargetToPlayer()
    {
        isTargetTrain = false;
    }

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        train = GameObject.Find("Train");
        player = GameObject.Find("player");

        train = train.transform.GetChild(0).gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if(isTargetTrain)
            transform.position = new Vector3 (train.transform.position.x - 5.0f, transform.position.y, 0.0f);
        else
            transform.position = new Vector3(player.transform.position.x -14.0f, transform.position.y, player.transform.position.z-14.0f);
    }
}
