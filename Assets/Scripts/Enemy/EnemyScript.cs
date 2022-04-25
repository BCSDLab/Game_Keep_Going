using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public GameObject target;
    public NavMeshAgent Enemy;
    public EnemyState State = EnemyState.Idle;

    [SerializeField]
    private float LockRange = 12f;
    [SerializeField]
    private float AttackRange = 3f;
    
    /*
        Idle -> Locked
        Locked -> Move, Attack
        Move -> Attack, Idle
        Attack -> Move, Idle
        * -> Death
    */
    public enum EnemyState
    {
        Idle,
        Locked,
        Move,
        Attack,
        Death
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private float GetDistance()
    {
        return Vector3.Distance(target.transform.position, this.transform.position);
    }

    private void FixedUpdate()
    {
        UpdateState();
        DoAction();
    }

    private void UpdateState()
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
            if ((State == EnemyState.Locked || State == EnemyState.Move) && distance <= AttackRange)
            {
                State = EnemyState.Attack;
            }
            else if ((State == EnemyState.Locked || State == EnemyState.Attack) && distance > AttackRange)
            {
                State = EnemyState.Move;
            }
        }


        // Move, Attack -> Idle
        if ((State == EnemyState.Move || State == EnemyState.Attack) && target == null)
        {
            State = EnemyState.Idle;
        }
    }

    private void DoAction()
    {
        if(State == EnemyState.Move)
        {
            Enemy.isStopped = false;
            Enemy.SetDestination(target.transform.position);
        }
        else if(State == EnemyState.Attack)
        {
            Enemy.isStopped = true;
            //Debug.Log("Enemy Attacking");

        }
        else if(State == EnemyState.Death)
        {
            //Debug.Log("Enemy Death");
        }
        else if(State == EnemyState.Locked)
        {
            
            //Debug.Log("Enemy Locked");
            // 타겟 방향을 계속 쳐다봄.
        }
    }

    public void SetState(EnemyState data)
    {
        State = data;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
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
