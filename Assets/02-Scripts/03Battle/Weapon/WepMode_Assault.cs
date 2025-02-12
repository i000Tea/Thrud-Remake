using System.Threading.Tasks;
using UnityEngine;

namespace TeaFramework
{
   /// <summary> 突击型 </summary>
   public abstract class WepMode_Assault : WepMode_0Base  {  }

   /// <summary> 突击型 精确框架 (多次点射) </summary>
   public class WepMode_Assault_PreciseFrame : WepMode_Assault
   {
      public override void OnShootEvent()
      {
         ExecuteDelayedTask();
      }
      private async void ExecuteDelayedTask()
      {
         for (int i = 0; i < expansion01; i++)
         {
            Debug.Log($"射击次数：{i}");
            OneShoot(i);
            // 延迟 0.1 秒
            await Task.Delay((int)expansion02);
         } 
      }
   }

   /// <summary> 突击型 能量框架 </summary>
   public class WepMode_Assault_EnergyFrame : WepMode_Assault
   {

   }

   /// <summary> 突击型 速射框架 </summary>
   public class WepMode_Assault_RapidFireFrame : WepMode_Assault
   {

   }

   /// <summary> 突击型 专注框架 </summary>
   public class WepMode_Assault_FocusFrame : WepMode_Assault
   {

   }

   /// <summary> 突击型 强攻框架 </summary>
   public class WepMode_Assault_StormFrame : WepMode_Assault
   {

   }
}