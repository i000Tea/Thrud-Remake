using UnityEngine;
namespace TeaFramework
{
   public class P_AngleOfView : P_IModular
   {
      private float xRotation = 0f;        // 记录垂直旋转角度\
      public override void Update()
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

         if (PlayerControl.I.ControlTakeOff) return;

         // 如果鼠标处于锁定状态，则可以旋转视角
         if (Cursor.lockState == CursorLockMode.Locked)
         {
            PerspectiveUpdate();
         }
      }

      private void PerspectiveUpdate()
      {
         float mouseX = InputValue.MouseX * Config.mouseSensitivity * Time.deltaTime;
         float mouseY = InputValue.MouseY * Config.mouseSensitivity * Time.deltaTime;

         xRotation -= mouseY;
         xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 限制垂直角度

         // 垂直旋转相机，水平旋转玩家身体
         Config.pitchingAxis.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
         Config.horizontalAxis.Rotate(Vector3.up * mouseX);
      }

   }
}
