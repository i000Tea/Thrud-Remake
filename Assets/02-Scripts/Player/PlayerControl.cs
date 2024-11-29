using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   public class PlayerControl : Singleton<PlayerControl>
   {
      #region 参数变量
      [Range(0.01f, 4f)] public float scale = 1;
      /// <summary> 控制权限被接管(开场动画/大招/暂停) </summary>
      public bool ControlTakeOff;
      public PlayerConfig config;
      public FaceCanvasConfig canvasConfig; 

      #region 挂载对象
      [SerializeField] private P_Movement movement;
      [SerializeField] private P_AngleOfView angleOfView;
      [SerializeField] private P_VisualEffect visualEffect;
      [SerializeField] private P_AnimatorControl animCtrl;
      #endregion

      private P_IModular[] modulars;

      #region 缓存参数
      #endregion

      #endregion

      protected override void Awake()
      {
         base.Awake();

         Time.timeScale = scale;
         modulars = new P_IModular[] { 
            new P_Movement(), 
            new P_AngleOfView(), 
            new P_VisualEffect(), 
            new P_AnimatorControl(), 
            new P_Aim(), 
         };
      }
      private void Update()
      {
         for (int i = 0; i < modulars.Length; i++)
         {
            modulars[i].Update();
         }
      }
      private void FixedUpdate()
      {
         for (int i = 0; i < modulars.Length; i++)
         {
            modulars[i].FixedUpdate();
         }
      }
   }
}
