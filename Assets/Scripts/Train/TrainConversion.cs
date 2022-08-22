using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainConversion : MonoBehaviour
{
    [SerializeField]
    private int rockNum; // ���� ������ rock_stack ��
    [SerializeField]
    private int woodNum; // ���� ������ wood_stack ��

    private int putRockNum; // ��ȯ ��⿡ ���� rock_stack ��
    private int putWoodNum; // ��ȯ ��⿡ ���� wood_stack ��

    [SerializeField]
    private bool rockPut; // train_conversionmodule�� rock_stack�� ������ true
    [SerializeField]
    private bool woodPut; // train_conversionmodule�� wood_stack�� ������ true
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
            Debug.Log("���� �� ��: " + woodNum + " �� �� ��: " + rockNum);
		}

		if (woodPut)
		{
            rockNum += putWoodNum;
            woodPut = false;
            Debug.Log("���� �� ��: " + woodNum + " �� �� ��: " + rockNum);
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
            Debug.Log("��ȯ ��⿡�� �� �ν�");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PickUpPutDown>().GetHoldItem() != null &&
            (other.GetComponent<PickUpPutDown>().GetHoldItem().CompareTag("WoodStack") || other.GetComponent<PickUpPutDown>().GetHoldItem().CompareTag("RockStack")))
        {
            canBlockPut = false;
            Debug.Log("��ȯ ��⿡�� ���� �־���");
        }
    }
}
