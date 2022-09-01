using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    public int railStackNum = 1; // 쌓여있는 레일의 수
	private PickUpPutDown player;
	//[SerializeField]
	//private Rail theRail;

    // 테스트를 위해 주석 처리함
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
    /// 레일을 받아온 다음 (개수 상관 x) 해당 레일을 가장 아래의 child로 배치한다음 이들의 StackCount를 재조정.
    /// </summary>
    /// <param name="rail"></param>
    public void PileUpRail(GameObject rail)
    {
        if (transform.childCount == 1) // 차일드 맨 끝일때 추가.
        {
            rail.transform.SetParent(this.transform); // 들고 있는 레일의 맨 겉부분을 올리면 됨.
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
    /// 재귀용 카운터.
    /// </summary>
    /// <param name="railneed"></param>
    /// <param name="count"></param>
    private GameObject DeleteRail(int railneed, int count)
    {
        if(railneed == count)
        {
            print(railneed + "만큼의 레일이 사라졌습니다.");
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
    /// 해당 레일 개수만큼 레일을 리턴시켜준다.
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
        if (transform.childCount != 1) // 가장 마지막 인덱스가 아니라면
        {
            transform.GetChild(1).GetComponent<Rail>().DisableAllCollider();
        }
    }
    */

    void Update()
    {
        // 테스틀를 위해 추가함
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
