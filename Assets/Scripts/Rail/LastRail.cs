using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastRail: MonoBehaviour
{
    [SerializeField]
    private PickUpPutDown player;
    [SerializeField]
    private GameObject canSetZone;

    // ���� ����


    // �ʿ��� ������Ʈ
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
