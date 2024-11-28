using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
namespace TeaFramework
{
   public enum AnimEvent
   {
      onMove,
      offMove,
   }
   [System.Serializable]
   public class P_AnimatorControl
   {
      public Animator playerAnimator;

      public void Initialize(Transform roleAxis)
      {
         "MoveEvent".OnAddAnotherList<AnimEvent>(MoveEvent);
         "MoveVelocity".OnAddAnotherList<float, float>(MoveVelocity);
         if (roleAxis.GetChild(0).GetChild(0).TryGetComponent(out Animator animator))
         {
            playerAnimator = animator;
            playerAnimator.SetTrigger("GameStart");
         }
      }
      private bool onMove;
      private float cacheFront, cacheSide;
      public void Update()
      {
         if (onMove)
         {
            onMove = false;
            playerAnimator.SetBool("OnMove", onMove);
         }
         Debug.Log(MathF.Abs(cacheFront) + MathF.Abs(cacheSide));
         if ((Input.GetKeyDown(KeyCode.W) ||
             Input.GetKeyDown(KeyCode.S) ||
             Input.GetKeyDown(KeyCode.A) ||
             Input.GetKeyDown(KeyCode.D) ||
             Input.GetKeyDown(KeyCode.UpArrow) ||
             Input.GetKeyDown(KeyCode.DownArrow) ||
             Input.GetKeyDown(KeyCode.LeftArrow) ||
             Input.GetKeyDown(KeyCode.RightArrow)) &&
             MathF.Abs(cacheFront) + MathF.Abs(cacheSide) < 1f)
         {
            onMove = true;
            playerAnimator.SetBool("OnMove", onMove);
         }
         Debug.Log(MathF.Abs(cacheFront) + MathF.Abs(cacheSide));
         playerAnimator.SetBool("Moving", MathF.Abs(cacheFront) + MathF.Abs(cacheSide) > 1);
      }
      public void MoveEvent(AnimEvent animEvent)
      {
         switch (animEvent)
         {
            default:
               break;
         }
      }
      public void MoveVelocity(float fornt, float side)
      {
         cacheFront = fornt; cacheSide = side;
         playerAnimator.SetFloat("MoveFront", cacheFront);
         playerAnimator.SetFloat("MoveSide", cacheSide);
      }
   }
}