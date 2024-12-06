using System.Collections;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.UI;

namespace TeaFramework
{
   public static class Gacha_UIOperation
   {
      /// <summary> 配置文件，用于存储与 UI 相关的参数和资源 </summary>
      private static Gacha_UIOperation_Config config;

      /// <summary>
      /// 初始化抽卡界面操作，设置配置文件
      /// </summary>
      /// <param name="inputConfig">输入的配置文件</param>
      public static void Initialize(this Gacha_UIOperation_Config inputConfig)
      {
         config = inputConfig;
         config.ResultUI.gameObject.SetActive(false);
      }

      /// <summary>
      /// 初始化时设置池 UI
      /// </summary>
      /// <param name="pool">池的 Transform 对象</param>
      /// <param name="data">池的相关数据</param>
      public static void PoolUISet(this Transform pool, GachaPool_Data data)
      {
         if (pool.GetChild(1).TryGetComponent(out Image adImg))
         {
            adImg.sprite = data.sprite_PoolBtnBg;
         }
         if (pool.GetChild(2).TryGetComponent(out Image tagImg))
         {
            var tagIndex = (int)data.pooltag;
            tagImg.sprite = config.tagSprite[tagIndex];
         }
      }

      /// <summary>
      /// 切换池的 UI
      /// </summary>
      /// <param name="poolBtnParent">池按钮的父级 Transform</param>
      /// <param name="index">当前池的索引</param>
      /// <param name="mainBg">主背景图片</param>
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

      /// <summary>
      /// 显示单张抽卡结果
      /// </summary>
      public static void ShowLotterySingle()
      {
         config.LotterySingle.gameObject.SetActive(true);
         config.LotteryResult.gameObject.SetActive(false);
      }

      /// <summary>
      /// 设置抽卡结果的图片内容
      /// </summary>
      /// <param name="datas">抽卡结果的物品数据数组</param>
      public static void SetLotteryResultSprite(GachaUIItemData[] datas)
      {
         // 先获取列表的父对象
         var lotteryResultChild = config.LotteryResult.GetChild(0);
         // 循环子对象
         for (int i = 0; i < lotteryResultChild.childCount; i++)
         {
            // 若datas的长度小于当前循环序号 则不存在 对象设置为关闭 跳过
            if (datas.Length <= i)
            {
               lotteryResultChild.GetChild(i).gameObject.SetActive(false);
               continue;
            }
            // 否则 打开
            lotteryResultChild.GetChild(i).gameObject.SetActive(true);
            // 
            Transform resultChild = lotteryResultChild.GetChild(i).GetChild(0);

            var data = datas[i];

            if (resultChild.GetChild(1).TryGetComponent(out Image img))
            {
               img.sprite = config.LotteryResultBg[data.bgIndex];
            }
            if (resultChild.GetChild(2).TryGetComponent(out img))
            {
               img.sprite = config.LotteryResultShadow[data.bgIndex];
            }
            if (resultChild.GetChild(3).TryGetComponent(out img))
            {
               img.sprite = data.lotteryResult;
            }
         }
      }

      /// <summary>
      /// 显示所有抽卡结果（带动画效果）
      /// </summary>
      public static IEnumerator ShowLotteryResult()
      {
         var lotteryResultChild = config.LotteryResult.GetChild(0);
         config.ResultUI.gameObject.SetActive(true);
         config.LotterySingle.gameObject.SetActive(false);

         for (int i = 0; i < lotteryResultChild.childCount; i++)
         {
            Transform resultChild = lotteryResultChild.GetChild(i);
            if (resultChild.TryGetComponent(out CanvasGroup cGroup))
            {
               cGroup.alpha = 0;
            }
         }

         config.LotteryResult.gameObject.SetActive(true);
         yield return new WaitForFixedUpdate();

         var delay = new WaitForSeconds(0.1f);

         foreach (Transform resultChild in lotteryResultChild)
         {
            if (resultChild.TryGetComponent(out Animator animator))
            {
               animator.Play("gacha_LotteryResult_On");
            }
            yield return delay;
         }
      }

      /// <summary>
      /// 隐藏所有抽卡结果
      /// </summary>
      public static void ShowLotteryOver()
      {
         config.ResultUI.gameObject.SetActive(false);
         config.LotterySingle.gameObject.SetActive(false);
         config.LotteryResult.gameObject.SetActive(false);
      }
   }

   [System.Serializable]
   public class Gacha_UIOperation_Config
   {
      /// <summary> 池标签，用于标记池的类型 </summary>
      [SerializeField] PoolTag _tag;
      /// <summary> 抽卡结果总界面 </summary>
      public RectTransform ResultUI;
      /// <summary> 单次抽卡结果界面 </summary>
      public RectTransform LotterySingle;
      /// <summary> 多次抽卡结果界面 </summary>
      public RectTransform LotteryResult;

      /// <summary> 各种池标签图片 </summary>
      public Sprite[] tagSprite;
      /// <summary> 池按钮未选中背景图片 </summary>
      public Sprite poolBtnBG;
      /// <summary> 池按钮选中背景图片 </summary>
      public Sprite poolBtnBGSelect;

      /// <summary> 主界面背景图片 </summary>
      public Image poolMainBG;

      /// <summary> 抽卡结果背景图片（按稀有度） </summary>
      public Sprite[] LotteryResultBg;
      /// <summary> 抽卡结果阴影图片（按稀有度） </summary>
      public Sprite[] LotteryResultShadow;
   }


   public class GachaUIItemData
   {
      public int bgIndex;
      public Sprite lotteryResult;
      public GachaUIItemData(Base_ItemData itemData)
      {
         bgIndex = itemData.Rarity - 2;
         if (itemData.GetType() == typeof(RoleItem_Data))
         {
            lotteryResult = (itemData as RoleItem_Data).sprite_LotteryLeader;
         }
         else if (itemData.GetType() == typeof(WeaponItem_Data))
         {
            lotteryResult = (itemData as WeaponItem_Data).sprite_LotteryResult;
         }
      }
   }
}
