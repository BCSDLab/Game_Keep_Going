using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MapManager : MonoBehaviour
{
    public static MapManager instance = null;

    [SerializeField]
    public Vector3 currentStartPos = new Vector3();
    [SerializeField]
    public Vector3 nextStartPos = new Vector3();

    [SerializeField]
    private GameObject MapPrefab;

    [SerializeField]
    public int seedGen = 0;
    public int seed = 0;

    public int stageLevel = 1;
    public int stageLength = 40;

    List<GameObject> Maps = new List<GameObject>();
    public Map currentMap;
    
    [SerializeField]
    public int snowingLevel = 6000; // 눈이 내리는 프레임 기준.
    [SerializeField]
    public bool isSnowing = true;
    public int snowTiming = 0; // get함수로 가져올수도 있는데, 속도가 뭐가 더 빠른지 모름.

    public Vector3 playerReturnPos;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    //
    private void Start()
    {
        FirstSetup();

        SetupNewLevel(40); // 1스테이지 제작
        SetupNewLevel(40); // 2스테이지 제작.
    }

    /// <summary>
    /// 새로운 레벨의 맵을 생성, 길이만 조절 가능함.
    /// </summary>
    /// <param name="stagelength">스테이지의 길이.</param>
    public void SetupNewLevel(int stagelength)
    {
        MapSetUp(stagelength, stageLevel);
    }

    void FixedUpdate()
    {
        if (isSnowing)
        {
            SnowsnowTimingUpdate();
        }
    }

    /// <summary>
    /// 맵매쉬를 만들어내는 함수.
    /// </summary>
    private void InvokeMapMeshgen()
    {
        MapMeshGenerator.instance.GenerateNavmesh();
    }

    private void FirstSetup()
    {
        currentStartPos = new Vector3(0, 0, 0);
        nextStartPos = new Vector3(0f, 0, 0);
    }


    public void MapSetUp(int stagelength, int difficulty)
    {
        currentStartPos = nextStartPos;
        Debug.Log("current" + currentStartPos);

        stageLength = stagelength;
        GameObject newmap = Instantiate(MapPrefab);
        Maps.Add(newmap);
        //currentMap = newmap.GetComponent<Map>();
        nextStartPos = currentStartPos + new Vector3(1.6f * stageLength, 0,  0);
        Debug.Log("next" + nextStartPos);
        Invoke("InvokeMapMeshgen", 0.1f);
        stageLevel++;
    }

    public void BlockBreak(S_BroadcastResource pkt)
    {
        Map map = MapPrefab.GetComponent<Map>();
        GameObject resObj = map.GetResourceObject(pkt.resourceIdx);
        Mining(resObj);
    }

    public void Mining(GameObject collObj)
    {
        Vector3 resourcesPos = collObj.transform.position;
        GameObject stoneResourceObj = Resources.Load("Prefabs/rock_stack") as GameObject;
        GameObject woodResourceObj = Resources.Load("Prefabs/wood_stack") as GameObject;

        resourcesPos.y = 1.6f;
        if (collObj.name == "stone(Clone)")
        {
            Instantiate(stoneResourceObj, resourcesPos, collObj.transform.rotation);
        }
        else if (collObj.name == "wood(Clone)")
        {
            Instantiate(woodResourceObj, resourcesPos, collObj.transform.rotation);
        }
        Destroy(collObj);
    }

    void SnowsnowTimingUpdate()
    {
        // 눈 쌓이는 부분.
        if (snowTiming > snowingLevel)
        {
            snowTiming = 0;
        }
        else
        {
            snowTiming++;
        }
    }

    int GetSnowLevel()
    {
        return snowTiming;
    }
}
