using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    Image hpBar;


    [SerializeField]
    private int hp;
    [SerializeField]
    private int maxhp;
    
    // Start is called before the first frame update
    void Start()
    {
        hp = 20;
        hpBar = GameObject.Find("hpBar").GetComponent<Image>();
    }

    /// <summary>
    /// ü�¿� �������� ��. ���࿡ ���� ������ ��쿡�� true���� ��ȯ��.
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
        return false;
    }

    /// <summary>
    /// ü���� ȸ����Ŵ. ���࿡ �ִ� ü�� �̻����� ȸ���Ѵٸ� �ش� ���� ��ȯ��.
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
        //Vector3 hpBarPos = this.transform.position;
        //hpBar.transform.position = Camera.main.WorldToScreenPoint(hpBarPos);
    }
}
