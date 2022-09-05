using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]
    public int PlayerId;

    private void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
    }
}
