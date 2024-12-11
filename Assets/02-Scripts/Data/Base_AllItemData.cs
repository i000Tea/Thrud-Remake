using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   public abstract class Base_AllItemData : ScriptableObject { }
   public abstract class Base_ItemData : ScriptableObject {

      [Header("通用数据")]
      public string itemName;
      public int itemID;
      public string itemPinyin = "角色的名字";
      [Range(1, 5)] public int Rarity = 0;
      /// <summary> 简介 </summary>
      [TextArea]
      public string profile = "没有文案";
   }
}