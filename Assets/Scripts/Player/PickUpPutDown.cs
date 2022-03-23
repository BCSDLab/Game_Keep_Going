using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* <����>
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
    private GameObject canSetZoneFront;
    private GameObject canSetZoneLeft;
    private GameObject canSetZoneRight;
    private CanSetZone canSetZoneFrontS;
    private CanSetZone canSetZoneLeftS;
    private CanSetZone canSetZoneRightS;
    private GameObject lastRailPos;
    private GameObject curvedRail;
    private GameObject lastBeforeRail; // ��ġ�� ������ ���� ����
    private GameObject equipPoint;
    private GameObject nearItem;
    private GameObject holdItem;

    public Rail nearRail;
    public Rail holdRail;

    private List<GameObject> railroad1;
    private List<GameObject> railroad2;

    // ���� ����
    private bool isHold = false;
	private bool isItemNear = false;
    public bool isHoldRail = false;


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
        lastBeforeRail = GameObject.Find("FIxedRail").transform.GetChild(7).gameObject;
        equipPoint = GameObject.FindGameObjectWithTag("EquipPoint");
        railroad1 = new List<GameObject>();
        railroad2 = new List<GameObject>();
        AddRailToList(lastBeforeRail);
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
        /// <summary>
        /// �ٴڿ� ������ 3�� �׿��ְ� ������ �ٴڰ� ù��°�� ����� ������ rail, rail���� ������ rail1, rail1���� ������ rail2�� �ϸ�
        /// rail�� rail1, rail2�� �θ� ������Ʈ�� �ȴ�. �׸��� rail�� 0��° �ڽ��� rail�� defalt������Ʈ, rail�� 1���� �ڽ��� rail1, 2��° �ڽ��� rail2�� �ȴ�.
        /// �̷��� �� ������ �� �Ʒ��� �θ� ������Ʈ�� box collider�� Ű�� ������ �ڽ� ���� ������Ʈ�� box collider�� ������ �̷��� �ߴ�.
        /// 
        /// �׿��ִ� ������ 3�� ������ �� : �÷��̾ ���� ���� ������ ���� �� rail�� �ν��� ���̰�, space�� ������ rail�� equipPoint�� �ڽĿ�����Ʈ�� �ǰ�
        /// rail�� �������� equipPoint�� ���������� ��������.
        /// �׿��ִ� ������ 3������ ���� �� : �׿��ִ� ������ �ٴڿ������� rail, rail1, rail2, rail3, rail4 �̷��� �ִٰ� �ϸ�
        /// �÷��̾ rail�� �������� �� space�� ������ rail3�� rail2�� �ڽ� ������Ʈ�� �ǰ� rail4�� rail2�� �ڽ� ������Ʈ�� �ȴ�.
        /// �� �� rail2�� equipPoint�� �ڽĿ�����Ʈ�� �ǰ� rail2�� �������� equipPoint�� ���������� ��������.
        /// </summary>
        if (nearItem.CompareTag("Rail"))
        {
            // �׿��ִ� ������ 3�� ������ ��
            if (nearRail.getInt() <= 3)
            {
                holdItem = nearItem;
                nearItem = null;
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
            // �׿��ִ� ������ 3������ ���� ��
            else if (nearRail.getInt() > 3)
            {
                //Debug.Log(nearRail.getInt());
                nearItem.transform.GetChild(nearRail.getInt() - 2).SetParent(nearItem.transform.GetChild(nearRail.getInt() - 3));
                //Debug.Log(nearRail.getInt());
                nearItem.transform.GetChild(nearRail.getInt() - 2).SetParent(nearItem.transform.GetChild(nearRail.getInt() - 3));
                //Debug.Log(nearRail.getInt());
                holdItem = nearItem.transform.GetChild(nearRail.getInt() - 3).gameObject;


                nearItem = null;
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
        /// <summary>
        /// �ٴڿ� �׿��ִ� ������ �ٴڿ������� rail, rail1, rail2 �̷��� �ִٰ� ����.
        /// �÷��̾ ����ִ� ���� 3���� �� �״� ������ �����ϸ�
        /// �÷��̾ ���� ���� ������ ���� �÷��̾ rail�� �����ϰ�, space�� ������ ��� �ִ� 3���� ������ rail�� �ڽ� ������Ʈ�� �ȴ�.
        /// ��� �ִ� ������ rail�� �ڽ� ������Ʈ�� �Ǵ� �����δ� ��� �ִ� ������ ù���� �ڽ� ���� -> ��� �ִ� ������ �ι�° �ڽ� ���� -> ��� �ִ� ���Ͽ��� �θ� �Ǵ� ���� ���̴�.
        /// </summary>
        else if (holdItem.CompareTag("Rail") && nearItem != null && nearItem.CompareTag("Rail") && nearItem.layer == 6)
		{
            nearRail = nearItem.GetComponent("Rail") as Rail; // �浹�� ������ Rail��ũ��Ʈ ��������
            holdRail = holdItem.GetComponent("Rail") as Rail; // ����ִ� ������ Rail��ũ��Ʈ ��������

            // �� �Ʒ��� ������ ��ġ ��ġ�� ����
            equipPoint.transform.DetachChildren();

            // ��� �ִ� ������ 1�� �� ��
            if (holdRail.getInt() == 1)
            {
                holdItem.transform.SetParent(nearItem.transform);

                holdItem.transform.position = nearItem.transform.position + new Vector3(0, 0.3f, 0) * nearRail.getInt();
                holdItem.transform.rotation = nearItem.transform.rotation;

                //holdItem.GetComponent<Rigidbody>().isKinematic = true;
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
            }

            isHold = false;
            isHoldRail = false;

        }

		// �տ� ���� ��ġ�ϱ�
		else if (holdItem.CompareTag("Rail") && canSetZoneFront.activeSelf == true)
        { 
            holdRail = holdItem.GetComponent("Rail") as Rail; // ����ִ� ������ Rail��ũ��Ʈ ��������

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

            AddRailToList(lastBeforeRail);
            //railroad1.Add(lastBeforeRail);
            //railroad2.Add(lastBeforeRail);
            //railroad3.Add(lastBeforeRail);
            //Debug.Log(railroad.Peek().position);
        }

        // ���ʿ� ���� ��ġ�ϱ�
        else if(holdItem.CompareTag("Rail") && canSetZoneLeft.activeSelf == true)
		{
            holdRail = holdItem.GetComponent("Rail") as Rail;

            RemoveRailToList(lastBeforeRail);
            //railroad1.Remove(lastBeforeRail);
            //railroad2.Remove(lastBeforeRail);
            //railroad3.Remove(lastBeforeRail);
            Destroy(lastBeforeRail);
            Transform lastBeforeRailT = lastBeforeRail.transform;
            GameObject curvedRailObject = Instantiate(curvedRail);
            int lastBeforeRailLayer = lastBeforeRail.layer;
            curvedRailObject.transform.position = lastBeforeRailT.position;
            curvedRailObject.layer = lastBeforeRailLayer;
            AddRailToList(curvedRailObject);
            //railroad1.Add(curvedRailObject);
            //railroad2.Add(curvedRailObject);
            //railroad3.Add(curvedRailObject);

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

            //lastBeforeRail.layer = 0;
            lastBeforeRail.layer = 7;

            canSetZoneFrontS.isThereRail = false;
            canSetZoneLeftS.isThereRail = false;
            canSetZoneRightS.isThereRail = false;

            AddRailToList(lastBeforeRail);
            //railroad1.Add(lastBeforeRail);
            //railroad2.Add(lastBeforeRail);
            //railroad3.Add(lastBeforeRail);
            //Debug.Log(railroad.Peek().position);
        }

        // �����ʿ� ���� ��ġ�ϱ�
        else if (holdItem.CompareTag("Rail") && canSetZoneRight.activeSelf == true)
        {
            holdRail = holdItem.GetComponent("Rail") as Rail;

            RemoveRailToList(lastBeforeRail);
            //railroad1.Remove(lastBeforeRail);
            //railroad2.Remove(lastBeforeRail);
            //railroad3.Remove(lastBeforeRail);
            Destroy(lastBeforeRail);
            Transform lastBeforeRailT = lastBeforeRail.transform;
            GameObject curvedRailObject = Instantiate(curvedRail);
            int lastBeforeRailLayer = lastBeforeRail.layer;
            curvedRailObject.transform.position = lastBeforeRailT.position;
            curvedRailObject.layer = lastBeforeRailLayer;
            AddRailToList(curvedRailObject);
            //railroad1.Add(curvedRailObject);
            //railroad2.Add(curvedRailObject);
            //railroad3.Add(curvedRailObject);

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

            //lastBeforeRail.layer = 0;
            lastBeforeRail.layer = 8;

            canSetZoneFrontS.isThereRail = false;
            canSetZoneLeftS.isThereRail = false;
            canSetZoneRightS.isThereRail = false;

            AddRailToList(lastBeforeRail);
            //railroad1.Add(lastBeforeRail);
            //railroad2.Add(lastBeforeRail);
            //railroad3.Add(lastBeforeRail);
            //Debug.Log(railroad.Peek().position);
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
