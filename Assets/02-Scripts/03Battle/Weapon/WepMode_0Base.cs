using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeaFramework
{
   [System.Serializable]
   public abstract class WepMode_0Base
   {
      public Transform showPoint;
      public GameObject bulletPrefab;

      /// <summary> 武器 伤害 </summary>
      public Vector2Int wep_damage;
      /// <summary> 武器 射程 </summary>
      public Vector2Int wep_gunshot;
      /// <summary> 武器 换弹时间 </summary>
      public Vector2 wep_reload;
      /// <summary> 武器 弹匣容量 </summary>
      public Vector2Int wep_magazineSize;
      /// <summary> 武器 射速 </summary>
      public Vector2Int wep_firingRate;
      /// <summary> 武器 稳定性 </summary>
      public Vector2Int wep_stability;

      public float wep_shootSpeed = 50;

      /// <summary> 开火间隔 </summary>
      private float firingInterval;
      /// <summary> 开火CD </summary>
      protected float canFiringTime;
      /// <summary> 扣下扳机 </summary>
      public virtual void PressTriggrt()
      {
         if (Time.time > canFiringTime)
         {
            if (firingInterval <= 0) firingInterval = 60f / wep_firingRate.x;
            canFiringTime = Time.time + firingInterval;
            // 执行一个开火事件
            Debug.Log("按下开火键,cd" + canFiringTime + " " + firingInterval);
            OnShootEvent();
         }
      }
      /// <summary> 时间步进 </summary>
      public virtual void TimeStep()
      {
      }

      public virtual void OnShootEvent()
      {
         OneShoot();
      }
      /// <summary> 进行一次射击 </summary>
      public virtual void OneShoot()
      {
         // 子弹预制件不为空
         if (!bulletPrefab) return;
         // 生成预制件
         var newBullet = Object.Instantiate(bulletPrefab, showPoint.position, showPoint.rotation);
         // 设定延迟后移除
         Object.Destroy(newBullet, 5);
         // 查询刚体组件

         // 刚体力移动
         if (newBullet.TryGetComponent(out Rigidbody rb))
            rb.velocity = showPoint.forward * wep_shootSpeed;
         if (newBullet.TryGetComponent(out Bullet_Base bullet))         
            bullet.damage = wep_damage.x;
         
      }
   }
}