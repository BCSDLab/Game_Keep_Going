using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockStack : MonoBehaviour
{
    public int rockStackNum = 1; // �׿��ִ� �� ���� ��

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CountRockNum();
    }

    private void CountRockNum()
    {
        rockStackNum = this.transform.childCount;
    }

    public int getInt()
    {
        return rockStackNum;
    }
}
