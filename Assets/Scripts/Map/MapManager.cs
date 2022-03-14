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

    public int stageLevel = 0;
    public int stageLength = 40;

    List<GameObject> Maps = new List<GameObject>();

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

        MapSetUp(40, 1);

        Invoke("InvokeMapMeshgen", 0.1f);

    }

    private void InvokeMapMeshgen()
    {
        MapMeshGenerator.instance.GenerateNavmesh();
    }

    private void FirstSetup()
    {
        currentStartPos = new Vector3(0, 0, 0);
        nextStartPos = new Vector3(0f, 0, 0);
    }

    private void MapSetUptest()
    {
        MapSetUp(40, 2);
    }

    private void MapSetUp(int stagelength, int difficulty)
    {
        currentStartPos = nextStartPos;
        Debug.Log("current" + currentStartPos);
        stageLevel++;
        stageLength = stagelength;
        GameObject newmap = Instantiate(MapPrefab);
        Maps.Add(newmap);
        nextStartPos = currentStartPos + new Vector3(1.6f * stageLength, 0,  0);
        Debug.Log("next" + nextStartPos);

    }
}
