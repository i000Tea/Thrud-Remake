using UnityEngine;

namespace TeaFramework
{
   [CreateAssetMenu(fileName = "NewRoleData", menuName = "thrudData/新建角色数据", order = 1)]
   public class RoleData : ScriptableObject
   {
      public string roleName;
      public float health = 360;
      public GameObject rolePreafab;
   }
   [System.Serializable]
   public class RoleBattleData
   {
      public RoleBattleData(GameObject role,float health)
      {
         this.role = role;
         maxHealth = nowHealth = health;
         if (!animator)
         {
            if (role.TryGetComponent(out Animator animator)) { }
            else if (role.transform.GetChild(0).TryGetComponent(out animator)) { }
            if (animator) this.animator = animator;
         }
      }
      public GameObject role;

      public float maxHealth;
      public float nowHealth;

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

      public void BeHit(float damage)
      {
         nowHealth -= damage;
         Debug.Log("玩家受到攻击，当前" + nowHealth);
         if (nowHealth < 0) nowHealth = 0;
         "UI-HealthUpdate".InvokeSomething((int)nowHealth, (int)maxHealth);
      }
   }
}
