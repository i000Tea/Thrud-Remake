using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   [System.Serializable]
   public class P_Movement
   {
      #region 参数变量
      // 参数
      private Rigidbody rb;
      private Transform forword;
      private Vector3 force = default;

      // 基础位移
      /// <summary> 水平方向的移动力 </summary>
      public float moveForce = 10f;

      // 冲刺
      private float inputSprintTimer = 0;
      [Range(0.0001f, 0.5f)] public float sprintInterval = 0.1f;
      /// <summary> 冲刺速度系数 </summary>
      public float sprintSpeedCoefficient = 10f;

      public float sprintCD = 3f;
      private float sprintCDOver = 0;
      #endregion
      public void Initialize(Rigidbody rb, Transform forword)
      {
         this.rb = rb;
         this.forword = forword;
         force = Vector3.zero;
      }
      public void Update()
      {
         if (Time.time > sprintCDOver)
         {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
               inputSprintTimer = Time.time;
            }            
         }
         if (Input.GetKeyUp(KeyCode.LeftShift))
         {
            if (inputSprintTimer + sprintInterval > Time.time)
            {
               var sprint = moveForce * sprintSpeedCoefficient * force.normalized;
               rb.AddForce(sprint);
               Debug.Log($"冲刺 {inputSprintTimer} {Time.time + sprintInterval} {Time.time}");
               sprintCDOver = Time.time + sprintCD;
            }
            else
            {
               Debug.Log($"冲刺判定失败{inputSprintTimer} {Time.time + sprintInterval} {Time.time}");
            }
            inputSprintTimer = 0;
         }
      }
      public void FixedUpdate()
      {
         // 获取输入
         float moveHorizontal = Input.GetAxis("Horizontal"); // 左右方向输入
         float moveVertical = Input.GetAxis("Vertical");     // 前后方向输入
         float moveDepth = Input.GetAxis("Depth");           // 上下方向输入

         // 将输入转换为局部方向的移动向量
         Vector3 localMove = new(moveHorizontal, moveDepth, moveVertical);

         // 将局部方向转换为世界方向
         force = forword.TransformDirection(localMove * moveForce);
         // 按住左Shift键加速         
         if (inputSprintTimer != 0 && Time.time > inputSprintTimer + (sprintInterval * 1.5f))
         {
            PlayerControl.I.OnSprint = true;
            force *= 2;
         }
         else
         {
            PlayerControl.I.OnSprint = false;
         }

         // 施加力到刚体
         rb.AddForce(force);
      }

   }
}
