using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    public int railStackNum = 1; // �׿��ִ� ������ ��
	private PickUpPutDown player;
	//[SerializeField]
	//private Rail theRail;

    // �׽�Ʈ�� ���� �ּ� ó����
    /*
    void UpdateRailCount()
    {
        if(transform.childCount == 1)
        {
            railStackNum = 1;
        }
        else
        {
            railStackNum = transform.GetChild(1).GetComponent<Rail>().railStackNum + 1;
        }
        if(transform.parent != null)
        {
            transform.parent.GetComponent<Rail>().UpdateRailCount();
        }        
    }

    /// <summary>
    /// ������ �޾ƿ� ���� (���� ��� x) �ش� ������ ���� �Ʒ��� child�� ��ġ�Ѵ��� �̵��� StackCount�� ������.
    /// </summary>
    /// <param name="rail"></param>
    public void PileUpRail(GameObject rail)
    {
        if (transform.childCount == 1) // ���ϵ� �� ���϶� �߰�.
        {
            rail.transform.SetParent(this.transform); // ��� �ִ� ������ �� �Ѻκ��� �ø��� ��.
            rail.transform.localPosition = new Vector3(0, 0.3f, 0);
            rail.transform.rotation = Quaternion.identity;
            rail.GetComponent<Rail>().UpdateRailCount();
        }
        else 
        {
            transform.GetChild(1).GetComponent<Rail>().PileUpRail(rail);
        }
    }

    /// <summary>
    /// ��Ϳ� ī����.
    /// </summary>
    /// <param name="railneed"></param>
    /// <param name="count"></param>
    private GameObject DeleteRail(int railneed, int count)
    {
        if(railneed == count)
        {
            print(railneed + "��ŭ�� ������ ��������ϴ�.");
            if(transform.parent != null)
            {
                GameObject tempParent = transform.parent.gameObject;
                transform.SetParent(null);
                tempParent.transform.GetComponent<Rail>().UpdateRailCount();
            }
            GetComponent<Collider>().enabled = false;
            return this.gameObject;
        }
        else
        {
            return transform.parent.GetComponent<Rail>().DeleteRail(railneed, ++count);
        }
    }


    /// <summary>
    /// �ش� ���� ������ŭ ������ ���Ͻ����ش�.
    /// </summary>
    /// <param name="railneed"></param>
    /// <param name="count"></param>
    public GameObject DeleteRail(int railneed)
    {
        print(gameObject.name);
        if(transform.childCount > 1)
        {
            return transform.GetChild(1).GetComponent<Rail>().DeleteRail(railneed);
        }
        else
        {
            if(railneed == 1)
            {
                GetComponent<Collider>().enabled = false;
                return this.gameObject;
            }
            else
            {
                return transform.parent.GetComponent<Rail>().DeleteRail(railneed, 2);
            }
        }
    }


    public void DisableAllCollider()
    {
        GetComponent<Collider>().enabled = false;
        if (transform.childCount != 1) // ���� ������ �ε����� �ƴ϶��
        {
            transform.GetChild(1).GetComponent<Rail>().DisableAllCollider();
        }
    }
    */

    void Update()
    {
        // �׽�Ʋ�� ���� �߰���
		CountRailNum();
    }

	private void CountRailNum()
	{
		railStackNum = this.transform.childCount;
	}

	public int GetInt()
	{
		return railStackNum;
	}
}
