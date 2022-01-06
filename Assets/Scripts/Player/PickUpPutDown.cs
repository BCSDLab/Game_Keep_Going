using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpPutDown : MonoBehaviour
{
    private GameObject equipPoint;
    private GameObject nearItem;

    // ���� ����
    private bool isHold = false;
	private bool isItemNear = false;

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
        if(isItemNear)
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

            Collider itemColider = nearItem.GetComponent<Collider>();
            Rigidbody itemRigidbody = nearItem.GetComponent<Rigidbody>();
            itemColider.enabled = false;
            itemRigidbody.isKinematic = true;

            isHold = true;
        }
	}

    private void TryItemPutDown()
	{
        ItemPutDown();
	}

    private void ItemPutDown()
	{
        Debug.Log("������ ��������");

        equipPoint.transform.DetachChildren();

        nearItem.transform.position = equipPoint.transform.position - new Vector3(0, 0.8f, 0);

        Collider itemColider = nearItem.GetComponent<Collider>();
        Rigidbody itemRigidbody = nearItem.GetComponent<Rigidbody>();
        itemColider.enabled = true;
        itemRigidbody.isKinematic = false;   

        isHold = false;
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
