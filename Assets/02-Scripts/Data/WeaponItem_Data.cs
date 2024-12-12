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
      public Vector2Int damage;

      /// <summary> 射程1 </summary>
      [Header("射程")]
      public Vector2Int gunshot;

      /// <summary> 换弹时间1 </summary>
      [Header("换弹时间")]
      public Vector2 reload;

      /// <summary> 弹匣容量1 </summary>
      [Header("弹匣容量")]
      public Vector2Int magazineSize;

      /// <summary> 射速1 </summary>
      [Header("射速")]
      public Vector2Int firingRate;

      /// <summary> 稳定性1 </summary>
      [Header("稳定性")]
      public Vector2Int stability;

      [Header("技能")]
      public string skillName = "没有技能";
      public string skillDescribe = "技能描述";

      [Header("模型预制件")]
      public GameObject wepPrefab;
      public GameObject bulletPrefab;
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