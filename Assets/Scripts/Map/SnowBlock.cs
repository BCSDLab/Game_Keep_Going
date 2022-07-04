using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBlock : Block
{
    int snowGenTiming = 0;
    [SerializeField]
    private int currentSnowLevel = 0;
    [SerializeField]
    private Material[] Transparent;
    [SerializeField]
    private Material[] NonTrans;

    bool isPlayerInBlock = false;
    float playerStandTime = 0.0f;
    float snowGradeDownTime = 0.5f;

    public void SetData(int Timing, int snowLevel, int d_x, int d_y)
    {
        snowGenTiming = Timing;
        currentSnowLevel = snowLevel;
        x = d_x;
        y = d_y;
    }

    // Start is called before the first frame update
    void Start()
    {
        SnowBlockUpdate();
    }



    private void FixedUpdate()
    {
        if(snowGenTiming == MapManager.instance.snowTiming)
        {
            if(currentSnowLevel < 5)
            {
                currentSnowLevel++;
                SnowBlockUpdate();
            }
        }
        if (isPlayerInBlock)
        {
            playerStandTime += Time.deltaTime;
            if(playerStandTime > snowGradeDownTime)
            {
                DigSnow();
                playerStandTime = 0.0f;
            }
        }
        else
        {
            playerStandTime = 0.0f;
        }

        
    }

    public void DigSnow()
    {
        if (currentSnowLevel > 0)
        {
            currentSnowLevel--;
            SnowBlockUpdate();
        }
    }

    public void SetSnowLevelTo()
    {

    }
    public void SnowBlockUpdate()
    {
        if (currentSnowLevel == 0)
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().materials = Transparent;
        }
        else
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().materials = NonTrans;
        }
        this.transform.localScale = new Vector3(1, 0.1f * currentSnowLevel, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isPlayerInBlock = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            isPlayerInBlock = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isPlayerInBlock = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            isPlayerInBlock = false;
        }
    }
}
