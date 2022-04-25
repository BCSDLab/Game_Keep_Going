using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBlock : Block
{
    int snowGenTiming = 0;
    [SerializeField]
    private int currentSnowLevel = 0;

    public void SetData(int Timing, int snowLevel, int d_x, int d_y)
    {
        snowGenTiming = Timing;
        currentSnowLevel = snowLevel;
        x = d_x;
        y = d_y;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }



    private void FixedUpdate()
    {
        if(snowGenTiming == MapManager.instance.snowTiming)
        {
            if(currentSnowLevel < 5)
            {
                currentSnowLevel++;
                SnowBlockUpdate();
            }
        }

        
    }

    public void DigSnow()
    {
        if (currentSnowLevel > 0)
        {
            currentSnowLevel--;
            SnowBlockUpdate();
        }
    }

    public void SetSnowLevelTo()
    {

    }
    public void SnowBlockUpdate()
    {
        this.transform.localScale = new Vector3(1, 0.2f * currentSnowLevel, 1);
    }
}
