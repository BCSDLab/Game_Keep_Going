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
    private GameObject lastRailPos;
    private GameObject equipPoint;
    private GameObject nearItem;

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

            //lastRailPos.transform.eulerAngles = new Vector3(0, 0, 0);
            //nearItem.transform.eulerAngles = new Vector3(0, 90, 0);
            //nearItem.transform.rotation = new Quaternion(0, 0, 0, 0);
            //nearItem.transform.Rotate(new Vector3(0, 90, 0));
            //nearItem.transform.Rotate(0, 0, 0);
            nearItem.transform.rotation = lastRailPos.transform.rotation;
            nearItem.transform.Rotate(0, 90, 0);



            Collider itemColider = nearItem.GetComponent<Collider>();
			Rigidbody itemRigidbody = nearItem.GetComponent<Rigidbody>();
			itemColider.enabled = true;
			itemRigidbody.isKinematic = false;

            nearItem.layer = 0;

			isHold = false;
			isHoldRail = false;

            canSetZoneFront.SetActive(false);
		}

        // ���ʿ� ���� ��ġ�ϱ�
        else if(nearItem.CompareTag("Rail") && canSetZoneLeft.activeSelf == true)
		{
            Debug.Log("���� ��ġ�ϱ�");

            equipPoint.transform.DetachChildren();

            lastRailPos.transform.Translate(new Vector3(0, 0, 1.6f));
            nearItem.transform.position = lastRailPos.transform.position;

            lastRailPos.transform.Rotate(0, -90, 0);
            //nearItem.transform.eulerAngles = new Vector3(0, 180, 0);
            //nearItem.transform.rotation = new Quaternion(0, 0, 0, 0);
            //nearItem.transform.Rotate(0, 180, 0);
            nearItem.transform.rotation = lastRailPos.transform.rotation;
            nearItem.transform.Rotate(0, 90, 0);

            Collider itemColider = nearItem.GetComponent<Collider>();
            Rigidbody itemRigidbody = nearItem.GetComponent<Rigidbody>();
            itemColider.enabled = true;
            itemRigidbody.isKinematic = false;

            nearItem.layer = 0;

            isHold = false;
            isHoldRail = false;

            canSetZoneLeft.SetActive(false);
        }

        // �����ʿ� ���� ��ġ�ϱ�
        else if (nearItem.CompareTag("Rail") && canSetZoneRight.activeSelf == true)
        {
            Debug.Log("���� ��ġ�ϱ�");

            equipPoint.transform.DetachChildren();

            lastRailPos.transform.Translate(new Vector3(0, 0, -1.6f));
            nearItem.transform.position = lastRailPos.transform.position;

            lastRailPos.transform.Rotate(0, 90, 0);
            //nearItem.transform.eulerAngles = new Vector3(0, 0, 0);
            //nearItem.transform.rotation = new Quaternion(0, 0, 0, 0);
            //nearItem.transform.Rotate(0, 0, 0);
            nearItem.transform.rotation = lastRailPos.transform.rotation;
            nearItem.transform.Rotate(0, 90, 0);

            Collider itemColider = nearItem.GetComponent<Collider>();
            Rigidbody itemRigidbody = nearItem.GetComponent<Rigidbody>();
            itemColider.enabled = true;
            itemRigidbody.isKinematic = false;

            nearItem.layer = 0;

            isHold = false;
            isHoldRail = false;

            canSetZoneRight.SetActive(false);
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
