using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TeaFramework
{
   /// <summary>
   /// 环境特效控制类，用于管理伤害文本的对象池和显示生命周期
   /// </summary>
   public class EnvEffectControl : Singleton<EnvEffectControl>
   {
      [SerializeField] private GameObject DamagePrefab; // 伤害文本的预制体
      [SerializeField] private Transform DamageParent; // 伤害文本的预制体

      private readonly List<GameObject> dmgTextShows = new(); // 正在显示的伤害文本列表
      private readonly List<GameObject> dmgTextPool = new();  // 可复用的伤害文本对象池

      [SerializeField] private float dmgTextShowTime = 2f; // 伤害文本显示时长
      [SerializeField] private int angleScope = 60; // 等待时间对象，用于减少频繁创建
      [SerializeField][Range(1, 10)] private float distMinScope = 1; // 等待时间对象，用于减少频繁创建
      [SerializeField][Range(1, 10)] private float distMaxScope = 3; // 等待时间对象，用于减少频繁创建
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
      private void LateUpdate()
      {
         if (PlayerControl.I.config.camera != null)
         {
            for (int i = 0; i < dmgTextShows.Count; i++)
            {
               dmgTextShows[i].transform.LockCamera(PlayerControl.I.config.camera);
               //dmgTextShows[i].transform.GetChild(0).LookatCamera(PlayerControl.I.config.camera);
            }

         }
      }
      /// <summary>
      /// 创建或复用一个伤害文本对象，并启动其显示生命周期
      /// </summary>
      /// <param name="Damage">伤害值</param>
      public void NewDamageText(Vector3 world, int Damage)
      {
         GameObject textObject;

         // 如果对象池中有可用对象，则复用
         if (dmgTextPool.Count > 0)
         {
            textObject = dmgTextPool[0];
            dmgTextPool.RemoveAt(0); // 从对象池中移除
         }
         // 如果正在显示的对象数量超过 60，则回收最早的一个对象
         else if (dmgTextShows.Count > 160)
         {
            textObject = dmgTextShows[0];
            dmgTextShows.RemoveAt(0); // 从显示列表中移除
         }
         // 否则创建新的对象
         else
         {
            textObject = Instantiate(DamagePrefab, DamageParent); // 实例化预制体
         }

         textObject.SetActive(true);

         // 设置位置
         textObject.transform.position = world;

         // 设置偏移位置
         var childObject = textObject.transform.GetChild(0).transform;
         {

            var angleInDegrees = Random.Range(-angleScope, angleScope);
            var InDegrees = Random.Range(distMinScope, distMaxScope);
            // 将角度转换为弧度
            float angleInRadians = angleInDegrees * Mathf.Deg2Rad;

            // 计算 x 和 y 坐标
            float x = Mathf.Cos(angleInRadians) * InDegrees;
            float y = Mathf.Sin(angleInRadians) * InDegrees;

            // 给 x 值一半概率变为负数 // 生成 0 或 1，50% 概率
            if (Random.Range(0, 2) == 0) { x = -x; }
            // 设置对象的相对坐标，z 固定为 1
            childObject.localPosition = new Vector3(x, y, -0.1f);

         }

         childObject.TryGetComponent(out TMP_Text tmpText);
         // 设置内容
         if (tmpText)
         {
            tmpText.text = Damage.ToString();
         }

         // 设置动画
         if (tmpText)
         {
            tmpText.color = new(1, 1, 1, 0);
            tmpText.DOColor(Color.white, 0.1f);
         }
         textObject.transform.localScale = Vector3.one * 0.7f;
         textObject.transform.DOScale(1, 0.1f);

         // 将对象加入正在显示的列表，并启动生命周期协程
         dmgTextShows.Add(textObject);
         StartCoroutine(DmgTextLife(textObject));
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

   public static class UIExpand
   {
      public static void LockCamera(this Transform transform, Camera camera)
      {
         transform.rotation = camera.transform.rotation;
         // 这里我的角色朝向和UI朝向是相反的，如果直接用LookAt()还需要把每个UI元素旋转过来。
         // 为了简单，用了下面这个方法。它实际上是一个反向旋转，可以简单理解为“负负得正”吧
         //transform.rotation = Quaternion.LookRotation(transform.position - PlayerControl.I.config.camera.transform.position);
      }
   }
}
