using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Base : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      Debug.Log(other.gameObject.tag);
      if (other.gameObject.CompareTag("enemy"))
      {
         Destroy(gameObject);
      }
   }
}
