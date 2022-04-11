using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collider : MonoBehaviour
{
    public GameObject explosion;

    private void OnTriggerEnter(Collider other){
        Debug.Log("Hits detected");
        GameObject e = Instantiate(explosion) as GameObject;

    }
}
