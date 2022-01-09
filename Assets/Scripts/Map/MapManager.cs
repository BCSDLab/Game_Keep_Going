using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    private string Seed = "sdxcveas"; // 시드는 8단어의 String으로 구성.

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

    private List<GameObject> blockSet;
    private List<GameObject> objectSet;
    private List<Vector2Int> objectPos;
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
    /// 최초 Setting.
    /// </summary>
    void FirstSetup()
    {
        // 몇몇 객체에 인스턴스 부여.
        block_EndPosition = block_HorizLength;
        objectSet = new List<GameObject>();
        objectPos = new List<Vector2Int>();
        blockSet = new List<GameObject>();

        // 최초 업데이트.
        RandomNumberGenSetup(); // 시드기반 랜덤 숫자 제너레이팅 설정.
        BaseField(); // 기반이 되는 블럭 설정.
        DataBasePositionSelection(); // 시드기반 오브젝트 제작.
        
    }

    /// <summary>
    /// 기반이 되는 땅을 생성. 
    /// block_StartPosition에서부터 block_EndPosition까지의 16개 단위 블럭을 생성.
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
    /// 시드 기반의 랜덤이 가능하도록 설정.
    /// </summary>
    void RandomNumberGenSetup()
    {
        int seednum = 0;
        for(int index = 0; index < 8; index++)
        {
            int c = (int)Seed[index] * index;
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
        for(int i = 0; i < block_EndPosition / 8; i++)
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
        int cnt = 10;
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
                if (y_setpos < block_VertLength)
                {
                    y_setpos++;
                }
                break;
            case 1: // 오른쪽
                if (x_setpos < block_HorizLength)
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
        print(x_setpos + " " + y_setpos);
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
        foreach(Vector2Int vec2 in objectPos)
        {
            if(vec2.x == x && vec2.y == y)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 해당 좌표축에 타입에 해당하는 오브젝트를 추가하고, 그 오브젝트와 위치를 리스트에 저장.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="type"></param>
    void AddObject(int x, int y, int type)
    {
        GameObject tempObject;
        tempObject = new GameObject();
        if (type == 0) //wood
        {
            Destroy(tempObject);
            tempObject = Instantiate(wood, new Vector3(x * BLOCK_SIZE, 0.0f, y * BLOCK_SIZE), Quaternion.identity, this.transform);
        }
        if (type == 1) // stone
        {
            Destroy(tempObject);
            tempObject = Instantiate(stone, new Vector3(x * BLOCK_SIZE, 1.6f, y * BLOCK_SIZE), Quaternion.identity, this.transform);
        }
        objectSet.Add(tempObject);
        objectPos.Add(new Vector2Int(x, y));
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
        tempBlock = new GameObject();
        if(type == 0)
        {
            Destroy(tempBlock);
            tempBlock = Instantiate(dummy, new Vector3(x * BLOCK_SIZE, 0, y * BLOCK_SIZE), Quaternion.identity, this.transform);
        }
        if (type == 1) // dirt
        {
            Destroy(tempBlock);
            tempBlock = Instantiate(dirt, new Vector3(x * BLOCK_SIZE, 0, y * BLOCK_SIZE), Quaternion.identity, this.transform);
        }
        else if(type == 2)
        {
            Destroy(tempBlock);
            tempBlock = Instantiate(water, new Vector3(x * BLOCK_SIZE, 0, y * BLOCK_SIZE), Quaternion.identity, this.transform);
        }
        blockSet.Insert(block_VertLength * x + y, tempBlock);
    }

    /// <summary>
    /// x,y,z의 함수를 Replace하는 함수. 
    /// 현재 코드 더러움.
    /// </summary>
    void ReplaceBlock(int x, int y, int type)
    {
        Destroy(blockSet[block_VertLength * x + y]);
        blockSet.RemoveAt(block_VertLength * x + y);
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
}
