using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    public int railStackNum = 1; // 쌓여있는 레일의 수
	private PickUpPutDown player;
	//[SerializeField]
	//private Rail theRail;

    void Start()
    {
        
    }

    void Update()
    {
		CountRailNum();
    }

	private void CountRailNum()
	{
		railStackNum = this.transform.childCount;
	}

	public int getInt()
	{
		return railStackNum;
	}
}
