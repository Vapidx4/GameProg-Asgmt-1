using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrbCollection : MonoBehaviour
{
   public GameObject particles;
   private void OnTriggerEnter(Collider other)
   {
      if (other.transform.CompareTag("BlueOrb"))
      {
         Debug.Log("Blue Orb Collected");
         gameObject.GetComponent<CharacterMovement>().canDoubleJump = 1;
         Destroy(Instantiate(particles, transform.position, transform.rotation),10);
         
         other.gameObject.SetActive(false);
         
         //wait 30 seconds and make it active again
         StartCoroutine(ReactivateBlueOrb(other.gameObject, 30f));
         
      }
      
      if (other.transform.CompareTag("YellowOrb"))
      {
         Debug.Log("Yellow Orb Collected");
         GameManager.Instance.IncrementScore(50);
         Destroy(Instantiate(particles, transform.position, transform.rotation),10);
         Destroy(other.gameObject);
      }
   }
   
   private IEnumerator ReactivateBlueOrb(GameObject blueOrb, float waitTime)
   {
      yield return new WaitForSeconds(waitTime);
      blueOrb.SetActive(true);
   }
}
