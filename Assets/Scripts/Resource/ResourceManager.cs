using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ResourcesType
{
    Rail = 1,
    Axe = 2,
    PickAxe = 3,
    Wood = 4,
    Stone = 5,
}
public class ResourceManager
{

    MyPlayer _myPlayer;
    ArrayList _resources = new ArrayList();
    public static ResourceManager Instance { get; } = new ResourceManager();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddResource(GameObject gameObject)
    {
        _resources.Add(gameObject);
    }
    public void RemoveResource(int idx)
    {
        GameObject gameObject = (GameObject)_resources[idx];
        _resources.Remove(gameObject);
        GameObject.Destroy(gameObject);
    }
    public int GetResourceIdx(GameObject gameObject)
    {
        return _resources.IndexOf(gameObject);
    }

    /*
    public void Add(S_ResourceList packet)
    {

        foreach (S_ResourceList.Resource R in packet.resources)
        {
            if (R.type)
            {
                Object obj = Resources.Load("Prefabs/stone");
                GameObject go = Object.Instantiate(obj) as GameObject;
                Resource resource = go.AddComponent<Resource>();
                resource.transform.position = new Vector3(R.posX, 1.6f, R.posZ);
                _resources.Add(resource.ResourceId, resource);
            }
            else
            {
                Object obj = Resources.Load("Prefabs/wood");
                GameObject go = Object.Instantiate(obj) as GameObject;
                Resource resource = go.AddComponent<Resource>();
                resource.transform.position = new Vector3(R.posX, 1.6f, R.posZ);
                _resources.Add(resource.ResourceId, resource);
            }
        }
    }

    /// <summary>
    /// 자원의 위치를 변경
    /// </summary>
    /// <param name="pkt"></param>
    internal void SetLocation(S_BroadcastResource pkt)
    {
        
    }
    */
}
