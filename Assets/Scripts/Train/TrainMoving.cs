using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * �̵��ӵ� 0.3, ȸ���ӵ� 17
 * �̵��ӵ� 0.1, ȸ���ӵ� 5.7
 */

public class TrainMoving : MonoBehaviour
{
    [SerializeField]
    private PickUpPutDown player;

    private bool hasPos = false;
    public bool isMove = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wait());
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasPos)
            RailRoad();

    }

    private void RailRoad()
	{
        if (player.GetRailRoad().Count != 0)
        {
            isMove = true;
            //Debug.Log(player.getRailQueue().Count);
            StartCoroutine(TrainMove(player.GetRailRoad()[0]));
            //railroad2.Add(player.GetRailRoad()[0]);
            player.GetRailRoad().RemoveAt(0);
		}
		//else
		//{
  //          isMove = false;
		//}
	}

    IEnumerator Wait()
	{
        hasPos = true;
        yield return new WaitForSeconds(5f);
        hasPos = false;
	}

    IEnumerator TrainMove(GameObject rail)
	{
        hasPos = true;
        //isMove = true;

        Vector3 desPos = rail.transform.position;
        

        if (rail.layer == 0)
        {
            while (Vector3.SqrMagnitude(transform.position - desPos) >= 0.001f)
            {
                transform.position = Vector3.MoveTowards(transform.position, desPos, 0.3f * Time.deltaTime);
                yield return null;
            }
        }

        else if(rail.layer == 7)
		{
            //Quaternion desRot = transform.rotation;
            //Debug.Log(desRot.y);
            //desRot.y -= 90;
            //Debug.Log(desRot.y);

            Debug.Log("�������� ȸ��");
            //transform.Rotate(0, -90, 0);

            while ((Vector3.SqrMagnitude(transform.position - desPos) >= 0.001f)/* && (Quaternion.Angle(transform.rotation, desRot) < 0.5f)*/)
            {
                transform.position = Vector3.MoveTowards(transform.position, desPos, 0.3f * Time.deltaTime);
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, desRot, 0.3f * Time.deltaTime);
                transform.Rotate(new Vector3(0, -17f * Time.deltaTime, 0));
                yield return null;
            }  
		}

        else if(rail.layer == 8)
		{
            //Quaternion desRot = transform.rotation;
            //Debug.Log(desRot.y);
            //desRot.y += 90;
            //Debug.Log(desRot.y);

            Debug.Log("���������� ȸ��");
            //transform.Rotate(0, 90, 0);

            while ((Vector3.SqrMagnitude(transform.position - desPos) >= 0.001f)/* && (Quaternion.Angle(transform.rotation, desRot) < 0.5f)*/)
            {
                transform.position = Vector3.MoveTowards(transform.position, desPos, 0.3f * Time.deltaTime);
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, desRot, 0.3f * Time.deltaTime);
                transform.Rotate(new Vector3(0, 17f * Time.deltaTime, 0));
                yield return null;
            }
        }

        transform.position = desPos;
        hasPos = false;
        isMove = false;
        Debug.Log(desPos + " ��ĭ �̵� �Ϸ�");
    }
}
