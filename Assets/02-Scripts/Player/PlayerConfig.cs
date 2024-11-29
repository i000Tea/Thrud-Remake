using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{

   [System.Serializable]
   public class PlayerConfig
   {

      #region 设置对象 transform
      /// <summary> 主控摄像机 </summary>
      public Camera camera;
      /// <summary> 刚体 </summary>
      public Rigidbody rb;
      public Animator playerAnimator;
      [Header("控制")]
      /// <summary> 控制器轴向-横轴 </summary>
      public Transform horizontalAxis;
      /// <summary> 控制器轴向-纵轴 </summary>
      public Transform pitchingAxis;
      /// <summary> 正前方向的参考对象 </summary>
      public Transform forword;
      [Header("显示")]
      /// <summary> 显示对象轴向 </summary>
      public Transform showViewAxis;
      #endregion

      #region 移动参数 movement

      #region 基础移动
      /// <summary> 深度(上下)移动取自于世界而非视角 </summary>
      [Header("移动参数")]
      public bool DepthFromWorld = false;
      /// <summary> 水平方向的移动力 </summary>
      public float moveForce = 10f;
      public float rollMultiForce = 10f;
      public float sprintMultiForce = 10f;

      /// <summary> 获取移动向量 </summary>
      /// <param name="forwordForce"></param>
      /// <param name="depthForce"></param>
      /// <param name="sideForce"></param>
      /// <returns></returns>
      public Vector3 ForceTransformDirection(float sideForce, float depthForce, float forwordForce)
      {
         // 设置 前进 与 侧向 的值
         var outputForce = forword.TransformDirection(new Vector3(sideForce, 0, forwordForce));

         // 根据配置设置高度值更新为当前视角还是世界空间
         var localDepth = new Vector3(0, depthForce, 0);
         if (!DepthFromWorld) { localDepth = forword.TransformDirection(localDepth); }

         return outputForce + localDepth;
      }
      /// <summary> 获取基于当前目标的刚体移动向量 </summary>
      public Vector3 GetRigFromForword() => forword.InverseTransformDirection(rb.velocity / 4f);
      public void GetRibSplitSpeed(out float forwardSpeed, out float sideSpeed)
      {
         forwardSpeed = Vector3.Dot(rb.velocity, forword.forward); // 前向速度
         sideSpeed = Vector3.Dot(rb.velocity, forword.right);     // 侧向速度
      }

      #endregion

      #region 翻滚与冲刺
      public Vector2 rollForce;
      /// <summary> 冲刺CD需求时间 </summary>
      [SerializeField] private float RollCD = 2f;
      /// <summary> 冲刺CD结束时间 </summary>
      private float OnRollTime = 0;

      /// <summary> 翻滚cd增加 </summary>
      public void RollCDAdd() => OnRollTime = Time.time;
      /// <summary> 翻滚准备就绪 </summary>
      public bool RollCDOver() => Time.time > OnRollTime + RollCD;

      /// <summary> 是否在冲刺状态 </summary>
      public bool onReadySprint;
      private bool onSprint;

      /// <summary> 翻滚后开始冲刺的间隔 </summary>
      [Range(0.0001f, 1f)] public float rollAfterSprintDelay = 0.7f;
      public bool OnSprint()
      {
         if (!onSprint && Input.GetKey(KeyCode.LeftShift) && onReadySprint && OnInputForword() && Time.time > OnRollTime + rollAfterSprintDelay)
         {
            Debug.Log($"开始冲刺{Time.time} {OnRollTime + rollAfterSprintDelay}");
            onSprint = true;
         }
         return onSprint;
      }
      public void OffSprint()
      {
         onReadySprint = false;
         onSprint = false;
      }

      public bool OnInputForword()
      {
         return
            Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.RightArrow);
      }
      #endregion

      #endregion

      #region 视角旋转 angleOfView
      [Header("视角旋转")]
      public float mouseSensitivity = 100f; // 鼠标灵敏度

      #endregion

      #region 视角与显示
      public float TargetFov => OnSprint() ? sprintFov : mainFov;
      [Header("视场角与显示对象")]
      /// <summary> 默认视角 </summary>
      public float mainFov = 60;
      /// <summary> 冲刺时视角 </summary>
      public float sprintFov = 90;
      /// <summary> 视角变化速率 </summary>
      public float fovLerpSpeed = 10f;
      /// <summary> 显示对象的旋转速度因子 </summary>
      public float rotationSpeed = 5f;
      #endregion
   }
}