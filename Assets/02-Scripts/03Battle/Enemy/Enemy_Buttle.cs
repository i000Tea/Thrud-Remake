using UnityEngine;
namespace TeaFramework
{
   public class Enemy_Buttle : MonoBehaviour
   {
      private void Start()
      {
         Destroy(gameObject, 10f);
      }
      private void OnTriggerEnter(Collider other)
      {
         var player = other.transform.FindParentComponent<PlayerControl>();
         if (player)
         {
            "PlayerBeHit".InvokeSomething(Random.Range(4, 7));
            Debug.Log("广播-玩家受到攻击");
            Destroy(gameObject);
         }
      }
   }
}