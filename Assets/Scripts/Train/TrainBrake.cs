using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBrake : MonoBehaviour
{
    [SerializeField]
    private bool woodPut = false; // 나무를 넣은 때(2프레임 정도?)만 true
    [SerializeField]
    private bool canWoodPut = false;

    private int woodNum; // 브레이크 모듈에 넣은 나무블럭 수


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (woodPut)
            StartCoroutine(BrakeTime());
    }

    public bool GetCanWoodPut()
	{
        return canWoodPut;
	}

	// 플레이어가 나무블럭을 든 채 브레이크 모듈과 충돌했을 때 woodput을 할 수 있게 한다.
	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player") && other.GetComponent<PickUpPutDown>().GetHoldItem() != null && other.GetComponent<PickUpPutDown>().GetHoldItem().CompareTag("WoodStack"))
		{
            woodNum = other.GetComponent<PickUpPutDown>().GetHoldItem().GetComponent<WoodStack>().getInt();
            canWoodPut = true;
            Debug.Log("브레이크 모듈에서 나무블럭 인식");
        }
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Player") && other.GetComponent<PickUpPutDown>().GetHoldItem() != null && other.GetComponent<PickUpPutDown>().GetHoldItem().CompareTag("WoodStack"))
		{
            canWoodPut = false;
            Debug.Log("브레이크 모듈에서 나무블럭과 멀어짐");
		}
	}

	public bool getWoodPut()
	{
        return woodPut;
	}

    public void setWoodPut(bool setBool)
	{
        woodPut = setBool;
	}

    IEnumerator BrakeTime()
	{
        yield return null;
        Debug.Log("멈추기 시작");
        woodPut = false;
        yield return new WaitForSeconds(3f*woodNum);
        Debug.Log("멈추기 끝");
    }
}
