using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSetZone : MonoBehaviour
{
    public bool isThereRail = false;
	public int canSetZone_BT; // canSetZone과 충돌했을 때의 blocktype
	public bool isRailComplete = false; // stickRail이 있는 곳까지 레일을 설치하면 true

	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Block>() != null)
		{
			if (other.GetComponent<Block>().block_Type == BlockType.GRASS)
			{
				canSetZone_BT = 0;
			}
			else if (other.GetComponent<Block>().block_Type == BlockType.WATER)
			{
				canSetZone_BT = 1;
			}
		}

		if (other.gameObject.CompareTag("Rail") && other.gameObject.layer == 0)
		{
            //this.gameObject.SetActive(false);
            isThereRail = true;
            //Debug.Log("레일이 설치되어있음");
		}

		if (other.gameObject.CompareTag("StickRail"))
		{
			Debug.Log("canSetZone이 스틱레일이랑 충돌함!!");
			isRailComplete = true;
		}
	}
}
