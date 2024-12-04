using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   [CreateAssetMenu(fileName = "gacha-pool-data", menuName = "thrudData/抽卡/卡池信息", order = 1)]
   public class GachaPool_ListData : Base_AllItemData
   {
      public List<GachaPool_Data> allPool;
   }
}
