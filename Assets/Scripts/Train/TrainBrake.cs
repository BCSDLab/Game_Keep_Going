using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBrake : MonoBehaviour
{
    [SerializeField]
    private bool woodPut = false; // 나무를 넣은 때(2프레임 정도?)만 true
    //private bool brakeTime = false; // 멈추는 시간인 5초동안 true

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

        Debug.Log("멈추기 시작");
        woodPut = false;
        yield return new WaitForSeconds(5f);
        Debug.Log("멈추기 끝");
    }
}
