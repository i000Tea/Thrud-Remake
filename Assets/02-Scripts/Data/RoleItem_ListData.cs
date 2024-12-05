using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   [CreateAssetMenu(fileName = "All Role Item Data", menuName = "thrudData/所有角色数据列表", order = 1)]
   public class RoleItem_ListData : Base_AllItemData
   {
      public List<RoleItem_Data> allRole;
   }
}