using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
namespace TeaFramework
{
   public class P_Movement : P_0ModularBase
   {
      #region 参数变量
      private Vector3 cacheForce;        // 值

      #endregion

      public override void Update()
      {
         if (PlayerControl.I.ControlTakeOff) return;
         AwaitRoll();
      }
      public override void FixedUpdate()
      {
         if (PlayerControl.I.ControlTakeOff) return;
         GetForce();
         OnMove();
      }

      private Vector3 GetForce()
      {
         // 将局部方向转换为世界方向
         cacheForce = Config.ForceTransformDirection(InputValue.moveHorizontal, InputValue.moveDepth, InputValue.moveVertical);

         return cacheForce;
      }
      public void OnMove()
      {
         // 当按住左shift时
         if (!InputValue.onSprint || !Config.OnInputForword()) { Config.OffSprint(); }

         var force = cacheForce * Config.moveForce;

         if (Config.OnSprint())
            force *= Config.sprintMultiForce;

         // 施加力到刚体
         Config.rb.AddForce(force);
      }

      /// <summary> 进行一次翻滚尝试 </summary>
      public void AwaitRoll()
      {
         // 当按下左shift时 若可以翻滚 则开始翻滚
         if (Input.GetKeyDown(KeyCode.LeftShift) && Config.RollCDOver())
         {
            Config.onReadySprint = true;
            // 设置准备冲刺
            Config.RollCDAdd();
            OnRoll();
         }
      }
      private void OnRoll()
      {
         // ================ 第一部分 设置力 ================ 
         // 获取输入向量
         var force = cacheForce;
         // 若输入向量为空，则默认向前
         if (force.magnitude < 0.1f) { force = Config.forword.forward; }
         else { force = force.normalized; }
         // 输入向量乘基础向量与翻滚增幅
         force *= Config.moveForce * Config.rollMultiForce;
         // 赋值
         Config.rb.AddForce(force);

         // ================ 第二部分 设置动画 ================
         var rollForce = new Vector2(InputValue.moveHorizontal, InputValue.moveVertical);
         if (rollForce == Vector2.zero)
            rollForce = Vector2.up;
         else
            rollForce = rollForce.normalized;
         // 设置翻滚向量
         Config.rollForce = rollForce;
         // 事件
         "RoleAnimatorEvent".InvokeSomething("OnRoll");
      }
   }
}
