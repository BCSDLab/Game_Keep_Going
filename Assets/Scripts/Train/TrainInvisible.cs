using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainInvisible : MonoBehaviour
{
    private GameObject[] train;

    [SerializeField]
    private bool isInvisible = false;
    

    void Start()
    {
        train = GameObject.FindGameObjectsWithTag("Train");
    }

    
    void Update()
    {
        if (isInvisible)
            StartCoroutine(Invisible());
    }

    IEnumerator Invisible()
	{
        isInvisible = false;
        SetCollider();
        yield return new WaitForSeconds(5f);
        SetCollider();
	}


    private void SetCollider()
	{
        for(int i = 0; i < train.Length; i++)
		{
            train[i].GetComponent<BoxCollider>().enabled = !train[i].GetComponent<BoxCollider>().enabled;
        }
	}

 //   private void SetColor()
	//{
 //       for(int i = 0; i < train.Length; i++)
	//	{
 //           Debug.Log(train[i].transform.GetChild(0));
 //           Color originalColor = train[i].transform.GetChild(0).GetComponent<MeshRenderer>().materials[0].color;
 //           train[i].GetComponent<MeshRenderer>().materials[0].color = new Color(0, 0, 0);
	//	}
	//}
}
