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
    /// ü�¿� �������� ��. ���࿡ ���� ������ ��쿡�� true���� ��ȯ��.
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
    /// ü���� ȸ����Ŵ. ���࿡ �ִ� ü�� �̻����� ȸ���Ѵٸ� �ش� ���� ��ȯ��.
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
    /// ���������� �׾��� ��쿡 true�� ����. ������ ���� ��쿡 �߰������� �̾߱��ؾ��ҰŸ� �̾߱��ϱ�.
    /// </summary>
    /// <returns></returns>
    public virtual bool DeathControl() 
    {
        return true;
    }

}
