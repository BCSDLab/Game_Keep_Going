using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainInvisible : MonoBehaviour
{
    private GameObject[] train;

    [SerializeField]
    private bool rockPut = false;
    [SerializeField]
    private bool canRockPut = false;

    private int rockNum;


    void Start()
    {
        train = GameObject.FindGameObjectsWithTag("Train");
    }

    
    void Update()
    {
        if (rockPut)
            StartCoroutine(Invisible());
    }

    public bool GetCanRockPut()
    {
        return canRockPut;
    }

    public bool GetRockPut()
    {
        return rockPut;
    }

    public void SetRockPut(bool setBool)
    {
        rockPut = setBool;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PickUpPutDown>().GetHoldItem() != null && other.GetComponent<PickUpPutDown>().GetHoldItem().CompareTag("RockStack"))
        {
            rockNum = other.GetComponent<PickUpPutDown>().GetHoldItem().GetComponent<RockStack>().getInt();
            canRockPut = true;
            Debug.Log("platform모듈에서 돌블럭 인식");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PickUpPutDown>().GetHoldItem() != null && other.GetComponent<PickUpPutDown>().GetHoldItem().CompareTag("RockStack"))
        {
            canRockPut = false;
            Debug.Log("platform모듈에서 돌블럭과 멀어짐");
        }
    }

    IEnumerator Invisible()
	{
        Debug.Log("platform모듈 시작");
        rockPut = false;
        SetCollider();
        yield return new WaitForSeconds(5f);
        SetCollider();
        Debug.Log("platform모듈 끝");
    }


    private void SetCollider()
	{
        for(int i = 0; i < train.Length; i++)
		{
            train[i].GetComponent<BoxCollider>().enabled = !train[i].GetComponent<BoxCollider>().enabled;
        }
	}

 //   private void SetColor()
	//{
 //       for(int i = 0; i < train.Length; i++)
	//	{
 //           Debug.Log(train[i].transform.GetChild(0));
 //           Color originalColor = train[i].transform.GetChild(0).GetComponent<MeshRenderer>().materials[0].color;
 //           train[i].GetComponent<MeshRenderer>().materials[0].color = new Color(0, 0, 0);
	//	}
	//}
}
