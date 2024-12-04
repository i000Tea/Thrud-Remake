using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   public class WeaponItem_Data : Base_ItemData
   {  
      [Header("基础数据")]
      public string wepName = "角色的名字";
      public string wepPinyin = "角色的名字";
      public int wepID = 999999;
      [Range(4, 5)] public int Rarity = 3;
      public WeaponType weaponType;
      public WeaponSubType weaponSubType;
      /// <summary> 简介 </summary>
      [TextArea]
      public string profile = "没有文案";
      [Header("战斗数值")]
      public float baseDamage = 360;
      [Header("技能")]
      public string skillName = "没有技能";
      public string skillDescribe = "技能描述";

      [Header("模型预制件")]
      public GameObject wepPreafab;
      /// <summary> 方图 结算显示 </summary>
      [Header("美术素材")]
      public Sprite sprite_Lottery;
      /// <summary> 竖条 </summary>
      public Sprite sprite_List;
      /// <summary> 横条 </summary>
      public Sprite sprite_Line;
      /// <summary> 小队 </summary>
   }
}