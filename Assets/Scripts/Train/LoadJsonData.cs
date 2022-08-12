using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadJsonData : MonoBehaviour
{
    public ModuleData moduleData;
    private GameObject module;

    public bool load = false;


    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(load == true)
		{
            ConstructNewTrain();
            load = false;
		}
    }


    private void ConstructNewTrain()
	{
        //TrainSingleton.instance1.LoadModuleDataFromJson();

        string path = Path.Combine(Application.dataPath, "moduleData.json");
        string jsonData = File.ReadAllText(path);
        moduleData = JsonUtility.FromJson<ModuleData>(jsonData);


        for (int i = 0; i < 6; i++)
		{
            if(moduleData.moduleList[i] == 0)
			{
                continue;
			}
			else
			{
                if(moduleData.moduleList[i] == 1)
				{
                    module = Instantiate(Resources.Load("Prefabs/train_mainmodule") as GameObject);
                }
                else if(moduleData.moduleList[i] == 2)
				{
                    module = Instantiate(Resources.Load("Prefabs/train_railmakingmodule") as GameObject);
                }
                else if (moduleData.moduleList[i] == 3)
                {
                    module = Instantiate(Resources.Load("Prefabs/train_savemodule") as GameObject);
                }
                else if (moduleData.moduleList[i] == 4)
                {
                    module = Instantiate(Resources.Load("Prefabs/train_breakingmodule") as GameObject);
                }
                else if (moduleData.moduleList[i] == 5)
                {
                    module = Instantiate(Resources.Load("Prefabs/train_conversionmodule") as GameObject);
                }
                else if (moduleData.moduleList[i] == 6)
                {
                    module = Instantiate(Resources.Load("Prefabs/train_platformmodule") as GameObject);
                }

                module.transform.parent = GameObject.Find("Train2").transform.GetChild(i);
                module.transform.localPosition = Vector3.zero;
            }
		}
	}
}
