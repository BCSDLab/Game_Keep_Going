using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStack : MonoBehaviour
{
    protected int stackNum = 1; // �׿��ִ� ��, ���� ���� ��

    protected void CountNum()
    {
        stackNum = transform.childCount;
    }

    public int GetInt()
    {
        return stackNum;
    }
}
