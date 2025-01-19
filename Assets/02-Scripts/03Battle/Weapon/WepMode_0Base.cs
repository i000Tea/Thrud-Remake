using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;

namespace TeaFramework
{
   [System.Serializable]
   public abstract class WepMode_0Base
   {
      #region 组件引用
      public Transform showPoint;
      public Weapon_ObjectComponent wepObjComp;
      #endregion

      #region 武器参数
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
      /// <summary> 子弹速度 </summary>
      protected virtual float bulletVelocity => 50;
      /// <summary> 扩散系数1 </summary>
      public float expansion01 = 1;
      /// <summary> 扩散系数2 </summary>
      public float expansion02 = 2;
      #endregion

      #region 射击控制
      /// <summary> 开火间隔 </summary>
      private float firingInterval;
      /// <summary> 开火CD </summary>
      protected float canFiringTime;
      #endregion


      /// <summary> 扣下扳机 </summary>
      public virtual void PressTriggrt()
      {
         if (Time.time > canFiringTime)
         {
            if (firingInterval <= 0) firingInterval = 60f / firingRate;
            canFiringTime = Time.time + firingInterval;
            // 执行一个开火事件
            //Debug.Log("按下开火键,cd" + canFiringTime + " " + firingInterval);
            MuzzleFireEffect();
            OnShootEvent();
         }
      }

      public virtual void OnShootEvent()
      {
         OneShoot(0);
      }
      public virtual void MuzzleFireEffect()
      {
         wepObjComp.eff_MuzzleFire.Play();
      }
      #region 子弹管理
      /// <summary> 进行一次射击 </summary>
      public virtual void OneShoot(int index)
      {
         // 子弹预制件不为空
         if (!wepObjComp.bulletPrefab) return;
         // 生成预制件
         var newBullet = Bullet_Inst(showPoint.position, showPoint.eulerAngles, index);

         Bullet_Stability(newBullet.transform, index);
         Bullet_Shoot(newBullet.transform, index);
         Bullet_SetDamage(newBullet.transform, index);

         // 设定延迟后移除
         Object.Destroy(newBullet, 5);
      }

      protected virtual GameObject Bullet_Inst(Vector3 position, Vector3 eulerAngles, int index = 0)
      {
         var newBullet = Object.Instantiate(wepObjComp.bulletPrefab, position, Quaternion.Euler(eulerAngles));
         // 设定延迟后移除
         Object.Destroy(newBullet, 5);
         return newBullet;
      }
      protected virtual void Bullet_Stability(Transform bullet, int index = 0)
      {
         // stability范围为0-100，值越大越稳定
         // 将stability限制在0-100范围内
         int clampedStability = Mathf.Clamp(stability, 0, 100);
         float maxDeviationAngle = (100f - clampedStability) * 0.05f;
         
         // 使用stability值来控制散布分布
         float randomPowerDistribution(float maxValue)
         {
             float randomValue = Random.value;
             // stability越高，幂次越大，分布越集中
             float power = 1 + (clampedStability / 25f); // 幂次范围为1-5
             float poweredRandom = Mathf.Pow(randomValue, power);
             return (Random.value > 0.5f ? 1 : -1) * poweredRandom * maxValue;
         }

         // 使用新的随机分布函数生成偏移值
         float horizontalDeviation = randomPowerDistribution(maxDeviationAngle);
         float verticalDeviation = randomPowerDistribution(maxDeviationAngle);
         
         // 应用角度偏移
         bullet.Rotate(new Vector3(verticalDeviation, horizontalDeviation, 0));
      }
      protected virtual void Bullet_Shoot(Transform bullet, int index = 0)
      {
         // 查询刚体组件 刚体力移动
         if (bullet.TryGetComponent(out Rigidbody rb))
         {
            rb.velocity = bullet.forward * bulletVelocity;
         }
      }
      protected virtual void Bullet_SetDamage(Transform bullet, int index = 0)
      {
         if (bullet.TryGetComponent(out Bullet_Base bulletB))
            bulletB.damage = damage;
      }
      #endregion
   }
}