using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ��ġ ���� �� ǥ��
public class LastRail: MonoBehaviour
{
    [SerializeField]
    private PickUpPutDown player;
    //[SerializeField]
    //private GameObject canSetZone;

    // ���� ����


    
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && player.isHoldRail)
		{
            //this.gameObject.SetActive(true);

            Transform trChild = this.transform.GetChild(0);
            trChild.gameObject.SetActive(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
            //this.gameObject.SetActive(false);

            Transform trChild = this.transform.GetChild(0);
            trChild.gameObject.SetActive(false);
        }
	}
}
