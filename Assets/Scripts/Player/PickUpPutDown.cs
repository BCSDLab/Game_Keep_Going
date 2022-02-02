using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * use gravity�� �׻� false
 * is kinematic�� �׻� true
 * is trigger�� �׻� true
 * tag�� �׻� Rail
 * ��ġ �Ǳ� �� ������ ���̾� : Item, ��ġ �� �� ������ ���̾� : Default
 * 
 * �ٴڿ� �ִ� ������ ����(�Ǵ� ���� ���� �� �� �Ʒ� ����) : box collider = true
 * ������ ���� ������(1��) : box collider = false
 * �ٸ� ���� ���� �÷����� : box collider = false
 * ��ġ�� ���� : box collider = true
 */

// spaceŰ�� �۵��ϴ� �ൿ(���� ����, ��������, ��ġ�ϱ� ��)
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
    private GameObject lastBeforeRail; // ��ġ�� ������ ���� ����
    private GameObject equipPoint;
    private GameObject nearItem;
    private GameObject holdItem;

    public Rail nearRail; 
    public Rail holdRail;
    

    // ���� ����
    private bool isHold = false;
	private bool isItemNear = false;
    public bool isHoldRail = false;



    void Start()
    {
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

    // ������ �ݱ� �õ�
    private void TryItemPickUp()
	{
        if(isItemNear && nearItem.layer == 6)
		{
            ItemPickUp();
		}
    }

    // ������ �ݱ�
    private void ItemPickUp()
	{
        nearRail = nearItem.GetComponent("Rail") as Rail; // �浹�� ������ Rail��ũ��Ʈ ��������
        //holdRail = holdItem.GetComponent("Rail") as Rail; // ����ִ� ������ Rail��ũ��Ʈ ��������

        // ���� �ݱ�
        if (nearItem.CompareTag("Rail"))
        {
            if (nearRail.getInt() <= 3)
            {
                holdItem = nearItem;
                nearItem = null;
                Debug.Log("���� ȹ��");
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
                Debug.Log(nearRail.getInt());
                nearItem.transform.GetChild(nearRail.getInt() - 2).SetParent(nearItem.transform.GetChild(nearRail.getInt() - 3));
                Debug.Log(nearRail.getInt());
                nearItem.transform.GetChild(nearRail.getInt() - 2).SetParent(nearItem.transform.GetChild(nearRail.getInt() - 3));
                Debug.Log(nearRail.getInt());
                holdItem = nearItem.transform.GetChild(nearRail.getInt() - 3).gameObject;


                nearItem = null;
                Debug.Log("���� ȹ��");
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

        // ���� ���
        else if (nearItem.CompareTag("Axe") || nearItem.CompareTag("Pickaxe"))
        {
            holdItem = nearItem;
            nearItem = null;
            Debug.Log("���� ȹ��");
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

    // ������ �������� �õ�
    private void TryItemPutDown()
	{
        ItemPutDown();
	}

    // ������ ��������, ���� ��ġ�ϱ�
    private void ItemPutDown()
	{

        // ���� �ٴڿ� ��������
        if (holdItem.CompareTag("Rail") && nearItem == null && canSetZoneFront.activeSelf == false && canSetZoneLeft.activeSelf == false && canSetZoneRight.activeSelf == false)
        {
            Debug.Log("���� ��������");

            equipPoint.transform.DetachChildren();
            holdItem.transform.position = equipPoint.transform.position - new Vector3(0, 1.2f, 0);

            Collider itemColider = holdItem.GetComponent<Collider>();
            //Rigidbody itemRigidbody = holdItem.GetComponent<Rigidbody>();
            itemColider.enabled = true;
            //itemRigidbody.isKinematic = false;

            isHold = false;
            isHoldRail = false;           
        }

		// ������ ���� ��� �� ���� �ױ�
		else if (holdItem.CompareTag("Rail") && nearItem != null && nearItem.CompareTag("Rail") && nearItem.layer == 6)
		{
            nearRail = nearItem.GetComponent("Rail") as Rail; // �浹�� ������ Rail��ũ��Ʈ ��������
            holdRail = holdItem.GetComponent("Rail") as Rail; // ����ִ� ������ Rail��ũ��Ʈ ��������
            Debug.Log("���� �ױ�");

            // �� �Ʒ��� ������ ��ġ ��ġ�� ����
            equipPoint.transform.DetachChildren();

            // ��� �ִ� ������ 1�� �� ��
            if (holdRail.getInt() == 1)
            {
                holdItem.transform.SetParent(nearItem.transform);

                holdItem.transform.position = nearItem.transform.position + new Vector3(0, 0.3f, 0) * nearRail.getInt();
                holdItem.transform.rotation = nearItem.transform.rotation;

                //holdItem.GetComponent<Rigidbody>().isKinematic = true;

                Debug.Log("������ 1�� ����");
            }
            // ��� �ִ� ������ 2�� �̻��� ��
            else if (holdRail.getInt() >= 2)
            {
                int i;
                for (i = 1; i < holdRail.getInt(); i++)
				{
                    // ù��° �ڽ�rail�� setparent�� ��ġ �ٲ����ϱ� �� �� 2,3��° �ڽ� ���� �ν��� ������ �Ǵ°���. i�� 1�� ����
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
                Debug.Log("������ 2�� �̻� ����");
            }

            isHold = false;
            isHoldRail = false;

        }

		// �տ� ���� ��ġ�ϱ�
		else if (holdItem.CompareTag("Rail") && canSetZoneFront.activeSelf == true)
		{
            holdRail = holdItem.GetComponent("Rail") as Rail; // ����ִ� ������ Rail��ũ��Ʈ ��������
            Debug.Log("���� ��ġ�ϱ�");

            if (holdRail.getInt() == 1) // ��� �ִ� ������ 1���� ��
            {
                equipPoint.transform.DetachChildren();
                lastBeforeRail = holdItem;

				isHold = false;
				isHoldRail = false;

                canSetZoneFront.SetActive(false);
            }
            else if(holdRail.getInt() > 1) // ��� �ִ� ������ 2�� �̻��� ��
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
        }

        // ���ʿ� ���� ��ġ�ϱ�
        else if(holdItem.CompareTag("Rail") && canSetZoneLeft.activeSelf == true)
		{
            holdRail = holdItem.GetComponent("Rail") as Rail;
            Debug.Log("���� ��ġ�ϱ�");

            Destroy(lastBeforeRail);
            Transform lastBeforeRailT = lastBeforeRail.transform;
            GameObject curvedRailObject = Instantiate(curvedRail);
            curvedRailObject.transform.position = lastBeforeRailT.position;

            if (holdRail.getInt() == 1) // ��� �ִ� ������ 1�� �� ��
            {
                equipPoint.transform.DetachChildren();   

                lastBeforeRail = holdItem;

                isHold = false;
                isHoldRail = false;

                canSetZoneLeft.SetActive(false);
            }
            else if(holdRail.getInt() > 1) // ��� �ִ� ������ 2�� �̻��� ��
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

            lastBeforeRail.layer = 0;

            canSetZoneFrontS.isThereRail = false;
            canSetZoneLeftS.isThereRail = false;
            canSetZoneRightS.isThereRail = false;
        }

        // �����ʿ� ���� ��ġ�ϱ�
        else if (holdItem.CompareTag("Rail") && canSetZoneRight.activeSelf == true)
        {
            holdRail = holdItem.GetComponent("Rail") as Rail;
            Debug.Log("���� ��ġ�ϱ�");

            Destroy(lastBeforeRail);
            Transform lastBeforeRailT = lastBeforeRail.transform;
            GameObject curvedRailObject = Instantiate(curvedRail);
            curvedRailObject.transform.position = lastBeforeRailT.position;

            if (holdRail.getInt() == 1) // ��� �ִ� ������ 1�� �� ��
            {
                equipPoint.transform.DetachChildren();

                lastBeforeRail = holdItem;

                isHold = false;
                isHoldRail = false;

                canSetZoneRight.SetActive(false);
            }
            else if(holdRail.getInt() > 1) // ��� �ִ� ������ 2�� �̻��� ��
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

            lastBeforeRail.layer = 0;

            canSetZoneFrontS.isThereRail = false;
            canSetZoneLeftS.isThereRail = false;
            canSetZoneRightS.isThereRail = false;
        }

        // ���� ��������
        else if (holdItem.CompareTag("Axe") || holdItem.CompareTag("Pickaxe"))
		{
            Debug.Log("���� ��������");

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
            Debug.Log("������ ����(������)");
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
