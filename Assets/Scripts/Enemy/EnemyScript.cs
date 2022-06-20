using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour, IDamageable
{
    public NavMeshAgent Enemy;
    [SerializeField]
    public EnemyState State = EnemyState.Idle;

    private float hp;
    private int maxhp;
    protected int enemyIdx;


    public enum EnemyState
    {
        Idle,
        Locked,
        Move,
        Attack,
        Death
    }

    protected virtual void FixedUpdate()
    {
        UpdateState();
        DoAction();
        /*if (networkManager != null)
            SendEnemyList();*/
    }

    public virtual void UpdateState()
    {

    }


    /// <summary>
    /// 양수의 데미지를 입을 경우 데미지로 처리, 음수의 데미지의 경우 힐로 처리함.
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    /// 
    public bool TakeHit(float damage, RaycastHit hit)
    {
        if(damage >= 0)
        {
            hp -= damage;
            if (hp < 0)
            {
                SetState(EnemyState.Death);
                return true;

            }
            return false;
        }
        else
        {
            if (hp - damage > maxhp)
            {
                float overamount = hp - maxhp;
                hp = maxhp;
                return true;
            }
            return false;
        }
    }

    public virtual void DoAction()
    {
    }

    protected virtual void Start()
    {
        FirstSet();
    }

    private void FirstSet()
    {
        hp = 10;
        maxhp = 10;
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
