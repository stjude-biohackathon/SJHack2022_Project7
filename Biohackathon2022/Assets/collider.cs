using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class collider : MonoBehaviour
{
    public GameObject explosion;

    string[] collisionExclusion = {"Dummy", "Shaft"};

    private void OnTriggerEnter(Collider other){

        if (!collisionExclusion.Contains(other.transform.name)){

            Debug.Log("Hits detected");

            GameObject e = Instantiate(explosion, other.transform.position, Quaternion.identity) as GameObject;
        }

    }
}
