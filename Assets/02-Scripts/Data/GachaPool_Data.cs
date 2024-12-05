using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   public class GachaPool_Data : Base_ItemData
   {
      public string poolName = "卡池名";
      public string poolPinyin = "dontset";
      public int poolID = 999999;
      public PoolTag pooltag;

      public Sprite sprite_PoolBg;
      public Sprite sprite_PoolBtnBg;

      //public List<int> poolV5UP;
      //public List<int> poolV5;
      //public List<int> poolV4UP;
      //public List<int> poolV4;
      //public List<int> poolV3;
      //public List<int> poolV2;

      public List<Base_ItemData> poolV5UP_ItemData = new();
      public List<Base_ItemData> poolV5_ItemData = new();
      public List<Base_ItemData> poolV4UP_ItemData = new();
      public List<Base_ItemData> poolV4_ItemData = new();
      public List<Base_ItemData> poolV3_ItemData = new();
      public List<Base_ItemData> poolV2_ItemData = new();
   }
}
