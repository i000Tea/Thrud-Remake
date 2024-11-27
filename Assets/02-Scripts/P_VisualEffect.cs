using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   [System.Serializable]
   public class P_VisualEffect
   {
      private Transform controlAxis; // 被控制的轴
      private Transform showViewAxis;  // 目标轴
      private Camera camera;
      public float mainFov = 60;
      public float sprintFov = 90;
      public float fovLerpSpeed = 10f;
      private float cacheFov;
      public float rotationSpeed = 5f; // 旋转速度（插值因子）
      public void Initialize(Transform controlAxis, Transform showViewAxis)
      {
         this.controlAxis = controlAxis;
         this.showViewAxis = showViewAxis;
         camera = Camera.main;
         cacheFov = mainFov;
      }
      public void InputUpdate()
      {
         ShowViewFollow();
         CameraFovFromOnSprint();
      }

      private void CameraFovFromOnSprint()
      {
         var target = PlayerControl.I.OnSprint ? sprintFov : mainFov;
         cacheFov = Mathf.Lerp(cacheFov, target, Time.deltaTime * fovLerpSpeed);
         //Debug.Log(cacheFov);
         camera.fieldOfView = cacheFov;
      }
      private void ShowViewFollow()
      {
         if (controlAxis == null || showViewAxis == null) return;

         // 目标旋转（与目标轴对齐）
         Quaternion targetRotation = controlAxis.rotation;

         // 使用插值平滑过渡到目标旋转
         Quaternion smoothedRotation = Quaternion.Lerp(showViewAxis.rotation, targetRotation, Time.deltaTime * rotationSpeed);

         // 强制锁定 z 轴欧拉角为 0
         Vector3 eulerAngles = smoothedRotation.eulerAngles;
         eulerAngles.z = 0f;
         smoothedRotation = Quaternion.Euler(eulerAngles);

         // 应用平滑后的旋转
         showViewAxis.rotation = smoothedRotation;
      }
   }
}