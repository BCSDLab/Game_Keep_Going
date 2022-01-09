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
    private string Seed = "sdxcveas"; // �õ�� 8�ܾ��� String���� ����.

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
    /// ���� Setting.
    /// </summary>
    void FirstSetup()
    {
        // ��� ��ü�� �ν��Ͻ� �ο�.
        block_EndPosition = block_HorizLength;
        objectSet = new List<GameObject>();
        objectPos = new List<Vector2Int>();
        blockSet = new List<GameObject>();

        // ���� ������Ʈ.
        RandomNumberGenSetup(); // �õ��� ���� ���� ���ʷ����� ����.
        BaseField(); // ����� �Ǵ� �� ����.
        DataBasePositionSelection(); // �õ��� ������Ʈ ����.
        
    }

    /// <summary>
    /// ����� �Ǵ� ���� ����. 
    /// block_StartPosition�������� block_EndPosition������ 16�� ���� ���� ����.
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
    /// �õ� ����� ������ �����ϵ��� ����.
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
    /// ������ y��ǥ�� �õ� ������� ���.
    /// </summary>
    /// <returns></returns>
    int RandomNumberVert()
    {
        return (Random.Range(0, block_VertLength));
    }

    /// <summary>
    /// ������ x��ǥ�� �õ� ������� ���.
    /// </summary>
    /// <returns></returns>
    int RandomNumberHoriz()
    {
        return (Random.Range(0, block_EndPosition));
    }

    /// <summary>
    /// ������ ������ �õ� ������� ���.
    /// </summary>
    /// <returns></returns>
    int RandomNumberDir()
    {
        return (Random.Range(0, 4));
    }

    /// <summary>
    /// Object Pile���� ����.
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
    /// �ش� ������ Object Pile�� ������.
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
    /// �ش� ��ġ�� ������� ��/��/��/�� �̵��� �ξ� ��ĭ ������ ������ ĭ�� �������ִ� �Լ�.
    /// </summary>
    /// <param name="x_setpos"></param>
    /// <param name="y_setpos"></param>
    /// <returns></returns>
    Vector2Int PileGenBasedOnPos(int x_setpos, int y_setpos)
    {
        switch (RandomNumberDir())
        {
            case 0: //����
                if (y_setpos < block_VertLength)
                {
                    y_setpos++;
                }
                break;
            case 1: // ������
                if (x_setpos < block_HorizLength)
                {
                    x_setpos++;
                }
                break;
            case 2: //����
                if (x_setpos > 0)
                {
                    x_setpos--;
                }
                break;
            case 3: //�Ʒ���
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
    /// �ش� ��ǥ�� �����ǿ� object�� ���°��� �������� ���θ� �˷��ִ� �Լ�.
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
    /// �ش� ��ǥ�࿡ Ÿ�Կ� �ش��ϴ� ������Ʈ�� �߰��ϰ�, �� ������Ʈ�� ��ġ�� ����Ʈ�� ����.
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
    /// �ش� ��ǥ�࿡ �����ϴ� ���� ����.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    GameObject GetBlock(int x, int y)
    {
        return blockSet[block_VertLength * x + y];
    }

    /// <summary>
    /// ���� �ش� ��ǥ�� �߰���. �ڵ� ������ ���� �ʿ�.
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
    /// x,y,z�� �Լ��� Replace�ϴ� �Լ�. 
    /// ���� �ڵ� ������.
    /// </summary>
    void ReplaceBlock(int x, int y, int type)
    {
        Destroy(blockSet[block_VertLength * x + y]);
        blockSet.RemoveAt(block_VertLength * x + y);
        AddBlock(x, y, type);
    }

    /// <summary>
    /// ���� BlockSet�� ���°� Valid�ϴ��� Ȯ���ϴ� �Լ�.
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
