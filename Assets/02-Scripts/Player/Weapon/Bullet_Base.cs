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
            enemy.BeHit(Random.Range(1,1000));
         }

         if (other.gameObject.CompareTag("enemy"))
         {
            Destroy(gameObject);
         }
      }
   }
}