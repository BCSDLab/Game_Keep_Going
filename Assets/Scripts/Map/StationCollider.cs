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
        // �񵿱������� Scene�� �ҷ����� ���� Coroutine�� ����Ѵ�.
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
