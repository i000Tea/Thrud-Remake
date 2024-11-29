using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   public abstract class P_IModular
   {
      protected PlayerConfig config => PlayerControl.I.config;
      protected FaceCanvasConfig canvasConfig => PlayerControl.I.canvasConfig;
      public virtual void Update() { }
      public virtual void FixedUpdate() { }
   }
}