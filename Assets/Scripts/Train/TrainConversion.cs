using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainConversion : MonoBehaviour
{
    [SerializeField]
    private int stoneNum;
    [SerializeField]
    private int woodNum;

    [SerializeField]
    private bool stonePut; // train_conversionmodule에 stone을 넣으면 true
    [SerializeField]
    private bool woodPut; // train_conversionmodule에 wood를 넣으면 true

    // Start is called before the first frame update
    void Start()
    {
        stoneNum = GameObject.Find("train_railmakingmodule").GetComponent<TrainRailMaking>().stoneNum;
        woodNum = GameObject.Find("train_railmakingmodule").GetComponent<TrainRailMaking>().woodNum;
    }

    // Update is called once per frame
    void Update()
    {
        Conversion();
    }

    void Conversion()
	{
		if (stonePut)
		{
            woodNum++;
            stonePut = false;
		}

		if (woodPut)
		{
            stoneNum++;
            woodPut = false;
		}
	}
}
