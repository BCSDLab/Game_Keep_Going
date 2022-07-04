using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance;
    public static EnemyManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(EnemyManager)) as EnemyManager;
            }
            return instance;
        }
    }

    [SerializeField]
    Transform enemyParant;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ReloadEnemyList();
    }
    private void ReloadEnemyList()
    {
        enemyParant = GameObject.Find("EnemyParant").transform;
    }

    public void MoveEnemy(S_BroadcastEnemyMove packet)
    {
        Transform enemy = enemyParant.GetChild(packet.enemyIdx - 1);
        Vector3 targetPos = new Vector3(packet.posX, packet.posY, packet.posZ);
        enemy.position = Vector3.MoveTowards(enemy.position, targetPos, 10 * Time.deltaTime);
        Debug.Log("Enemy Move!!!!   " + targetPos);
    }

    public void EnemyMove()
    {
        foreach (Transform trans in enemyParant)
        {
            trans.position = new Vector3(5, 3, 10);
        }
    }
}
