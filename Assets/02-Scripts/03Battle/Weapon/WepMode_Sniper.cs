using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeaFramework
{
   /// <summary> 狙击型武器 </summary>
   public abstract class WepMode_Sniper : WepMode_0Base
   {
      public override void OneShoot()
      {
         Ray ray = new(showPoint.position, showPoint.forward);
         float maxDistance = 100f;  // 限制射线的最大距离为100米

         Vector3 point;
         //声明一个RaycastHit结构体，存储碰撞信息
         if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance))
         {
            // 如果射线碰到物体，打印碰撞信息
            Debug.Log(hitInfo.collider.gameObject.name);
            Debug.Log(hitInfo.point);
            Debug.Log(hitInfo.collider.transform.position);
            point = hitInfo.point;
         }
         else
         {
            // 如果没有碰到物体，返回射线的终点并打印
            Vector3 endPoint = ray.origin + ray.direction * maxDistance;
            Debug.Log("射线未碰到物体，终点位置：" + endPoint);
            point = endPoint;
         }
         var newBullet = InstBullet(point, Vector3.zero);
         if (newBullet.TryGetComponent(out Bullet_Base bullet))
            bullet.damage = damage;
        var line = Object.Instantiate(wepObjComp.bulletLinePrefab);
         if(line.TryGetComponent(out LineRenderer lineRender))
         {
            // lineRender.
         }
      }

   }
   /// <summary> 狙击型武器 穿透型 </summary>
   public class WepMode_Sniper_Penetrating : WepMode_Sniper
   {

   }
   /// <summary> 狙击型武器 连发型 </summary>
   public class WepMode_Sniper_Tandem : WepMode_Sniper
   {

   }
   /// <summary> 狙击型武器 迅捷型 </summary>
   public class WepMode_Sniper_Swift : WepMode_Sniper
   {

   }
   /// <summary> 狙击型武器 充能型 </summary>
   public class WepMode_Sniper_Charged : WepMode_Sniper
   {

   }
}