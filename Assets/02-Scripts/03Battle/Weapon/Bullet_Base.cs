using UnityEngine;
namespace TeaFramework
{
   public class Bullet_Base : MonoBehaviour
   {
      bool canHit = true;
      public int damage = 1;
      private void OnTriggerEnter(Collider other)
      {
         if (!canHit) return;

         var bullet = other.transform.FindParentComponent<Bullet_Base>();
         if (bullet) return;

         var enemy = other.transform.FindParentComponent<EnemyEntity_Main>();

         if (enemy)
         {
            enemy.BeHit(damage);
            canHit = false;
         }
         Destroy(gameObject, 0.05f);
         Debug.Log("子弹碰撞到对象后销毁" + other.gameObject.name);
      }
   }
}