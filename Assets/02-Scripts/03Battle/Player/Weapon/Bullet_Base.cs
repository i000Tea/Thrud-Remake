using UnityEngine;
namespace TeaFramework
{
   public class Bullet_Base : MonoBehaviour
   {
      bool canHit=true;
      private void OnTriggerEnter(Collider other)
      {
         if (!canHit) return;
         var enemy = other.transform.FindParentComponent<EnemyEntity_Main>();

         if (enemy)
         {
            enemy.BeHit();
            canHit = false;
         }
         Destroy(gameObject, 0.05f);      
      }
   }
}