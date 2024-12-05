using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   [CreateAssetMenu(fileName = "All Gacha Pool Data", menuName = "thrudData/所有卡池信息列表", order = 1)]
   public class GachaPool_ListData : Base_AllItemData
   {
      public List<GachaPool_Data> allPool;
   }
}
