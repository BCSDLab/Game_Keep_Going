using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastRail: MonoBehaviour
{
    [SerializeField]
    private PickUpPutDown player;
    [SerializeField]
    private GameObject canSetZone;

    // 상태 변수


    // 필요한 컴포넌트
    private BoxCollider theBoxCollider;
    
    void Start()
    {
        theBoxCollider = GetComponent<BoxCollider>();
    }

    
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && player.isHoldRail)
		{
            canSetZone.SetActive(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
            canSetZone.SetActive(false);
		}
	}
}
