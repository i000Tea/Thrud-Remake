using UnityEngine;

namespace TeaFramework
{
   public class RoleItem_Data : Base_ItemData
   {
      [Header("基础数据")]
      public string enName = "englishName";
      public DamageElement damageElement;
      public RoleSpecialty roleDefinition;
      public int inPoolType;
      [Header("战斗数值")]
      public float health = 360;
      [Header("模型预制件")]
      public GameObject rolePreafab;
      [Header("美术素材")]
      public Sprite sprite_listIcon;
      public Sprite sprite_Lottery;
      public Sprite sprite_LotteryLeader;
      public Sprite sprite_roleUI;
      /// <summary> 小队 </summary>
      [Header("设定文案")]
      public string onTeam = "未知小队";
      /// <summary> 势力，派系 </summary>
      public string onFactions = "未知派系";
      /// <summary> 出生地 </summary>
      public string birthplace = "未知出生地";
      /// <summary> 出生 </summary>
      public string birthday ="13月31日";
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