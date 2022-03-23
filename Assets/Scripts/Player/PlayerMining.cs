using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMining : MonoBehaviour
{
    // Start is called before the first frame update
    Image timeImage;
    GameObject stoneResourceObj;
    GameObject woodResourceObj;
    Coroutine miningCorutine;
    
    static float mineTime = 3.0f;

    void Start()
    {
        timeImage = GameObject.Find("timeImage").GetComponent<Image>();
        stoneResourceObj = Resources.Load("Prefabs/rock_stack") as GameObject;
        woodResourceObj =  Resources.Load("Prefabs/wood_stack") as GameObject;
        miningCorutine = null;
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Resources")
        {
            if (miningCorutine == null)
            {
                GameObject resourceObject = collider.gameObject;
                Debug.Log(collider.gameObject.name);
                StartMining(resourceObject);
            }
        }
        else
        {
            return;
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Resources")
        {
            Debug.Log("stopMining");
            StopMining();
        }
    }

    public void StartMining(GameObject collObj)
    {
        if (miningCorutine != null)
            StopCoroutine(miningCorutine);

        miningCorutine = StartCoroutine(MiningTime(collObj));
        Vector3 timePos = collObj.transform.position;
        timePos.y = 2.6f;
        timeImage.transform.position = Camera.main.WorldToScreenPoint(timePos);
        timeImage.enabled = true;
    }
    public void StopMining()
    {
        StopCoroutine(miningCorutine);
        miningCorutine = null;
        timeImage.enabled = false;
    }
    public void Mining(GameObject collObj)
    {
        Vector3 resourcesPos = collObj.transform.position;
        resourcesPos.y = 1.6f;
        if(collObj.name == "stone(Clone)")
            Instantiate(stoneResourceObj, resourcesPos, collObj.transform.rotation);
        else if(collObj.name == "wood(Clone)")
            Instantiate(woodResourceObj, resourcesPos, collObj.transform.rotation);
        
        Destroy(collObj);
        miningCorutine = null;
        timeImage.enabled = false;
    }

    IEnumerator MiningTime(GameObject collObj)
    {
        float curTime = 0.0f;
        while(curTime < mineTime)
        {
            curTime += Time.deltaTime;
            timeImage.fillAmount = (curTime / mineTime);
            yield return new WaitForFixedUpdate();
        }
        Mining(collObj);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
