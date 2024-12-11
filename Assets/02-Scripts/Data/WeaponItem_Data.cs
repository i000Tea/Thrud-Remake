using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
namespace TeaFramework
{
   public class WeaponItem_Data : Base_ItemData
   {
      [Header("基础数据")]
      public WeaponType weaponType;
      public WeaponSubType weaponSubType;
      public int getType = 0;
      /// <summary> 伤害1 </summary>
      [Header("战斗数值")]
      [Header("伤害")]
      public int damage_Base = 360;
      /// <summary> 伤害2 </summary>
      public int damage_Max = 360;

      /// <summary> 射程1 </summary>
      [Header("射程")]
      public int gunshot_Base = 360;
      /// <summary> 射程2 </summary>
      public int gunshot_Max = 360;

      /// <summary> 换弹时间1 </summary>
      [Header("换弹时间")]
      public float reload_Base = 360;
      /// <summary> 换弹时间2 </summary>
      public float reload_Max = 360;

      /// <summary> 弹匣容量1 </summary>
      [Header("弹匣容量")]
      public int magazineSize_Base = 360;
      /// <summary> 弹匣容量2 </summary>
      public int magazineSize_Max = 360;

      /// <summary> 射速1 </summary>
      [Header("射速")]
      public int firingRate_Base = 360;
      /// <summary> 射速2 </summary>
      public int firingRate_Max = 360;

      /// <summary> 稳定性1 </summary>
      [Header("稳定性")]
      public int stability_Base = 360;
      /// <summary> 稳定性2 </summary>
      public int stability_Max = 360;

      [Header("技能")]
      public string skillName = "没有技能";
      public string skillDescribe = "技能描述";

      [Header("模型预制件")]
      public GameObject wepPrefab;
      /// <summary> 方图 结算显示 </summary>
      [Header("美术素材")]
      public Sprite sprite_LotteryResult;
      /// <summary> 竖条 </summary>
      public Sprite sprite_Square;
      /// <summary> 横条 </summary>
      public Sprite sprite_Line;
      /// <summary> 小队 </summary>
   }
}