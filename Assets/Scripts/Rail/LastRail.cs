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
        Debug.Log(other.gameObject.tag);
        Debug.Log(player.IsHoldRail());
        Debug.Log(canSetZone.isThereRail);


		if (other.gameObject.CompareTag("Player") && !canSetZone.isThereRail)
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
	}
}
