using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStranding : EnemyScript
{
    public int strandingCount = 0;
    public int strandingInterval = 300;
    public float range = 3.0f;
    public float speed = 1.0f;

    protected override void Start()
    {
        base.Start();
        FirstSetup(300, 1.0f, 5.0f);
    }

    /// <summary>
    /// 움직이는 주기, 속도, 최대 이동 반응 범위 등을 설정.
    /// 현재는 그냥 기본값으로만 되어있음.
    /// </summary>
    public void FirstSetup(int Interval, float m_speed, float m_range)
    {
        speed = m_speed;
        this.gameObject.GetComponent<NavMeshAgent>().speed = speed;
        strandingInterval = Interval;
        range = m_range;
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    void Update()
    {
        if(strandingCount > strandingInterval)
        {
            Vector3 point;
            if (RandomPoint(transform.position, range, out point))
            {
                Enemy.SetDestination(point);
            }
            strandingCount = 0;
        }
        strandingCount++;
    }


    
}
