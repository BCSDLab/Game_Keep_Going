using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreBar : MonoBehaviour
{
    Color innerColor;

    // Start is called before the first frame update
    void Start()
    {
        innerColor = transform.GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
            innerColor = new Color(1,1,1);
            transform.GetComponent<Image>().color = innerColor;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
            innerColor = new Color(95 / 255f, 194 / 255f, 255 / 255f);
            transform.GetComponent<Image>().color = innerColor;
		}
	}
}
