using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartWindow : MonoBehaviour
{
    GameObject windowImg;
    GameObject roomBtn;

    NetworkManager networkManager;

    // Start is called before the first frame update
    void Start()
    {
        windowImg = Resources.Load("Prefabs/WindowImage") as GameObject;
        roomBtn = Resources.Load("Prefabs/RoomBtn") as GameObject;
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWindow()
    {
        Vector2 vec2Center = new Vector2(480, 240);
        GameObject.Instantiate(windowImg, vec2Center, Quaternion.identity, GameObject.Find("Canvas").transform);

        for (int i = 1; i < 10; i++)
        {
            int idx = i;
            Vector2 vec2Btn = new Vector2();
            vec2Btn.x = vec2Center.x + 70 * ((i - 1) % 3 - 1);
            vec2Btn.y = vec2Center.y - 70 * ((i - 1) / 3 - 1);
            roomBtn.GetComponentInChildren<Text>().text = i.ToString();
            GameObject cloneBtn = GameObject.Instantiate(roomBtn, vec2Btn, Quaternion.identity, GameObject.Find("Canvas").transform);
            cloneBtn.AddComponent<RoomButton>();
            cloneBtn.SetActive(true);
            cloneBtn.GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene(1); networkManager.ConnectRoom(idx);});
        }
    }

}
