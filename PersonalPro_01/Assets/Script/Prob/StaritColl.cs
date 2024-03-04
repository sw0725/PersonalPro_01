using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaritColl : MonoBehaviour
{
    Transform GoPoint;

    private void Awake()
    {
        GoPoint = transform.GetChild(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) 
        {
            other.transform.position = GoPoint.position;
        }
    }
}
