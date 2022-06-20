using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWolf : EnemyMelee
{
    public override void GiveDamageToTarget(int attackmode)
    {
        if(attackmode == 0) // �⺻ ����
        {
            target.gameObject.GetComponent<PlayerStat>().TakeDamage(1);
            Debug.Log("������ �ֱ� ����!");
        }
    }
}
