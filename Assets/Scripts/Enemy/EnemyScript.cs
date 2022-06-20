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
    /// ����� �������� ���� ��� �������� ó��, ������ �������� ��� ���� ó����.
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
    /// ���������� �׾��� ��쿡 true�� ����. ������ ���� ��쿡 �߰������� �̾߱��ؾ��ҰŸ� �̾߱��ϱ�.
    /// </summary>
    /// <returns></returns>
    public virtual bool DeathControl() 
    {
        return true;
    }

}
