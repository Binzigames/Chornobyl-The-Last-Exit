using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    public Transform teleportTarget; 

    private void OnTriggerEnter(Collider other)
    {
        // ��������, �� �� �������
        if (other.CompareTag("Player"))
        {
            other.transform.position = teleportTarget.position;
        }
    }
}

