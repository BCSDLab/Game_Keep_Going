using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    [SerializeField]
    Image hpBar;


    [SerializeField]
    private int hp;
    [SerializeField]
    private int maxhp;
    NetworkManager networkManager;

    [SerializeField]
    GameObject hpObj;
    // Start is called before the first frame update
    void Start()
    {
        hpObj = Resources.Load("Prefabs/hpBar") as GameObject;
        maxhp = 20;
        hp = 20;
        hpBar = GameObject.Instantiate(hpObj, Vector2.zero, Quaternion.identity, GameObject.Find("Canvas").transform).GetComponent<Image>();
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    public void SendHp()
    {
        C_Health healthPacket = new C_Health();
        healthPacket.health = hp;
        networkManager.Send(healthPacket.Write());
    }

    public void SetHealth(int packetHp)
    {
        hp = packetHp;
    }

    /// <summary>
    /// 체력에 데미지를 줌. 만약에 적이 쓰러질 경우에는 true값을 반환함.
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public bool TakeDamage(int damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            return true;

        }
        else
        {
            HPBarUpdate();
        }
        SendHp();
        return false;
    }

    /// <summary>
    /// 체력을 회복시킴. 만약에 최대 체력 이상으로 회복한다면 해당 값을 반환함.
    /// </summary>
    /// <param name="healamount"></param>
    /// <returns></returns>
    public int TakeHeal(int healamount)
    {
        if (hp + healamount > maxhp)
        {
            int overamount = hp - maxhp;
            hp = maxhp;
            return overamount;
        }
        else
        {
            hp = hp + healamount;
            HPBarUpdate();
        }
        return 0;
    }

    private bool HPBarUpdate()
    {
        hpBar.fillAmount = (float)hp / maxhp;
        return true;
    }

    private void Update()
    {
        Vector3 hpBarPos = this.transform.GetChild(2).transform.position;
        hpBar.transform.position = Camera.main.WorldToScreenPoint(hpBarPos);
        HPBarUpdate();
    }

    public int GetHP()
    {
        return hp;
    }

}
