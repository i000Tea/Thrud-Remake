using UnityEngine;

namespace TeaFramework
{
   public class P_Aim : P_0ModularBase
   {
      Vector2 screenTarget;

      public override void FixedUpdate()
      {
         FindSelectEnemy();
      }
      public override void LateUpdate()
      {
         ArmUpdate();
      }
      /// <summary> 瞄准对象插值移动 </summary>
      private void ArmUpdate()
      {
         RectTransform rect = UiConfig.lockEnemyArm;
         var dist = Vector2.Distance(rect.anchoredPosition, screenTarget);
         if (dist > 50 || dist <= 0) { Config.lockSpeedMulti = 1; }
         else if (dist > 15) { Config.lockSpeedMulti = 4; }
         else { Config.lockSpeedMulti = 12; }
         rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, screenTarget, Config.LockEnemySpeed * Time.deltaTime);
      }
      /// <summary> 查询目标敌人(需要优化算法) </summary>
      private void FindSelectEnemy()
      {
         EnemyEntity_Main index = null;
         float indexDist = float.MaxValue; // 当前已锁定敌人与玩家的世界距离
         screenTarget = Vector2.zero;

         if (!PlayerControl.I.ControlTakeOff)
         {
            for (int i = 0; i < EnemyControl.enemys.Count; i++)
            {
               if (!EnemyControl.enemys[i].onCame) continue;

               // 获取敌人的世界坐标
               Vector3 targetPosition = EnemyControl.enemys[i].transform.position;

               // 转换到屏幕坐标和 Canvas 坐标
               Vector3 screenPosition = Config.camera.WorldToScreenPoint(targetPosition);
               RectTransformUtility.ScreenPointToLocalPointInRectangle(
                   UiConfig.faceCanvas.transform as RectTransform,
                   screenPosition,
                   Config.camera,
                   out Vector2 canvasPosition
               );

               // 计算 Canvas 坐标的距离
               float canvasDist = canvasPosition.magnitude;

               // 如果屏幕距离不在锁定范围内，跳过
               if (canvasDist >= Config.enemyLockDistance) continue;

               // 获取与玩家的世界距离
               float worldDist = Vector3.Distance(PlayerControl.I.transform.position, targetPosition);

               // 如果距离差异大于一倍，优先选择更近的目标；否则优先选择屏幕距离更近的目标
               if (worldDist <= indexDist / 2 || (worldDist < indexDist && canvasDist < indexDist))
               {
                  indexDist = worldDist; // 更新最近距离
                  index = EnemyControl.enemys[i]; // 更新锁定敌人
                  screenTarget = canvasPosition; // 更新屏幕目标位置
               }
            }
         }

         Config.lockEnemy = index;
      }
   }
}
