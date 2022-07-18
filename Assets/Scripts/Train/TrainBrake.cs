using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBrake : MonoBehaviour
{
    [SerializeField]
    private bool woodPut = false; // ������ ���� ��(2������ ����?)�� true
    [SerializeField]
    private bool canWoodPut = false;

    private int woodNum; // �극��ũ ��⿡ ���� ������ ��


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

	// �÷��̾ �������� �� ä �극��ũ ���� �浹���� �� woodput�� �� �� �ְ� �Ѵ�.
	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player") && other.GetComponent<PickUpPutDown>().GetHoldItem() != null && other.GetComponent<PickUpPutDown>().GetHoldItem().CompareTag("WoodStack"))
		{
            woodNum = other.GetComponent<PickUpPutDown>().GetHoldItem().GetComponent<WoodStack>().getInt();
            canWoodPut = true;
            Debug.Log("�극��ũ ��⿡�� ������ �ν�");
        }
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Player") && other.GetComponent<PickUpPutDown>().GetHoldItem() != null && other.GetComponent<PickUpPutDown>().GetHoldItem().CompareTag("WoodStack"))
		{
            canWoodPut = false;
            Debug.Log("�극��ũ ��⿡�� �������� �־���");
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
        Debug.Log("���߱� ����");
        woodPut = false;
        yield return new WaitForSeconds(3f*woodNum);
        Debug.Log("���߱� ��");
    }
}
