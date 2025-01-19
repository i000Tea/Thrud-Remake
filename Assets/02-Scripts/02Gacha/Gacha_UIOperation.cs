using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using UnityEngine.Playables;
using TMPro;

namespace TeaFramework
{
   /// <summary>
   /// 抽卡系统UI操作静态类
   /// 负责处理所有与抽卡界面相关的UI操作和动画
   /// </summary>
   public static class Gacha_UIOperation
   {
      /// <summary> UI配置文件，存储所有UI相关参数和资源引用 </summary>
      private static Gacha_UIOperation_Config _config;
      /// <summary> 缓存的抽卡结果数据 </summary>
      private static GachaUIItemData[] _cacheDatas;

      public static string btnEvent = default;

      /// <summary>
      /// 初始化抽卡界面操作，设置配置文件
      /// </summary>
      /// <param name="inputConfig">输入的配置文件</param>
      public static void Initialize(this Gacha_UIOperation_Config inputConfig)
      {
         _config = inputConfig;
         _config.ResultUI.gameObject.SetActive(false);
         _config.GlobalVolumeCanvas.SetActive(false);
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
            tagImg.sprite = _config.tagSprite[tagIndex];
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
               btnImg.sprite = index == i ? _config.poolBtnBGSelect : _config.poolBtnBG;
            }
         }
         _config.poolMainBG.sprite = mainBg;
      }

      /// <summary>
      /// 设置抽卡结果的图片内容
      /// </summary>
      /// <param name="datas">抽卡结果的物品数据数组，包含每个抽取物品的详细信息</param>
      public static void SetLotteryResultSprite(GachaUIItemData[] datas)
      {
         _config.LotterySingle.gameObject.SetActive(false);
         _config.LotteryResult.gameObject.SetActive(false);

         _cacheDatas = datas;
         var lotteryResultChild = _config.LotteryResult.GetChild(0);

         for (int i = 0; i < lotteryResultChild.childCount; i++)
         {
            var currentChild = lotteryResultChild.GetChild(i);

            if (_cacheDatas.Length <= i)
            {
               currentChild.gameObject.SetActive(false);
               continue;
            }

            currentChild.gameObject.SetActive(true);
            Transform resultChild = currentChild.GetChild(0);
            var data = _cacheDatas[i];

            SetResultSprites(resultChild, data);
         }
      }

      /// <summary>
      /// 设置单个抽卡结果的精灵图片
      /// </summary>
      private static void SetResultSprites(Transform resultChild, GachaUIItemData data)
      {
         if (resultChild.GetChild(1).TryGetComponent(out Image backgroundImage))
         {
            backgroundImage.sprite = _config.LotteryResultBg[data.rarity - 3];
         }
         if (resultChild.GetChild(2).TryGetComponent(out Image shadowImage))
         {
            shadowImage.sprite = _config.LotteryResultShadow[data.rarity - 3];
         }
         if (resultChild.GetChild(3).TryGetComponent(out Image resultImage))
         {
            resultImage.sprite = data.lotteryResult;
         }
      }

      /// <summary>
      /// 显示单张抽卡结果
      /// </summary>
      public static IEnumerator ShowLotterySingle(PlayableDirector singleDirector)
      {
         _config.ResultUI.gameObject.SetActive(true);
         _config.LotterySingle.gameObject.SetActive(true);
         _config.LotteryResult.gameObject.SetActive(false);
         for (int i = 0; i < _cacheDatas.Length; i++)
         {
            btnEvent = string.Empty;
            var data = _cacheDatas[i];
            singleDirector.time = 0;
            singleDirector.Play();
            yield return new WaitForFixedUpdate();

            _config.starParent.GetChild(0).gameObject.SetActive(data.rarity >= 5);
            _config.starParent.GetChild(1).gameObject.SetActive(data.rarity >= 4);

            for (int n = 0; n < _config.particleParent.childCount; n++)
            {
               _config.starParent.GetChild(n).gameObject.SetActive(5 - data.rarity == n);
            }
            for (int n = 0; n < _config.bgColorParent.childCount; n++)
            {
               _config.bgColorParent.GetChild(n).gameObject.SetActive(5 - data.rarity == n);
            }
            _config.mainShow.sprite = data.lotterySingle;
            _config.nameText.text = data.name;
            _config.enNameText.text = data.enName;
            _config.introduceText.text = data.introduce;
            _config.occupationBG.sprite = data.occupationBG;
            _config.occupation.sprite = data.occupation;
            _config.occupation.gameObject.SetActive(!data.isWep);
            while (true)
            {
               yield return new WaitForFixedUpdate();
               Debug.Log("当前ui事件"+btnEvent);
               if ("next".Equals(btnEvent)) break;
               else if ("skip".Equals(btnEvent)) yield break;
            }
            Debug.Log("结束" + i);
            btnEvent = default;
         }
         yield return 0;
         Debug.Log("单显示全部结束");
      }

      /// <summary>
      /// 显示所有抽卡结果（带动画效果）
      /// </summary>
      public static IEnumerator ShowLotteryResult()
      {
         var lotteryResultChild = _config.LotteryResult.GetChild(0);
         _config.GlobalVolumeCanvas.SetActive(true);
         _config.ResultUI.gameObject.SetActive(true);
         yield return new WaitForFixedUpdate();
         _config.MainUI.gameObject.SetActive(false);
         _config.LotterySingle.gameObject.SetActive(false);

         for (int i = 0; i < lotteryResultChild.childCount; i++)
         {
            Transform resultChild = lotteryResultChild.GetChild(i);
            if (resultChild.TryGetComponent(out CanvasGroup cGroup))
            {
               cGroup.alpha = 0;
            }
         }

         _config.LotteryResult.gameObject.SetActive(true);
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
         _config.MainUI.gameObject.SetActive(true);
         _config.ResultUI.gameObject.SetActive(false);
         _config.GlobalVolumeCanvas.SetActive(false);
         _config.LotterySingle.gameObject.SetActive(false);
         _config.LotteryResult.gameObject.SetActive(false);
      }
   }

   /// <summary>
   /// UI操作配置类，用于序列化存储所有UI相关的参数和资源引用
   /// </summary>
   [System.Serializable]
   public class Gacha_UIOperation_Config
   {
      [SerializeField, FormerlySerializedAs("_tag")]
      private PoolTag poolTag;

      [Tooltip("主界面UI的RectTransform组件")]
      public RectTransform MainUI;

      [Tooltip("全局渲染特效画布")]
      public GameObject GlobalVolumeCanvas;

      [Tooltip("抽卡结果界面的RectTransform组件")]
      public RectTransform ResultUI;

      [Tooltip("单次抽卡结果界面的RectTransform组件")]
      public RectTransform LotterySingle;

      public Transform starParent;
      public Transform bgColorParent;
      public Transform particleParent;
      public TMP_Text nameText;
      public TMP_Text enNameText;
      public TMP_Text introduceText;
      public Image mainShow;
      public Image occupationBG;
      public Image occupation;

      [Tooltip("多次抽卡结果界面的RectTransform组件")]
      public RectTransform LotteryResult;

      [Tooltip("各种池类型的标签图片数组")]
      public Sprite[] tagSprite;

      [Tooltip("池按钮未选中状态的背景图片")]
      public Sprite poolBtnBG;

      [Tooltip("池按钮选中状态的背景图片")]
      public Sprite poolBtnBGSelect;

      [Tooltip("主界面的背景图片组件")]
      public Image poolMainBG;
      public Sprite[] LotterySingleBg;
      public Sprite[] LotteryPositionIcon;
      public Sprite[] LotteryWepIcon;

      [Tooltip("不同稀有度的抽卡结果背景图片数组")]
      public Sprite[] LotteryResultBg;

      [Tooltip("不同稀有度的抽卡结果阴影图片数组")]
      public Sprite[] LotteryResultShadow;
   }

   /// <summary>
   /// 抽卡UI物品数据类，用于存储单个抽卡结果的显示数据
   /// </summary>
   public class GachaUIItemData
   {
      /// <summary> 背景图片索引，基于物品稀有度计算 </summary>
      public int rarity;
      public bool isWep;

      /// <summary> 抽卡结果展示用的精灵图片 </summary>
      public Sprite lotteryResult;
      public Sprite lotterySingle;

      public Sprite occupationBG;
      public Sprite occupation;

      public string name;
      public string enName;
      public string introduce;

      /// <summary>
      /// 构造函数，根据物品数据初始化UI显示数据
      /// </summary>
      /// <param name="itemData">基础物品数据</param>
      public GachaUIItemData(Base_ItemData itemData, Gacha_UIOperation_Config config)
      {
         rarity = itemData.Rarity;
         if (itemData is RoleItem_Data roleData)
         {
            isWep = false;
            lotteryResult = roleData.sprite_LotteryLeader;
            lotterySingle = roleData.sprite_Lottery;
            var i1 = (int)roleData.damageElement;
            var i2 = (int)roleData.roleDefinition;
            occupationBG = config.LotteryPositionIcon[i1 * 5];
            occupation = config.LotteryPositionIcon[i1 * 5 + i2 + 1];
            Debug.Log($"获取角色职业分支图标值 {i1 * 5} {i1 * 5 + i2 + 1}");
            name = roleData.itemName;
            enName = roleData.enName;
         }
         else if (itemData is WeaponItem_Data weaponData)
         {
            isWep = true;
            lotteryResult = weaponData.sprite_LotteryResult;
            lotterySingle = weaponData.sprite_Square;

            occupationBG = config.LotteryWepIcon[(int)weaponData.weaponType];
            name = weaponData.itemName;
            enName = weaponData.itemPinyin;
         }
      }
   }
}
