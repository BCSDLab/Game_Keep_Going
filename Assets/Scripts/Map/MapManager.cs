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

    public int stageLevel = 1;
    public int stageLength = 40;

    List<GameObject> Maps = new List<GameObject>();

    [SerializeField]
    public int snowingLevel = 6000; // ���� ������ ������ ����.
    [SerializeField]
    public bool isSnowing = true;
    public int snowTiming = 0; // get�Լ��� �����ü��� �ִµ�, �ӵ��� ���� �� ������ ��.



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

        MapSetUp(40, stageLevel);
        

    }

    void FixedUpdate()
    {
        if (isSnowing)
        {
            SnowsnowTimingUpdate();
        }
    }

    /// <summary>
    /// �ʸŽ��� ������ �Լ�.
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
        nextStartPos = currentStartPos + new Vector3(1.6f * stageLength, 0,  0);
        Debug.Log("next" + nextStartPos);
        Invoke("InvokeMapMeshgen", 0.1f);
        stageLevel++;
    }


    void SnowsnowTimingUpdate()
    {
        // �� ���̴� �κ�.
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
