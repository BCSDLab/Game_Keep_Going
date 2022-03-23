using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainInvisible : MonoBehaviour
{
    private BoxCollider train_mainmodule;
    private BoxCollider train_railmakingmodule;
    private BoxCollider train_savemodule;
    private BoxCollider boxCollider;

    [SerializeField]
    private bool isInvisible = false;
    

    void Start()
    {
        train_mainmodule = GameObject.Find("train_mainmodule").GetComponent<BoxCollider>();
        train_railmakingmodule = GameObject.Find("train_railmakingmodule").GetComponent<BoxCollider>();
        train_savemodule = GameObject.Find("train_savemodule").GetComponent<BoxCollider>();
        boxCollider = GetComponent<BoxCollider>();
    }

    
    void Update()
    {
        if (isInvisible)
            StartCoroutine(Invisible());
    }

    IEnumerator Invisible()
	{
        SetCollider();
        yield return new WaitForSeconds(5f);
        SetCollider();
        isInvisible = false;
	}


    private void SetCollider()
	{
        train_mainmodule.enabled = !train_mainmodule.enabled;
        train_railmakingmodule.enabled = !train_railmakingmodule.enabled;
        train_savemodule.enabled = !train_savemodule.enabled;
        boxCollider.enabled = !boxCollider.enabled;
	}
}
