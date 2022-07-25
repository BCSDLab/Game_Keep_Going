using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSingleton : MonoBehaviour
{
    public static TrainSingleton instance = null;

	private void Awake()
	{
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }


    public enum ModuleType
    {
        Default,
        Main,
        RailMaking,
        Save,
        Brake,
        Conversion,
        Platform
    }

    //public List<int> trainList;
    public Dictionary<string, int> moduleList;


    // Start is called before the first frame update
    void Start()
    {
        //trainList = new List<int>();
        moduleList = new Dictionary<string, int>();

        //moduleList.Add("Module1", (int)ModuleType.Main);
        //moduleList.Add("Module2", (int)ModuleType.RailMaking);
        //moduleList.Add("Module3", (int)ModuleType.Save);
        //moduleList.Add("Module4", (int)ModuleType.Default);
        //moduleList.Add("Module5", (int)ModuleType.Default);
        //moduleList.Add("Module6", (int)ModuleType.Default);

        moduleList.Add("Module1", 1); // 1은 ModuleCase칸에 모듈이 있는 상태
        moduleList.Add("Module2", 1);
        moduleList.Add("Module3", 1);
        moduleList.Add("Module4", 0); // 0은 ModuleCase칸에 모듈이 없는 상태
        moduleList.Add("Module5", 0);
        moduleList.Add("Module6", 0);
    }

    // Update is called once per frame
    void Update()
    {
        //moduleList["Module1"] = GameObject.Find("Train").transform.Find("ModuleCase1").childCount - 1;
        //moduleList["Module2"] = GameObject.Find("Train").transform.Find("ModuleCase2").childCount - 1;
        //moduleList["Module3"] = GameObject.Find("Train").transform.Find("ModuleCase3").childCount - 1;
        //moduleList["Module4"] = GameObject.Find("Train").transform.Find("ModuleCase4").childCount - 1;
        //moduleList["Module5"] = GameObject.Find("Train").transform.Find("ModuleCase5").childCount - 1;
        //moduleList["Module6"] = GameObject.Find("Train").transform.Find("ModuleCase6").childCount - 1;
    }

    public void Add(ModuleType type)
	{

	}

    public void Remove(ModuleType type)
	{
        //trainList.Remove((int)type);
	}
}
