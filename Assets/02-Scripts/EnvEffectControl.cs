using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeaFramework
{
   /// <summary>
   /// 环境特效控制类，用于管理伤害文本的对象池和显示生命周期
   /// </summary>
   public class EnvEffectControl : Singleton<EnvEffectControl>
   {
      [SerializeField] private GameObject DamagePrefab; // 伤害文本的预制体
      private readonly List<GameObject> dmgTextShows = new(); // 正在显示的伤害文本列表
      private readonly List<GameObject> dmgTextPool = new();  // 可复用的伤害文本对象池
      [SerializeField] private float dmgTextShowTime = 2f; // 伤害文本显示时长
      private WaitForSeconds lifeTime; // 等待时间对象，用于减少频繁创建

      /// <summary>
      /// 初始化方法，清理列表并设置显示时间
      /// </summary>
      protected override void Awake()
      {
         lifeTime = new WaitForSeconds(dmgTextShowTime); // 初始化显示时长
         dmgTextShows.Clear(); // 清空显示列表
         dmgTextPool.Clear();  // 清空对象池
         base.Awake(); // 调用父类的 Awake 方法
      }

      /// <summary>
      /// 创建或复用一个伤害文本对象，并启动其显示生命周期
      /// </summary>
      /// <param name="Damage">伤害值</param>
      public void NewDamageText(int Damage)
      {
         GameObject text;

         // 如果对象池中有可用对象，则复用
         if (dmgTextPool.Count > 0)
         {
            text = dmgTextPool[0];
            dmgTextPool.RemoveAt(0); // 从对象池中移除
         }
         // 如果正在显示的对象数量超过 60，则回收最早的一个对象
         else if (dmgTextShows.Count > 60)
         {
            text = dmgTextShows[0];
            dmgTextShows.RemoveAt(0); // 从显示列表中移除
         }
         // 否则创建新的对象
         else
         {
            text = Instantiate(DamagePrefab); // 实例化预制体
         }
         // 将对象加入正在显示的列表，并启动生命周期协程
         dmgTextShows.Add(text);
         StartCoroutine(DmgTextLife(text));
      }

      /// <summary>
      /// 控制伤害文本的显示生命周期，管理其从显示到回收到对象池的过程
      /// </summary>
      /// <param name="DmgInst">伤害文本对象</param>
      private IEnumerator DmgTextLife(GameObject DmgInst)
      {
         // 在协程开始时，将对象从对象池移除并加入显示列表
         if (dmgTextPool.Contains(DmgInst))
            dmgTextPool.Remove(DmgInst); // 确保对象从池中移除
         if (!dmgTextShows.Contains(DmgInst))
            dmgTextShows.Add(DmgInst); // 加入显示列表

         // 等待显示时长
         yield return lifeTime;

         // 显示结束后，将对象从显示列表移除并加入对象池
         if (dmgTextShows.Contains(DmgInst))
            dmgTextShows.Remove(DmgInst); // 从显示列表移除
         if (!dmgTextPool.Contains(DmgInst))
            dmgTextPool.Add(DmgInst); // 加入对象池

         // 可选：隐藏对象或重置其状态
         DmgInst.SetActive(false); // 隐藏伤害文本对象
      }
   }
}
