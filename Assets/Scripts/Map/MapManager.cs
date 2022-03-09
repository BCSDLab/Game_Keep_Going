using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MapManager : MonoBehaviour
{
    private const float BLOCK_SIZE = 1.6f;
    
    private int block_StartPosition = 0;
    private int block_EndPosition;
    [SerializeField]
    private int block_HorizLength = 40;
    [SerializeField]
    private int block_VertLength = 24;

    [SerializeField]
    private string Seed = "qweasfds"; // ½Ãµå´Â 8´Ü¾îÀÇ StringÀ¸·Î ±¸¼º.

    [SerializeField]
    private GameObject dirt;
    [SerializeField]
    private GameObject water;
    [SerializeField]
    private GameObject dummy;
    [SerializeField]
    private GameObject wood;
    [SerializeField]
    private GameObject stone;
    [SerializeField]
    private GameObject rock;
    [SerializeField]
    private GameObject marker;
    [SerializeField]
    private GameObject station;
    [SerializeField]
    public GameObject rail;
    [SerializeField]
    public GameObject lastrailpos;


    private List<GameObject> blockSet;
    [SerializeField]
    private List<Vector2Int> colliderBlockSet;
    private List<GameObject> objectSet;
    private List<Vector2Int> objectPos;

    [SerializeField]
    private List<int> LakeLineTop;
    [SerializeField]
    private List<int> LakeLineBottom;
    [SerializeField]
    private List<int> HillLineTop;
    [SerializeField]
    private List<int> HillLineBottom;
    public static MapManager instance;
    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        FirstSetup();
    }

    /// <summary>
    /// ÃÖÃÊ Setting.
    /// </summary>
    void FirstSetup()
    {
        // ¸î¸î °´Ã¼¿¡ ÀÎ½ºÅÏ½º ºÎ¿©.
        block_EndPosition = block_HorizLength;
        objectSet = new List<GameObject>();
        objectPos = new List<Vector2Int>();
        blockSet = new List<GameObject>();
        colliderBlockSet = new List<Vector2Int>();
        LakeLineTop = new List<int>();
        LakeLineBottom = new List<int>();
        HillLineTop = new List<int>();
        HillLineBottom = new List<int>();
        // ÃÖÃÊ ¾÷µ¥ÀÌÆ®.

        RandomNumberGenSetup(); // ½Ãµå±â¹İ ·£´ı ¼ıÀÚ Á¦³Ê·¹ÀÌÆÃ ¼³Á¤.
        BaseField(); // ±â¹İÀÌ µÇ´Â ºí·° ¼³Á¤.
        
        LakeLineGen(); // LakeÀÇ ±âÁØÀÌ µÇ´Â Line»ı¼º.
        //LakeLineTest();
        LakeGroupGen(); // Lake»ı¼º.
        HillLineGen(); // Hill Line »ı¼º.
        //HillLineTest();
        HillGroupGen(); // Hill Group »ı¼º.

        DataBasePositionSelection(); // ½Ãµå±â¹İ ¿ÀºêÁ§Æ® Á¦ÀÛ.
        StationGen(); // Station »ı¼º.

       
        
        

    }


    void StationGen()
    {
        int x = Random.Range(0, 4);
        int y = Random.Range(block_VertLength / 2 - 5, block_VertLength / 2 + 5);
        while (true)
        {
            if(IsObjectPosValid(x, y) && IsObjectPosValid(x+1,y) && IsObjectPosValid(x,y-1))
            {
                AddObject(x, y, 5);
                AddObject(x, y - 1, 4);
                lastrailpos.transform.position = new Vector3(x * BLOCK_SIZE, 1.6f, (y -1) * BLOCK_SIZE);
                break;
            }
            else
            {
                x = Random.Range(0, 4);
                y = Random.Range(block_VertLength / 2 - 5, block_VertLength / 2 + 5);
            }
        }
    }
    /// <summary>
    /// °­ Áö¿ªÀÌ ¹İÀ¸·Î °¡¸£Áö ¾Êµµ·Ï ¸¸µå´Â ±â´É. ¾çÂÊ È£¼öÀÇ ÃÖ´ë ¹üÀ§¸¦ ¸¸µé¾î³õÀ½.
    /// À§ÂÊ LineÀÇ °æ¿ì´Â ÃÖ´ë ±æÀÌ½Ã °¡¿îµ¥¿¡¼­ 3 ¶³¾îÁö°í
    /// ¾Æ·§ÂÊ LineÀÇ °æ¿ì´Â 4Ä­ ¶³¾îÁö°Ô.
    /// </summary>
    void LakeLineGen()
    {
        LakeLineTop.Add(Random.Range(8, (block_VertLength / 2) - 4));
        LakeLineBottom.Add(Random.Range((block_VertLength / 2)+ 4, block_VertLength - 4));
        for (int i = 1; i < block_HorizLength ; i++)
        {
            int up = LakeLineTop[LakeLineTop.Count - 1];
            int down = LakeLineBottom[LakeLineBottom.Count - 1];

            up += Random.Range(-3, 3);
            down += Random.Range(-3, 3);
            if(up > (block_VertLength / 2) - 1)
            {
                up = (block_VertLength / 2) - 1;
            }
            else if(up < 3)
            {
                up = 3;
            }
            if(down < (block_VertLength / 2) + 1)
            {
                down = (block_VertLength / 2) + 1;
            }
            else if(down > block_VertLength - 3)
            {
                down = block_VertLength - 3;
            }
            LakeLineTop.Add(up);
            LakeLineBottom.Add(down);
        }
    }

    void HillLineGen()
    {
        HillLineTop.Add(Random.Range(4, (block_VertLength / 2) - 4));
        HillLineBottom.Add(Random.Range((block_VertLength / 2) + 4, block_VertLength - 6));
        for (int i = 1; i < block_HorizLength; i++)
        {
            int up = HillLineTop[HillLineTop.Count - 1];
            int down = HillLineBottom[HillLineBottom.Count - 1];

            up += Random.Range(-2, 2);
            down += Random.Range(-2, 2);
            if (up > (block_VertLength / 2) - 3)
            {
                up = (block_VertLength / 2) - 3;
            }
            else if (up < 4)
            {
                up = 4;
            }
            if (down < (block_VertLength / 2) + 3)
            {
                down = (block_VertLength / 2) + 3;
            }
            else if (down > block_VertLength - 4)
            {
                down = block_VertLength - 4;
            }
            HillLineTop.Add(up);
            HillLineBottom.Add(down);
        }
    }

    void HillGroupGen()
    {

        GenHillFromCenter(10);
        GenHillFromTop(5);
        GenHillFromBottom(15);
        GenHillFromBottom(5);
        GenHillFromTop(15);
        GenHillFromTop(25);
        GenHillFromCenter(35);
        GenHillFromBottom(20);
    }

    int GenHillFromCenter(int x)
    {
        if (x + 3 >= block_HorizLength || x == -1)
        {
            return -1;
        }
        int ybottom = block_VertLength / 2;
        int ytop = block_VertLength / 2;
        int dx = x;
        bool isReachedLine = false;
        while (!isReachedLine && dx < block_HorizLength)
        {
            ytop += Random.Range(0, 2);
            ybottom += Random.Range(-2, 0);
            if (ytop > HillLineBottom[dx] || ybottom < HillLineTop[dx])
            {
                
                isReachedLine = true;
            }
            else
            {
                AddObject(dx, ytop, 2);
                AddObject(dx, ybottom, 2);
                
                for (int dy = block_VertLength / 2 ; dy < ytop - 1; dy++)
                {
                 
                    AddObject(dx, dy, 2);
                }
                for(int dy = block_VertLength / 2; dy > ybottom + 1; dy--)
                {
                 
                    AddObject(dx, dy, 2);
                }
                
            }
            print(dx + " , " + ytop);
            print(dx + " , " + ybottom);
            dx++;
        }
        dx--;
        print(" ytop is : " + ytop);
        print(" ybottom is : " + ybottom);
        while (isReachedLine && dx < block_HorizLength)
        {
            ytop += Random.Range(-2, 0);
            ybottom += Random.Range(0, 2);
            if (ytop > block_VertLength / 2 || ybottom < block_VertLength / 2)
            {
                isReachedLine = false;
            }
            else
            {
                if(isReachedLine == false)
                {
                    break;
                }
                AddObject(dx, ytop, 2);
                AddObject(dx, ybottom, 2);

                
                for (int dy = block_VertLength / 2 ; dy < ytop - 1; dy++)
                {
                    
                    AddObject(dx, dy, 2);
                }
                for(int dy = block_VertLength / 2; dy > ybottom + 1; dy--)
                {
                    
                    AddObject(dx, dy, 2);
                }
                
            }
            dx++;
            print("¿Ô³Ä?");
        }
        return dx;
    }

    int GenHillFromTop(int x)
    {
        if (x + 3 >= block_HorizLength || x == -1)
        {
            return -1;
        }
        int y = 0;
        int dx = x;
        bool isReachedLine = false;
        while (!isReachedLine && dx < block_HorizLength)
        {
            y += Random.Range(0, 3);
            if (y > HillLineTop[dx])
            {
                isReachedLine = true;
            }
            else
            {
                for (int dy = 0; dy < y; dy++)
                {
                    if (!IsObjectPosValid(dx, dy))
                    {
                        return dx;
                    }
                    AddObject(dx, dy, 2);
                   
                }
            }
            dx++;
        }
        dx--;
        while (isReachedLine && dx < block_HorizLength)
        {
            y += Random.Range(-3, 0);
            if (y <= 0)
            {
                isReachedLine = false;
            }
            else
            {
                for (int dy = 0; dy < y; dy++)
                {
                    if (!IsObjectPosValid(dx, dy))
                    {
                        return dx;
                    }
                    AddObject(dx, dy, 2);
                }
            }
            dx++;
        }
        return dx;
    }

    int GenHillFromBottom(int x)
    {
        if (x + 3 >= block_HorizLength || x == -1)
        {
            return -1;
        }
        int y = block_VertLength;
        int dx = x;
        bool isReachedLine = false;
        while (!isReachedLine && dx < block_HorizLength)
        {
            y += Random.Range(-3, 0);
            if (y < HillLineBottom[dx])
            {
                isReachedLine = true;
            }
            else
            {
                for (int dy = y + 1; dy < block_VertLength; dy++)
                {
                    AddObject(dx, dy, 2);
                }
            }
            dx++;
        }
        dx--;
        while (isReachedLine && dx < block_HorizLength)
        {
            y += Random.Range(0, 3);
            if (y >= block_VertLength)
            {
                isReachedLine = false;
            }
            else
            {
                for (int dy = y; dy < block_VertLength; dy++)
                {
                    AddObject(dx, dy, 2);
                }
            }
            dx++;
        }
        return dx;
    }

    void HillLineTest()
    {
        for (int x = 0; x < block_HorizLength; x++)
        {
            AddObject(x, HillLineTop[x], 9);
            AddObject(x, HillLineBottom[x], 9);
        }

    }
    void LakeLineTest() {
        for (int x = 0; x < block_HorizLength; x++) {
            for(int y = 0; y < LakeLineTop[x]; y++)
            {
                ReplaceBlock(x, y, 0);
            }
            for (int y = block_VertLength-1; y > LakeLineBottom[x]; y--)
            {
                ReplaceBlock(x, y, 0);
            }
        }

    }

    /// <summary>
    /// ¾î´À ¸éÀ» ±âÁ¡À¸·Î ÇØ¼­ ¸¸µé¾îÁö´Â Å« È£¼ö Á¦ÀÛ ½ºÅ©¸³Æ®. LakelineÀ» ±âÁØÀ¸·Î ÇØ¼­ ±× ¼±Àº ¸ø³ÑÀ½.
    /// </summary>
    void LakeGroupGen()
    {
        int countermaking = 0;
       
        while (countermaking != -1)
        {
            print(countermaking + "¿¡¼­ À§È£¼ö »ı¼º.");
            countermaking = GenLakeFromTop(countermaking);
            if(countermaking  > 4)
            {
                countermaking -= 4;
            }
            print(countermaking + "¿¡¼­ ¾Æ·¡È£¼ö »ı¼º.");
            countermaking = GenLakeFromBottom(countermaking);
        }
    }

    /// <summary>
    /// ¹ŞÀº xÁÂÇ¥¸¦ ½ÃÀÛÀ¸·Î ÁÂÇ¥»ó À§ÂÊ (¹Ù¶óº¸´Â ±âÁØ ¾Æ·¡ÂÊ)ÀÇ È£¼ö »ı¼º.
    /// È£¼öÀÇ °¡Àå ¸¶Áö¸· ÁÂÇ¥¸¦ return.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    int GenLakeFromTop(int x)
    {
        if(x+3 >= block_HorizLength || x == -1)
        {
            return -1;
        }
        int y = 0;
        int dx = x;
        bool isReachedLine = false;
        while (!isReachedLine && dx < block_HorizLength)
        {
            y += Random.Range(0, 3);
            if (y > LakeLineTop[dx])
            {
                isReachedLine = true;
            }
            else
            {
                for (int dy = 0; dy < y; dy++)
                {
                    ReplaceBlock(dx, dy, 2);
                }
            }
            dx++;
        }
        dx--;
        print(y + " " + dx);
        while (isReachedLine && dx < block_HorizLength)
        {
            y += Random.Range(-3, 0);
            if (y <= 0)
            {
                isReachedLine = false;
            }
            else
            {
                for (int dy = 0; dy < y; dy++)
                {
                    ReplaceBlock(dx, dy, 2);
                }
            }
            dx++;
        }
        return dx;
    }

    /// <summary>
    /// ¹ŞÀº xÁÂÇ¥¸¦ ½ÃÀÛÀ¸·Î ÁÂÇ¥»ó Å«ÂÊ (¹Ù¶óº¸´Â ±âÁØ À§ÂÊ)ÀÇ È£¼ö »ı¼º.
    /// È£¼öÀÇ °¡Àå ¸¶Áö¸· ÁÂÇ¥¸¦ return.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    int GenLakeFromBottom(int x)
    {
        if (x+3 >= block_HorizLength || x == -1)
        {
            return -1;
        }
        int y = block_VertLength;
        int dx = x;
        bool isReachedLine = false;
        while (!isReachedLine && dx < block_HorizLength)
        {
            y += Random.Range(-3, 0);
            if (y < LakeLineBottom[dx])
            {
                isReachedLine = true;
            }
            else
            {
                for (int dy = y; dy < block_VertLength; dy++)
                {
                    ReplaceBlock(dx, dy, 2);
                }
            }
            dx++;
        }
        dx--;
        print(y + " " + dx);
        while (isReachedLine && dx < block_HorizLength)
        {
            y += Random.Range(0, 3);
            if (y >= block_VertLength)
            {
                isReachedLine = false;
            }
            else
            {
                for (int dy = y; dy < block_VertLength; dy++)
                {
                    ReplaceBlock(dx, dy, 2);
                }
            }
            dx++;
        }
        return dx;
    }
    /// <summary>
    /// ±â¹İÀÌ µÇ´Â ¶¥À» »ı¼º. 
    /// block_StartPosition¿¡¼­ºÎÅÍ block_EndPosition±îÁöÀÇ 16°³ ´ÜÀ§ ºí·°À» »ı¼º.
    /// </summary>
    void BaseField()
    {
        for(int index = block_StartPosition; index < block_EndPosition; index++)
        {
            for(int vertPos = 0; vertPos < block_VertLength; vertPos++)
            {
                AddBlock(index, vertPos, 1);
            }
        }
    }

    /// <summary>
    /// ½Ãµå ±â¹İÀÇ ·£´ıÀÌ °¡´ÉÇÏµµ·Ï ¼³Á¤.
    /// </summary>
    void RandomNumberGenSetup()
    {
        int seednum = 0;
        for(int index = 0; index < 8; index++)
        {
            int c = (int)Seed[index] * (int)Mathf.Pow(index * (int)Seed[index], index);
            seednum += c;
        }
        print("Seed is " + Seed);
        print("SeedBased Number is " + seednum);
        int[] noiseValues;
        Random.seed = seednum;
    }

    /// <summary>
    /// ·£´ıÇÑ yÁÂÇ¥¸¦ ½Ãµå ±â¹İÀ¸·Î Ãâ·Â.
    /// </summary>
    /// <returns></returns>
    int RandomNumberVert()
    {
        return (Random.Range(0, block_VertLength));
    }

    /// <summary>
    /// ·£´ıÇÑ xÁÂÇ¥¸¦ ½Ãµå ±â¹İÀ¸·Î Ãâ·Â.
    /// </summary>
    /// <returns></returns>
    int RandomNumberHoriz()
    {
        return (Random.Range(0, block_EndPosition));
    }

    /// <summary>
    /// ·£´ıÇÑ ¹æÇâÀ» ½Ãµå ±â¹İÀ¸·Î Ãâ·Â.
    /// </summary>
    /// <returns></returns>
    int RandomNumberDir()
    {
        return (Random.Range(0, 4));
    }

    /// <summary>
    /// Object PileµéÀ» »ı¼º.
    /// </summary>
    void DataBasePositionSelection()
    {
        for(int i = 0; i < block_EndPosition / 8; i++)
        {
            GenObjectPile(RandomNumberHoriz(), RandomNumberVert(), 0);
            GenObjectPile(RandomNumberHoriz(), RandomNumberVert(), 1);
        }
    }

    /// <summary>
    /// ÇØ´ç ÁöÁ¡¿¡ Object PileÀ» »ı¼ºÇÔ.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="type"></param>
    void GenObjectPile(int x, int y, int type)
    {
        int cnt = 15;
        int x_setpos = x;
        int y_setpos = y;

        for(int i = 0; i < cnt; i++)
        {
            Vector2Int changedpos = PileGenBasedOnPos(x_setpos, y_setpos);
            x_setpos = changedpos.x;
            y_setpos = changedpos.y;
            if (IsObjectPosValid(x_setpos, y_setpos))
            {
                AddObject(x_setpos, y_setpos, type);
            }
        }
    }

    /// <summary>
    /// ÇØ´ç À§Ä¡¸¦ ±â¹İÀ¸·Î »ó/ÇÏ/ÁÂ/¿ì ÀÌµ¿À» µÎ¾î ÇÑÄ­ ¶³¾îÁø ·£´ıÇÑ Ä­À» ÁöÁ¤ÇØÁÖ´Â ÇÔ¼ö.
    /// </summary>
    /// <param name="x_setpos"></param>
    /// <param name="y_setpos"></param>
    /// <returns></returns>
    Vector2Int PileGenBasedOnPos(int x_setpos, int y_setpos)
    {
        switch (RandomNumberDir())
        {
            case 0: //À§ÂÊ
                if (y_setpos < block_VertLength-1)
                {
                    y_setpos++;
                }
                break;
            case 1: // ¿À¸¥ÂÊ
                if (x_setpos < block_HorizLength-1)
                {
                    x_setpos++;
                }
                break;
            case 2: //¿ŞÂÊ
                if (x_setpos > 0)
                {
                    x_setpos--;
                }
                break;
            case 3: //¾Æ·¡ÂÊ
                if (y_setpos > 0)
                {
                    y_setpos--;
                }
                break;
        }
        //print(x_setpos + " " + y_setpos);
        return new Vector2Int(x_setpos, y_setpos);
    }

    /// <summary>
    /// ÇØ´ç ÁÂÇ¥ÀÇ Æ÷Áö¼Ç¿¡ object¸¦ ³õ´Â°ÍÀÌ °¡´ÉÇÑÁö ¿©ºÎ¸¦ ¾Ë·ÁÁÖ´Â ÇÔ¼ö.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    bool IsObjectPosValid(int x, int y)
    {
        foreach (Vector2Int vec in colliderBlockSet)
        {
            if (vec.x == x && vec.y == y)
            {
                print("¿ÀºêÁ§Æ®¿Í ¹° ºí·° °ãÄ§ È®ÀÎ");
                return false;
            }
        }

        foreach (Vector2Int vec2 in objectPos)
        {
            if(vec2.x == x && vec2.y == y)
            {
                print("¿ÀºêÁ§Æ®¿Í ¿ÀºêÁ§Æ® °ãÄ§ È®ÀÎ");
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// ÇØ´ç ÁÂÇ¥¿¡ ¸¸¾à collider blockÀÌ ÀÖ´Ù¸é (¹°, ¿ë¾Ï) ÇØ´ç ¼Ó¼ºÀ» Á¦°Å.
    /// replaceblock°ú ÇÔ²² »ç¿ëÇÒ ¿¹Á¤.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    bool RemoveColliderBlockPos(int x, int y)
    {
        foreach(Vector2Int vec2 in colliderBlockSet)
        {
            if(vec2.x == x && vec2.y == y)
            {
                colliderBlockSet.Remove(vec2);
                return true;
            }
        }
        return false;
    }

    

    

    /// <summary>
    /// ÇØ´ç ÁÂÇ¥Ãà¿¡ Å¸ÀÔ¿¡ ÇØ´çÇÏ´Â ¿ÀºêÁ§Æ®¸¦ Ãß°¡ÇÏ°í, ±× ¿ÀºêÁ§Æ®¿Í À§Ä¡¸¦ ¸®½ºÆ®¿¡ ÀúÀå.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="type"></param>
    void AddObject(int x, int y, int type)
    {
        
        if (IsObjectPosValid(x,y))
        {
            GameObject tempObject;
            if (type == 0) //wood
            {
                tempObject = Instantiate(wood, new Vector3(x * BLOCK_SIZE, 0.0f, y * BLOCK_SIZE), Quaternion.identity, this.transform);
            }
            else if (type == 1) // stone
            {
                tempObject = Instantiate(stone, new Vector3(x * BLOCK_SIZE, 1.6f, y * BLOCK_SIZE), Quaternion.identity, this.transform);
            }
            else if(type == 2) // rock
            {
                tempObject = Instantiate(rock, new Vector3(x * BLOCK_SIZE, 1.6f, y * BLOCK_SIZE), Quaternion.identity, this.transform);
            }
            else if(type == 4) // rail
            {
                tempObject = Instantiate(rail, new Vector3(x * BLOCK_SIZE, 1.6f, y * BLOCK_SIZE), Quaternion.identity, this.transform);
            }
            else if(type == 5) // station
            {
                tempObject = Instantiate(station, new Vector3(x * BLOCK_SIZE + 0.8f, 1.6f, y * BLOCK_SIZE), Quaternion.identity * Quaternion.Euler(0,180,0), this.transform);
            }
            else if(type == 9) // marker
            {
                tempObject = Instantiate(marker, new Vector3(x * BLOCK_SIZE, 1.6f, y * BLOCK_SIZE), Quaternion.identity, this.transform);
            }
            else
            {
                tempObject = new GameObject();
                print("Á¤ÇØÁöÁö ¾ÊÀº Å¸ÀÔÀÇ ¿ÀºêÁ§Æ®¸¦ ¼³Ä¡ÇÏ·Á Çß½À´Ï´Ù.");
            }
            objectSet.Add(tempObject);
            objectPos.Add(new Vector2Int(x, y));
        }
        else
        {
            print("(" + x + "," + y + ") ÁöÁ¡ÀÇ (" + type + " )¿ÀºêÁ§Æ® Ãß°¡°¡ ¿ÀºêÁ§Æ® °ãÄ§À¸·Î Ãë¼ÒµÇ¾ú½À´Ï´Ù.");

        }
    }

    /// <summary>
    /// ÇØ´ç ÁÂÇ¥Ãà¿¡ Á¸ÀçÇÏ´Â ºí·°À» ¸®ÅÏ.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    GameObject GetBlock(int x, int y)
    {
<<<<<<< HEAD
        return blockSet[block_VertLength * x + y];
    }

    /// <summary>
    /// ºí·°À» ÇØ´ç ÁÂÇ¥¿¡ Ãß°¡ÇÔ. ÄÚµå ´õ·¯¿ò ¼öÁ¤ ÇÊ¿ä.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="type"></param>
    void AddBlock(int x, int y, int type)
=======
        FirstSetup();
        
        MapSetUp(40, 1);

    }

    private void FirstSetup() 
>>>>>>> 6f6e62d... ë§µ ë ˆì´ì•„ì›ƒ ì¶”ê°€ ë° ëª¹ ê¸°ì´ˆ ì„¤ì •
    {
        GameObject tempBlock;
        if(type == 0)
        {
            tempBlock = Instantiate(dummy, new Vector3(x * BLOCK_SIZE, 0, y * BLOCK_SIZE), Quaternion.identity, this.transform);
        }
        if (type == 1) // dirt
        {  
            tempBlock = Instantiate(dirt, new Vector3(x * BLOCK_SIZE, 0, y * BLOCK_SIZE), Quaternion.identity, this.transform);
        }
        else if(type == 2) // water
        {
            tempBlock = Instantiate(water, new Vector3(x * BLOCK_SIZE, 0, y * BLOCK_SIZE), Quaternion.identity, this.transform);
            colliderBlockSet.Add(new Vector2Int(x, y));
        }
        else
        {
            tempBlock = new GameObject();
        }
        blockSet.Insert(block_VertLength * x + y, tempBlock);
    }

    /// <summary>
    /// x,y,zÀÇ ÇÔ¼ö¸¦ ReplaceÇÏ´Â ÇÔ¼ö. 
    /// 0Àº ºó°Å, 1Àº Èë, 2´Â ¹°
    /// ÇöÀç ÄÚµå ´õ·¯¿ò.
    /// </summary>
    void ReplaceBlock(int x, int y, int type)
    {
        Destroy(blockSet[block_VertLength * x + y]);
        blockSet.RemoveAt(block_VertLength * x + y);
        RemoveColliderBlockPos(x, y);
        AddBlock(x, y, type);

    }

    /// <summary>
    /// ÇöÀç BlockSetÀÇ »óÅÂ°¡ ValidÇÏ´ÂÁö È®ÀÎÇÏ´Â ÇÔ¼ö.
    /// </summary>
    /// <returns></returns>
    bool IsBlockSetValid()
    {
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
