using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   public class P_VisualEffect: P_IModular
   {
      private float cacheFov;
      public P_VisualEffect()
      {
         cacheFov = config.TargetFov;
      }
      public override void Update()
      {
         CameraFovFromOnSprint();
         ShowViewFollow();
      }

      /// <summary> 相机视场角平滑运动 </summary>
      private void CameraFovFromOnSprint()
      {
         cacheFov = Mathf.Lerp(cacheFov, config.TargetFov, Time.deltaTime * config.fovLerpSpeed);
         //Debug.Log(cacheFov);
         config.camera.fieldOfView = cacheFov;
      }
      /// <summary> 显示角色模型平滑运动 </summary>
      private void ShowViewFollow()
      {
         // 相机目标的旋转值（与目标轴对齐）
         Quaternion targetRotation = config.forword.rotation;

         // 使用插值平滑过渡到目标旋转
         Quaternion smoothedRotation = Quaternion.Lerp
            (config.showViewAxis.rotation,
            targetRotation, Time.deltaTime * config.rotationSpeed);

         // 强制锁定 z 轴欧拉角为 0
         Vector3 eulerAngles = smoothedRotation.eulerAngles;
         eulerAngles.z = 0f;
         smoothedRotation = Quaternion.Euler(eulerAngles);

         // 应用平滑后的旋转
         config.showViewAxis.rotation = smoothedRotation;
      }
   }
}