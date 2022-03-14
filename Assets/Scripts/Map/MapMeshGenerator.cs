using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapMeshGenerator : MonoBehaviour
{
    public static MapMeshGenerator instance = null;


    private Vector3 _generatePos = new Vector3(50, 0, 50);
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
        //GenerateNavmesh();
    }

    public void GenerateNavmesh()
    {
        //GameObject obj = Instantiate(_mapPrefab, _generatePos, Quaternion.identity, transform);
        _generatePos += new Vector3(50, 0, 50);

        NavMeshSurface[] surfaces = gameObject.GetComponentsInChildren<NavMeshSurface>();

        foreach (var s in surfaces)
        {
            s.RemoveData();
            s.BuildNavMesh();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GenerateNavmesh();
        }
    }
}
