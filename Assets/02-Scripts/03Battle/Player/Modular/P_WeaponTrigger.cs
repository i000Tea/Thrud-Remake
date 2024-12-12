using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   public class P_WeaponTrigger : P_0ModularBase
   {
      public override void Update()
      {
         if (InputValue.onFire)
         {
            NowBattleData.wepMode.PressTriggrt();
         }
         for (int i = 0; i < EntityBattleDatas.Length; i++)
         {
            EntityBattleDatas[i].wepMode.TimeStep();
         }
      }
   }
}
