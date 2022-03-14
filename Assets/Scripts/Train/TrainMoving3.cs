using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainMoving3 : MonoBehaviour
{
	[SerializeField]
	private PickUpPutDown player;
	private GameObject trainMain;

	private bool hasPos = false;

	void Start()
	{
		StartCoroutine(Wait());
		trainMain = GameObject.Find("train_mainmodule");
	}

	void Update()
	{
		if (!hasPos)
		{
			if (trainMain.transform.GetComponent<TrainMoving>().isMove)
				RailRoad();
			else
				StopAllCoroutines();
		}

	}

	private void RailRoad()
	{
		if (player.GetRailRoad3().Count > 1)
		{
			Debug.Log(trainMain.transform.GetComponent<TrainMoving>().isMove);
			StartCoroutine(TrainMove(player.GetRailRoad3()[0]));
			player.GetRailRoad3().RemoveAt(0);
		}
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

		Vector3 desPos = rail.transform.position;

		if (rail.layer == 0)
		{
			while (Vector3.SqrMagnitude(transform.position - desPos) >= 0.001f)
			{
				transform.position = Vector3.MoveTowards(transform.position, desPos, 0.3f * Time.deltaTime);
				yield return null;
			}
		}

		else if (rail.layer == 7)
		{
			Debug.Log("�������� ȸ��");

			while ((Vector3.SqrMagnitude(transform.position - desPos) >= 0.001f))
			{
				transform.position = Vector3.MoveTowards(transform.position, desPos, 0.3f * Time.deltaTime);
				transform.Rotate(new Vector3(0, -17f * Time.deltaTime, 0));
				yield return null;
			}
		}

		else if (rail.layer == 8)
		{
			Debug.Log("���������� ȸ��");

			while ((Vector3.SqrMagnitude(transform.position - desPos) >= 0.001f))
			{
				transform.position = Vector3.MoveTowards(transform.position, desPos, 0.3f * Time.deltaTime);
				transform.Rotate(new Vector3(0, 17f * Time.deltaTime, 0));
				yield return null;
			}
		}

		transform.position = desPos;
		hasPos = false;
		Debug.Log(desPos + " ��ĭ �̵� �Ϸ�");

		Debug.Log("3���� ������ ������ ���� : " + rail);
	}
}
