using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeaFramework
{
   [System.Serializable]
   public abstract class WepMode_0Base
   {
      public Transform showPoint;
      public Weapon_ObjectComponent wepObjComp;

      /// <summary> 武器 伤害 </summary>
      public int damage;
      /// <summary> 武器 射程 </summary>
      public int gunshot;
      /// <summary> 武器 换弹时间 </summary>
      public float reload;
      /// <summary> 武器 弹匣容量 </summary>
      public int magazineSize;
      /// <summary> 武器 射速 </summary>
      public int firingRate;
      /// <summary> 武器 稳定性 </summary>
      public int stability;

      /// <summary>  弹速 </summary>
      public float bulletVelocity = 50;

      /// <summary>  弹速 </summary>
      public float expansion01 = 1;
      /// <summary>  弹速 </summary>
      public float expansion02 = 2;

      /// <summary> 开火间隔 </summary>
      private float firingInterval;
      /// <summary> 开火CD </summary>
      protected float canFiringTime;
      /// <summary> 扣下扳机 </summary>
      public virtual void PressTriggrt()
      {
         if (Time.time > canFiringTime)
         {
            if (firingInterval <= 0) firingInterval = 60f / firingRate;
            canFiringTime = Time.time + firingInterval;
            // 执行一个开火事件
            //Debug.Log("按下开火键,cd" + canFiringTime + " " + firingInterval);
            OnShootEvent();
         }
      }
      /// <summary> 时间步进 </summary>
      public virtual void TimeStep()
      {
      }

      public virtual void OnShootEvent()
      {
         wepObjComp.eff_MuzzleFire.Play();
         OneShoot();
      }
      /// <summary> 进行一次射击 </summary>
      public virtual void OneShoot()
      {
         // 子弹预制件不为空
         if (!wepObjComp.bulletPrefab) return;
         // 生成预制件
         var newBullet = InstBullet(showPoint.position, showPoint.eulerAngles);
         // 设定延迟后移除
         Object.Destroy(newBullet, 5);

         // 查询刚体组件 刚体力移动
         if (newBullet.TryGetComponent(out Rigidbody rb))
            rb.velocity = showPoint.forward * bulletVelocity;
         if (newBullet.TryGetComponent(out Bullet_Base bullet))
            bullet.damage = damage;
      }
      protected virtual GameObject InstBullet(Vector3 position, Vector3 eulerAngles)
      {
         var newBullet = Object.Instantiate(wepObjComp.bulletPrefab, position, Quaternion.Euler(eulerAngles));
         // 设定延迟后移除
         Object.Destroy(newBullet, 5);
         return newBullet;

      }
   }
}