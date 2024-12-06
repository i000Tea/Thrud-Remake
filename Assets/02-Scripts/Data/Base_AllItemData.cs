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
      [Range(1, 5)] public int Rarity = 0;
   }
}