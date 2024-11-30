using UnityEngine;

namespace TeaFramework
{
   public class P_Aim : P_IModular
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
      /// <summary>  </summary>
      private void ArmUpdate()
      {
         RectTransform rect = canvasConfig.lockEnemyArm;
         var dist = Vector2.Distance(rect.anchoredPosition, screenTarget);
         if (dist > 50 || dist <= 0) { config.lockSpeedMulti = 1; }
         else if (dist > 15) { config.lockSpeedMulti = 4; }
         else { config.lockSpeedMulti = 12; }
         rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, screenTarget, config.LockEnemySpeed * Time.deltaTime);
      }
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
               Vector3 screenPosition = config.camera.WorldToScreenPoint(targetPosition);
               RectTransformUtility.ScreenPointToLocalPointInRectangle(
                   canvasConfig.faceCanvas.transform as RectTransform,
                   screenPosition,
                   config.camera,
                   out Vector2 canvasPosition
               );

               // 计算 Canvas 坐标的距离
               float canvasDist = canvasPosition.magnitude;

               // 如果屏幕距离不在锁定范围内，跳过
               if (canvasDist >= config.enemyLockDistance) continue;

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

         config.lockEnemy = index;
      }
   }
}
