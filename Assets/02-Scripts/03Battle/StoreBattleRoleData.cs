using System.Collections.Generic;
using UnityEngine;

namespace TeaFramework
{
   /// <summary>
   /// 战斗角色数据存储类
   /// 用于管理和存储战斗中的角色和武器数据
   /// 继承自单例模式基类
   /// </summary>
   public class StoreBattleRoleData : Singleton<StoreBattleRoleData>
   {
      [SerializeField] private RoleItem_Data[] loadRoles;      // 可序列化的角色数据数组
      [SerializeField] private WeaponItem_Data[] loadWeps;     // 可序列化的武器数据数组
      public EntityBattleData[] battleDatas;                   // 存储所有实体战斗数据的数组

      protected override void Awake()
      {
         base.Awake();
         DontDestroyOnLoad(gameObject);                        // 场景切换时不销毁该对象
         List<EntityBattleData> battleDatas = new();           // 创建临时列表存储战斗数据
         
         // 遍历所有角色和武器数据，创建对应的战斗实体数据
         for (int i = 0; i < loadRoles.Length; i++)
         {
            var roleData = loadRoles[i];
            var wepData = loadWeps[i];
            if (roleData == null) { Debug.LogError("角色数据为空"); continue; }
            else if (wepData == null) { Debug.LogError("武器数据为空"); continue; }
            battleDatas.Add(new EntityBattleData(roleData, wepData));
         }
         this.battleDatas = battleDatas.ToArray();             // 将列表转换为数组存储
      }
   }

   /// <summary>
   /// 实体战斗数据类
   /// 包含角色和武器的所有战斗相关数据
   /// </summary>
   [System.Serializable]
   public class EntityBattleData
   {
      /// <summary> 
      /// 构造函数：通过角色数据和武器数据创建战斗实体
      /// </summary>
      /// <param name="role">角色数据</param>
      /// <param name="wep">武器数据</param>
      public EntityBattleData(RoleItem_Data role, WeaponItem_Data wep)
      {
         // 设置角色相关数据
         roleName = role.itemName;                             // 设置角色名称
         roleInst = Object.Instantiate(role.rolePreafab).transform;  // 实例化角色预制体
         // 获取角色的动画控制器组件
         if (roleInst.GetChild(0).TryGetComponent(out Animator animator)) roleAnim = animator;

         // 设置武器相关数据
         wepName = wep.itemName;                              // 设置武器名称
         wepInst = Object.Instantiate(wep.wepPrefab).transform;  // 实例化武器预制体
         wep_Type = wep.weaponSubType;                        // 设置武器类型

         // 获取并配置武器组件
         if (wepInst.TryGetComponent(out Weapon_ObjectComponent wepCtrl))
         {
            wepComponent = wepCtrl;

            // 创建并配置武器模组
            var wepMode = CreateWepMode();                    // 根据武器类型创建对应的武器模组
            // 设置武器属性参数
            wepMode.damage = wep.damage;                      // 伤害值
            wepMode.gunshot = wep.gunshot;                    // 射击音效
            wepMode.reload = wep.reload;                      // 装弹音效
            wepMode.magazineSize = wep.magazineSize;          // 弹匣容量
            wepMode.firingRate = wep.firingRate;              // 射速
            wepMode.stability = wep.stability;                // 稳定性
            
            wepMode.expansion01 = wep.expandValue01;
            wepMode.expansion02 = wep.expandValue02;
            
            // 设置武器组件引用
            wepMode.wepObjComp = wepCtrl;                    // 设置武器对象组件
            wepMode.showPoint = wepCtrl.showPoint;           // 设置子弹生成点
            this.wepMode = wepMode;                          // 保存武器模组引用
         }
      }

      /// <summary> 
      /// 根据武器类型创建对应的武器模组实例
      /// </summary>
      /// <returns>返回创建的武器模组实例</returns>
      private WepMode_0Base CreateWepMode()
      {
         WepMode_0Base mode = null;
         switch (wep_Type)
         {
            // 突击步枪系列武器模组
            case WeaponSubType.assault_PreciseFrame:          // 精确框架
               mode = new WepMode_Assault_PreciseFrame(); break;
            case WeaponSubType.assault_EnergyFrame:           // 能量框架
               mode = new WepMode_Assault_EnergyFrame(); break;
            case WeaponSubType.assault_RapidFireFrame:        // 速射框架
               mode = new WepMode_Assault_RapidFireFrame(); break;
            case WeaponSubType.assault_FocusFrame:            // 专注框架
               mode = new WepMode_Assault_FocusFrame(); break;
            case WeaponSubType.assault_StormFrame:            // 风暴框架
               mode = new WepMode_Assault_StormFrame(); break;

            // 重型武器系列模组
            case WeaponSubType.heavy_TrackingMissile:         // 追踪导弹
               mode = new WepMode_Heavy_TrackingMissile(); break;
            case WeaponSubType.heavy_PowerfulMissile:         // 强力导弹
               mode = new WepMode_Heavy_PowerfulMissile(); break;
            case WeaponSubType.heavy_LightweightMissile:      // 轻型导弹
               mode = new WepMode_Heavy_LightweightMissile(); break;
            case WeaponSubType.heavy_MultiLoadedMissile:      // 多重装填导弹
               mode = new WepMode_Heavy_MultiLoadedMissile(); break;

            // 霰弹枪系列模组
            case WeaponSubType.scatter_SmashShot:             // 粉碎射击
               mode = new WepMode_Scatter_SmashShot(); break;
            case WeaponSubType.scatter_PrecisionShot:         // 精确射击
               mode = new WepMode_Scatter_PrecisionShot(); break;

            // 狙击枪系列模组
            case WeaponSubType.sniper_Penetrating:            // 穿透型
               mode = new WepMode_Sniper_Penetrating(); break;
            case WeaponSubType.sniper_Tandem:                 // 串联型
               mode = new WepMode_Sniper_Tandem(); break;
            case WeaponSubType.sniper_Swift:                  // 快速型
               mode = new WepMode_Sniper_Swift(); break;
            case WeaponSubType.sniper_Charged:                // 蓄力型
               mode = new WepMode_Sniper_Charged(); break;
            default: break;
         }
         return mode;
      }

      // 角色相关数据
      public string roleName;           // 角色名称
      public Animator roleAnim;         // 角色动画控制器
      public Transform roleInst;        // 角色实例Transform组件

      // 武器相关数据
      public string wepName;            // 武器名称
      public Transform wepInst;         // 武器实例Transform组件
      public WeaponSubType wep_Type;    // 武器子类型

      public Weapon_ObjectComponent wepComponent;  // 武器对象组件
      public WepMode_0Base wepMode;     // 武器模组实例
   }
}