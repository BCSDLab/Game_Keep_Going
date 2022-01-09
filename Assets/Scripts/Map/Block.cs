using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    enum Type { Grass, Water };

    public int x;
    public int y;
    private Type block_Type;


    // Start is called before the first frame update
    void Start()
    {
        block_Type = Type.Grass;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
