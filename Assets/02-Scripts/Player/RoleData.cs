using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace TeaFramework
{
   [CreateAssetMenu(fileName = "NewRoleData", menuName = "thrudData/新建角色数据", order = 1)]
   public class RoleData : ScriptableObject
   {
      public string roleName;
      public GameObject rolePreafab;
      public AnimatorController roleBattleAnimator;
      public AnimatorController roleHomeAnimator;
   }
   [System.Serializable]
   public class RoleBattleData
   {
      public RoleBattleData(GameObject role)
      {
         this.role = role;

         if (!animator)
         {
            if (role.TryGetComponent(out Animator animator)) { }
            else if (role.transform.GetChild(0).TryGetComponent(out animator)) { }
            if (animator) this.animator = animator;
         }
      }
      public GameObject role;
      public Animator animator;

      public void EnterSet(int index)
      {
         if (index <= 0)
         {
            animator.SetTrigger("GameStart");
         }
         else
         {
            role.SetActive(false);
         }
      }
   }
}
