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
        Main,
        RailMaking,
        Save,
        Brake,
        Conversion,
        Platform
    }

    public List<int> trainList;


    // Start is called before the first frame update
    void Start()
    {
        trainList = new List<int>();
        AddToList(ModuleType.Main);
        AddToList(ModuleType.RailMaking);
        AddToList(ModuleType.Save);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToList(ModuleType type)
	{
        trainList.Add((int)type);
	}

    public void RemoveToList(ModuleType type)
	{
        trainList.Remove((int)type);
	}
}
