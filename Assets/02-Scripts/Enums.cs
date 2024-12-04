using System;

namespace TeaFramework
{
   /// <summary> 伤害元素 </summary>
   public enum DamageElement
   {
      [EnumDescription("无伤害", "none", "不伤害")]
      none,
      [EnumDescription("冰冻伤害", "冰", "寒冷")]
      Ice,
      [EnumDescription("火焰伤害", "火", "火焰")]
      Fire,
      [EnumDescription("风系伤害", "风", "风暴")]
      Wind,
      [EnumDescription("电击伤害", "电", "雷电", "雷")]
      Electric,
   }

   /// <summary> 武器类型 </summary>
   public enum WeaponType
   {
      [EnumDescription("突击型武器", "冲锋")]
      wep_Assault,
      [EnumDescription("散射型武器", "散射")]
      wep_Scatter,
      [EnumDescription("狙击型武器", "狙击")]
      wep_Sniper,
      [EnumDescription("重装型武器", "重型")]
      wep_Heavy,
   }

   /// <summary> 角色定位(职业) </summary>
   public enum RoleSpecialty
   {
      [EnumDescription("作战先锐", "先锋")]
      specialty_Pioneer,
      [EnumDescription("符能学者", "符文", "学者")]
      specialty_Rune,
      [EnumDescription("赋能专家", "增强", "赋能")]
      specialty_Enhancer,
      [EnumDescription("医疗专员", "医生", "医疗")]
      specialty_Medic,
   }

   public enum PoolTag
   {
      [EnumDescription("新手")]
      rawRecruit = -1,
      [EnumDescription("常驻")]
      normal,
      [EnumDescription("赛季")]
      competitionSeason,
      [EnumDescription("武装")]
      weapon,
      [EnumDescription("限定")]
      limit,
   }

   [Serializable]
   public static class ConversionEnumOrString
   {
      public static T StringToEnum<T>(this object str) where T : Enum
      {
         return str.ToString().StringToEnum<T>();
      }
      /// <summary>
      /// 将字符串转换为指定的枚举类型，支持模糊匹配多个描述项
      /// </summary>
      public static T StringToEnum<T>(this string str) where T : Enum
      {
         // 查找匹配的枚举值，忽略大小写
         foreach (var field in typeof(T).GetFields())
         {
            // 获取 EnumDescriptionAttribute 的描述内容
            var attribute = (EnumDescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(EnumDescriptionAttribute));
            if (attribute != null)
            {
               // 遍历所有描述项，进行模糊匹配
               foreach (var description in attribute.Descriptions)
               {
                  if (description.Contains(str, StringComparison.OrdinalIgnoreCase))
                  {
                     return (T)Enum.Parse(typeof(T), field.Name);
                  }
               }
            }
         }

         // 如果没有找到匹配项，则返回默认值并输出错误信息
         Console.WriteLine($"无法将 '{str}' 转换为枚举类型 {typeof(T)}");
         return default;

      }
   }

   /// <summary>
   /// 自定义的枚举描述 Attribute，用于存储枚举项的多个描述。
   /// </summary>
   [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
   public class EnumDescriptionAttribute : Attribute
   {
      public string[] Descriptions { get; }

      public EnumDescriptionAttribute(params string[] descriptions)
      {
         Descriptions = descriptions;
      }
   }
}