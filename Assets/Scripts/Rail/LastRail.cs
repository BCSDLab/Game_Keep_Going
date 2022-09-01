using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ·¹ÀÏ ¼³Ä¡ °¡´É Á¸ Ç¥½Ã
public class LastRail: MonoBehaviour
{
    private PickUpPutDown player;
    private PickUpPutDown otherPlayer;
    private CanSetZone canSetZone;
	public int LastRail_BT; // LastRail에서 충돌했을 때의 BlockType


    void Start()
    {
        player = GameObject.Find("player").transform.GetComponent<PickUpPutDown>();
        canSetZone = this.gameObject.transform.GetChild(0).GetComponent<CanSetZone>();
    }

	/*
	void Update()
	{
		//if (player == null)
		//{
		//	GameObject playerObj = GameObject.Find("player");
		//	if (playerObj.GetComponents<MyPlayer>() != null)
		//		player = playerObj.transform.GetComponent<PickUpPutDown>();
		//}
	}
	*/

	private void OnTriggerEnter(Collider other)
	{
        Debug.Log("BlockType은" + canSetZone.canSetZone_BT + " " + this.gameObject.name);

		if (other.GetComponent<Block>() != null)
		{
			if (other.GetComponent<Block>().block_Type == BlockType.WATER)
			{
				LastRail_BT = 1;
				//Debug.Log("블럭타입이 1!");
			}
		}

		if (other.gameObject.CompareTag("Player") && !canSetZone.isThereRail && canSetZone.canSetZone_BT != 1)
		{
            if (other.transform.GetComponent<PickUpPutDown>().isHoldRail)
            {
                Transform trChild = this.transform.GetChild(0).transform.GetChild(0);
                trChild.gameObject.SetActive(true);
            }
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
            Transform trChild = this.transform.GetChild(0).transform.GetChild(0);
            trChild.gameObject.SetActive(false);
        }

		if (other.GetComponent<Block>() != null)
		{
			if (other.GetComponent<Block>().block_Type == BlockType.WATER)
			{
				LastRail_BT = 0;
				//Debug.Log("블럭타입이 1!");
			}
		}
	}
}
