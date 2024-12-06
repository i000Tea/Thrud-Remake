using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TeaFramework
{
   public static class Gacha_UIOperation
   {
      private static Gacha_UIOperation_Config config;
      public static void Initialize(this Gacha_UIOperation_Config inputConfig)
      {
         config = inputConfig;
         config.ResultUI.gameObject.SetActive(false);
      }
      /// <summary>
      /// 初始化时池ui设置
      /// </summary>
      /// <param name="pool"></param>
      /// <param name="data"></param>
      public static void PoolUISet(this Transform pool, GachaPool_Data data)
      {
         if (pool.GetChild(1).TryGetComponent(out Image adImg))
         {
            adImg.sprite = data.sprite_PoolBtnBg;
         }
         if (pool.GetChild(2).TryGetComponent(out Image tagImg))
         {
            var tagindex = (int)data.pooltag;
            Debug.Log(tagindex);
            Debug.Log(tagImg.sprite);
            Debug.Log(config);
            Debug.Log(config.tagSprite[(int)data.pooltag]);

            tagImg.sprite = config.tagSprite[(int)data.pooltag];
         }
      }
      /// <summary>
      /// 池切换
      /// </summary>
      public static void PoolUISwitch(this Transform poolBtnParent, int index, Sprite mainBg)
      {
         for (int i = 0; i < poolBtnParent.childCount; i++)
         {
            if (poolBtnParent.GetChild(i).GetChild(0).TryGetComponent(out Image btnImg))
            {
               btnImg.sprite = index == i ? config.poolBtnBGSelect : config.poolBtnBG;
            }
         }
         config.poolMainBG.sprite = mainBg;
      }

      /// <summary> 显示单张结果 </summary>
      public static void ShowLotterySingle()
      {
         config.LotterySingle.gameObject.SetActive(true);
         config.LotteryResult.gameObject.SetActive(false);
      }
      public static void SetLotteryResultSprite(int[] rarity, Sprite[] items)
      {
         var lotteryResultChild = config.LotteryResult.GetChild(0);
         for (int i = 0; i < lotteryResultChild.childCount; i++)
         {
            Transform resultChild = lotteryResultChild.GetChild(i);
            if (resultChild.GetChild(1).TryGetComponent(out Image img)) { img.sprite = config.LotteryResultBg[rarity[i] - 2]; }
            if (resultChild.GetChild(2).TryGetComponent(out img)) { img.sprite = config.LotteryResultShadow[rarity[i] - 2]; }
            if (resultChild.GetChild(3).TryGetComponent(out img)) { img.sprite = items[i]; }

         }

      }
      /// <summary> 显示全部结果 </summary>
      public static IEnumerator ShowLotteryResult()
      {
         var lotteryResultChild = config.LotteryResult.GetChild(0);
         config.ResultUI.gameObject.SetActive(true);
         config.LotterySingle.gameObject.SetActive(false);

         for (int i = 0; i < lotteryResultChild.childCount; i++)
         {
            // 隐藏所有子元素的 CanvasGroup
            Transform resultChild = lotteryResultChild.GetChild(i);
            if (resultChild.TryGetComponent(out CanvasGroup cGroup))
            {
               cGroup.alpha = 0;
               Debug.Log(cGroup.alpha);
            }
            else { Debug.Log("未查找到CanvasGroup"); }
         }

         config.LotteryResult.gameObject.SetActive(true);
         yield return new WaitForFixedUpdate();

         // 延迟时间，避免在每次循环中创建多个 WaitForSeconds 实例
         var delay = new WaitForSeconds(0.1f);

         // 播放动画
         foreach (Transform resultChild in lotteryResultChild)
         {
            if (resultChild.TryGetComponent(out Animator animator))
            {
               animator.Play("gacha_LotteryResult_On");
            }
            yield return delay;
         }
      }

   }

   [System.Serializable]
   public class Gacha_UIOperation_Config
   {
      [SerializeField] PoolTag _tag;
      public RectTransform ResultUI;
      public RectTransform LotterySingle;
      public RectTransform LotteryResult;

      public Sprite[] tagSprite;
      public Sprite poolBtnBG;
      public Sprite poolBtnBGSelect;

      public Image poolMainBG;

      public Sprite[] LotteryResultBg;
      public Sprite[] LotteryResultShadow;
   }
}
