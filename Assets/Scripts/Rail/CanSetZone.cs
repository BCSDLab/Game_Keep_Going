using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSetZone : MonoBehaviour
{
    public bool isThereRail = false;
	public int canSetZone_BT; // canSetZone과 충돌했을 때의 blocktype

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
            Debug.Log("레일이 설치되어있음");
		}

		if (other.gameObject.CompareTag("StickRail"))
		{
			// other가 다음 스테이지의 첫 레일이 되는 코드 필요
			// 한 스테이지가 끝나면 railroad리스트를 초기화하고 이 other레일을 railroad에 넣기
			//other.tag = "Rail";
		}
	}
}
