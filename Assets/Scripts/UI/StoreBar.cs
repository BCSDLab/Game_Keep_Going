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

            // 이 아래는 새 스테이지 제너레이팅 겸 카메라 위치 복구용.
            other.gameObject.transform.position = MapManager.instance.playerReturnPos;
            MapManager.instance.SetupNewLevel(MapManager.instance.stageLength);
            CameraManager.instance.ChangeTargetToTrain();
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
