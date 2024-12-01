using UnityEngine;

namespace TeaFramework
{
   public class P_Health : P_IModular
   {
      private float maxHealth;
      private float nowHealth;
      public P_Health()
      {
         maxHealth = nowHealth = config.health;
         "PlayerBeHit".OnAddAnotherList<int>(BeHit);
      }
      public override void OnDestroy()
      {
         "PlayerBeHit".OnRemoveAnotherList<int>(BeHit);
      }
      public void BeHit(int damage)
      {
         nowHealth -= damage;
         Debug.Log("玩家受到攻击，当前" + nowHealth);
         if (nowHealth < 0) nowHealth = 0;
         "UI-HealthUpdate".InvokeSomething((int)nowHealth, (int)maxHealth);
      }
   }
}
