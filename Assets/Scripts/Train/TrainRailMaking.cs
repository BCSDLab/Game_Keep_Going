using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainRailMaking : MonoBehaviour
{
    Image timeImage;
    Coroutine makingCorutine;
    static float makeTime = 3.0f;
    public int stoneNum, woodNum;
    TrainRailSaving trainRailSaving;
    // Start is called before the first frame update
    void Start()
    {
        trainRailSaving = GameObject.Find("train_savemodule").GetComponent<TrainRailSaving>();
        timeImage = GameObject.Find("timeImage").GetComponent<Image>();
    }
    void init()
    {
        stoneNum = 0;
        woodNum = 0;
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

    void RailMaking()
    {
        trainRailSaving.CreateRails();
        stoneNum--;
        woodNum--;
        timeImage.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        while(stoneNum * woodNum > 0 && timeImage.enabled == false)
        {
            StartRailMaking();
        }
    }
}
