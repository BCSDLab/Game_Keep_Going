using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* <레일>
 * use gravity는 항상 false
 * is kinematic은 항상 true
 * is trigger는 항상 true
 * tag는 항상 Rail
 * 설치 되기 전 레일의 레이어 : Item, 설치 된 후 레일의 레이어 : Default
 * 
 * 바닥에 있는 아이템 레일(또는 쌓인 레일 중 맨 아래 레일) : box collider = true
 * 레일을 집고 있으면(1개) : box collider = false
 * 다른 레일 위에 올려지면 : box collider = false
 * 설치된 레일 : box collider = true
 */

// space키로 작동하는 행동(레일 집기, 내려놓기, 설치하기 등)

public class PickUpPutDown : MonoBehaviour
{
    private GameObject canSetZoneFront;
    private GameObject canSetZoneLeft;
    private GameObject canSetZoneRight;
    private CanSetZone canSetZoneFrontS;
    private CanSetZone canSetZoneLeftS;
    private CanSetZone canSetZoneRightS;
    [SerializeField]
    private GameObject lastRailPos;
    private GameObject curvedRail;
    [SerializeField]
    private GameObject lastBeforeRail; // 설치한 레일의 이전 레일
    private GameObject equipPoint;
    [SerializeField]
    private GameObject nearItem;
    [SerializeField]
    private GameObject holdItem;
    private Vector3 PutDownPosition = Vector3.zero;

    private List<GameObject> railroad1;
    private List<GameObject> railroad2;

    // 상태 변수
    [SerializeField]
    private bool isHold = false;
    [SerializeField]
	private bool isItemNear = false;

    public bool IsHoldRail()
    {
        return (isHold && holdItem.tag == "Rail");
    }

    public bool IsHold()
    {
        return isHold;
    }

    void Start()
    {
        canSetZoneFront = GameObject.Find("PreCanSetZoneFront").transform.GetChild(0).gameObject;
        canSetZoneLeft = GameObject.Find("PreCanSetZoneLeft").transform.GetChild(0).gameObject;
        canSetZoneRight = GameObject.Find("PreCanSetZoneRight").transform.GetChild(0).gameObject;
        canSetZoneFrontS = GameObject.Find("PreCanSetZoneFront").GetComponent<CanSetZone>();
        canSetZoneLeftS = GameObject.Find("PreCanSetZoneLeft").GetComponent<CanSetZone>();
        canSetZoneRightS = GameObject.Find("PreCanSetZoneRight").GetComponent<CanSetZone>();
        lastRailPos = GameObject.Find("LastRailPos");
        curvedRail = Resources.Load("Prefabs/rail_curvedbase") as GameObject;
        lastBeforeRail = GameObject.Find("FixedRail");
        lastBeforeRail = lastBeforeRail.transform.GetChild(7).gameObject;
        equipPoint = GameObject.FindGameObjectWithTag("EquipPoint");
        railroad1 = new List<GameObject>();
        railroad2 = new List<GameObject>();
        AddRailToList(lastBeforeRail);
    }

    void Update()
	{
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isHold)
            {
                print("물건 집기!");
                TryItemPickUp();


            }
            else
            {

                TryItemPutDown();
            }
        }
        /// 만약 물건을 3개 이하로 들고 있는 상태에서, 물건을 지나치게 된다면.
        if (false)
        {
            // 물건을 집어 추가로 올린다.
        }

    }

    private void AddRailToList(GameObject rail)
	{
        railroad1.Add(rail);
        railroad2.Add(rail);
    }

    private void RemoveRailToList(GameObject rail)
	{
        railroad1.Remove(rail);
        railroad2.Remove(rail);
    }

    public List<GameObject> GetRailRoad()
	{
        return railroad1;
	}

    public List<GameObject> GetRailRoad2()
    {
        return railroad2;
    }


    // 아이템 줍기 시도
    private void TryItemPickUp()
	{
        if(isItemNear && nearItem.layer == 6)
		{
            ItemPickUp();
        }
    }

    // 아이템 줍기
    private void ItemPickUp()
	{
        // 레일 줍기
        /// <summary>
        /// 바닥에 레일이 3개 쌓여있고 씬에서 바닥과 첫번째로 가까운 레일을 rail, rail위의 레일을 rail1, rail1위의 레일을 rail2라 하면
        /// rail은 rail1, rail2의 부모 오브젝트가 된다. 그리고 rail의 0번째 자식은 rail의 defalt오브젝트, rail의 1번쨰 자식은 rail1, 2번째 자식은 rail2가 된다.
        /// 이렇게 한 이유는 맨 아래의 부모 오브젝트만 box collider를 키고 나머지 자식 레일 오브젝트는 box collider를 끄려고 이렇게 했다.
        /// 
        /// 쌓여있는 레일이 3개 이하일 때 : 플레이어가 쌓인 레일 쪽으로 갔을 때 rail을 인식할 것이고, space를 누르면 rail은 equipPoint의 자식오브젝트가 되고
        /// rail의 포지션이 equipPoint의 포지션으로 맞춰진다.
        /// 쌓여있는 레일이 3개보다 많을 때 : 쌓여있는 레일이 바닥에서부터 rail, rail1, rail2, rail3, rail4 이렇게 있다고 하면
        /// 플레이어가 rail을 감지했을 때 space를 누르면 rail3이 rail2의 자식 오브젝트가 되고 rail4도 rail2의 자식 오브젝트가 된다.
        /// 그 뒤 rail2는 equipPoint의 자식오브젝트가 되고 rail2의 포지션이 equipPoint의 포지션으로 맞춰진다.
        /// </summary>
        /// 
        if (nearItem.CompareTag("Rail"))
        {
            Rail nearRail = nearItem.GetComponent<Rail>();
            nearRail = nearItem.GetComponent("Rail") as Rail; // 충돌한 레일의 Rail스크립트 가져오기
                                                              //holdRail = holdItem.GetComponent("Rail") as Rail; // 들고있는 레일의 Rail스크립트 가져오기
                                                              // 쌓여있는 레일이 3개 이하일 때
            if (nearRail.getInt() <= 3)
            {
                holdItem = nearRail.DeleteRail(nearRail.getInt());
                nearItem = null;
                nearRail = null;
            }
            // 쌓여있는 레일이 3개보다 많을 때
            else if (nearRail.getInt() > 3)
            {
                holdItem = nearRail.DeleteRail(3);
                nearItem = null;
                nearRail = null;
            }

            holdItem.transform.SetParent(equipPoint.transform);
            holdItem.transform.localPosition = Vector3.zero;
            holdItem.transform.rotation = new Quaternion(0, 0, 0, 0);
            holdItem.transform.Rotate(new Vector3(0, 90, 0));

            isHold = true;

        }

        // 도끼 들기
        else if (nearItem.CompareTag("Axe") || nearItem.CompareTag("Pickaxe"))
        {
            holdItem = nearItem;
            nearItem = null;
            Debug.Log("도끼 획득");
            holdItem.transform.SetParent(equipPoint.transform);
            holdItem.transform.localPosition = Vector3.zero;
            //holdItem.transform.Translate(new Vector3(0.4f, 0, 0));
            holdItem.transform.localPosition = Vector3.right * 0.4f;
            holdItem.transform.rotation = new Quaternion(0, 0, 0, 0);
            holdItem.transform.Rotate(0, 90, 0);

            holdItem.GetComponent<Collider>().enabled = false;
            //holdItem.GetComponent<Rigidbody>().isKinematic = true;

            isHold = true;
        }

	}

    // 아이템 내려놓기 시도
    private void TryItemPutDown()
	{
        ItemPutDown();
	}

    // 아이템 내려놓기, 레일 설치하기
    private void ItemPutDown()
	{
        /*
        // 레일 바닥에 내려놓기
        if (holdItem.CompareTag("Rail") && nearItem == null && canSetZoneFront.activeSelf == false && canSetZoneLeft.activeSelf == false && canSetZoneRight.activeSelf == false)
        {

            equipPoint.transform.DetachChildren();
            holdItem.transform.position = PutDownPosition + new Vector3(0,1.6f,0);
            holdItem.transform.rotation = Quaternion.identity;

            holdItem.GetComponent<Collider>().enabled = true;

            isHold = false;
            holdItem = null;
        }

        // 레일이 있을 경우 그 위에 쌓기
        else if (holdItem.CompareTag("Rail") && nearItem.CompareTag("Rail") && nearItem.layer == 6)
		{
            holdItem.GetComponent<Rail>().DisableAllCollider();
            nearItem.GetComponent<Rail>().PileUpRail(holdItem);
            holdItem = null;

            isHold = false;
        }
        */

		// 앞에 레일 설치하기
		if (holdItem.CompareTag("Rail") && canSetZoneFront.activeSelf == true)
        {
            Rail holdRail = holdItem.GetComponent<Rail>();

            if (holdRail.getInt() == 1) // 들고 있는 레일이 1개일 때
            {
                equipPoint.transform.DetachChildren();
                lastBeforeRail = holdRail.DeleteRail(1);

				isHold = false;

                canSetZoneFront.SetActive(false);
            }
            else if(holdRail.getInt() > 1) // 들고 있는 레일이 2개 이상일 때
			{
                lastBeforeRail = holdRail.DeleteRail(1);
            }

            lastRailPos.transform.Translate(new Vector3(1.6f, 0, 0));
            lastBeforeRail.transform.position = lastRailPos.transform.position;

            lastBeforeRail.transform.rotation = lastRailPos.transform.rotation;
            lastBeforeRail.transform.Rotate(0, 90, 0);

            Collider itemColider = lastBeforeRail.GetComponent<Collider>();
            itemColider.enabled = true;

            lastBeforeRail.layer = 0;

            canSetZoneFrontS.isThereRail = false;
            canSetZoneLeftS.isThereRail = false;
            canSetZoneRightS.isThereRail = false;

            AddRailToList(lastBeforeRail);
        }

        // 왼쪽에 레일 설치하기
        else if(holdItem.CompareTag("Rail") && canSetZoneLeft.activeSelf == true)
		{
            Rail holdRail = holdItem.GetComponent<Rail>();

            RemoveRailToList(lastBeforeRail);
            Destroy(lastBeforeRail);
            Transform lastBeforeRailT = lastBeforeRail.transform;
            GameObject curvedRailObject = Instantiate(curvedRail);
            int lastBeforeRailLayer = lastBeforeRail.layer;
            curvedRailObject.transform.position = lastBeforeRailT.position;
            curvedRailObject.layer = lastBeforeRailLayer;
            AddRailToList(curvedRailObject);

            if (holdRail.getInt() == 1) // 들고 있는 레일이 1개 일 때
            {
                equipPoint.transform.DetachChildren();
                lastBeforeRail = holdRail.DeleteRail(1);

                isHold = false;

                canSetZoneFront.SetActive(false);
            }
            else if(holdRail.getInt() > 1) // 들고 있는 레일이 2개 이상일 때
			{
                lastBeforeRail = holdRail.DeleteRail(1);
            }

            lastRailPos.transform.Translate(new Vector3(0, 0, 1.6f));
            lastBeforeRail.transform.position = lastRailPos.transform.position;

            lastRailPos.transform.Rotate(0, -90, 0);
            lastBeforeRail.transform.rotation = lastRailPos.transform.rotation;
            lastBeforeRail.transform.Rotate(0, 90, 0);

            curvedRailObject.transform.rotation = lastRailPos.transform.rotation;
            curvedRailObject.transform.Rotate(0, 90, 0);

            Collider itemColider = lastBeforeRail.GetComponent<Collider>();
            itemColider.enabled = true;
            lastBeforeRail.layer = 7;

            canSetZoneFrontS.isThereRail = false;
            canSetZoneLeftS.isThereRail = false;
            canSetZoneRightS.isThereRail = false;

            AddRailToList(lastBeforeRail);
        }

        // 오른쪽에 레일 설치하기
        else if (holdItem.CompareTag("Rail") && canSetZoneRight.activeSelf == true)
        {
            Rail holdRail = holdItem.GetComponent<Rail>();

            RemoveRailToList(lastBeforeRail);
            Destroy(lastBeforeRail);
            Transform lastBeforeRailT = lastBeforeRail.transform;
            GameObject curvedRailObject = Instantiate(curvedRail);
            int lastBeforeRailLayer = lastBeforeRail.layer;
            curvedRailObject.transform.position = lastBeforeRailT.position;
            curvedRailObject.layer = lastBeforeRailLayer;
            AddRailToList(curvedRailObject);

            if (holdRail.getInt() == 1) // 들고 있는 레일이 1개 일 때
            {
                equipPoint.transform.DetachChildren();
                lastBeforeRail = holdRail.DeleteRail(1);

                isHold = false;

                canSetZoneFront.SetActive(false);
            }
            else if(holdRail.getInt() > 1) // 들고 있는 레일이 2개 이상일 때
			{
                lastBeforeRail = holdRail.DeleteRail(1);
            }

            lastRailPos.transform.Translate(new Vector3(0, 0, -1.6f));
            lastBeforeRail.transform.position = lastRailPos.transform.position;

            lastRailPos.transform.Rotate(0, 90, 0);
            lastBeforeRail.transform.rotation = lastRailPos.transform.rotation;
            lastBeforeRail.transform.Rotate(0, 90, 0);

            curvedRailObject.transform.rotation = lastRailPos.transform.rotation;
            curvedRailObject.transform.Rotate(0, 180, 0);

            Collider itemColider = lastBeforeRail.GetComponent<Collider>();
            itemColider.enabled = true;

            lastBeforeRail.layer = 8;

            canSetZoneFrontS.isThereRail = false;
            canSetZoneLeftS.isThereRail = false;
            canSetZoneRightS.isThereRail = false;

            AddRailToList(lastBeforeRail);

        }

        // 도끼 내려놓기
        else if (holdItem.CompareTag("Axe") || holdItem.CompareTag("Pickaxe"))
		{
            Debug.Log("도끼 내려놓기");

            equipPoint.transform.DetachChildren();
            holdItem.transform.position = equipPoint.transform.position - new Vector3(0, 1.2f, 0);

            holdItem.GetComponent<Collider>().enabled = true;
            //holdItem.GetComponent<Rigidbody>().isKinematic = false;

            isHold = false;
        }
    }

    private void OnTriggerEnter(Collider other)
	{
        if(other.gameObject.layer == LayerMask.NameToLayer("Item"))
		{
            Debug.Log("아이템 감지(반투명)");
            isItemNear = true;
            nearItem = other.gameObject;
		}
	}
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            Debug.Log("아이템과 멀어짐");
            isItemNear = false;
            nearItem = null;
        }
    }

    public void SetPutDownPosition(Vector3 _position)
    {
        Debug.Log("놓는 블럭 교체" + _position);
        PutDownPosition = _position;
    }
}
