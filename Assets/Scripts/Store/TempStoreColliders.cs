using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempStoreColliders : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            // ����� �Ŵ������� ���� �������������� �Ѿ�� �κ� �����.
        }
    }
}
