using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
namespace TeaFramework
{
   public class P_AnimatorControl : P_IModular
   {
      private Animator PlayerAnim => config.playerAnimator;
      public P_AnimatorControl()
      {
         if (PlayerAnim) { PlayerAnim.SetTrigger("GameStart"); }
         "PlayerAnimatorEvent".OnAddAnotherList<string>(PlayerAnimatorEvent);
      }
      public override void Update()
      {
         MoveVelocity();
      }
      public void MoveVelocity()
      {
         var data = config.GetRigFromForword();
         //Debug.Log(data);
         PlayerAnim.SetFloat("MoveFront", data.z);
         PlayerAnim.SetFloat("MoveSide", data.x);

         PlayerAnim.SetBool("OnSprint", config.OnSprint());
      }
      private void PlayerAnimatorEvent(string eventName)
      {
         switch (eventName)
         {
            case "OnRoll":
               PlayerAnim.SetFloat("RollFront", config.rollForce.y);
               PlayerAnim.SetFloat("RollSide", config.rollForce.x);
               PlayerAnim.SetTrigger("OnRoll");
               break;
            default:
               break;
         }
      }
   }
}