using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   [CreateAssetMenu(fileName = "All Wep Item Data", menuName = "thrudData/武装数据", order = 1)]
   public class WeaponItem_ListData : Base_AllItemData
   {
      public List<WeaponItem_Data> allwep;
   }
}