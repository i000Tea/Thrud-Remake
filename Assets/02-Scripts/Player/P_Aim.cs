using UnityEngine;

namespace TeaFramework
{
   public class P_Aim : P_IModular
   {
      Vector2 screenTarget;
      public override void Update()
      {
         RectTransform rect = canvasConfig.lockEnemyArm;
         var dist = Vector2.Distance(rect.anchoredPosition, screenTarget);
         Debug.Log(dist);
         if (dist > 50 || dist <= 0) { config.lockSpeedMulti = 1; }
         else if (dist > 15) { config.lockSpeedMulti = 4; }
         else { config.lockSpeedMulti = 12; }

         rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, screenTarget, config.LockEnemySpeed * Time.deltaTime);

      }
      public override void FixedUpdate()
      {
         Enemy_Main index = null;
         float indexDist = float.MaxValue;
         screenTarget = Vector2.zero;
         for (int i = 0; i < EnemyControl.enemys.Count; i++)
         {
            // 1. 获取物体在世界中的位置
            Vector3 targetPosition = EnemyControl.enemys[i].transform.position;

            // 2. 将世界坐标转换为屏幕坐标
            Vector3 screenPosition = config.camera.WorldToScreenPoint(targetPosition);
            // 3. 将屏幕坐标转换为 Canvas 坐标
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasConfig.faceCanvas.transform as RectTransform, // Canvas 的 RectTransform
                screenPosition,                                      // 屏幕坐标
                config.camera,                                       // 渲染该 Canvas 的相机
                out Vector2 canvasPosition                           // 输出 Canvas 坐标
            );
            var dist = canvasPosition.magnitude;
            if (dist < config.enemyLockDistance && dist < indexDist)
            {
               indexDist = dist;
               index = EnemyControl.enemys[i];
               screenTarget = canvasPosition;
            }
         }
         config.lockEnemy = index;
      }
   }
}
