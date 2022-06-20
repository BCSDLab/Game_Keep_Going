using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyFollowing
{
    protected float attackPreDelay;
    protected float attackDelay;
    protected float attackPostDelay;
    protected int attackDamege = 5;

    [SerializeField]
    protected bool isAttackFinished = false;
    [SerializeField]
    protected bool isAttackStarted = false;

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
            if ((State == EnemyState.Locked || State == EnemyState.Move) && distance <= AttackRange)
            {
                State = EnemyState.Attack;
            }
            else if ((State == EnemyState.Locked || State == EnemyState.Attack && isAttackFinished) && distance > AttackRange)
            {
                State = EnemyState.Move;
                isAttackFinished = true;
                isAttackStarted = false;
            }
        }


        // Move, Attack -> Idle
        if ((State == EnemyState.Move || State == EnemyState.Attack) && target == null)
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
        else if (State == EnemyState.Attack && isAttackFinished == true && isAttackStarted == false) // ������ ���۵Ǵ� ���.
        {
            Enemy.isStopped = true;
            AttackMotion();
            isAttackStarted = true;
            isAttackFinished = false;
            Debug.Log("����!!!");
        }
        else if(State == EnemyState.Attack && isAttackStarted == true)
        {
            Debug.Log("���ø�� ���ư�����...");
        }
        else if (State == EnemyState.Death)
        {
            //Debug.Log("Enemy Death");
        }
        else if (State == EnemyState.Locked)
        {

            //Debug.Log("Enemy Locked");
            // Ÿ�� ������ ��� �Ĵٺ�.
        }
    }

    protected void AttackMotion()
    {
        StartCoroutine(Attack(0, 0.5f, 0.5f, 0.5f));
    }

    IEnumerator Attack(int attackmode, float pre, float attack, float post)
    {
        Debug.Log("���� ���� ����.");
        // ���� ���� ���
        yield return new WaitForSecondsRealtime(pre);
        
        if (GetDistance() <= AttackRange) // ��Ÿ� ���� ���� �ִٸ� ������ ��.
        {
            Debug.Log("���� ����.");
            // ���� ���
            yield return new WaitForSecondsRealtime(attack);
            // ������ �ִ� ��ũ��Ʈ
            GiveDamageToTarget(attackmode);
            Debug.Log("���� �ĵ� ����.");
            yield return new WaitForSecondsRealtime(post);
            isAttackFinished = true;
            yield return null;
        }
        else // ���� ��Ÿ����� ���� �Ÿ��� �� �ִٸ� ��ҵ�.
        {
            isAttackFinished = true;
            yield return null;
        }

        yield return null; // Ȥ�ó�
    }

    public virtual void GiveDamageToTarget(int attackmode)
    {
        target.GetComponent<PlayerStat>().TakeDamage(attackDamege);
    }
}
