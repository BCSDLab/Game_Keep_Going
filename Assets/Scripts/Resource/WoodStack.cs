using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodStack : MonoBehaviour
{
    public int woodStackNum = 1; // 쌓여있는 나무 블럭의 수

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CountWoodNum();
    }

    private void CountWoodNum()
    {
        woodStackNum = this.transform.childCount;
    }

    public int getInt()
    {
        return woodStackNum;
    }
}
