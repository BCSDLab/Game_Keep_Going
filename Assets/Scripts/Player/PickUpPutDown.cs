using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 레일 집기, 내려놓기, 설치하기
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
    private GameObject equipPoint;
    private GameObject nearItem;
    [SerializeField]
    private GameObject lastBeforeRail; // 설치한 레일의 이전 레일

    // 상태 변수
    private bool isHold = false;
	private bool isItemNear = false;
    public bool isHoldRail = false;

    // 필요한 컴포넌트
    private BoxCollider theBoxCollider;


    void Start()
    {
        theBoxCollider = GetComponent<BoxCollider>();
        equipPoint = GameObject.FindGameObjectWithTag("EquipPoint");

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
    }

    private void TryItemPickUp()
	{
        if(isItemNear && nearItem.layer == 6)
		{
            ItemPickUp();
		}
    }

    private void ItemPickUp()
	{      

		if (nearItem.CompareTag("Rail"))
		{
            Debug.Log("레일 획득");
            nearItem.transform.SetParent(equipPoint.transform);
            nearItem.transform.localPosition = Vector3.zero;
            nearItem.transform.rotation = new Quaternion(0, 0, 0, 0);
            nearItem.transform.Rotate(new Vector3(0, 90, 0));

            Collider itemColider = nearItem.GetComponent<Collider>();
            Rigidbody itemRigidbody = nearItem.GetComponent<Rigidbody>();
            itemColider.enabled = false;
            itemRigidbody.isKinematic = true;

            isHold = true;
            isHoldRail = true;
        }
	}

    private void TryItemPutDown()
	{
        ItemPutDown();
	}

    private void ItemPutDown()
	{
        // 레일 내려놓기
        if (nearItem.CompareTag("Rail") && canSetZoneFront.activeSelf == false && canSetZoneLeft.activeSelf == false && canSetZoneRight.activeSelf == false)
        {
            Debug.Log("아이템 내려놓기");

            equipPoint.transform.DetachChildren();
            nearItem.transform.position = equipPoint.transform.position - new Vector3(0, 1.2f, 0);

            Collider itemColider = nearItem.GetComponent<Collider>();
            Rigidbody itemRigidbody = nearItem.GetComponent<Rigidbody>();
            itemColider.enabled = true;
            itemRigidbody.isKinematic = false;

            isHold = false;
            isHoldRail = false;
            
        }

        // 앞에 레일 설치하기
		else if (nearItem.CompareTag("Rail") && canSetZoneFront.activeSelf == true)
		{
			Debug.Log("레일 설치하기");

			equipPoint.transform.DetachChildren();

            lastRailPos.transform.Translate(new Vector3(1.6f, 0, 0));
            nearItem.transform.position = lastRailPos.transform.position;

            nearItem.transform.rotation = lastRailPos.transform.rotation;
            nearItem.transform.Rotate(0, 90, 0);

            Collider itemColider = nearItem.GetComponent<Collider>();
			Rigidbody itemRigidbody = nearItem.GetComponent<Rigidbody>();
			itemColider.enabled = true;
			itemRigidbody.isKinematic = false;
          
            nearItem.layer = 0;

            isHold = false;
			isHoldRail = false;
            canSetZoneFrontS.isThereRail = false;
            canSetZoneLeftS.isThereRail = false;
            canSetZoneRightS.isThereRail = false;

            canSetZoneFront.SetActive(false);
            lastBeforeRail = nearItem;
        }

        // 왼쪽에 레일 설치하기
        else if(nearItem.CompareTag("Rail") && canSetZoneLeft.activeSelf == true)
		{
            Debug.Log("레일 설치하기");

            equipPoint.transform.DetachChildren();
            //lastBeforeRail.SetActive(false);
            Destroy(lastBeforeRail);
            Transform lastBeforeRailT = lastBeforeRail.transform;
            GameObject curvedRailObject = Instantiate(curvedRail);
            //curvedRailObject.transform.position = lastBeforeRail.transform.position;
            curvedRailObject.transform.position = lastBeforeRailT.position;
            curvedRailObject.transform.position = lastBeforeRailT.position;

            lastRailPos.transform.Translate(new Vector3(0, 0, 1.6f));
            nearItem.transform.position = lastRailPos.transform.position;

            lastRailPos.transform.Rotate(0, -90, 0);
            nearItem.transform.rotation = lastRailPos.transform.rotation;
            nearItem.transform.Rotate(0, 90, 0);

            curvedRailObject.transform.rotation = lastRailPos.transform.rotation;
            curvedRailObject.transform.Rotate(0, 90, 0);

            Collider itemColider = nearItem.GetComponent<Collider>();
            Rigidbody itemRigidbody = nearItem.GetComponent<Rigidbody>();
            itemColider.enabled = true;
            itemRigidbody.isKinematic = false;

            
            nearItem.layer = 0;

            isHold = false;
            isHoldRail = false;
            canSetZoneFrontS.isThereRail = false;
            canSetZoneLeftS.isThereRail = false;
            canSetZoneRightS.isThereRail = false;

            canSetZoneLeft.SetActive(false);
            lastBeforeRail = nearItem;
        }

        // 오른쪽에 레일 설치하기
        else if (nearItem.CompareTag("Rail") && canSetZoneRight.activeSelf == true)
        {
            Debug.Log("레일 설치하기");

            equipPoint.transform.DetachChildren();
            Destroy(lastBeforeRail);
            Transform lastBeforeRailT = lastBeforeRail.transform;
            //lastBeforeRail.SetActive(false);
            GameObject curvedRailObject = Instantiate(curvedRail);
            //curvedRailObject.transform.position = lastBeforeRail.transform.position;
            curvedRailObject.transform.position = lastBeforeRailT.position;

            lastRailPos.transform.Translate(new Vector3(0, 0, -1.6f));
            nearItem.transform.position = lastRailPos.transform.position;

            lastRailPos.transform.Rotate(0, 90, 0);
            nearItem.transform.rotation = lastRailPos.transform.rotation;
            nearItem.transform.Rotate(0, 90, 0);

            curvedRailObject.transform.rotation = lastRailPos.transform.rotation;
            curvedRailObject.transform.Rotate(0, 180, 0);

            Collider itemColider = nearItem.GetComponent<Collider>();
            Rigidbody itemRigidbody = nearItem.GetComponent<Rigidbody>();
            itemColider.enabled = true;
            itemRigidbody.isKinematic = false;

            
            nearItem.layer = 0;

            isHold = false;
            isHoldRail = false;
            canSetZoneFrontS.isThereRail = false;
            canSetZoneLeftS.isThereRail = false;
            canSetZoneRightS.isThereRail = false;

            canSetZoneRight.SetActive(false);
            lastBeforeRail = nearItem;
        }
    }

	private void OnTriggerEnter(Collider other)
	{
        if(other.gameObject.layer == LayerMask.NameToLayer("Item"))
		{
            Debug.Log("아이템 감지");
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
