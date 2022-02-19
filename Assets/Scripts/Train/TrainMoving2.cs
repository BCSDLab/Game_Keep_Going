using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainMoving2 : MonoBehaviour
{
	[SerializeField]
	private PickUpPutDown player;
	private GameObject trainMain;

	private bool hasPos = false;

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(Wait());
		trainMain = GameObject.Find("train_mainmodule");
	}

	// Update is called once per frame
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
		if (player.GetRailRoad2().Count > 1)
		{
			//Debug.Log(player.getRailQueue().Count);
			StartCoroutine(TrainMove(player.GetRailRoad2()[0]));
			//railroad2.Add(player.GetRailRoad()[0]);
			player.GetRailRoad2().RemoveAt(0);
		}
		//else
		//{
		//	StopAllCoroutines();
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
		//yield return new WaitForSeconds(5f);

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
			//Quaternion desRot = transform.rotation;
			//Debug.Log(desRot.y);
			//desRot.y -= 90;
			//Debug.Log(desRot.y);

			Debug.Log("왼쪽으로 회전");
			//transform.Rotate(0, -90, 0);

			while ((Vector3.SqrMagnitude(transform.position - desPos) >= 0.001f)/* && (Quaternion.Angle(transform.rotation, desRot) < 0.5f)*/)
			{
				transform.position = Vector3.MoveTowards(transform.position, desPos, 0.3f * Time.deltaTime);
				//transform.rotation = Quaternion.RotateTowards(transform.rotation, desRot, 0.3f * Time.deltaTime);
				transform.Rotate(new Vector3(0, -17f * Time.deltaTime, 0));
				yield return null;
			}
		}

		else if (rail.layer == 8)
		{
			//Quaternion desRot = transform.rotation;
			//Debug.Log(desRot.y);
			//desRot.y += 90;
			//Debug.Log(desRot.y);

			Debug.Log("오른쪽으로 회전");
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
		Debug.Log(desPos + " 한칸 이동 완료");

		Debug.Log("2번쨰 기차가 참고한 레일 : " + rail);
	}














	/*
	[SerializeField]
	private TrainMoving train;
	private List<GameObject> railroad3;

	private bool hasPos = false;

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(Wait());
		railroad3 = new List<GameObject>();
	}

	// Update is called once per frame
	void Update()
	{
		if (!hasPos)
			RailRoad();
	}

	public List<GameObject> GetRailRoad3()
	{
		return railroad3;
	}

	private void RailRoad()
	{
		if (train.GetRailList2().Count != 0)
		{
			//Debug.Log(train.GetRailList2().Count);
			StartCoroutine(TrainMove(train.GetRailList2()[0]));
			train.GetRailList2().RemoveAt(0);
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

			Debug.Log("왼쪽으로 회전");
			//transform.Rotate(0, -90, 0);

			while ((Vector3.SqrMagnitude(transform.position - desPos) >= 0.001f))
			{
				transform.position = Vector3.MoveTowards(transform.position, desPos, 0.3f * Time.deltaTime);
				transform.Rotate(new Vector3(0, -17f * Time.deltaTime, 0));
				yield return null;
			}
		}

		else if (rail.layer == 8)
		{
			Debug.Log("오른쪽으로 회전");
			//transform.Rotate(0, 90, 0);

			while ((Vector3.SqrMagnitude(transform.position - desPos) >= 0.001f))
			{
				transform.position = Vector3.MoveTowards(transform.position, desPos, 0.3f * Time.deltaTime);
				transform.Rotate(new Vector3(0, 17f * Time.deltaTime, 0));
				yield return null;
			}
		}

		transform.position = desPos;
		hasPos = false;
		railroad3.Add(rail);
	}
	*/
}
