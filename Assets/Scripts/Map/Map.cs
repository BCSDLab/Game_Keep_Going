using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Map : MonoBehaviour
{
    [SerializeField]
    private int stage_generation = 1;
    [SerializeField]
    private Vector3 map_BasedPos;

    private const float BLOCK_SIZE = 1.6f;

    private int block_StartPosition = 0;
    private int block_EndPosition;
    [SerializeField]
    private int block_HorizLength = 40;
    [SerializeField]
    private int block_VertLength = 24;

    [SerializeField]
    private string Seed = "qweasfds"; // 시드는 8단어의 String으로 구성.

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


    public void SetStageLevel()
    {
        stage_generation = MapManager.instance.stageLevel;
        block_HorizLength = MapManager.instance.stageLength;
        map_BasedPos = MapManager.instance.currentStartPos;
        Debug.Log("테스트 --" + map_BasedPos);
        // 플레이어의 수에 따라 가로축 또는 세로축의 길이가 달라지게.
    }

    // Start is called before the first frame update
    void Start()
    {
        ResourceLoad();
        SetStageLevel();
        FirstSetup();
    }

    void ResourceLoad()
    {
        dirt = Resources.Load("Prefabs/dirtBlock") as GameObject;
        water = Resources.Load("Prefabs/waterBlock") as GameObject;
        dummy = Resources.Load("Prefabs/dummyBlock") as GameObject;
        wood = Resources.Load("Prefabs/wood") as GameObject;
        stone = Resources.Load("Prefabs/stone") as GameObject;
        rock = Resources.Load("Prefabs/TempRock") as GameObject;
        marker = Resources.Load("Prefabs/Marker") as GameObject;
        station = Resources.Load("Prefabs/station_soviet") as GameObject;
        rail = Resources.Load("Prefabs/rail") as GameObject;
        lastrailpos = GameObject.Find("LastRailPos");
    }

    /// <summary>
    /// 최초 Setting.
    /// </summary>
    void FirstSetup()
    {

        // 몇몇 객체에 인스턴스 부여.
        block_EndPosition = block_HorizLength;
        objectSet = new List<GameObject>();
        objectPos = new List<Vector2Int>();
        blockSet = new List<GameObject>();
        colliderBlockSet = new List<Vector2Int>();
        LakeLineTop = new List<int>();
        LakeLineBottom = new List<int>();
        HillLineTop = new List<int>();
        HillLineBottom = new List<int>();
        // 최초 업데이트.

        if (NetworkManager.Instance.isHost)
        {
            RandomNumberGenSetup(); // 시드기반 랜덤 숫자 제너레이팅 설정.
            Debug.Log("Host");
        }
        else
            Random.seed = MapManager.instance.seed;
        BaseField(); // 기반이 되는 블럭 설정.
        LakeLineGen(); // Lake의 기준이 되는 Line생성.
        //LakeLineTest();
        LakeGroupGen(); // Lake생성.
        HillLineGen(); // Hill Line 생성.
        //HillLineTest();
        HillGroupGen(); // Hill Group 생성.

        DataBasePositionSelection(); // 시드기반 오브젝트 제작.
        StationGen(); // Station 생성.






    }

    void secondSetupTest()
    {

    }


    void StationGen()
    {
        int x = Random.Range(0, 4);
        int y = Random.Range(block_VertLength / 2 - 5, block_VertLength / 2 + 5);
        while (true)
        {
            if (IsObjectPosValid(x, y) && IsObjectPosValid(x + 1, y) && IsObjectPosValid(x, y - 1))
            {
                AddObject(x, y, 5);
                AddObject(x, y - 1, 4);
                lastrailpos.transform.position = new Vector3(x * BLOCK_SIZE, 1.6f, (y - 1) * BLOCK_SIZE);
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
    /// 강 지역이 반으로 가르지 않도록 만드는 기능. 양쪽 호수의 최대 범위를 만들어놓음.
    /// 위쪽 Line의 경우는 최대 길이시 가운데에서 3 떨어지고
    /// 아랫쪽 Line의 경우는 4칸 떨어지게.
    /// </summary>
    void LakeLineGen()
    {
        LakeLineTop.Add(Random.Range(8, (block_VertLength / 2) - 4));
        LakeLineBottom.Add(Random.Range((block_VertLength / 2) + 4, block_VertLength - 4));
        for (int i = 1; i < block_HorizLength; i++)
        {
            int up = LakeLineTop[LakeLineTop.Count - 1];
            int down = LakeLineBottom[LakeLineBottom.Count - 1];

            up += Random.Range(-3, 3);
            down += Random.Range(-3, 3);
            if (up > (block_VertLength / 2) - 1)
            {
                up = (block_VertLength / 2) - 1;
            }
            else if (up < 3)
            {
                up = 3;
            }
            if (down < (block_VertLength / 2) + 1)
            {
                down = (block_VertLength / 2) + 1;
            }
            else if (down > block_VertLength - 3)
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

                for (int dy = block_VertLength / 2; dy < ytop - 1; dy++)
                {

                    AddObject(dx, dy, 2);
                }
                for (int dy = block_VertLength / 2; dy > ybottom + 1; dy--)
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
                if (isReachedLine == false)
                {
                    break;
                }
                AddObject(dx, ytop, 2);
                AddObject(dx, ybottom, 2);


                for (int dy = block_VertLength / 2; dy < ytop - 1; dy++)
                {

                    AddObject(dx, dy, 2);
                }
                for (int dy = block_VertLength / 2; dy > ybottom + 1; dy--)
                {

                    AddObject(dx, dy, 2);
                }

            }
            dx++;
            print("왔냐?");
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
    void LakeLineTest()
    {
        for (int x = 0; x < block_HorizLength; x++)
        {
            for (int y = 0; y < LakeLineTop[x]; y++)
            {
                ReplaceBlock(x, y, 0);
            }
            for (int y = block_VertLength - 1; y > LakeLineBottom[x]; y--)
            {
                ReplaceBlock(x, y, 0);
            }
        }

    }

    /// <summary>
    /// 어느 면을 기점으로 해서 만들어지는 큰 호수 제작 스크립트. Lakeline을 기준으로 해서 그 선은 못넘음.
    /// </summary>
    void LakeGroupGen()
    {
        int countermaking = 0;

        while (countermaking != -1)
        {
            print(countermaking + "에서 위호수 생성.");
            countermaking = GenLakeFromTop(countermaking);
            if (countermaking > 4)
            {
                countermaking -= 4;
            }
            print(countermaking + "에서 아래호수 생성.");
            countermaking = GenLakeFromBottom(countermaking);
        }
    }

    /// <summary>
    /// 받은 x좌표를 시작으로 좌표상 위쪽 (바라보는 기준 아래쪽)의 호수 생성.
    /// 호수의 가장 마지막 좌표를 return.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    int GenLakeFromTop(int x)
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
    /// 받은 x좌표를 시작으로 좌표상 큰쪽 (바라보는 기준 위쪽)의 호수 생성.
    /// 호수의 가장 마지막 좌표를 return.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    int GenLakeFromBottom(int x)
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
    /// 기반이 되는 땅을 생성. 
    /// block_StartPosition에서부터 block_EndPosition까지의 16개 단위 블럭을 생성.
    /// </summary>
    void BaseField()
    {
        for (int index = block_StartPosition; index < block_EndPosition; index++)
        {
            for (int vertPos = 0; vertPos < block_VertLength; vertPos++)
            {
                AddBlock(index, vertPos, 1);
            }
        }
    }

    /// <summary>
    /// 시드 기반의 랜덤이 가능하도록 설정.
    /// </summary>
    void RandomNumberGenSetup()
    {
        int seednum = 0;
        for (int index = 0; index < 8; index++)
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
    /// 랜덤한 y좌표를 시드 기반으로 출력.
    /// </summary>
    /// <returns></returns>
    int RandomNumberVert()
    {
        return (Random.Range(0, block_VertLength));
    }

    /// <summary>
    /// 랜덤한 x좌표를 시드 기반으로 출력.
    /// </summary>
    /// <returns></returns>
    int RandomNumberHoriz()
    {
        return (Random.Range(0, block_EndPosition));
    }

    /// <summary>
    /// 랜덤한 방향을 시드 기반으로 출력.
    /// </summary>
    /// <returns></returns>
    int RandomNumberDir()
    {
        return (Random.Range(0, 4));
    }

    /// <summary>
    /// Object Pile들을 생성.
    /// </summary>
    void DataBasePositionSelection()
    {
        for (int i = 0; i < block_EndPosition / 8; i++)
        {
            GenObjectPile(RandomNumberHoriz(), RandomNumberVert(), 0);
            GenObjectPile(RandomNumberHoriz(), RandomNumberVert(), 1);
        }
    }

    /// <summary>
    /// 해당 지점에 Object Pile을 생성함.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="type"></param>
    void GenObjectPile(int x, int y, int type)
    {
        int cnt = 15;
        int x_setpos = x;
        int y_setpos = y;

        for (int i = 0; i < cnt; i++)
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
    /// 해당 위치를 기반으로 상/하/좌/우 이동을 두어 한칸 떨어진 랜덤한 칸을 지정해주는 함수.
    /// </summary>
    /// <param name="x_setpos"></param>
    /// <param name="y_setpos"></param>
    /// <returns></returns>
    Vector2Int PileGenBasedOnPos(int x_setpos, int y_setpos)
    {
        switch (RandomNumberDir())
        {
            case 0: //위쪽
                if (y_setpos < block_VertLength - 1)
                {
                    y_setpos++;
                }
                break;
            case 1: // 오른쪽
                if (x_setpos < block_HorizLength - 1)
                {
                    x_setpos++;
                }
                break;
            case 2: //왼쪽
                if (x_setpos > 0)
                {
                    x_setpos--;
                }
                break;
            case 3: //아래쪽
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
    /// 해당 좌표의 포지션에 object를 놓는것이 가능한지 여부를 알려주는 함수.
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
                print("오브젝트와 물 블럭 겹침 확인");
                return false;
            }
        }

        foreach (Vector2Int vec2 in objectPos)
        {
            if (vec2.x == x && vec2.y == y)
            {
                print("오브젝트와 오브젝트 겹침 확인");
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 해당 좌표에 만약 collider block이 있다면 (물, 용암) 해당 속성을 제거.
    /// replaceblock과 함께 사용할 예정.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    bool RemoveColliderBlockPos(int x, int y)
    {
        foreach (Vector2Int vec2 in colliderBlockSet)
        {
            if (vec2.x == x && vec2.y == y)
            {
                colliderBlockSet.Remove(vec2);
                return true;
            }
        }
        return false;
    }





    /// <summary>
    /// 해당 좌표축에 타입에 해당하는 오브젝트를 추가하고, 그 오브젝트와 위치를 리스트에 저장.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="type"></param>
    void AddObject(int x, int y, int type)
    {

        if (IsObjectPosValid(x, y))
        {
            GameObject tempObject;
            if (type == 0) //wood
            {
                tempObject = Instantiate(wood, map_BasedPos + new Vector3(x * BLOCK_SIZE, 0.0f, y * BLOCK_SIZE), Quaternion.identity, this.transform);
            }
            else if (type == 1) // stone
            {
                tempObject = Instantiate(stone, map_BasedPos + new Vector3(x * BLOCK_SIZE, 1.6f, y * BLOCK_SIZE), Quaternion.identity, this.transform);
            }
            else if (type == 2) // rock
            {
                tempObject = Instantiate(rock, map_BasedPos + new Vector3(x * BLOCK_SIZE, 1.6f, y * BLOCK_SIZE), Quaternion.identity, this.transform);
            }
            else if (type == 4) // rail
            {
                tempObject = Instantiate(rail, map_BasedPos + new Vector3(x * BLOCK_SIZE, 1.6f, y * BLOCK_SIZE), Quaternion.identity, this.transform);
            }
            else if (type == 5) // station
            {
                tempObject = Instantiate(station, map_BasedPos + new Vector3(x * BLOCK_SIZE + 0.8f, 1.6f, y * BLOCK_SIZE), Quaternion.identity * Quaternion.Euler(0, 180, 0), this.transform);
            }
            else if (type == 9) // marker
            {
                tempObject = Instantiate(marker, map_BasedPos + new Vector3(x * BLOCK_SIZE, 1.6f, y * BLOCK_SIZE), Quaternion.identity, this.transform);
            }
            else
            {
                tempObject = new GameObject();
                print("정해지지 않은 타입의 오브젝트를 설치하려 했습니다.");
            }
            objectSet.Add(tempObject);
            objectPos.Add(new Vector2Int(x, y));
        }
        else
        {
            print("(" + x + "," + y + ") 지점의 (" + type + " )오브젝트 추가가 오브젝트 겹침으로 취소되었습니다.");

        }
    }

    /// <summary>
    /// 해당 좌표축에 존재하는 블럭을 리턴.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    GameObject GetBlock(int x, int y)
    {
        return blockSet[block_VertLength * x + y];
    }

    /// <summary>
    /// 블럭을 해당 좌표에 추가함. 코드 더러움 수정 필요.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="type"></param>
    void AddBlock(int x, int y, int type)
    {
        GameObject tempBlock;
        if (type == 0)
        {
            tempBlock = Instantiate(dummy, map_BasedPos + new Vector3(x * BLOCK_SIZE, 0, y * BLOCK_SIZE), Quaternion.identity, this.transform);
        }
        if (type == 1) // dirt
        {
            tempBlock = Instantiate(dirt, map_BasedPos + new Vector3(x * BLOCK_SIZE, 0, y * BLOCK_SIZE), Quaternion.identity, this.transform);
        }
        else if (type == 2) // water
        {
            tempBlock = Instantiate(water, map_BasedPos + new Vector3(x * BLOCK_SIZE, -0.3f, y * BLOCK_SIZE), Quaternion.identity, this.transform);
            colliderBlockSet.Add(new Vector2Int(x, y));
        }
        else
        {
            tempBlock = new GameObject();
        }
        blockSet.Insert(block_VertLength * x + y, tempBlock);
    }

    /// <summary>
    /// x,y,z의 함수를 Replace하는 함수. 
    /// 0은 빈거, 1은 흙, 2는 물
    /// 현재 코드 더러움.
    /// </summary>
    void ReplaceBlock(int x, int y, int type)
    {
        Destroy(blockSet[block_VertLength * x + y]);
        blockSet.RemoveAt(block_VertLength * x + y);
        RemoveColliderBlockPos(x, y);
        AddBlock(x, y, type);

    }

    /// <summary>
    /// 현재 BlockSet의 상태가 Valid하는지 확인하는 함수.
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

    public int GetResourceIndex(GameObject gameObject)
    {
        Vector2Int vec2 = new Vector2Int(((int)gameObject.transform.position.x), (int)gameObject.transform.position.z);
        int resourceIdx;
        resourceIdx = objectPos.IndexOf(vec2);
        return resourceIdx;
    }

    public GameObject GetResourceObject(int resourceIdx)
    {
        GameObject gameObject = objectSet[resourceIdx];
        return gameObject;
    }
}
