using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
    // Start is called before the first frame update
    Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.interactable = true;
        button.onClick.AddListener(RoomBtnOnClick);
    }

    void RoomBtnOnClick()
    {
        Debug.Log("1");
    }
    // Update is called once per frame
    void Update()
    {
    }
}
