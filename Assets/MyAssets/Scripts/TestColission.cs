using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestColission : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("test");
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
    }
}
