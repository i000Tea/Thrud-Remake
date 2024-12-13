using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   [CreateAssetMenu(fileName = "gacha-pool-data", menuName = "thrudData/所有道具数据列表", order = 1)]
   public class PropItem_ListData : Base_AllItemData
   {
      public List<PropItem_Data> allProp;
   }
}
