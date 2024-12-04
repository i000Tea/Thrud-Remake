using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   [CreateAssetMenu(fileName = "gacha-pool-data", menuName = "thrudData/抽卡/卡池信息", order = 1)]
   public class GachaPool_Data : ScriptableObject
   {
      public string poolName;
      public PoolTag poolTag;
      public Sprite gachaBg;
      public Sprite gachaBtnBg;
      public string poolId;
      public string pool_mainDisplay;
      public string pool_HorizontalGraph;

      public List<Base_ItemData> r5Up;
      public List<Base_ItemData> r5;
      public List<Base_ItemData> r4Up;
      public List<Base_ItemData> r4;
      public List<Base_ItemData> r3;
      public List<Base_ItemData> r2;
   }
}
