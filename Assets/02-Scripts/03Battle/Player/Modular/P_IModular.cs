using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   public abstract class P_IModular
   {
      protected PlayerConfig Config => PlayerControl.I.config;
      protected FaceCanvasConfig UiConfig => PlayerControl.I.canvasConfig;
      public virtual void Start() { }
      public virtual void Update() { }
      public virtual void FixedUpdate() { }
      public virtual void LateUpdate() { }      
      public virtual void OnDestroy() { }
   }
}