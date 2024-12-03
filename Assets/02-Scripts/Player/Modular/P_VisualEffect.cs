using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   public class P_VisualEffect : P_IModular
   {
      private float cacheFov;
      private float healthWidth;
      private float healthHeigth;
      public P_VisualEffect()
      {
         cacheFov = Config.TargetFov;
         healthWidth = UiConfig.onHealth.sizeDelta.x;
         healthHeigth = UiConfig.onHealth.sizeDelta.y;
         "UI-HealthUpdate".OnAddAnotherList<int, int>(UI_HealthUpdate);
      }
      public override void OnDestroy()
      {
         "UI-HealthUpdate".OnRemoveAnotherList<int, int>(UI_HealthUpdate);
      }
      public void UI_HealthUpdate(int now, int max)
      {
         Debug.Log(now + " " + max);
         UiConfig.onHealth.sizeDelta = new(healthWidth * now / max, healthHeigth);
         UiConfig.onHealthText.text = $"{now}/{max}";
      }
      public override void Update()
      {
         CameraFovFromOnSprint();
         ShowViewFollow();
      }

      /// <summary> 相机视场角平滑运动 </summary>
      private void CameraFovFromOnSprint()
      {
         cacheFov = Mathf.Lerp(cacheFov, Config.TargetFov, Time.deltaTime * Config.fovLerpSpeed);
         //Debug.Log(cacheFov);
         Config.camera.fieldOfView = cacheFov;
      }
      /// <summary> 显示角色模型平滑运动 </summary>
      private void ShowViewFollow()
      {
         // 相机目标的旋转值（与目标轴对齐）
         Quaternion targetRotation = Config.forword.rotation;

         // 使用插值平滑过渡到目标旋转
         Quaternion smoothedRotation = Quaternion.Lerp
            (Config.showViewAxis.rotation,
            targetRotation, Time.deltaTime * Config.rotationSpeed);

         // 强制锁定 z 轴欧拉角为 0
         Vector3 eulerAngles = smoothedRotation.eulerAngles;
         eulerAngles.z = 0f;
         smoothedRotation = Quaternion.Euler(eulerAngles);

         // 应用平滑后的旋转
         Config.showViewAxis.rotation = smoothedRotation;
      }
   }
}