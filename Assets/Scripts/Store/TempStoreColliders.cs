using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempStoreColliders : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            // 스토어 매니저에서 게임 다음스테이지로 넘어가는 부분 만들기.
        }
    }
}
