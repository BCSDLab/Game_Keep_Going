using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 레일 설치 가능 존 표시
public class LastRail: MonoBehaviour
{
    [SerializeField]
    private PickUpPutDown player;
    [SerializeField]
    private CanSetZone canSetZone;


    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && player.isHoldRail && !canSetZone.isThereRail)
		{
            Transform trChild = this.transform.GetChild(0);
            trChild.gameObject.SetActive(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
            Transform trChild = this.transform.GetChild(0);
            trChild.gameObject.SetActive(false);
        }
	}
}
