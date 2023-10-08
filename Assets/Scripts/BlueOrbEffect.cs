using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueOrbEffect : MonoBehaviour
{
    public ParticleSystem particles;

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Player")){
            Debug.Log("Blue Orb Collected");
            gameObject.SetActive(false);

        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.right, 50 * Time.deltaTime);
    }
}