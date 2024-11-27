using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   [System.Serializable]
   public class P_AngleOfView
   {
      public float mouseSensitivity = 100f; // 鼠标灵敏度
      public Transform horizontalAxis;         // 角色的身体，用于旋转视角
      public Transform pitchingAxis;         // 角色的身体，用于旋转视角

      private float xRotation = 0f;        // 记录垂直旋转角度
      public void Initialize(Transform horizontalAxis, Transform pitchingAxis)
      {
         this.horizontalAxis = horizontalAxis;
         this.pitchingAxis = pitchingAxis;
      }
      public void InputUpdate()
      {
         // 按下 ESC 键退出鼠标锁定
         if (Input.GetKeyDown(KeyCode.Escape))
         {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
         }
         // 点击鼠标左键重新锁定鼠标
         else if (Input.GetMouseButtonDown(0))
         {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
         }

         // 如果鼠标处于锁定状态，则可以旋转视角
         if (Cursor.lockState == CursorLockMode.Locked)
         {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 限制垂直角度

            // 垂直旋转相机，水平旋转玩家身体
            pitchingAxis.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            horizontalAxis.Rotate(Vector3.up * mouseX);
         }
      }
   }
}
