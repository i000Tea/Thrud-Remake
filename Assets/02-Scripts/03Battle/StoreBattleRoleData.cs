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
      public EntityBattleData(RoleItem_Data role, WeaponItem_Data wep)
      {
         roleName = role.itemName;
         wepName = wep.itemName;

         roleInst = Object.Instantiate(role.rolePreafab).transform;
         if (roleInst.GetChild(0).TryGetComponent(out Animator animator)) roleAnim = animator;

         wepInst = Object.Instantiate(wep.wepPrefab).transform;

         if (wepInst.TryGetComponent(out Weapon_ObjectComponent wepCtrl))
         {
            wepComponent = wepCtrl;

            var wepMode = CreateWepMode();
            wepMode.wep_damage = wep.damage;
            wepMode.wep_gunshot = wep.gunshot;
            wepMode.wep_reload = wep.reload;
            wepMode.wep_magazineSize = wep.magazineSize;
            wepMode.wep_firingRate = wep.firingRate;
            wepMode.wep_stability = wep.stability;

            wepMode.bulletPrefab = wep.bulletPrefab;

            wepMode.showPoint = wepCtrl.showPoint;

            this.wepMode = wepMode;
         }
      }

      private WepMode_0Base CreateWepMode()
      {
         WepMode_0Base mode = null;
         switch (wep_Type)
         {
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

            case WeaponSubType.heavy_TrackingMissile:
               mode = new WepMode_Heavy_TrackingMissile(); break;
            case WeaponSubType.heavy_PowerfulMissile:
               mode = new WepMode_Heavy_PowerfulMissile(); break;
            case WeaponSubType.heavy_LightweightMissile:
               mode = new WepMode_Heavy_LightweightMissile(); break;
            case WeaponSubType.heavy_MultiLoadedMissile:
               mode = new WepMode_Heavy_MultiLoadedMissile(); break;

            case WeaponSubType.scatter_SmashShot:
               mode = new WepMode_Scatter_SmashShot(); break;
            case WeaponSubType.scatter_PrecisionShot:
               mode = new WepMode_Scatter_PrecisionShot(); break;

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
      public string wepName;
      public Transform roleInst;
      public Transform wepInst;
      public Weapon_ObjectComponent wepComponent;
      public WepMode_0Base wepMode;

      public WeaponSubType wep_Type;
   }
}