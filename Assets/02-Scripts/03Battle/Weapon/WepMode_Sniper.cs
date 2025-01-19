using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeaFramework
{
   /// <summary> 狙击型武器 </summary>
   public abstract class WepMode_Sniper : WepMode_0Base
   {
      /// <summary>
      /// 执行单次射击
      /// 使用射线检测进行精确打击，并生成视觉反馈效果
      /// </summary>
      public override void OneShoot(int index = 0)
      {
         Ray ray = new(showPoint.position, showPoint.forward);
         float maxDistance = 100f;  // 射线最大检测距离（米）

         Vector3 point;
         //使用射线检测判断是否击中目标
         if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance))
         {
            // 击中目标，记录碰撞点信息
            Debug.Log($"击中物体：{hitInfo.collider.gameObject.name}，碰撞点：{hitInfo.point}，物体位置：{hitInfo.collider.transform.position}");
            point = hitInfo.point;
         }
         else
         {
            // 未击中目标，使用最大射程点
            point = ray.origin + ray.direction * maxDistance;
            Debug.Log($"射线未击中目标，终点位置：{point}");
         }

         // 生成子弹特效
         var newBullet = Bullet_Inst(point, Vector3.zero);
         if (newBullet.TryGetComponent(out Bullet_Base bullet))
            bullet.damage = damage;

         // 生成射线视觉效果
         var line = Object.Instantiate(wepObjComp.bulletLinePrefab);
         if (line.TryGetComponent(out LineRenderer lineRender))
         {
            // 设置射线起点（枪口）和终点（碰撞点/最大射程点）
            lineRender.positionCount = 2;
            lineRender.SetPosition(0, showPoint.position);
            lineRender.SetPosition(1, point);

            // 设置射线宽度
            lineRender.startWidth = 0.05f;
            lineRender.endWidth = 0.05f;

            // 设置射线持续时间
            Object.Destroy(line, 0.2f);
         }
      }
   }
   /// <summary> 狙击型武器-穿透型：子弹可以穿透多个目标 </summary>
   public class WepMode_Sniper_Penetrating : WepMode_Sniper
   {

   }
   /// <summary> 狙击型武器-连发型：可以快速连续发射多发子弹 </summary>
   public class WepMode_Sniper_Tandem : WepMode_Sniper
   {

   }
   /// <summary> 狙击型武器-迅捷型：具有更快的射击和装填速度 </summary>
   public class WepMode_Sniper_Swift : WepMode_Sniper
   {

   }
   /// <summary> 狙击型武器-充能型：可以蓄力发射更强力的子弹 </summary>
   public class WepMode_Sniper_Charged : WepMode_Sniper
   {

   }
}