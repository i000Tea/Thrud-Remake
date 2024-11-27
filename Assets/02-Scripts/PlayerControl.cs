using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   public class PlayerControl : Singleton<PlayerControl>
   {
      #region 参数变量

      #region 挂载对象
      [SerializeField] private P_Movement movement;
      [SerializeField] private P_AngleOfView angleOfView;
      [SerializeField] private P_VisualEffect visualEffect;
      private Rigidbody rig;
      #endregion

      #region 缓存参数
      public bool OnSprint;
      #endregion

      #endregion
      protected override void Awake()
      {
         base.Awake();

         rig = GetComponent<Rigidbody>();
         movement.Initialize(rig, transform.GetChild(0).GetChild(0));
         angleOfView.Initialize(
            transform.GetChild(0),
            transform.GetChild(0).GetChild(0));

         visualEffect.Initialize(transform.GetChild(0).GetChild(0), transform.GetChild(1));
      }
      private void Update()
      {
         angleOfView.InputUpdate();
         visualEffect.InputUpdate();
         movement.Update();
      }
      private void FixedUpdate()
      {
         movement.FixedUpdate();
      }      
   }
}
