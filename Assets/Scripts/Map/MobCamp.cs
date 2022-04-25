using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Mobs
{
    Stranding = 0,
    Following
};

public class MobCamp : MonoBehaviour
{
    

    [SerializeField]
    private GameObject mobStranding; // 만약 몹 수가 늘어나면 배열로 받아놓을것.
    [SerializeField]
    private GameObject mobFollowing;

    private float spawnRadius;
    private int spawnInterval = 1000;
    private int maxSpawnCount = 1;

    public Mobs Mobkind = Mobs.Stranding;

    public int mobSpawnedEachCount;
    
    // Start is called before the first frame update
    void Start()
    {
        spawnRadius = 2.0f;
        mobSpawnedEachCount = 3;
        MobSpawn();
    }

    public void MobSpawn()
    {
        for(int i = 0; i < mobSpawnedEachCount; i++)
        {
            Vector3 point;
            if (RandomPoint(transform.position, spawnRadius, out point))
            {
                Instantiate(mobStranding, point, Quaternion.identity);
            }
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 5.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                Debug.Log(result);
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    public int count = 0;
    public int spawnCount = 0;
    // Update is called once per frame
    void Update()
    {
        if (count > spawnInterval && spawnCount < maxSpawnCount)
        {
            count = 0;
            
            MobSpawn();
        }
        count++;
    }
}
