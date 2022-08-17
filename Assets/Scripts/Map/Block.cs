using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    GRASS,
    WATER
};

public class Block : MonoBehaviour
{
    public BlockType block_Type;

    public int x, y;

    // Start is called before the first frame update
    void Start()
    {
        //block_Type = BlockType.GRASS;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
