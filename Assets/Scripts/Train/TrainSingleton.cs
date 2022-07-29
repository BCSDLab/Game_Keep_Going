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

    [SerializeField]
    public Dictionary<string, int> moduleList;


    // Start is called before the first frame update
    void Start()
    {
        moduleList = new Dictionary<string, int>();

        moduleList.Add("Module1", 1);
        moduleList.Add("Module2", 2);
        moduleList.Add("Module3", 3);
        moduleList.Add("Module4", 0);
        moduleList.Add("Module5", 0);
        moduleList.Add("Module6", 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

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
            if(moduleList[moduleL[i]] == 0)
			{
                continue;
			}

            if(moduleList[moduleL[i-1]] == 0)
			{
                moduleList[moduleL[i-1]] = moduleList[moduleL[i]];
                moduleList[moduleL[i]] = 0;

                GameObject.Find("Train").transform.GetChild(i).transform.GetChild(1).parent = GameObject.Find("Train").transform.GetChild(i - 1);
                GameObject.Find("Train").transform.GetChild(i - 1).GetChild(1).transform.localPosition = Vector3.zero;
            }
		}

        for(int i = 0; i < moduleL.Count - 1; i++)
		{
            if(moduleList[moduleL[i]] != 0)
			{
                continue;
			}

            if(moduleList[moduleL[i+1]] != 0)
			{
                moduleList[moduleL[i]] = moduleList[moduleL[i + 1]];
                moduleList[moduleL[i + 1]] = 0;

                GameObject.Find("Train").transform.GetChild(i+1).transform.GetChild(1).parent = GameObject.Find("Train").transform.GetChild(i);
                GameObject.Find("Train").transform.GetChild(i).GetChild(1).transform.localPosition = Vector3.zero;
            }
		}


		Debug.Log("모듈 배열" + instance.moduleList["Module1"]);
		Debug.Log(instance.moduleList["Module2"]);
		Debug.Log(instance.moduleList["Module3"]);
		Debug.Log(instance.moduleList["Module4"]);
		Debug.Log(instance.moduleList["Module5"]);
		Debug.Log(instance.moduleList["Module6"]);
	}

}
