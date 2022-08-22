using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainRailMaking : MonoBehaviour
{
    Image timeImage;
    Coroutine makingCorutine;
    static float makeTime = 3.0f;
    public int rockNum, woodNum;
    [SerializeField]
    TrainRailSaving trainRailSaving;
    [SerializeField]
    private bool canResoucePut = false;
    // Start is called before the first frame update
    void Start()
    {
        init();
        trainRailSaving = GameObject.Find("train_savemodule").GetComponent<TrainRailSaving>();
        timeImage = GameObject.Find("timeImage").GetComponent<Image>();
    }
    void init()
    {
        rockNum = 0;
        woodNum = 0;
    }

    public bool GetCanResoucePut()
    {
        return canResoucePut;
    }

    void StartRailMaking()
    {
        if (makingCorutine != null)
            StopCoroutine(makingCorutine);

        makingCorutine = StartCoroutine(RailMakingTime());
        Vector3 timePos = transform.position;
        timePos.y = 2.1f;
        timeImage.transform.position = Camera.main.WorldToScreenPoint(timePos);
        timeImage.enabled = true;
    }

    public void SavingResource(bool type, int num = 0)
    {
        //wood = false, Rock = true;
        if (type)
            rockNum += num;
        else
            woodNum += num;
        canResoucePut = false;
    }

    IEnumerator RailMakingTime()
    {
        float curTime = 0.0f;
        while (curTime < makeTime)
        {
            curTime += Time.deltaTime;
            timeImage.fillAmount = (curTime / makeTime);
            yield return new WaitForFixedUpdate();
        }
        RailMaking();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PickUpPutDown>().GetHoldItem() != null)
        {
            GameObject holdItem = other.GetComponent<PickUpPutDown>().GetHoldItem();
            if (holdItem.CompareTag("WoodStack") || holdItem.CompareTag("RockStack"))
            {
                canResoucePut = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PickUpPutDown>().GetHoldItem() != null)
        {
            GameObject holdItem = other.GetComponent<PickUpPutDown>().GetHoldItem();
            if (holdItem.CompareTag("WoodStack") || holdItem.CompareTag("RockStack"))
            {
                canResoucePut = false;
            }
        }
    }

    void RailMaking()
    {
        trainRailSaving.CreateRails();
        rockNum--;
        woodNum--;
        timeImage.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        while(rockNum * woodNum > 0 && timeImage.enabled == false)
        {
            StartRailMaking();
        }
    }
}
