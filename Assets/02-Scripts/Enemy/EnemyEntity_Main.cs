using UnityEngine;

namespace TeaFramework
{
   public class EnemyEntity_Main : MonoBehaviour
   {
      public bool onCame;
      [SerializeField] private DamageElement element;

      #region ui
      [SerializeField][Range(0f, 100f)] private float uiYOffset = 1f;
      private EnemyEntity_UI UIInst;
      #endregion

      #region numerical value
      [SerializeField] private float BaseHP = 100;
      #endregion

      private void Start()
      {
         if (!UIInst)
         {
            UIInst = this.NewEnemyEntityUI(element, uiYOffset);
         }
         EnemyControl.enemys.Add(this);
      }

      public void BeHit(float damage = 1)
      {
         BaseHP -= damage;
         if (BaseHP <= 0)
         {
            Debug.Log("die");
         }
         EnvEffectControl.I.NewDamageText(transform.position, (int)damage);
      }
      private void OnBecameVisible()
      {
         onCame = true;
         //Debug.Log(this.name.ToString() + "这个物体出现在屏幕里面了");
      }
      //物体离开屏幕  
      private void OnBecameInvisible()
      {
         onCame = false;
         //Debug.Log(this.name.ToString() + "这个物体离开屏幕里面了");
      }
   }
}
