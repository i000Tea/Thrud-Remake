using System.Collections.Generic;
using UnityEngine;

namespace TeaFramework
{
   public class P_ViewModule : P_0ModularBase
   {
      public P_ViewModule()
      {
         "RoleSwitch".OnAddAnotherList(RoleSwitch);
         "RoleAnimatorEvent".OnAddAnotherList<string>(RoleAnimatorEvent);
         "PlayerBeHit".OnAddAnotherList<int>(BeHit);
      }
      public override void Start()
      {
         for (int i = 0; i < EntityBattleDatas.Length; i++)
         {
            EntityBattleDatas[i].roleInst.parent = Config.roleParent;
            EntityBattleDatas[i].roleInst.SetLocalPositionAndRotation(Vector3.zero,Quaternion.identity);
            EntityBattleDatas[i].wepInst.parent = Config.wepParent;
            EntityBattleDatas[i].wepInst.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
         }
         Debug.Log(EntityBattleDatas[0].roleAnim);
         EntityBattleDatas[0].roleAnim.SetTrigger("GameStart");
         RoleSwitch();
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
         var anim = NowBattleData.roleAnim;
         var data = Config.GetRigFromForword();
         //Debug.Log(data);
         anim.SetFloat("MoveFront", data.z);
         anim.SetFloat("MoveSide", data.x);

         anim.SetBool("OnSprint", Config.OnSprint());
      }

      public void RoleSwitch()
      {
         Debug.Log("角色切换");
         for (int i = 0; i < EntityBattleDatas.Length; i++)
         {
            //Debug.Log($"角色切换 {Config.selectRoleIndedx} {i} {Config.selectRoleIndedx == i}");
            EntityBattleDatas[i].roleInst.gameObject.SetActive(Config.selectRoleIndedx == i);
            EntityBattleDatas[i].wepInst.gameObject.SetActive(Config.selectRoleIndedx == i);
         }
      }

      private void RoleAnimatorEvent(string eventName)
      {
         var anim = NowBattleData.roleAnim;
         switch (eventName)
         {
            case "OnRoll":
               anim.SetFloat("RollFront", Config.rollForce.y);
               anim.SetFloat("RollSide", Config.rollForce.x);
               anim.Play("RollBlend", 0, 0f);
               //anim.SetTrigger("OnRoll");
               break;
            default:
               break;
         }
      }

      public void BeHit(int damage)
      {

      }
   }
}
