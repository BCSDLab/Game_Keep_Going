using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StartBar : MonoBehaviour
{
    Color innerColor;
    Color originColor;
    Image thisImage;
    int maxPlayer = 1;
    int curPlayer = 0;
    Coroutine startCoroutine = null;
    TextMeshProUGUI startText;

    // Start is called before the first frame update
    void Start()
    {
        thisImage = GetComponent<Image>();
        innerColor = new Color(1, 1, 1);
        originColor = new Color(95 / 255f, 194 / 255f, 255 / 255f);
        startText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        ChangeStartText();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    void ChangeStartText()
    {
        maxPlayer = GameObject.FindGameObjectsWithTag("Player").Length;
        startText.text = "시작하기 (" + curPlayer + "/" + maxPlayer + ")";
    }

    IEnumerator StartingTime()
    {
        float curTime = 0.0f;
        while (curTime < 5.0f)
        {
            curTime += Time.deltaTime;
            thisImage.fillAmount = (curTime / 5.0f);
            yield return new WaitForFixedUpdate();
        }
        StartGame();
    }

    private void StartGame()
    {
        SceneManager.LoadScene(2);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            curPlayer++;
            ChangeStartText();
            thisImage.color = innerColor;
            if(curPlayer == maxPlayer)
            {
                if(startCoroutine == null)
                    startCoroutine = StartCoroutine(StartingTime());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            curPlayer--;
            ChangeStartText();
            if (curPlayer == 0)
            {
                transform.GetComponent<Image>().color = originColor;
                StopCoroutine(startCoroutine);
                startCoroutine = null;
            }
        }
    }
}
