using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStack : MonoBehaviour
{
    protected int stackNum = 1; // 쌓여있는 돌, 나무 블럭의 수

    protected void CountNum()
    {
        stackNum = transform.childCount;
    }

    public int GetInt()
    {
        return stackNum;
    }
}
