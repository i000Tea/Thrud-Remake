using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeaFramework
{
   /// <summary> 散射型武器 </summary>
   public abstract class WepMode_Scatter : WepMode_0Base
   {
      public override void OnShootEvent()
      {
         for (int i = 0; i < expansion01; i++)
         {
            OneShoot(i);
         }
      }
   }
   /// <summary> 散射型武器 重击霰弹 </summary>
   public class WepMode_Scatter_SmashShot : WepMode_Scatter
   {
   }
   /// <summary> 散射型武器 精确霰弹 (横向散布) </summary>
   public class WepMode_Scatter_PrecisionShot : WepMode_Scatter
   {
      const float stabilityMultiplier = 0.25f; // 散射角度的乘算系数

      protected override void Bullet_Stability(Transform bullet, int index = 0)
      {
         // 计算每颗子弹之间的角度
         // stability现在表示最左到最右的总角度，并应用乘算系数
         float totalAngle = stability * stabilityMultiplier;
         float anglePerBullet = 0;
         float startAngle = -totalAngle / 2; // 起始角度（最左侧）
         
         if (expansion01 > 1)
         {
            anglePerBullet = totalAngle / (expansion01 - 1); // 计算相邻子弹间的角度
         }
         
         // 设置子弹旋转
         bullet.Rotate(0, startAngle + (anglePerBullet * index), 0);
      }
   }
}