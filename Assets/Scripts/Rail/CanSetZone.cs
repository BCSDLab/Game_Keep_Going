using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSetZone : MonoBehaviour
{
    public bool isThereRail = false;
	public int blockType;

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.layer == 10)
		{
			if (other.GetComponent<Block>().block_Type == BlockType.GRASS)
			{
				blockType = 0;
			}
			else if (other.GetComponent<Block>().block_Type == BlockType.WATER)
			{
				blockType = 1;
			}
		}

		if (other.gameObject.CompareTag("Rail") && other.gameObject.layer == 0)
		{
            //this.gameObject.SetActive(false);
            isThereRail = true;
            Debug.Log("������ ��ġ�Ǿ�����");
		}

		if (other.gameObject.CompareTag("StickRail"))
		{
			// other�� ���� ���������� ù ������ �Ǵ� �ڵ� �ʿ�
			// �� ���������� ������ railroad����Ʈ�� �ʱ�ȭ�ϰ� �� other������ railroad�� �ֱ�
			//other.tag = "Rail";
		}
	}
}
