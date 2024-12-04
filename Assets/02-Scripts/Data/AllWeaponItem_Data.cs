using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   [CreateAssetMenu(fileName = "All Wep Item Data", menuName = "thrudData/武装数据", order = 1)]
   public class AllWeaponItem_Data : Base_AllItemData
   {
      public List<WeaponItem_Data> allwep;
   }
   public class WeaponItem_Data : Base_ItemData
   {  
      [Header("基础数据")]
      public string wepName = "角色的名字";
      public string wepPinyin = "角色的名字";
      public string enName = "englishName";
      public int wepID = 999999;
      [Range(4, 5)] public int Rarity = 3;
      public WeaponType weaponType;
      /// <summary> 简介 </summary>
      [TextArea]
      public string profile = "没有文案";
      [Header("战斗数值")]
      public float health = 360;
      [Header("模型预制件")]
      public GameObject wepPreafab;
      [Header("美术素材")]
      public Sprite sprite_listIcon;
      public Sprite sprite_Lottery;
      public Sprite sprite_LotteryLeader;
      public Sprite sprite_wepUI;
      /// <summary> 小队 </summary>
      [Header("设定文案")]
      public string onTeam = "未知小队";
      /// <summary> 势力，派系 </summary>
      public string onFactions = "未知派系";
      /// <summary> 出生地 </summary>
      public string birthplace = "未知出生地";
      /// <summary> 出生 </summary>
      public string birthday = "13月31日";
      /// <summary> 年龄 </summary>
      public int Age = 18;
      /// <summary> 身高 </summary>
      public int height = 157;
      /// <summary> 特长爱好 </summary>
      public string SpecialtyAndHobby = "未知特长和爱好";
      public string CV = "未知配音";
      /// <summary> 喜好菜式 </summary>
      public string favoriteCuisine = "未知喜好菜式";
   }
}