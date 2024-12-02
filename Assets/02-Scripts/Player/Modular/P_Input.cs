using UnityEngine;

namespace TeaFramework
{
   public class P_Input : P_IModular
   {
      public override void Update()
      {
         if (PlayerControl.I.ControlTakeOff) return;

         InputValue.MouseX = Input.GetAxis("Mouse X");
         InputValue.MouseY = Input.GetAxis("Mouse Y");
         InputValue.moveHorizontal = Input.GetAxis("Horizontal"); // 左右方向输入
         InputValue.moveVertical = Input.GetAxis("Vertical");     // 前后方向输入
         InputValue.moveDepth = Input.GetAxis("Depth");           // 上下方向输入

         int index = -1;
         if (Input.GetKeyDown(KeyCode.Alpha1)) { index = 0; }
         else if (Input.GetKeyDown(KeyCode.Alpha2)) { index = 1; }
         else if (Input.GetKeyDown(KeyCode.Alpha3)) { index = 2; }
         else if (Input.GetKeyDown(KeyCode.Alpha4)) { index = 3; }
         if (index >= 0 && index <= Config.loadRoles.Count)
         {
            Config.selectRoleIndedx = index;
            "RoleSwitch".InvokeSomething();
         }
      }
   }
}