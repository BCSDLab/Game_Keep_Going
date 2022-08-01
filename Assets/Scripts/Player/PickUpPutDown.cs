using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* <레일>
 * use gravity는 항상 false
 * is kinematic은 항상 true
 * is trigger는 항상 true
 * tag는 항상 Rail
 * 설치 되기 전 레일의 레이어 : Item, 설치 된 후 레일의 레이어 : Default
 * 
 * 바닥에 있는 아이템 레일(또는 쌓인 레일 중 맨 아래 레일) : box collider = true
 * 레일을 집고 있으면(1개) : box collider = false
 * 다른 레일 위에 올려지면 : box collider = false
 * 설치된 레일 : box collider = true
 */

// space키로 작동하는 행동(레일 집기, 내려놓기, 설치하기 등)

public class PickUpPutDown : MonoBehaviour
{
	private GameObject canSetZoneFront;
	private GameObject canSetZoneLeft;
	private GameObject canSetZoneRight;
	private CanSetZone canSetZoneFrontS;
	private CanSetZone canSetZoneLeftS;
	private CanSetZone canSetZoneRightS;
	[SerializeField]
	private GameObject lastRailPos;
	private GameObject curvedRail;
	[SerializeField]
	private GameObject lastBeforeRail; // 설치한 레일의 이전 레일
	private GameObject equipPoint;
	[SerializeField]
	private GameObject nearItem;
	[SerializeField]
	private GameObject holdItem;

	public Rail nearRail;
	public Rail holdRail;
	private WoodStack nearWood;
	private WoodStack holdWood;
	private RockStack nearRock;
	private RockStack holdRock;

	private TrainBrake canWoodPut;
	private TrainConversion canBlockPut;
	private TrainInvisible canRockPut;

	private Vector3 PutDownPosition = Vector3.zero;

	private List<GameObject> railroad1;
	private List<GameObject> railroad2;

	// 상태 변수
	[SerializeField]
	private bool isHold = false;
	[SerializeField]
	private bool isItemNear = false;

	public bool isHoldRail = false;
	private bool isTrainNear = false;

	private bool isModule1Empty = false;
	private bool isModule2Empty = false;
	private bool isModule3Empty = false;
	private bool isModule4Empty = false;
	private bool isModule5Empty = false;
	private bool isModule6Empty = false;


	public bool IsHoldRail()
	{
		return (isHold && holdItem.tag == "Rail");
	}

	public bool IsHold()
	{
		return isHold;
	}

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

		//lastBeforeRail = GameObject.Find("FixedRail");
		//lastBeforeRail = lastBeforeRail.transform.GetChild(7).gameObject;

		equipPoint = GameObject.FindGameObjectWithTag("EquipPoint");
		if (GameObject.Find("FIxedRail") != null)
		{
			lastBeforeRail = GameObject.Find("FIxedRail").transform.GetChild(7).gameObject;
			railroad1 = new List<GameObject>();
			railroad2 = new List<GameObject>();
			AddRailToList(lastBeforeRail);
		}

		if (GameObject.Find("Train").transform.Find("train_breakingmodule") != null)
		{
			if (GameObject.Find("Train").transform.Find("train_breakingmodule").gameObject.activeSelf == true)
			{
				canWoodPut = GameObject.Find("Train").transform.Find("train_breakingmodule").gameObject.GetComponent<TrainBrake>();
			}
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (!isHold)
			{
				TryItemPickUp();
				ModulePickUp(); // 이 함수는 상점에서만 호출될 수 있도록 코드 추가 필요
			}
			else
			{
				TryItemPutDown();
				if (GameObject.Find("Train").transform.Find("train_breakingmodule") != null)
				{
					if (GameObject.Find("Train").transform.Find("train_breakingmodule").gameObject.activeSelf == true)
					{
						WoodPutToBrake();
					}
				}
				if (GameObject.Find("Train").transform.Find("train_conversionmodule") != null)
				{
					if (GameObject.Find("Train").transform.Find("train_conversionmodule").gameObject.activeSelf == true)
					{
						BlockPutToConvert();
					}
				}
				if (GameObject.Find("Train").transform.Find("train_platformmodule") != null)
				{
					if (GameObject.Find("Train").transform.Find("train_platformmodule").gameObject.activeSelf == true)
					{
						RockPutForPlatform();
					}
				}
			}
		}
	}

	private void RockPutForPlatform()
	{
		canRockPut = GameObject.Find("train_platformmodule").transform.GetComponent<TrainInvisible>();
		if (canRockPut.GetCanRockPut())
		{
			GameObject.Find("train_platformmodule").transform.GetComponent<TrainInvisible>().SetRockPut(true);
			Object.Destroy(holdItem);
			holdItem = null;
			Debug.Log("isHold " + isHold);
		}
	}

	// 플레이어가 블럭 넣으면 rockPut이런거 true로 해주기, holdItem은 destroy
	private void BlockPutToConvert()
	{
		// 플레이어가 변환 모듈과 충돌한 상태이고, holdItem이 null이 아니고, holdItem이 WoodStack이거나 RockStack이면 true
		canBlockPut = GameObject.Find("train_conversionmodule").transform.GetComponent<TrainConversion>();
		if (canBlockPut.getCanBlockPut())
		{
			if (holdItem.CompareTag("RockStack"))
			{
				GameObject.Find("train_conversionmodule").transform.GetComponent<TrainConversion>().setRockPut(true);
			}
			else if (holdItem.CompareTag("WoodStack"))
			{
				GameObject.Find("train_conversionmodule").transform.GetComponent<TrainConversion>().setWoodPut(true);
			}
			Object.Destroy(holdItem);
			canBlockPut.setCanBlockPut(false);
			holdItem = null;
		}
	}

	private void WoodPutToBrake()
	{
		canWoodPut = GameObject.Find("Train").transform.Find("train_breakingmodule").gameObject.GetComponent<TrainBrake>();
		if (canWoodPut.GetCanWoodPut())
		{
			// 들고 있던 나무 블럭 destroy하기
			// woodPut = true로 하기
			GameObject.Find("Train").transform.Find("train_breakingmodule").gameObject.GetComponent<TrainBrake>().setWoodPut(true);
			Object.Destroy(holdItem);
			holdItem = null;


			Debug.Log("나무 블럭이 적용됨!!!!!");
		}
	}


	public GameObject GetHoldItem()
	{
		return holdItem;
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


	// 아이템 줍기 시도
	private void TryItemPickUp()
	{
		if (nearItem != null)
		{
			if (isItemNear && nearItem.layer == 6)
			{
				ItemPickUp();
			}
		}
	}

	// 설치된 모듈 줍기
	private void ModulePickUp()
	{
		if (nearItem != null)
		{
			if (nearItem.CompareTag("Train"))
			{
				if (nearItem.transform.parent.CompareTag("ModuleCase"))
				{
					holdItem = nearItem;
					nearItem = null;

					if (holdItem.transform.parent.name == "ModuleCase1")
					{
						TrainSingleton.instance.moduleList["Module1"] = 0;
					}
					else if (holdItem.transform.parent.name == "ModuleCase2")
					{
						TrainSingleton.instance.moduleList["Module2"] = 0;
					}
					else if (holdItem.transform.parent.name == "ModuleCase3")
					{
						TrainSingleton.instance.moduleList["Module3"] = 0;
					}
					else if (holdItem.transform.parent.name == "ModuleCase4")
					{
						TrainSingleton.instance.moduleList["Module4"] = 0;
					}
					else if (holdItem.transform.parent.name == "ModuleCase5")
					{
						TrainSingleton.instance.moduleList["Module5"] = 0;
					}
					else if (holdItem.transform.parent.name == "ModuleCase6")
					{
						TrainSingleton.instance.moduleList["Module6"] = 0;
					}

					Debug.Log("설치되었던 모듈 주움!");
					holdItem.transform.SetParent(equipPoint.transform);
					holdItem.transform.localPosition = Vector3.zero;
					holdItem.transform.localPosition = Vector3.right + Vector3.up * -0.7f + Vector3.forward * 0.1f;
					holdItem.transform.rotation = new Quaternion(0, 0, 0, 0);
					if (holdItem.name == "train_mainmodule")
					{
						holdItem.transform.Rotate(new Vector3(0, 180, 0));
					}

					TrainSingleton.instance.PullTrain();

					holdItem.GetComponent<Collider>().enabled = false;
					holdItem.layer = 6;

					isHold = true;


				}
			}
		}
	}

	// 아이템 줍기
	private void ItemPickUp()
	{
		nearRail = nearItem.GetComponent("Rail") as Rail; // 충돌한 레일의 Rail스크립트 가져오기
														  //holdRail = holdItem.GetComponent("Rail") as Rail; // 들고있는 레일의 Rail스크립트 가져오기
		nearWood = nearItem.GetComponent("WoodStack") as WoodStack;
		nearRock = nearItem.GetComponent("RockStack") as RockStack;


		// 레일 줍기
		/// <summary>
		/// 바닥에 레일이 3개 쌓여있고 씬에서 바닥과 첫번째로 가까운 레일을 rail, rail위의 레일을 rail1, rail1위의 레일을 rail2라 하면
		/// rail은 rail1, rail2의 부모 오브젝트가 된다. 그리고 rail의 0번째 자식은 rail의 defalt오브젝트, rail의 1번 자식은 rail1, 2번째 자식은 rail2가 된다.
		/// 이렇게 한 이유는 맨 아래의 부모 오브젝트만 box collider를 키고 나머지 자식 레일 오브젝트는 box collider를 끄려고 이렇게 했다.
		/// 
		/// 쌓여있는 레일이 3개 이하일 때 : 플레이어가 쌓인 레일 쪽으로 갔을 때 rail을 인식할 것이고, space를 누르면 rail은 equipPoint의 자식오브젝트가 되고
		/// rail의 포지션이 equipPoint의 포지션으로 맞춰진다.
		/// 쌓여있는 레일이 3개보다 많을 때 : 쌓여있는 레일이 바닥에서부터 rail, rail1, rail2, rail3, rail4 이렇게 있다고 하면
		/// 플레이어가 rail을 감지했을 때 space를 누르면 rail3이 rail2의 자식 오브젝트가 되고 rail4도 rail2의 자식 오브젝트가 된다.
		/// 그 뒤 rail2는 equipPoint의 자식오브젝트가 되고 rail2의 포지션이 equipPoint의 포지션으로 맞춰진다.
		/// </summary>
		/// 
		if (nearItem.CompareTag("Rail"))
		{
			Rail nearRail = nearItem.GetComponent<Rail>();
			nearRail = nearItem.GetComponent("Rail") as Rail; // 충돌한 레일의 Rail스크립트 가져오기
															  //holdRail = holdItem.GetComponent("Rail") as Rail; // 들고있는 레일의 Rail스크립트 가져오기
															  // 쌓여있는 레일이 3개 이하일 때
			if (nearRail.getInt() <= 3)
			{
				//develop
				holdItem = nearRail.DeleteRail(nearRail.getInt());
				nearItem = null;
				nearRail = null;
			}
			// 쌓여있는 레일이 3개보다 많을 때
			else if (nearRail.getInt() > 3)
			{
				//develop
				holdItem = nearRail.DeleteRail(3);
				nearItem = null;
				nearRail = null;
			}

			holdItem.transform.SetParent(equipPoint.transform);
			holdItem.transform.localPosition = Vector3.zero;
			holdItem.transform.rotation = new Quaternion(0, 0, 0, 0);
			holdItem.transform.Rotate(new Vector3(0, 90, 0));

			isHold = true;

		}

		// 도끼 들기, 곡괭이 들기
		else if (nearItem.CompareTag("Axe") || nearItem.CompareTag("Pickaxe"))
		{
			holdItem = nearItem;
			nearItem = null;
			Debug.Log("도끼 획득");
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

		// 나무 블럭 들기
		else if (nearItem.CompareTag("WoodStack"))
		{
			// 쌓여있는 나무블럭이 3개 이하일 때
			if (nearWood.getInt() <= 3)
			{
				holdItem = nearItem;
				nearItem = null;
				holdItem.transform.SetParent(equipPoint.transform);
				holdItem.transform.localPosition = Vector3.zero;
				holdItem.transform.rotation = new Quaternion(0, 0, 0, 0);
				holdItem.transform.Rotate(new Vector3(0, 90, 0));

				holdItem.GetComponent<Collider>().enabled = false;

				isHold = true;
			}
			// 쌓여있는 나무블럭이 3개보다 많을 때
			else if (nearWood.getInt() > 3)
			{
				nearItem.transform.GetChild(nearWood.getInt() - 2).SetParent(nearItem.transform.GetChild(nearWood.getInt() - 3));
				nearItem.transform.GetChild(nearWood.getInt() - 2).SetParent(nearItem.transform.GetChild(nearWood.getInt() - 3));
				holdItem = nearItem.transform.GetChild(nearWood.getInt() - 3).gameObject;


				nearItem = null;
				holdItem.transform.SetParent(equipPoint.transform);
				holdItem.transform.localPosition = Vector3.zero;
				holdItem.transform.rotation = new Quaternion(0, 0, 0, 0);
				holdItem.transform.Rotate(new Vector3(0, 90, 0));

				holdItem.GetComponent<Collider>().enabled = false;

				isHold = true;
			}
		}

		// 돌 블럭 들기
		else if (nearItem.CompareTag("RockStack"))
		{
			// 쌓여있는 돌 블럭이 3개 이하일 때
			if (nearRock.getInt() <= 3)
			{
				holdItem = nearItem;
				nearItem = null;
				holdItem.transform.SetParent(equipPoint.transform);
				holdItem.transform.localPosition = Vector3.zero;
				holdItem.transform.rotation = new Quaternion(0, 0, 0, 0);
				holdItem.transform.Rotate(new Vector3(0, 90, 0));

				holdItem.GetComponent<Collider>().enabled = false;

				isHold = true;
			}
			// 쌓여있는 돌 블럭이 3개보다 많을 때
			else if (nearRock.getInt() > 3)
			{
				nearItem.transform.GetChild(nearRock.getInt() - 2).SetParent(nearItem.transform.GetChild(nearRock.getInt() - 3));
				nearItem.transform.GetChild(nearRock.getInt() - 2).SetParent(nearItem.transform.GetChild(nearRock.getInt() - 3));
				holdItem = nearItem.transform.GetChild(nearRock.getInt() - 3).gameObject;


				nearItem = null;
				holdItem.transform.SetParent(equipPoint.transform);
				holdItem.transform.localPosition = Vector3.zero;
				holdItem.transform.rotation = new Quaternion(0, 0, 0, 0);
				holdItem.transform.Rotate(new Vector3(0, 90, 0));

				holdItem.GetComponent<Collider>().enabled = false;

				isHold = true;
			}
		}

		// 모듈 들기
		else if (nearItem.CompareTag("Train"))
		{
			holdItem = nearItem;
			nearItem = null;

			Debug.Log("아이템인 모듈 주움!");
			holdItem.transform.SetParent(equipPoint.transform);
			holdItem.transform.localPosition = Vector3.zero;
			holdItem.transform.localPosition = Vector3.right + Vector3.up * -0.7f + Vector3.forward * 0.1f;
			holdItem.transform.rotation = new Quaternion(0, 0, 0, 0);
			if (holdItem.name == "train_mainmodule")
			{
				holdItem.transform.Rotate(new Vector3(0, 180, 0));
			}

			holdItem.GetComponent<Collider>().enabled = false;

			isHold = true;
		}
	}

	// 아이템 내려놓기 시도
	private void TryItemPutDown()
	{
		ItemPutDown();
	}

	// 아이템 내려놓기, 레일 설치하기
	private void ItemPutDown()
	{
		// 레일 바닥에 내려놓기
		if (holdItem.CompareTag("Rail") && nearItem == null && canSetZoneFront.activeSelf == false && canSetZoneLeft.activeSelf == false && canSetZoneRight.activeSelf == false)
		{

			equipPoint.transform.DetachChildren();
			holdItem.transform.position = PutDownPosition + new Vector3(0, 1.6f, 0);
			holdItem.transform.rotation = Quaternion.identity;

			holdItem.GetComponent<Collider>().enabled = true;

			isHold = false;
			isHoldRail = false;
			holdItem = null;
		}

		// 레일이 있을 경우 그 위에 쌓기
		/// <summary>
		/// 바닥에 쌓여있는 레일이 바닥에서부터 rail, rail1, rail2 이렇게 있다고 하자.
		/// 플레이어가 들고있는 레일 3개를 더 쌓는 과정을 설명하면
		/// 플레이어가 쌓인 레일 쪽으로 가면 플레이어가 rail을 감지하고, space를 누르면 들고 있는 3개의 레일은 rail의 자식 오브젝트가 된다.
		/// 들고 있던 레일이 rail의 자식 오브젝트가 되는 순서로는 들고 있던 레일의 첫번 자식 레일 -> 들고 있던 레일의 두번째 자식 레일 -> 들고 있던 레일에서 부모가 되는 레일 순이다.
		/// </summary>
		else if (holdItem.CompareTag("Rail") && nearItem != null && nearItem.CompareTag("Rail") && nearItem.layer == 6)
		{
			nearRail = nearItem.GetComponent("Rail") as Rail; // 충돌한 레일의 Rail스크립트 가져오기
			holdRail = holdItem.GetComponent("Rail") as Rail; // 들고있는 레일의 Rail스크립트 가져오기

			// 맨 아래의 레일을 설치 위치로 설정
			equipPoint.transform.DetachChildren();

			// 들고 있는 레일이 1개 일 때
			if (holdRail.getInt() == 1)
			{
				holdItem.transform.SetParent(nearItem.transform);

				holdItem.transform.position = nearItem.transform.position + new Vector3(0, 0.3f, 0) * nearRail.getInt();
				holdItem.transform.rotation = nearItem.transform.rotation;
			}
			// 들고 있는 레일이 2개 이상일 때
			else if (holdRail.getInt() >= 2)
			{
				int i;
				for (i = 1; i < holdRail.getInt(); i++)
				{
					// 첫번째 자식rail을 setparent로 위치 바꿨으니까 그 뒤 2,3번째 자식 레일 인식이 문제가 되는것임. i를 1로 수정
					Debug.Log(i);
					holdItem.transform.GetChild(1).transform.position = nearItem.transform.position + new Vector3(0, 0.3f, 0) * (nearRail.getInt() + i - 1);
					holdItem.transform.GetChild(1).transform.rotation = nearItem.transform.rotation;
					holdItem.transform.GetChild(1).transform.SetParent(nearItem.transform);
				}

				holdItem.transform.SetParent(nearItem.transform);

				holdItem.transform.position = nearItem.transform.position + new Vector3(0, 0.3f, 0) * (nearRail.getInt() + i - 1);
				holdItem.transform.rotation = nearItem.transform.rotation;

				//holdItem.transform.DetachChildren();
			}

			isHold = false;
			isHoldRail = false;
			holdItem = null;
		}
		/*
//Develop          
//          else if (holdItem.CompareTag("Rail") && nearItem.CompareTag("Rail") && nearItem.layer == 6)
//          {
//            holdItem.GetComponent<Rail>().DisableAllCollider();
//            nearItem.GetComponent<Rail>().PileUpRail(holdItem);
//            holdItem = null;
//            isHold = false;
//          }
        }
        */

		// 앞에 레일 설치하기
		else if (holdItem.CompareTag("Rail") && canSetZoneFront.activeSelf == true)
		{
			holdRail = holdItem.GetComponent("Rail") as Rail; // 들고있는 레일의 Rail스크립트 가져오기
															  //Develop
															  // 앞에 레일 설치하기
															  //		if (holdItem.CompareTag("Rail") && canSetZoneFront.activeSelf == true)
															  //        {
															  //            Rail holdRail = holdItem.GetComponent<Rail>();


			if (holdRail.getInt() == 1) // 들고 있는 레일이 1개일 때
			{
				equipPoint.transform.DetachChildren();
				//develop       lastBeforeRail = holdRail.DeleteRail(1);


				isHold = false;
				isHoldRail = false;
				holdItem = null;

				canSetZoneFront.SetActive(false);
			}
			else if (holdRail.getInt() > 1) // 들고 있는 레일이 2개 이상일 때
			{
				//develop       lastBeforeRail = holdRail.DeleteRail(1);
				lastBeforeRail = holdItem.transform.GetChild(holdRail.getInt() - 1).gameObject;
				holdItem.transform.GetChild(holdRail.getInt() - 1).parent = null;

			}

			lastRailPos.transform.Translate(new Vector3(1.6f, 0, 0));
			lastBeforeRail.transform.position = lastRailPos.transform.position;

			lastBeforeRail.transform.rotation = lastRailPos.transform.rotation;
			lastBeforeRail.transform.Rotate(0, 90, 0);

			Collider itemColider = lastBeforeRail.GetComponent<Collider>();
			itemColider.enabled = true;

			lastBeforeRail.layer = 0;

			canSetZoneFrontS.isThereRail = false;
			canSetZoneLeftS.isThereRail = false;
			canSetZoneRightS.isThereRail = false;

			AddRailToList(lastBeforeRail);
		}

		// 왼쪽에 레일 설치하기

		else if (holdItem.CompareTag("Rail") && canSetZoneLeft.activeSelf == true)
		{
			holdRail = holdItem.GetComponent("Rail") as Rail;


			RemoveRailToList(lastBeforeRail);
			Destroy(lastBeforeRail);
			Transform lastBeforeRailT = lastBeforeRail.transform;
			GameObject curvedRailObject = Instantiate(curvedRail);
			int lastBeforeRailLayer = lastBeforeRail.layer;
			curvedRailObject.transform.position = lastBeforeRailT.position;
			curvedRailObject.layer = lastBeforeRailLayer;
			AddRailToList(curvedRailObject);

			if (holdRail.getInt() == 1) // 들고 있는 레일이 1개 일 때
			{
				equipPoint.transform.DetachChildren();


				lastBeforeRail = holdItem;
				//develop       lastBeforeRail = holdRail.DeleteRail(1);
				isHold = false;
				isHoldRail = false;
				holdItem = null;


				canSetZoneFront.SetActive(false);
			}

			else if (holdRail.getInt() > 1) // 들고 있는 레일이 2개 이상일 때
			{
				lastBeforeRail = holdItem.transform.GetChild(holdRail.getInt() - 1).gameObject;
				//develop       lastBeforeRail = holdRail.DeleteRail(1);                
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
			itemColider.enabled = true;
			lastBeforeRail.layer = 7;

			canSetZoneFrontS.isThereRail = false;
			canSetZoneLeftS.isThereRail = false;
			canSetZoneRightS.isThereRail = false;

			AddRailToList(lastBeforeRail);
		}

		// 오른쪽에 레일 설치하기
		else if (holdItem.CompareTag("Rail") && canSetZoneRight.activeSelf == true)
		{
			Rail holdRail = holdItem.GetComponent<Rail>();

			RemoveRailToList(lastBeforeRail);
			Destroy(lastBeforeRail);
			Transform lastBeforeRailT = lastBeforeRail.transform;
			GameObject curvedRailObject = Instantiate(curvedRail);
			int lastBeforeRailLayer = lastBeforeRail.layer;
			curvedRailObject.transform.position = lastBeforeRailT.position;
			curvedRailObject.layer = lastBeforeRailLayer;
			AddRailToList(curvedRailObject);

			if (holdRail.getInt() == 1) // 들고 있는 레일이 1개 일 때
			{
				equipPoint.transform.DetachChildren();
				//develop       lastBeforeRail = holdRail.DeleteRail(1);

				isHold = false;
				isHoldRail = false;
				holdItem = null;


				canSetZoneFront.SetActive(false);
			}

			else if (holdRail.getInt() > 1) // 들고 있는 레일이 2개 이상일 때
			{
				lastBeforeRail = holdItem.transform.GetChild(holdRail.getInt() - 1).gameObject;
				//develop       lastBeforeRail = holdRail.DeleteRail(1);                
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
			itemColider.enabled = true;

			lastBeforeRail.layer = 8;

			canSetZoneFrontS.isThereRail = false;
			canSetZoneLeftS.isThereRail = false;
			canSetZoneRightS.isThereRail = false;

			AddRailToList(lastBeforeRail);

		}

		// 도끼 내려놓기, 곡괭이 내려놓기, 나무 블럭 내려놓기, 돌 블럭 내려놓기
		else if (nearItem == null && (holdItem.CompareTag("Axe") || holdItem.CompareTag("Pickaxe") || holdItem.CompareTag("WoodStack") || holdItem.CompareTag("RockStack")))
		{
			Debug.Log("바닥에 내려놓기");
			Debug.Log(nearItem);

			equipPoint.transform.DetachChildren();
			holdItem.transform.position = equipPoint.transform.position - new Vector3(0, 1.2f, 0);

			holdItem.GetComponent<Collider>().enabled = true;

			isHold = false;
			holdItem = null;
		}

		// 나무 블럭이 있을 경우 그 위에 쌓기
		else if (holdItem.CompareTag("WoodStack") && nearItem != null && nearItem.CompareTag("WoodStack") && nearItem.layer == 6)
		{
			Debug.Log("나무블럭 위에 쌓기");

			nearWood = nearItem.GetComponent("WoodStack") as WoodStack; // 충돌한 나무 블럭의 WoodStack스크립트 가져오기
			holdWood = holdItem.GetComponent("WoodStack") as WoodStack; // 들고있는 나무 블럭의 WoodStack스크립트 가져오기

			// 맨 아래의 나무 블럭을 설치 위치로 설정
			equipPoint.transform.DetachChildren();

			// 들고 있는 나무 블럭이 1개 일 때
			if (holdWood.getInt() == 1)
			{
				holdItem.transform.SetParent(nearItem.transform);

				holdItem.transform.position = nearItem.transform.position + new Vector3(0, 0.3f, 0) * nearWood.getInt();
				holdItem.transform.rotation = nearItem.transform.rotation;
			}

			// 들고 있는 나무 블럭이 2개 이상일 때
			else if (holdWood.getInt() >= 2)
			{
				int i;
				for (i = 1; i < holdWood.getInt(); i++)
				{
					Debug.Log(i);
					holdItem.transform.GetChild(1).transform.position = nearItem.transform.position + new Vector3(0, 0.3f, 0) * (nearWood.getInt() + i - 1);
					holdItem.transform.GetChild(1).transform.rotation = nearItem.transform.rotation;
					holdItem.transform.GetChild(1).transform.SetParent(nearItem.transform);
				}

				holdItem.transform.SetParent(nearItem.transform);

				holdItem.transform.position = nearItem.transform.position + new Vector3(0, 0.3f, 0) * (nearWood.getInt() + i - 1);
				holdItem.transform.rotation = nearItem.transform.rotation;
			}
			isHold = false;
			holdItem = null;
		}

		// 돌 블럭이 있을 경우 그 위에 쌓기
		else if (holdItem.CompareTag("RockStack") && nearItem != null && nearItem.CompareTag("RockStack") && nearItem.layer == 6)
		{
			nearRock = nearItem.GetComponent("RockStack") as RockStack; // 충돌한 돌 블럭의 RockStack스크립트 가져오기
			holdRock = holdItem.GetComponent("RockStack") as RockStack; // 들고있는 돌 블럭의 RockStack스크립트 가져오기

			// 맨 아래의 돌 블럭을 설치 위치로 설정
			equipPoint.transform.DetachChildren();

			// 들고 있는 돌 블럭이 1개 일 때
			if (holdRock.getInt() == 1)
			{
				holdItem.transform.SetParent(nearItem.transform);

				holdItem.transform.position = nearItem.transform.position + new Vector3(0, 0.3f, 0) * nearRock.getInt();
				holdItem.transform.rotation = nearItem.transform.rotation;
			}

			// 들고 있는 돌 블럭이 2개 이상일 때
			else if (holdRock.getInt() >= 2)
			{
				int i;
				for (i = 1; i < holdRock.getInt(); i++)
				{
					Debug.Log(i);
					holdItem.transform.GetChild(1).transform.position = nearItem.transform.position + new Vector3(0, 0.3f, 0) * (nearRock.getInt() + i - 1);
					holdItem.transform.GetChild(1).transform.rotation = nearItem.transform.rotation;
					holdItem.transform.GetChild(1).transform.SetParent(nearItem.transform);
				}

				holdItem.transform.SetParent(nearItem.transform);

				holdItem.transform.position = nearItem.transform.position + new Vector3(0, 0.3f, 0) * (nearRock.getInt() + i - 1);
				holdItem.transform.rotation = nearItem.transform.rotation;
			}
			isHold = false;
			holdItem = null;
		}

		// 모듈 바닥에 내려놓기
		else if (!isTrainNear && nearItem == null && holdItem.CompareTag("Train"))
		{
			Debug.Log("모듈 바닥에 내려놓기");

			Transform holdItemT = holdItem.transform;

			equipPoint.transform.DetachChildren();
			holdItem.transform.position = holdItemT.position + new Vector3(0, -0.4f, 0);

			holdItem.GetComponent<Collider>().enabled = true;

			isHold = false;
			holdItem = null;
		}

		//모듈 기차에 붙이기 - 모듈의 레이어 바꾸기, 모듈의 포지션 조정, 위치한 ModuleCase의 자식 오브젝트로 두기
		else if (isTrainNear && holdItem.CompareTag("Train"))
		{
			if (!(isModule1Empty || isModule2Empty || isModule3Empty || isModule4Empty || isModule5Empty || isModule6Empty))
			{
				return;
			}


			equipPoint.transform.DetachChildren();

			if (isModule1Empty)
			{
				holdItem.transform.parent = GameObject.Find("Train").transform.Find("ModuleCase1").transform;
				Debug.Log("첫번째 모듈 케이스");

				holdItem.transform.localPosition = Vector3.zero;
				holdItem.transform.rotation = new Quaternion(0, 0, 0, 0);

				SetModuleDic("Module1");

				GameObject.Find("Train").transform.Find("ModuleCase1").transform.Find("Cube").gameObject.SetActive(false);
			}
			else if (isModule2Empty)
			{
				holdItem.transform.parent = GameObject.Find("Train").transform.Find("ModuleCase2").transform;
				Debug.Log("두번째 모듈 케이스");

				holdItem.transform.localPosition = Vector3.zero;
				holdItem.transform.rotation = new Quaternion(0, 0, 0, 0);

				SetModuleDic("Module2");

				GameObject.Find("Train").transform.Find("ModuleCase2").transform.Find("Cube").gameObject.SetActive(false);
			}
			else if (isModule3Empty)
			{
				holdItem.transform.parent = GameObject.Find("Train").transform.Find("ModuleCase3").transform;
				Debug.Log("세번째 모듈 케이스");

				holdItem.transform.localPosition = Vector3.zero;
				holdItem.transform.rotation = new Quaternion(0, 0, 0, 0);

				SetModuleDic("Module3");

				GameObject.Find("Train").transform.Find("ModuleCase3").transform.Find("Cube").gameObject.SetActive(false);
			}
			else if (isModule4Empty)
			{
				holdItem.transform.parent = GameObject.Find("Train").transform.Find("ModuleCase4").transform;
				Debug.Log("네번째 모듈 케이스");

				holdItem.transform.localPosition = Vector3.zero;
				holdItem.transform.rotation = new Quaternion(0, 0, 0, 0);

				SetModuleDic("Module4");

				GameObject.Find("Train").transform.Find("ModuleCase4").transform.Find("Cube").gameObject.SetActive(false);
			}
			else if (isModule5Empty)
			{
				holdItem.transform.parent = GameObject.Find("Train").transform.Find("ModuleCase5").transform;
				Debug.Log("다섯번째 모듈 케이스");

				holdItem.transform.localPosition = Vector3.zero;
				holdItem.transform.rotation = new Quaternion(0, 0, 0, 0);

				SetModuleDic("Module5");

				GameObject.Find("Train").transform.Find("ModuleCase5").transform.Find("Cube").gameObject.SetActive(false);
			}
			else if (isModule6Empty)
			{
				holdItem.transform.parent = GameObject.Find("Train").transform.Find("ModuleCase6").transform;
				Debug.Log("여섯번째 모듈 케이스");

				holdItem.transform.localPosition = Vector3.zero;
				holdItem.transform.rotation = new Quaternion(0, 0, 0, 0);

				SetModuleDic("Module6");

				GameObject.Find("Train").transform.Find("ModuleCase6").transform.Find("Cube").gameObject.SetActive(false);
			}

			holdItem.GetComponent<Collider>().enabled = true;
			holdItem.layer = 0;

			isHold = false;
			holdItem = null;


			//Debug.Log("모듈 배열" + TrainSingleton.instance.moduleList["Module1"]);
			//Debug.Log(TrainSingleton.instance.moduleList["Module2"]);
			//Debug.Log(TrainSingleton.instance.moduleList["Module3"]);
			//Debug.Log(TrainSingleton.instance.moduleList["Module4"]);
			//Debug.Log(TrainSingleton.instance.moduleList["Module5"]);
			//Debug.Log(TrainSingleton.instance.moduleList["Module6"]);
		}
	}

	private void SetModuleDic(string moduleNum)
	{
		if (holdItem.name == "train_mainmodule")
		{
			holdItem.transform.Rotate(new Vector3(0, 180, 0));
			TrainSingleton.instance.moduleList[moduleNum] = 1;
		}
		else if (holdItem.name == "train_railmakingmodule")
		{
			TrainSingleton.instance.moduleList[moduleNum] = 2;
		}
		else if (holdItem.name == "train_savemodule")
		{
			TrainSingleton.instance.moduleList[moduleNum] = 3;
		}
		else if (holdItem.name == "train_breakingmodule")
		{
			TrainSingleton.instance.moduleList[moduleNum] = 4;
		}
		else if (holdItem.name == "train_conversionmodule")
		{
			TrainSingleton.instance.moduleList[moduleNum] = 5;
		}
		else if (holdItem.name == "train_platformmodule")
		{
			TrainSingleton.instance.moduleList[moduleNum] = 6;
		}

		TrainSingleton.instance.PullTrain();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
		{
			//Debug.Log("아이템 감지(반투명)");
			isItemNear = true;
			nearItem = other.gameObject;
		}

		if (other.gameObject.layer == LayerMask.NameToLayer("Default") && other.CompareTag("Train"))
		{
			isItemNear = true;
			nearItem = other.gameObject;
		}

		if (holdItem != null)
		{
			if (other.CompareTag("ModuleCase") && holdItem.CompareTag("Train"))
			{
				isTrainNear = true; // ModuleCase1~6과 충돌하면 true

				if (other.name == "ModuleCase1")
				{
					if (TrainSingleton.instance.moduleList["Module1"] == 0)
					{
						GameObject.Find("Train").transform.Find("ModuleCase1").transform.Find("Cube").gameObject.SetActive(true);
						isModule1Empty = true;
					}
				}
				else if (other.name == "ModuleCase2")
				{
					if (TrainSingleton.instance.moduleList["Module2"] == 0)
					{
						GameObject.Find("Train").transform.Find("ModuleCase2").transform.Find("Cube").gameObject.SetActive(true);
						isModule2Empty = true;
					}
				}
				else if (other.name == "ModuleCase3")
				{
					if (TrainSingleton.instance.moduleList["Module3"] == 0)
					{
						GameObject.Find("Train").transform.Find("ModuleCase3").transform.Find("Cube").gameObject.SetActive(true);
						isModule3Empty = true;
					}
				}
				else if (other.name == "ModuleCase4")
				{
					if (TrainSingleton.instance.moduleList["Module4"] == 0)
					{
						GameObject.Find("Train").transform.Find("ModuleCase4").transform.Find("Cube").gameObject.SetActive(true);
						isModule4Empty = true;
					}
				}
				else if (other.name == "ModuleCase5")
				{
					if (TrainSingleton.instance.moduleList["Module5"] == 0)
					{
						GameObject.Find("Train").transform.Find("ModuleCase5").transform.Find("Cube").gameObject.SetActive(true);
						isModule5Empty = true;
					}
				}
				else if (other.name == "ModuleCase6")
				{
					if (TrainSingleton.instance.moduleList["Module6"] == 0)
					{
						GameObject.Find("Train").transform.Find("ModuleCase6").transform.Find("Cube").gameObject.SetActive(true);
						isModule6Empty = true;
					}
				}
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
		{
			//Debug.Log("아이템과 멀어짐");
			isItemNear = false;
			nearItem = null;
		}

		if (other.gameObject.layer == LayerMask.NameToLayer("Default") && other.CompareTag("Train"))
		{
			isItemNear = false;
			nearItem = null;
		}

		else if (other.CompareTag("ModuleCase"))
		{
			isTrainNear = false;

			if (other.name == "ModuleCase1")
			{
				isModule1Empty = false;
				GameObject.Find("Train").transform.Find("ModuleCase1").transform.Find("Cube").gameObject.SetActive(false);
			}
			else if (other.name == "ModuleCase2")
			{
				isModule2Empty = false;
				GameObject.Find("Train").transform.Find("ModuleCase2").transform.Find("Cube").gameObject.SetActive(false);
			}
			else if (other.name == "ModuleCase3")
			{
				isModule3Empty = false;
				GameObject.Find("Train").transform.Find("ModuleCase3").transform.Find("Cube").gameObject.SetActive(false);
			}
			else if (other.name == "ModuleCase4")
			{
				isModule4Empty = false;
				GameObject.Find("Train").transform.Find("ModuleCase4").transform.Find("Cube").gameObject.SetActive(false);
			}
			else if (other.name == "ModuleCase5")
			{
				isModule5Empty = false;
				GameObject.Find("Train").transform.Find("ModuleCase5").transform.Find("Cube").gameObject.SetActive(false);
			}
			else if (other.name == "ModuleCase6")
			{
				isModule6Empty = false;
				GameObject.Find("Train").transform.Find("ModuleCase6").transform.Find("Cube").gameObject.SetActive(false);
			}
		}
	}

	public void SetPutDownPosition(Vector3 _position)
	{
		Debug.Log("놓는 블럭 교체" + _position);
		PutDownPosition = _position;
	}
}
