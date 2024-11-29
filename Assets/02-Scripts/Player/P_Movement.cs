using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
namespace TeaFramework
{
   public class P_Movement : P_IModular
   {
      #region 参数变量
      private float moveHorizontal; // 左右方向输入
      private float moveVertical;   // 前后方向输入
      private float moveDepth;      // 上下方向输入
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

      public void OnMove()
      {
         // 当按住左shift时
         if (!Input.GetKey(KeyCode.LeftShift) || !config.OnInputForword()) { config.OffSprint(); }

         var force = cacheForce * config.moveForce;

         if (config.OnSprint())
            force *= config.sprintMultiForce;

         // 施加力到刚体
         config.rb.AddForce(force);
      }
      /// <summary> 进行翻滚 </summary>
      public void AwaitRoll()
      {
         // 当按下左shift时 若可以翻滚 则开始翻滚
         if (Input.GetKeyDown(KeyCode.LeftShift) && config.RollCDOver())
         {
            config.onReadySprint = true;
            // 设置准备冲刺
            config.RollCDAdd();
            OnRoll();
         }
      }
      private void OnRoll()
      {
         // ================ 第一部分 设置力 ================ 
         // 获取输入向量
         var force = cacheForce;
         // 若输入向量为空，则默认向前
         if (force.magnitude < 0.1f) { force = config.forword.forward; }
         else { force = force.normalized; }
         // 输入向量乘基础向量与翻滚增幅
         force *= config.moveForce * config.rollMultiForce;
         // 赋值
         config.rb.AddForce(force);

         // ================ 第二部分 设置动画 ================
         var rollForce = new Vector2(moveHorizontal, moveVertical);
         if (rollForce == Vector2.zero)
            rollForce = Vector2.up;
         else
            rollForce = rollForce.normalized;
         // 设置翻滚向量
         config.rollForce = rollForce;
         // 事件
         "PlayerAnimatorEvent".InvokeSomething("OnRoll");
      }
      private Vector3 GetForce()
      {
         // 获取输入
         moveHorizontal = Input.GetAxis("Horizontal"); // 左右方向输入
         moveVertical = Input.GetAxis("Vertical");     // 前后方向输入
         moveDepth = Input.GetAxis("Depth");           // 上下方向输入
                                                       // 将局部方向转换为世界方向
         cacheForce = config.ForceTransformDirection(moveHorizontal, moveDepth, moveVertical);

         return cacheForce;
      }
   }
}
