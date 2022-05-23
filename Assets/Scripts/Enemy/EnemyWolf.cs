using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWolf : EnemyMelee
{
    public override void GiveDamageToTarget(int attackmode)
    {
        if(attackmode == 0) // 기본 공격
        {
            target.gameObject.GetComponent<PlayerStat>().TakeDamage(1);
            Debug.Log("데미지 주기 성공!");
        }
    }
}
