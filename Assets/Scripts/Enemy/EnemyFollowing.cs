using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowing : EnemyScript
{
    public GameObject target;

    [SerializeField]
    protected float LockRange = 12f;
    [SerializeField]
    protected float AttackRange = 3f;


    private void FixedUpdate()
    {
        UpdateState();
        DoAction();
    }


    protected float GetDistance()
    {
        return Vector3.Distance(target.transform.position, this.transform.position);
    }


    bool SetTarget(GameObject m_target)
    {
        // 혹시 안되는 조건이 있다면
        if (false)
        {
            return false;
        }
        target = m_target;
        return true;
    }

     /*
    Idle -> Locked
    Locked -> Move, Attack
    Move -> Attack, Idle
    Attack -> Move, Idle
    -> Death
    */

    public override void UpdateState()
    {
        // idle -> Locked
        if (State == EnemyState.Idle && target != null)
        {
            State = EnemyState.Locked;
        }

        // locked -> move, attack
        // move-> attack , attack -> move
        if (target != null)
        {
            float distance = GetDistance();
            if ((State == EnemyState.Locked ) && distance > AttackRange)
            {
                State = EnemyState.Move;
            }
        }
        // Move, Attack -> Idle
        if ((State == EnemyState.Move) && target == null)
        {
            State = EnemyState.Idle;
        }

        if (State == EnemyState.Death)
        {

        }
    }

    public override void DoAction() 
    {
        if (State == EnemyState.Move)
        {
            Enemy.isStopped = false;
            
            Enemy.SetDestination(target.transform.position);
        }
        else if (State == EnemyState.Death)
        {
            //Debug.Log("Enemy Death");
        }
        else if (State == EnemyState.Locked)
        {

            //Debug.Log("Enemy Locked");
            // 타겟 방향을 계속 쳐다봄.
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            target = null;
        }
    }
}
