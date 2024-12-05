using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   public class PropItem_Data : Base_ItemData
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
