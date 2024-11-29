using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeaFramework
{
   public class Enemy_Main : MonoBehaviour
   {
      private void Awake()
      {
         EnemyControl.enemys.Add(this);
      }
   }
}
