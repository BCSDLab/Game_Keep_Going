using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TrainSingleton : MonoBehaviour
{
    // instance�� 2�� �ּ� ù��°�� ����� 0�� instance, �ι����� ����� 1�� instance�� �ΰ� ����°���ʹ� destroy
    // �� 2���� ���� �ϴ� �Լ� ����� -> �������� ���� ��ġ�ϸ� ������Ʈ�� �����Ǽ� �ٸ� �������� �پ���� -> ������ �÷��� �� �������� ��쿡�� ���������� ���� �����Ǳ�
    public static TrainSingleton instance1 = null;
    //public static TrainSingleton instance2 = null;

	private void Awake()
	{
		if (instance1 == null)
		{
			instance1 = this;
			DontDestroyOnLoad(this.gameObject);
		}
		//else if (instance2 == null)
		//{
		//	instance2 = this;
		//	DontDestroyOnLoad(this.gameObject);
		//}
		else
		{
			Destroy(this.gameObject);
		}
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

    [SerializeField]
    public Dictionary<string, int> moduleDic;

    //public Dictionary<string, int> moduleList2;


    // Start is called before the first frame update
    void Start()
    {
        moduleDic = new Dictionary<string, int>();

        moduleDic.Add("Module1", 1);
        moduleDic.Add("Module2", 2);
        moduleDic.Add("Module3", 3);
        moduleDic.Add("Module4", 0);
        moduleDic.Add("Module5", 0);
        moduleDic.Add("Module6", 0);

        //moduleList2 = new Dictionary<string, int>();

        //moduleList2.Add("Module1", 1);
        //moduleList2.Add("Module2", 2);
        //moduleList2.Add("Module3", 3);
        //moduleList2.Add("Module4", 0);
        //moduleList2.Add("Module5", 0);
        //moduleList2.Add("Module6", 0);

        SyncData();
        SaveModuleDataToJson();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �������� ���� ��ų� ��ġ�� �� ��ĭ ����� ��ĭ ����
    public void PullTrain()
	{
        List<string> moduleL = new List<string>();
        moduleL.Add("Module1");
        moduleL.Add("Module2");
        moduleL.Add("Module3");
        moduleL.Add("Module4");
        moduleL.Add("Module5");
        moduleL.Add("Module6");

        for(int i = moduleL.Count - 1; i > 0; i--)
		{
            if(moduleDic[moduleL[i]] == 0)
			{
                continue;
			}

            if(moduleDic[moduleL[i-1]] == 0)
			{
                moduleDic[moduleL[i-1]] = moduleDic[moduleL[i]];
                moduleDic[moduleL[i]] = 0;

                GameObject.Find("Train").transform.GetChild(i).transform.GetChild(1).parent = GameObject.Find("Train").transform.GetChild(i - 1);
                GameObject.Find("Train").transform.GetChild(i - 1).GetChild(1).transform.localPosition = Vector3.zero;
            }
		}

        for(int i = 0; i < moduleL.Count - 1; i++)
		{
            if(moduleDic[moduleL[i]] != 0)
			{
                continue;
			}

            if(moduleDic[moduleL[i+1]] != 0)
			{
                moduleDic[moduleL[i]] = moduleDic[moduleL[i + 1]];
                moduleDic[moduleL[i + 1]] = 0;

                GameObject.Find("Train").transform.GetChild(i+1).transform.GetChild(1).parent = GameObject.Find("Train").transform.GetChild(i);
                GameObject.Find("Train").transform.GetChild(i).GetChild(1).transform.localPosition = Vector3.zero;
            }
		}

        SyncData();
        SaveModuleDataToJson();



        Debug.Log("�ν��ͽ�1�� ��� �迭" + instance1.moduleDic["Module1"]);
		Debug.Log(instance1.moduleDic["Module2"]);
		Debug.Log(instance1.moduleDic["Module3"]);
		Debug.Log(instance1.moduleDic["Module4"]);
		Debug.Log(instance1.moduleDic["Module5"]);
		Debug.Log(instance1.moduleDic["Module6"]);


		//Debug.Log("�ν��ͽ�2�� ��� �迭" + instance2.moduleList["Module1"]);
		//Debug.Log(instance2.moduleList["Module2"]);
		//Debug.Log(instance2.moduleList["Module3"]);
		//Debug.Log(instance2.moduleList["Module4"]);
		//Debug.Log(instance2.moduleList["Module5"]);
		//Debug.Log(instance2.moduleList["Module6"]);
	}




    public ModuleData moduleData;
    public Dictionary<string, int> newModuleDic;

    public void SyncData()
	{
        moduleData.moduleList[0] = moduleDic["Module1"];
        moduleData.moduleList[1] = moduleDic["Module2"];
        moduleData.moduleList[2] = moduleDic["Module3"];
        moduleData.moduleList[3] = moduleDic["Module4"];
        moduleData.moduleList[4] = moduleDic["Module5"];
        moduleData.moduleList[5] = moduleDic["Module6"];
    }


    public void SaveModuleDataToJson()
	{
        string jsonData = JsonUtility.ToJson(moduleData, true);
        string path = Path.Combine(Application.dataPath, "moduleData.json");
        File.WriteAllText(path, jsonData);
	}

    public void LoadModuleDataFromJson()
	{
        string path = Path.Combine(Application.dataPath, "moduleData.json");
        string jsonData = File.ReadAllText(path);
        moduleData = JsonUtility.FromJson<ModuleData>(jsonData);
	}


    public void SetNewModuleDic()
	{
        LoadModuleDataFromJson();

        newModuleDic.Add("Module1", moduleData.moduleList[0]);
        newModuleDic.Add("Module2", moduleData.moduleList[1]);
        newModuleDic.Add("Module3", moduleData.moduleList[2]);
        newModuleDic.Add("Module4", moduleData.moduleList[3]);
        newModuleDic.Add("Module5", moduleData.moduleList[4]);
        newModuleDic.Add("Module6", moduleData.moduleList[5]);
    }


}

[System.Serializable]
public class ModuleData
{
    public int[] moduleList = { 0, 0, 0, 0, 0, 0 };
}