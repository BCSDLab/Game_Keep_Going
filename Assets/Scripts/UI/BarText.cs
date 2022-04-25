using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BarText : MonoBehaviour
{
    Color textColor;

    // Start is called before the first frame update
    void Start()
    {
        textColor = transform.GetComponent<TextMeshProUGUI>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
            textColor = new Color(95 / 255f, 194 / 255f, 255 / 255f);
            transform.GetComponent<TextMeshProUGUI>().color = textColor;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
            textColor = new Color(1, 1, 1);
            transform.GetComponent<TextMeshProUGUI>().color = textColor;
		}
	}
}
