using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSetZone : MonoBehaviour
{
    public bool isThereRail = false;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Rail") && other.gameObject.layer == 0)
		{
            //this.gameObject.SetActive(false);
            isThereRail = true;
            Debug.Log("레일이 설치되어있음");
		}
	}
}
