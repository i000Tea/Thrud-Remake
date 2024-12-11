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
         //roleInst.gameObject.SetActive(false);
         if (roleInst.GetChild(0).TryGetComponent(out Animator animator)) roleAnim = animator;
         wepInst = Object.Instantiate(wep.wepPrefab).transform;
         //wepInst.gameObject.SetActive(false);
         if (wepInst.TryGetComponent(out Weapon_Base wepCtrl)) wepControl = wepCtrl;
         wep_Type = wep.weaponSubType;
         wep_damage = wep.damage_Base;
         wep_gunshot = wep.gunshot_Base;
         wep_reload = wep.reload_Base;
         wep_magazineSize = wep.magazineSize_Base;
         wep_firingRate = wep.firingRate_Base;
         wep_stability = wep.stability_Base;

      }
      public string roleName;
      public Animator roleAnim;
      public string wepName;
      public Transform roleInst;
      public Transform wepInst;
      public Weapon_Base wepControl;

      public WeaponSubType wep_Type;
      /// <summary> 武器 伤害 </summary>
      public int wep_damage;
      /// <summary> 武器 射程 </summary>
      public int wep_gunshot;
      /// <summary> 武器 换弹时间 </summary>
      public float wep_reload;
      /// <summary> 武器 弹匣容量 </summary>
      public int wep_magazineSize;
      /// <summary> 武器 射速 </summary>
      public int wep_firingRate;
      /// <summary> 武器 稳定性 </summary>
      public int wep_stability;
   }
}