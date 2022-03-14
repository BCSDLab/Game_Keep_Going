using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    MyPlayer _myPlayer;
    Dictionary<int, Resource> _resources = new Dictionary<int, Resource>();
    public static ResourceManager Instance { get; } = new ResourceManager();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
