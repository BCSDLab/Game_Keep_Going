using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreBar : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    Color innerColor;

    // Start is called before the first frame update
    void Start()
    {
        innerColor = transform.GetComponent<Image>().color;
        player = GameObject.Find("player");
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

            // �� �Ʒ��� �� �������� ���ʷ����� �� ī�޶� ��ġ ������.
            other.gameObject.transform.position = MapManager.instance.playerReturnPos;
            MapManager.instance.SetupNewLevel(MapManager.instance.stageLength);
            CameraManager.instance.ChangeTargetToTrain();
            // ���澾 ��Ź��.
            player.GetComponent<PickUpPutDown>().StoreLoadUnload();
            GameObject train = GameObject.Find("Train");
            train.GetComponent<LoadJsonData>().loadInStore = true;
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
