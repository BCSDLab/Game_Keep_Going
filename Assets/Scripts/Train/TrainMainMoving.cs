using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 이동속도 0.3, 회전속도 17
 * 이동속도 0.1, 회전속도 5.7
 */


public class TrainMainMoving : MonoBehaviour
{
	private PickUpPutDown player;

	private bool hasPos = false;
	public bool isMove = false;
	[SerializeField]
	private bool woodPut = false; // 브레이크 모듈에 사용되는 변수

	void Start()
	{
		StartCoroutine(Wait());
		player = GameObject.Find("player").GetComponent<PickUpPutDown>();
	}

	void Update()
	{
		if (woodPut)
			TrainBrake();
		if (!hasPos)
			RailRoad();

	}

	private void TrainBrake()
	{
		StartCoroutine(Brake());
	}

	IEnumerator Brake()
	{
		//Debug.Log("기차 멈추기");
		hasPos = true;
		yield return new WaitForSeconds(5f);
		woodPut = false;
		hasPos = false;
	}

	private void RailRoad()
	{
		if (player.GetRailRoad().Count != 0)
		{
			isMove = true;
			//Debug.Log(player.GetRailRoad().Count);
			Debug.Log(player.GetRailRoad()[0]);
			StartCoroutine(TrainMove(player.GetRailRoad()[0]));
			player.GetRailRoad().RemoveAt(0);
		}
		//else
		//{
		//	isMove = false;
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

			while ((Vector3.SqrMagnitude(transform.position - desPos) >= 0.001f))
			{
				transform.position = Vector3.MoveTowards(transform.position, desPos, 0.3f * Time.deltaTime);
				transform.Rotate(new Vector3(0, -17f * Time.deltaTime, 0));
				yield return null;
			}
		}

		else if (rail.layer == 8)
		{

			while ((Vector3.SqrMagnitude(transform.position - desPos) >= 0.001f))
			{
				transform.position = Vector3.MoveTowards(transform.position, desPos, 0.3f * Time.deltaTime);
				transform.Rotate(new Vector3(0, 17f * Time.deltaTime, 0));
				yield return null;
			}
		}
		//Debug.Log("한칸 이동 완료");
		transform.position = desPos;
		hasPos = false;

		//if(player.GetRailRoad().Count == 0)
		//	isMove = false;
	}
}
