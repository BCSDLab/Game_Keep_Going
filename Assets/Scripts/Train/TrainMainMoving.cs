using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 이동속도 0.3, 회전속도 17
 * 이동속도 0.1, 회전속도 5.7
 */


public class TrainMainMoving : MonoBehaviour
{
    [SerializeField]
	private PickUpPutDown player;

	private bool hasPos = false;
	public bool isMove = false; // 레일 끝에 도착했을 때 메인 기차가 멈추면 뒤의 기차도 멈추게 하기 위한 변수

	private int location = 0;


	void Start()
	{
		StartCoroutine(Wait());
		player = GameObject.Find("player").GetComponent<PickUpPutDown>();
	}

	void Update()
	{
		if (GameObject.Find("train_breakingmodule_parent").transform.GetChild(0).gameObject.activeSelf == true)
		{
			if (GameObject.Find("train_breakingmodule_parent").transform.GetChild(0).GetComponent<TrainBrake>().getWoodPut())
			{
				StopAllCoroutines();
				StartCoroutine(trainBrake());
			}
		}

		if (!hasPos)
			RailRoad();

	}

	IEnumerator trainBrake()
	{
		yield return new WaitForSeconds(5f);

		hasPos = false;
		Debug.Log("hasPos " + hasPos);
	}

	private void RailRoad()
	{
		//if (player.GetRailRoad().Count != 0)
		if(player.GetRailRoad().Count - location > 0)
		{
			isMove = true;
			//Debug.Log(player.GetRailRoad().Count);
			//Debug.Log(player.GetRailRoad()[0]);
			StartCoroutine(TrainMove(player.GetRailRoad()[location]));
			//player.GetRailRoad().RemoveAt(0);
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
		location++;

		isMove = false;
	}
}
