using UnityEngine;
namespace TeaFramework
{
   public class Bullet_Base : MonoBehaviour
   {
      private void OnTriggerEnter(Collider other)
      {
         //Debug.Log(other.gameObject.tag);
         if (other.transform.TryGetComponent(out EnemyEntity_Main enemy))
         {
            enemy.BeHit();
         }

         if (other.gameObject.CompareTag("enemy"))
         {
            Destroy(gameObject);
         }
      }
   }
}