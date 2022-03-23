using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainModuleMoving : MonoBehaviour
{
	private PickUpPutDown player;
	private GameObject trainMain;

	private bool hasPos = false;

	private int location = 0;

	void Start()
	{
		StartCoroutine(Wait());
		trainMain = GameObject.Find("train_mainmodule");
		player = GameObject.Find("player").GetComponent<PickUpPutDown>();
	}

	void Update()
	{
		if (!hasPos)
		{
			if (trainMain.transform.GetComponent<TrainMainMoving>().isMove)
				RailRoad();
			else
				StopAllCoroutines();
		}
	}

	private void RailRoad()
	{
		//if (player.GetRailRoad2().Count > 1)
		//{
		
		if (player.GetRailRoad2().Count - (location+1) > 0)
		{
			//Debug.Log(player.getRailQueue().Count);
			//Debug.Log(location);
			//Debug.Log(player.GetRailRoad2()[location]);
			StartCoroutine(TrainMove(player.GetRailRoad2()[location]));
			//StartCoroutine(TrainMove(player.GetRailRoad2()[0]));
			//railroad2.Add(player.GetRailRoad()[0]);
			//player.GetRailRoad2().RemoveAt(0);
		//}
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

		transform.position = desPos;
		hasPos = false;
		location++;
		//Debug.Log(desPos + " 한칸 이동 완료");

		//Debug.Log("2번쨰 기차가 참고한 레일 : " + rail);
	}
}
