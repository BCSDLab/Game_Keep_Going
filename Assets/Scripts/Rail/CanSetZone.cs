using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSetZone : MonoBehaviour
{
    public bool isThereRail = false;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Rail"))
		{
            //this.gameObject.SetActive(false);
            isThereRail = true;
            Debug.Log("레일이 설치되어있음");
		}
	}
}
