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
        else if (State == EnemyState.Attack && isAttackFinished == true && isAttackStarted == false) // 공격이 시작되는 경우.
        {
            Enemy.isStopped = true;
            AttackMotion();
            isAttackStarted = true;
            isAttackFinished = false;
            Debug.Log("공격!!!");
        }
        else if(State == EnemyState.Attack && isAttackStarted == true)
        {
            Debug.Log("어택모션 돌아가는중...");
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

    protected void AttackMotion()
    {
        StartCoroutine(Attack(0, 0.5f, 0.5f, 0.5f));
    }

    IEnumerator Attack(int attackmode, float pre, float attack, float post)
    {
        Debug.Log("공격 선딜 시작.");
        // 공격 선딜 모션
        yield return new WaitForSecondsRealtime(pre);
        
        if (GetDistance() <= AttackRange) // 사거리 내에 적이 있다면 공격이 들어감.
        {
            Debug.Log("공격 시작.");
            // 공격 모션
            yield return new WaitForSecondsRealtime(attack);
            // 데미지 주는 스크립트
            GiveDamageToTarget(attackmode);
            Debug.Log("공격 후딜 시작.");
            yield return new WaitForSecondsRealtime(post);
            isAttackFinished = true;
            yield return null;
        }
        else // 만약 사거리보다 현재 거리가 더 멀다면 취소됨.
        {
            isAttackFinished = true;
            yield return null;
        }

        yield return null; // 혹시나
    }

    public virtual void GiveDamageToTarget(int attackmode)
    {
        target.GetComponent<PlayerStat>().TakeDamage(attackDamege);
    }
}
