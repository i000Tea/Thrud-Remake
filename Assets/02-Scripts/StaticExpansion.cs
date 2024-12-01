using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   public static class StaticExpansion
   {
      public static T FindParentComponent<T>(this Transform target) where T : Component
      {
         for (int i = 0; i < 9; i++)
         {
            if (target == null) break;
            if (target.TryGetComponent(out T component))
            {
               return component;
            }
            else target = target.parent;
         }
         return null;
      }
   }
}
