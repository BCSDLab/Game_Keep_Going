using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StationCollider : MonoBehaviour
{
    int SceneLoad = 2;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Train")
        {
            LoadShopScene();
        }
    }

    public void LoadShopScene()
    {
        // 비동기적으로 Scene을 불러오기 위해 Coroutine을 사용한다.
        StartCoroutine(LoadMyAsyncScene());
    }

    IEnumerator LoadMyAsyncScene()
    {
      
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneLoad);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
