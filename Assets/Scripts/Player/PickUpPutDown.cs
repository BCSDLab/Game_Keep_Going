using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
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
    [SerializeField]
    private GameObject canSetZoneFront;
    [SerializeField]
    private GameObject canSetZoneLeft;
    [SerializeField]
    private GameObject canSetZoneRight;
    [SerializeField]
    private CanSetZone canSetZoneFrontS;
    [SerializeField]
    private CanSetZone canSetZoneLeftS;
    [SerializeField]
    private CanSetZone canSetZoneRightS;
    [SerializeField]
    private GameObject lastRailPos;
    [SerializeField]
    private GameObject curvedRail;
    [SerializeField]
    private GameObject lastBeforeRail; // 설치한 레일의 이전 레일
    private GameObject equipPoint;
    private GameObject nearItem;
    private GameObject holdItem;
    [SerializeField]
    private GameObject train;

    public Rail nearRail;
    public Rail holdRail;

    public List<GameObject> railroad1;
    public List<GameObject> railroad2;
    public List<GameObject> railroad3;

    // 상태 변수
    private bool isHold = false;
	private bool isItemNear = false;
    public bool isHoldRail = false;
    public bool isStageClear = false;


    void Start()
    {
        equipPoint = GameObject.FindGameObjectWithTag("EquipPoint");
        railroad1 = new List<GameObject>();
        railroad1.Add(lastBeforeRail);
        railroad2 = new List<GameObject>();
        railroad2.Add(lastBeforeRail);
        railroad3 = new List<GameObject>();
        railroad3.Add(lastBeforeRail);
        //Debug.Log(railroad.Peek().position);
    }

    void Update()
	{
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(!isHold)
                TryItemPickUp();
            else
                TryItemPutDown();
        }

        //TryStageClear();
    }

    public List<GameObject> GetRailRoad()
	{
        return railroad1;
	}

    public List<GameObject> GetRailRoad2()
    {
        return railroad2;
    }

    public List<GameObject> GetRailRoad3()
    {
        return railroad3;
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
        nearRail = nearItem.GetComponent("Rail") as Rail; // 충돌한 레일의 Rail스크립트 가져오기
        //holdRail = holdItem.GetComponent("Rail") as Rail; // 들고있는 레일의 Rail스크립트 가져오기

        // 레일 줍기
        if (nearItem.CompareTag("Rail"))
        {
            if (nearRail.getInt() <= 3)
            {
                holdItem = nearItem;
                nearItem = null;
                Debug.Log("레일 획득");
                holdItem.transform.SetParent(equipPoint.transform);
                holdItem.transform.localPosition = Vector3.zero;
                holdItem.transform.rotation = new Quaternion(0, 0, 0, 0);
                holdItem.transform.Rotate(new Vector3(0, 90, 0));

                Collider itemColider = holdItem.GetComponent<Collider>();
                //Rigidbody itemRigidbody = holdItem.GetComponent<Rigidbody>();
                itemColider.enabled = false;
                //itemRigidbody.isKinematic = true;

                isHold = true;
                isHoldRail = true;
            }
            else if (nearRail.getInt() > 3)
            {
                //Debug.Log(nearRail.getInt());
                nearItem.transform.GetChild(nearRail.getInt() - 2).SetParent(nearItem.transform.GetChild(nearRail.getInt() - 3));
                //Debug.Log(nearRail.getInt());
                nearItem.transform.GetChild(nearRail.getInt() - 2).SetParent(nearItem.transform.GetChild(nearRail.getInt() - 3));
                //Debug.Log(nearRail.getInt());
                holdItem = nearItem.transform.GetChild(nearRail.getInt() - 3).gameObject;


                nearItem = null;
                Debug.Log("레일 획득");
                holdItem.transform.SetParent(equipPoint.transform);
                holdItem.transform.localPosition = Vector3.zero;
                holdItem.transform.rotation = new Quaternion(0, 0, 0, 0);
                holdItem.transform.Rotate(new Vector3(0, 90, 0));

                Collider itemColider = holdItem.GetComponent<Collider>();
                itemColider.enabled = false;

                isHold = true;
                isHoldRail = true;
            }
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

        // 레일 바닥에 내려놓기
        if (holdItem.CompareTag("Rail") && nearItem == null && canSetZoneFront.activeSelf == false && canSetZoneLeft.activeSelf == false && canSetZoneRight.activeSelf == false)
        {
            Debug.Log("레일 내려놓기");

            equipPoint.transform.DetachChildren();
            holdItem.transform.position = equipPoint.transform.position - new Vector3(0, 1.2f, 0);

            Collider itemColider = holdItem.GetComponent<Collider>();
            //Rigidbody itemRigidbody = holdItem.GetComponent<Rigidbody>();
            itemColider.enabled = true;
            //itemRigidbody.isKinematic = false;

            isHold = false;
            isHoldRail = false;           
        }

		// 레일이 있을 경우 그 위에 쌓기
		else if (holdItem.CompareTag("Rail") && nearItem != null && nearItem.CompareTag("Rail") && nearItem.layer == 6)
		{
            nearRail = nearItem.GetComponent("Rail") as Rail; // 충돌한 레일의 Rail스크립트 가져오기
            holdRail = holdItem.GetComponent("Rail") as Rail; // 들고있는 레일의 Rail스크립트 가져오기
            Debug.Log("레일 쌓기");

            // 맨 아래의 레일을 설치 위치로 설정
            equipPoint.transform.DetachChildren();

            // 들고 있는 레일이 1개 일 때
            if (holdRail.getInt() == 1)
            {
                holdItem.transform.SetParent(nearItem.transform);

                holdItem.transform.position = nearItem.transform.position + new Vector3(0, 0.3f, 0) * nearRail.getInt();
                holdItem.transform.rotation = nearItem.transform.rotation;

                //holdItem.GetComponent<Rigidbody>().isKinematic = true;

                Debug.Log("레일이 1개 쌓임");
            }
            // 들고 있는 레일이 2개 이상일 때
            else if (holdRail.getInt() >= 2)
            {
                int i;
                for (i = 1; i < holdRail.getInt(); i++)
				{
                    // 첫번째 자식rail을 setparent로 위치 바꿨으니까 그 뒤 2,3번째 자식 레일 인식이 문제가 되는것임. i를 1로 수정
                    Debug.Log(i);
                    holdItem.transform.GetChild(1).transform.position = nearItem.transform.position + new Vector3(0, 0.3f, 0) * (nearRail.getInt() + i - 1);
                    holdItem.transform.GetChild(1).transform.rotation = nearItem.transform.rotation;
                    //holdItem.transform.GetChild(1).GetComponent<Rigidbody>().isKinematic = true;
                    holdItem.transform.GetChild(1).transform.SetParent(nearItem.transform);
                }

                holdItem.transform.SetParent(nearItem.transform);

                holdItem.transform.position = nearItem.transform.position + new Vector3(0, 0.3f, 0) * (nearRail.getInt() + i - 1);
                holdItem.transform.rotation = nearItem.transform.rotation;

                //holdItem.GetComponent<Rigidbody>().isKinematic = true;
                //holdItem.transform.DetachChildren();
                Debug.Log("레일이 2개 이상 쌓임");
            }

            isHold = false;
            isHoldRail = false;

        }

		// 앞에 레일 설치하기
		else if (holdItem.CompareTag("Rail") && canSetZoneFront.activeSelf == true)
        { 
            holdRail = holdItem.GetComponent("Rail") as Rail; // 들고있는 레일의 Rail스크립트 가져오기
            Debug.Log("앞에 레일 설치하기");

            if (holdRail.getInt() == 1) // 들고 있는 레일이 1개일 때
            {
                equipPoint.transform.DetachChildren();
                lastBeforeRail = holdItem;

				isHold = false;
				isHoldRail = false;

                canSetZoneFront.SetActive(false);
            }
            else if(holdRail.getInt() > 1) // 들고 있는 레일이 2개 이상일 때
			{
                lastBeforeRail = holdItem.transform.GetChild(holdRail.getInt() - 1).gameObject;
                holdItem.transform.GetChild(holdRail.getInt() - 1).parent = null;
            }

            lastRailPos.transform.Translate(new Vector3(1.6f, 0, 0));
            lastBeforeRail.transform.position = lastRailPos.transform.position;

            lastBeforeRail.transform.rotation = lastRailPos.transform.rotation;
            lastBeforeRail.transform.Rotate(0, 90, 0);

            Collider itemColider = lastBeforeRail.GetComponent<Collider>();
            //Rigidbody itemRigidbody = holdItem.GetComponent<Rigidbody>();
            itemColider.enabled = true;
            //itemRigidbody.isKinematic = false;

            lastBeforeRail.layer = 0;

            canSetZoneFrontS.isThereRail = false;
            canSetZoneLeftS.isThereRail = false;
            canSetZoneRightS.isThereRail = false;

            railroad1.Add(lastBeforeRail);
            railroad2.Add(lastBeforeRail);
            railroad3.Add(lastBeforeRail);
            //StageClear();
            //Debug.Log(railroad.Peek().position);
        }

        // 왼쪽에 레일 설치하기
        else if(holdItem.CompareTag("Rail") && canSetZoneLeft.activeSelf == true)
		{
            holdRail = holdItem.GetComponent("Rail") as Rail;
            Debug.Log("왼쪽에 레일 설치하기");

            railroad1.Remove(lastBeforeRail);
            railroad2.Remove(lastBeforeRail);
            railroad3.Remove(lastBeforeRail);
            Destroy(lastBeforeRail);
            Transform lastBeforeRailT = lastBeforeRail.transform;
            GameObject curvedRailObject = Instantiate(curvedRail);
            int lastBeforeRailLayer = lastBeforeRail.layer;
            curvedRailObject.transform.position = lastBeforeRailT.position;
            curvedRailObject.layer = lastBeforeRailLayer;
            railroad1.Add(curvedRailObject);
            railroad2.Add(curvedRailObject);
            railroad3.Add(curvedRailObject);

            if (holdRail.getInt() == 1) // 들고 있는 레일이 1개 일 때
            {
                equipPoint.transform.DetachChildren();   

                lastBeforeRail = holdItem;

                isHold = false;
                isHoldRail = false;

                canSetZoneLeft.SetActive(false);
            }
            else if(holdRail.getInt() > 1) // 들고 있는 레일이 2개 이상일 때
			{
                lastBeforeRail = holdItem.transform.GetChild(holdRail.getInt() - 1).gameObject;
                holdItem.transform.GetChild(holdRail.getInt() - 1).parent = null;
            }

            lastRailPos.transform.Translate(new Vector3(0, 0, 1.6f));
            lastBeforeRail.transform.position = lastRailPos.transform.position;

            lastRailPos.transform.Rotate(0, -90, 0);
            lastBeforeRail.transform.rotation = lastRailPos.transform.rotation;
            lastBeforeRail.transform.Rotate(0, 90, 0);

            curvedRailObject.transform.rotation = lastRailPos.transform.rotation;
            curvedRailObject.transform.Rotate(0, 90, 0);

            Collider itemColider = lastBeforeRail.GetComponent<Collider>();
            //Rigidbody itemRigidbody = lastBeforeRail.GetComponent<Rigidbody>();
            itemColider.enabled = true;
            //itemRigidbody.isKinematic = false;

            //lastBeforeRail.layer = 0;
            lastBeforeRail.layer = 7;

            canSetZoneFrontS.isThereRail = false;
            canSetZoneLeftS.isThereRail = false;
            canSetZoneRightS.isThereRail = false;

            railroad1.Add(lastBeforeRail);
            railroad2.Add(lastBeforeRail);
            railroad3.Add(lastBeforeRail);
            //StageClear();
            //Debug.Log(railroad.Peek().position);
        }

        // 오른쪽에 레일 설치하기
        else if (holdItem.CompareTag("Rail") && canSetZoneRight.activeSelf == true)
        {
            holdRail = holdItem.GetComponent("Rail") as Rail;
            Debug.Log("오른쪽에 레일 설치하기");

            railroad1.Remove(lastBeforeRail);
            railroad2.Remove(lastBeforeRail);
            railroad3.Remove(lastBeforeRail);
            Destroy(lastBeforeRail);
            Transform lastBeforeRailT = lastBeforeRail.transform;
            GameObject curvedRailObject = Instantiate(curvedRail);
            int lastBeforeRailLayer = lastBeforeRail.layer;
            curvedRailObject.transform.position = lastBeforeRailT.position;
            curvedRailObject.layer = lastBeforeRailLayer;
            railroad1.Add(curvedRailObject);
            railroad2.Add(curvedRailObject);
            railroad3.Add(curvedRailObject);

            if (holdRail.getInt() == 1) // 들고 있는 레일이 1개 일 때
            {
                equipPoint.transform.DetachChildren();

                lastBeforeRail = holdItem;

                isHold = false;
                isHoldRail = false;

                canSetZoneRight.SetActive(false);
            }
            else if(holdRail.getInt() > 1) // 들고 있는 레일이 2개 이상일 때
			{
                lastBeforeRail = holdItem.transform.GetChild(holdRail.getInt() - 1).gameObject;
                holdItem.transform.GetChild(holdRail.getInt() - 1).parent = null;
            }

            lastRailPos.transform.Translate(new Vector3(0, 0, -1.6f));
            lastBeforeRail.transform.position = lastRailPos.transform.position;

            lastRailPos.transform.Rotate(0, 90, 0);
            lastBeforeRail.transform.rotation = lastRailPos.transform.rotation;
            lastBeforeRail.transform.Rotate(0, 90, 0);

            curvedRailObject.transform.rotation = lastRailPos.transform.rotation;
            curvedRailObject.transform.Rotate(0, 180, 0);

            Collider itemColider = lastBeforeRail.GetComponent<Collider>();
            //Rigidbody itemRigidbody = lastBeforeRail.GetComponent<Rigidbody>();
            itemColider.enabled = true;
            //itemRigidbody.isKinematic = false;

            //lastBeforeRail.layer = 0;
            lastBeforeRail.layer = 8;

            canSetZoneFrontS.isThereRail = false;
            canSetZoneLeftS.isThereRail = false;
            canSetZoneRightS.isThereRail = false;

            railroad1.Add(lastBeforeRail);
            railroad2.Add(lastBeforeRail);
            railroad3.Add(lastBeforeRail);
            //StageClear();
            //Debug.Log(railroad.Peek().position);
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
}
