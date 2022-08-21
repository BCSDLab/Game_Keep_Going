using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ·¹ÀÏ ¼³Ä¡ °¡´É Á¸ Ç¥½Ã
public class LastRail: MonoBehaviour
{
    private PickUpPutDown player;
    private PickUpPutDown otherPlayer;
    private CanSetZone canSetZone;


    void Start()
    {
        player = GameObject.Find("player").transform.GetComponent<PickUpPutDown>();
        canSetZone = this.gameObject.transform.GetChild(0).GetComponent<CanSetZone>();
    }

	//private void Update()
	//{
	//	if(canSetZone.blockType == 1)
	//	{
 //           Transform trChild = this.transform.GetChild(0).transform.GetChild(0);
 //           trChild.gameObject.SetActive(false);
 //       }
	//}

	/*
    void Update()
    {
//      if(player == null)
//      {
//          GameObject playerObj = GameObject.Find("player");
//          if (playerObj.GetComponents<MyPlayer>() != null)
//              player = playerObj.transform.GetComponent<PickUpPutDown>();
//      }
    }
    */
	private void OnTriggerEnter(Collider other)
	{
        //Debug.Log(other.gameObject.tag);
        //Debug.Log(player.IsHoldRail());
        //Debug.Log(canSetZone.isThereRail);
        Debug.Log("BlockType은" + canSetZone.blockType);


		if (other.gameObject.CompareTag("Player") && !canSetZone.isThereRail)
		{
            if (canSetZone.blockType != 1)
            {
                if (other.transform.GetComponent<PickUpPutDown>().isHoldRail)
                {
                    Transform trChild = this.transform.GetChild(0).transform.GetChild(0);
                    trChild.gameObject.SetActive(true);
                }
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
	}
}
