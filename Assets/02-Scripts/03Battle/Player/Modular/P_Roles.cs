using System.Collections.Generic;
using UnityEngine;

namespace TeaFramework
{
   public class P_Roles : P_IModular
   {
      private readonly List<RoleBattleData> battleRoles = new();
      private RoleBattleData NowBattleData => battleRoles[Config.selectRoleIndedx];
      public P_Roles()
      {
         battleRoles.Clear();
         for (int i = 0; i < Config.loadRoles.Count; i++)
         {
            var roldData = Config.loadRoles[i];
            var prefab = roldData.rolePreafab;
            if (!prefab) continue;

            battleRoles.Add(new RoleBattleData(Object.Instantiate(prefab, Config.roleParent), roldData.health));
         }
         for (int i = 0; i < battleRoles.Count; i++)
         {
            battleRoles[i].EnterSet(i);
         }
         "RoleSwitch".OnAddAnotherList(RoleSwitch);
         "RoleAnimatorEvent".OnAddAnotherList<string>(RoleAnimatorEvent);
         "PlayerBeHit".OnAddAnotherList<int>(BeHit);
      }
      public override void Start()
      {
         NowBattleData.BeHit(0);
      }
      public override void Update()
      {
         MoveVelocity();
      }
      public override void OnDestroy()
      {
         "RoleSwitch".OnRemoveAnotherList(RoleSwitch);
         "RoleAnimatorEvent".OnRemoveAnotherList<string>(RoleAnimatorEvent);
         "PlayerBeHit".OnRemoveAnotherList<int>(BeHit);
      }

      public void MoveVelocity()
      {
         var anim = NowBattleData.animator;
         var data = Config.GetRigFromForword();
         //Debug.Log(data);
         anim.SetFloat("MoveFront", data.z);
         anim.SetFloat("MoveSide", data.x);

         anim.SetBool("OnSprint", Config.OnSprint());
      }

      public void RoleSwitch()
      {
         Debug.Log("角色切换");
         for (int i = 0; i < battleRoles.Count; i++)
         {
            //Debug.Log($"角色切换 {Config.selectRoleIndedx} {i} {Config.selectRoleIndedx == i}");
            battleRoles[i].role.SetActive(Config.selectRoleIndedx == i);
         }
      }

      private void RoleAnimatorEvent(string eventName)
      {
         var anim = battleRoles[Config.selectRoleIndedx].animator;
         switch (eventName)
         {
            case "OnRoll":
               anim.SetFloat("RollFront", Config.rollForce.y);
               anim.SetFloat("RollSide", Config.rollForce.x);
               anim.SetTrigger("OnRoll");
               break;
            default:
               break;
         }
      }

      public void BeHit(int damage) => NowBattleData.BeHit(damage);

   }
}
