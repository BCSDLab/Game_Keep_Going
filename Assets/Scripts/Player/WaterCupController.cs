using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCupController : MonoBehaviour
{
    public bool isFill;
    public bool isHold;
    bool isColl;
    bool isTrain;
    GameObject player;
    GameObject collObject;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Train")
        {
            collObject = collider.gameObject;
            isColl = true;
            isTrain = true;
        }
        else if (collider.gameObject.tag == "Water")
        {
            collObject = collider.gameObject;
            isColl = true;
            isTrain = false;
        }
        else
        {
            return;
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        isColl = false;
        collObject = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        isColl = false;
        isFill = false;
        isHold = false;
    }

    public void Holding()
    {
        isHold = true;
    }

    public void PutDown()
    {
        isHold = false;
    }

    void OnEvent()
    {
        if (isHold && isColl)
        {
            if(isTrain)
            {
                PourWater();
            }
            else
            {
                FillWater();
            }
        }
    }

    void FillWater()
    {
        isFill = true;
    }
    void PourWater()
    {
        isFill = false;
        collObject.GetComponent<TrainColorTest>().CoolingTrain();
    }
    // Update is called once per frame
    void Update()
    {
        if (isHold)
        {
            transform.SetParent(player.transform.GetChild(1));
            transform.localPosition = Vector3.zero;
        }
    }
}
