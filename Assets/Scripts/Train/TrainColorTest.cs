using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainColorTest : MonoBehaviour
{
    Color color;
    Coroutine colorCorutine;
    float colorTime = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        color = GetComponent<MeshRenderer>().materials[0].color;
        StartColor();
    }

    void StartColor()
    {
        if (colorCorutine != null)
            StopCoroutine(colorCorutine);

        colorCorutine = StartCoroutine(ColorCorutine());
        Vector3 timePos = transform.position;
    }
    IEnumerator ColorCorutine()
    {
            float curtime = 0.0f;
        while (color.r < 20.0f)
        {
            curtime += Time.deltaTime;
            color.r = Mathf.Pow(1.1f, curtime);
            GetComponent<MeshRenderer>().materials[0].color = color;
            yield return new WaitForFixedUpdate();
        }
        color.r = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
