using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBrake : MonoBehaviour
{
    [SerializeField]
    private bool woodPut = false; // ������ ���� ��(2������ ����?)�� true
    //private bool brakeTime = false; // ���ߴ� �ð��� 5�ʵ��� true

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

    public bool getWoodPut()
	{
        return woodPut;
	}

    IEnumerator BrakeTime()
	{
        yield return null;

        Debug.Log("���߱� ����");
        woodPut = false;
        yield return new WaitForSeconds(5f);
        Debug.Log("���߱� ��");
    }
}
