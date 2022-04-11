using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other){
        Debug.Log("Hits detected");
    }
}
