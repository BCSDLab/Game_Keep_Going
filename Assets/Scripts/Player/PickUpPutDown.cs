using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ����, ��������, ��ġ�ϱ�
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
    private GameObject lastBeforeRail; // ��ġ�� ������ ���� ����

    // ���� ����
    private bool isHold = false;
	private bool isItemNear = false;
    public bool isHoldRail = false;

    // �ʿ��� ������Ʈ
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
            Debug.Log("���� ȹ��");
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
        // ���� ��������
        if (nearItem.CompareTag("Rail") && canSetZoneFront.activeSelf == false && canSetZoneLeft.activeSelf == false && canSetZoneRight.activeSelf == false)
        {
            Debug.Log("������ ��������");

            equipPoint.transform.DetachChildren();
            nearItem.transform.position = equipPoint.transform.position - new Vector3(0, 1.2f, 0);

            Collider itemColider = nearItem.GetComponent<Collider>();
            Rigidbody itemRigidbody = nearItem.GetComponent<Rigidbody>();
            itemColider.enabled = true;
            itemRigidbody.isKinematic = false;

            isHold = false;
            isHoldRail = false;
            
        }

        // �տ� ���� ��ġ�ϱ�
		else if (nearItem.CompareTag("Rail") && canSetZoneFront.activeSelf == true)
		{
			Debug.Log("���� ��ġ�ϱ�");

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

        // ���ʿ� ���� ��ġ�ϱ�
        else if(nearItem.CompareTag("Rail") && canSetZoneLeft.activeSelf == true)
		{
            Debug.Log("���� ��ġ�ϱ�");

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

        // �����ʿ� ���� ��ġ�ϱ�
        else if (nearItem.CompareTag("Rail") && canSetZoneRight.activeSelf == true)
        {
            Debug.Log("���� ��ġ�ϱ�");

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
            Debug.Log("������ ����");
            isItemNear = true;
            nearItem = other.gameObject;
		}
	}
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            Debug.Log("�����۰� �־���");
            isItemNear = false;
            nearItem = null;
        }
    }
}
