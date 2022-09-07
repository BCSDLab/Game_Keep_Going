using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSetZone : MonoBehaviour
{
    public bool isThereRail = false;
	public int canSetZone_BT; // canSetZone�� �浹���� ���� blocktype
	public bool isRailComplete = false; // stickRail�� �ִ� ������ ������ ��ġ�ϸ� true

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
            //Debug.Log("������ ��ġ�Ǿ�����");
		}

		if (other.gameObject.CompareTag("StickRail"))
		{
			Debug.Log("canSetZone�� ��ƽ�����̶� �浹��!!");
			isRailComplete = true;
		}
	}
}
