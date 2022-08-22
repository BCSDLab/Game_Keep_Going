using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainConversion : MonoBehaviour
{
    [SerializeField]
    private int rockNum; // 현재 기차의 rock_stack 수
    [SerializeField]
    private int woodNum; // 현재 기차의 wood_stack 수

    private int putRockNum; // 변환 모듈에 넣은 rock_stack 수
    private int putWoodNum; // 변환 모듈에 넣은 wood_stack 수

    [SerializeField]
    private bool rockPut; // train_conversionmodule에 rock_stack을 넣으면 true
    [SerializeField]
    private bool woodPut; // train_conversionmodule에 wood_stack를 넣으면 true
    [SerializeField]
    private bool canBlockPut = false;

    // Start is called before the first frame update
    void Start()
    {
        rockNum = GameObject.Find("train_railmakingmodule").GetComponent<TrainRailMaking>().rockNum;
        woodNum = GameObject.Find("train_railmakingmodule").GetComponent<TrainRailMaking>().woodNum;
    }

    // Update is called once per frame
    void Update()
    {
        Conversion();
    }

    public bool getCanBlockPut()
	{
        return canBlockPut;
	}

    public void setCanBlockPut(bool setBool)
	{
        canBlockPut = setBool;
	}

    public bool getWoodPut()
	{
        return woodPut;
	}

    public void setWoodPut(bool setBool)
	{
        woodPut = setBool;
	}

    public bool getRockPut()
	{
        return rockPut;
	}

    public void setRockPut(bool setBool)
	{
        rockPut = setBool;
	}

    void Conversion()
	{
		if (rockPut)
		{
            woodNum += putRockNum;
            rockPut = false;
            Debug.Log("나무 블럭 수: " + woodNum + " 돌 블럭 수: " + rockNum);
		}

		if (woodPut)
		{
            rockNum += putWoodNum;
            woodPut = false;
            Debug.Log("나무 블럭 수: " + woodNum + " 돌 블럭 수: " + rockNum);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PickUpPutDown>().GetHoldItem() != null &&
            (other.GetComponent<PickUpPutDown>().GetHoldItem().CompareTag("WoodStack") || other.GetComponent<PickUpPutDown>().GetHoldItem().CompareTag("RockStack")))
        {
			if (other.GetComponent<PickUpPutDown>().GetHoldItem().CompareTag("WoodStack"))
			{
                putWoodNum = other.GetComponent<PickUpPutDown>().GetHoldItem().GetComponent<WoodStack>().GetInt();
            }
            else if (other.GetComponent<PickUpPutDown>().GetHoldItem().CompareTag("RockStack"))
			{
                putRockNum = other.GetComponent<PickUpPutDown>().GetHoldItem().GetComponent<RockStack>().GetInt();
            }           

            canBlockPut = true;
            Debug.Log("변환 모듈에서 블럭 인식");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PickUpPutDown>().GetHoldItem() != null &&
            (other.GetComponent<PickUpPutDown>().GetHoldItem().CompareTag("WoodStack") || other.GetComponent<PickUpPutDown>().GetHoldItem().CompareTag("RockStack")))
        {
            canBlockPut = false;
            Debug.Log("변환 모듈에서 블럭과 멀어짐");
        }
    }
}
