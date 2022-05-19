using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{

    public NavMeshAgent Enemy;
    [SerializeField]
    public EnemyState State = EnemyState.Idle;

    private int hp;
    private int maxhp;
    

    

    public enum EnemyState
    {
        Idle,
        Locked,
        Move,
        Attack,
        Death
    }

    private void FixedUpdate()
    {
        UpdateState();
        DoAction();
    }

    public virtual void UpdateState()
    {

    }


    public virtual void DoAction()
    {
    }

    private void Start()
    {
        FirstSet();
    }

    private void FirstSet()
    {
        hp = 10;
        maxhp = 10;
    }

    /// <summary>
    /// 체력에 데미지를 줌. 만약에 적이 쓰러질 경우에는 true값을 반환함.
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public bool TakeDamage(int damage)
    {
        hp -= damage;
        if(hp < 0)
        {
            SetState(EnemyState.Death);
            return true;

        }
        return false;
    }
    
    /// <summary>
    /// 체력을 회복시킴. 만약에 최대 체력 이상으로 회복한다면 해당 값을 반환함.
    /// </summary>
    /// <param name="healamount"></param>
    /// <returns></returns>
    public int TakeHeal(int healamount)
    {
        if(hp + healamount > maxhp)
        {
            int overamount = hp - maxhp;
            hp = maxhp;
            return overamount;
        }
        return 0;
    }

    public void SetState(EnemyState data)
    {
        State = data;
    }

    /// <summary>
    /// 정상적으로 죽었을 경우에 true값 생성. 문제가 생길 경우에 추가적으로 이야기해야할거리 이야기하기.
    /// </summary>
    /// <returns></returns>
    public virtual bool DeathControl() 
    {
        return true;
    }

}
