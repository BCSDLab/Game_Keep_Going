using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ·¹ÀÏ ¼³Ä¡ °¡´É Á¸ Ç¥½Ã
public class LastRail: MonoBehaviour
{
    private PickUpPutDown player;
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
		if (other.gameObject.CompareTag("Player") && player.IsHoldRail() && !canSetZone.isThereRail)
		{
            Transform trChild = this.transform.GetChild(0).transform.GetChild(0);
            trChild.gameObject.SetActive(true);
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
