using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   [CreateAssetMenu(fileName = "All Wep Item Data", menuName = "thrudData/所有武装数据列表", order = 1)]
   public class WeaponItem_ListData : Base_AllItemData
   {
      public GameObject baseBulletPreafab;
      public List<WeaponItem_Data> allwep = new();
   }
}