using System.Collections.Generic;
using UnityEngine;

namespace TeaFramework
{
   public class StoreBattleRoleData : Singleton<StoreBattleRoleData>
   {
      [SerializeField] private RoleItem_Data[] loadRoles;
      [SerializeField] private WeaponItem_Data[] loadWeps;
      public EntityBattleData[] battleDatas;
      protected override void Awake()
      {
         base.Awake();
         DontDestroyOnLoad(gameObject);
         List<EntityBattleData> battleDatas = new();
         for (int i = 0; i < loadRoles.Length; i++)
         {
            var roleData = loadRoles[i];
            var wepData = loadWeps[i];
            if (roleData == null) { Debug.LogError("角色数据为空"); continue; }
            else if (wepData == null) { Debug.LogError("武器数据为空"); continue; }
            battleDatas.Add(new EntityBattleData(roleData, wepData));
         }
         this.battleDatas = battleDatas.ToArray();
      }
   }
   [System.Serializable]
   public class EntityBattleData
   {
      /// <summary> 输入角色和武器 配置一个实体战斗对象数据 </summary>
      /// <param name="role"></param>
      /// <param name="wep"></param>
      public EntityBattleData(RoleItem_Data role, WeaponItem_Data wep)
      {
         // 角色名
         roleName = role.itemName;
         // 实例化角色预制件
         roleInst = Object.Instantiate(role.rolePreafab).transform;
         // 角色动画控件
         if (roleInst.GetChild(0).TryGetComponent(out Animator animator)) roleAnim = animator;

         // 武器名
         wepName = wep.itemName;
         // 实例化武器预制件
         wepInst = Object.Instantiate(wep.wepPrefab).transform;
         // 设置武器模组
         wep_Type = wep.weaponSubType;
         // 当武器预制件上存在武器组件
         if (wepInst.TryGetComponent(out Weapon_ObjectComponent wepCtrl))
         {
            // 配置武器组件
            wepComponent = wepCtrl;

            // 创建武器模组
            var wepMode = CreateWepMode();
            // 配置模组参数
            wepMode.damage = wep.damage;
            wepMode.gunshot = wep.gunshot;
            wepMode.reload = wep.reload;
            wepMode.magazineSize = wep.magazineSize;
            wepMode.firingRate = wep.firingRate;
            wepMode.stability = wep.stability;

            // 赋值子弹预制件
            wepMode.wepObjComp = wepCtrl;

            // 赋值子弹生成点
            wepMode.showPoint = wepCtrl.showPoint;
            // 赋值武器模组
            this.wepMode = wepMode;
         }
      }

      /// <summary> 根据模组生成对应实例化类 </summary>
      private WepMode_0Base CreateWepMode()
      {
         WepMode_0Base mode = null;
         switch (wep_Type)
         {
            // 突击大类
            case WeaponSubType.assault_PreciseFrame:
               mode = new WepMode_Assault_PreciseFrame(); break;
            case WeaponSubType.assault_EnergyFrame:
               mode = new WepMode_Assault_EnergyFrame(); break;
            case WeaponSubType.assault_RapidFireFrame:
               mode = new WepMode_Assault_RapidFireFrame(); break;
            case WeaponSubType.assault_FocusFrame:
               mode = new WepMode_Assault_FocusFrame(); break;
            case WeaponSubType.assault_StormFrame:
               mode = new WepMode_Assault_StormFrame(); break;

            // 重击大类
            case WeaponSubType.heavy_TrackingMissile:
               mode = new WepMode_Heavy_TrackingMissile(); break;
            case WeaponSubType.heavy_PowerfulMissile:
               mode = new WepMode_Heavy_PowerfulMissile(); break;
            case WeaponSubType.heavy_LightweightMissile:
               mode = new WepMode_Heavy_LightweightMissile(); break;
            case WeaponSubType.heavy_MultiLoadedMissile:
               mode = new WepMode_Heavy_MultiLoadedMissile(); break;

            // 霰弹大类
            case WeaponSubType.scatter_SmashShot:
               mode = new WepMode_Scatter_SmashShot(); break;
            case WeaponSubType.scatter_PrecisionShot:
               mode = new WepMode_Scatter_PrecisionShot(); break;

            // 狙击大类
            case WeaponSubType.sniper_Penetrating:
               mode = new WepMode_Sniper_Penetrating(); break;
            case WeaponSubType.sniper_Tandem:
               mode = new WepMode_Sniper_Tandem(); break;
            case WeaponSubType.sniper_Swift:
               mode = new WepMode_Sniper_Swift(); break;
            case WeaponSubType.sniper_Charged:
               mode = new WepMode_Sniper_Charged(); break;
            default: break;
         }
         return mode;
      }

      public string roleName;
      public Animator roleAnim;
      public Transform roleInst;

      public string wepName;
      public Transform wepInst;
      public WeaponSubType wep_Type;

      public Weapon_ObjectComponent wepComponent;
      public WepMode_0Base wepMode;
   }
}