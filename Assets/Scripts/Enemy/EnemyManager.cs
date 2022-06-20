using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; } = new EnemyManager();

    GameObject[] enemyList;

    private void ReloadEnemyList()
    {
        enemyList = GameObject.FindGameObjectsWithTag("Enemy");
    }

    public void MoveEnemy(S_BroadcastEnemyMove packet)
    {
        GameObject enemy = enemyList[packet.enemyIdx];
        enemy.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
    }
}
