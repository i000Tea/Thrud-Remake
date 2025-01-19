using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization; // 添加命名空间

namespace TeaFramework
{
   /// <summary>
   /// 抽卡系统的主控制器
   /// 负责管理抽卡界面、动画播放和结果展示
   /// </summary>
   public class Gacha_MainControl : MonoBehaviour
   {
      #region 成员变量与组件引用

      [Tooltip("编辑器模式下的时间缩放倍数")]
      [SerializeField] private float multiSpeed = 1;

      [Header("场景对象与预制件")]
      [Tooltip("卡池按钮的父对象，用于动态生成卡池按钮")]
      [SerializeField] private Transform poolBtnParent;

      [Tooltip("卡池按钮的预制件")]
      [FormerlySerializedAs("poolBtnPrfab")]
      [SerializeField] private GameObject poolButtonPrefab;

      [Header("动画时间轴")]
      [Tooltip("输入动画")]
      [FormerlySerializedAs("tl_Input")]
      [SerializeField] private PlayableDirector timelineInput;

      [Tooltip("待机动画")]
      [FormerlySerializedAs("tl_Idle")]
      [SerializeField] private PlayableDirector timelineIdle;

      [Tooltip("普通单抽动画")]
      [FormerlySerializedAs("g01")]
      [SerializeField] private PlayableDirector gachaSingle;

      [Tooltip("高稀有度单抽动画")]
      [FormerlySerializedAs("g01v5")]
      [SerializeField] private PlayableDirector gachaSingleRare;

      [Tooltip("普通十连动画")]
      [FormerlySerializedAs("g10")]
      [SerializeField] private PlayableDirector gachaTen;

      [Tooltip("高稀有度十连动画")]
      [FormerlySerializedAs("g10v5")]
      [SerializeField] private PlayableDirector gachaTenRare;

      [Tooltip("单次展示动画")]
      [FormerlySerializedAs("gSingle")]
      [SerializeField] private PlayableDirector gachaShowSingle;

      [Header("石板对象与材质")]
      /// <summary>石板模型对象数组</summary>
      [SerializeField] private MeshRenderer[] slates = new MeshRenderer[10];
      /// <summary>稀有度2的石板材质</summary>
      [SerializeField] private Material slateMatV2;
      /// <summary>稀有度3的石板材质</summary>
      [SerializeField] private Material slateMatV3;
      /// <summary>稀有度4的石板材质</summary>
      [SerializeField] private Material slateMatV4;
      /// <summary>稀有度5的石板材质</summary>
      [SerializeField] private Material slateMatV5;
      /// <summary>提升概率后的稀有度5的石板材质</summary>
      [SerializeField] private Material slateMatV5Up;

      [Header("UI索引")]
      /// <summary>用于管理UI操作的配置数据</summary>
      [SerializeField] private Gacha_UIOperation_Config uiConfig;

      [Header("卡池索引")]
      /// <summary>当前开放的卡池数据</summary>
      [SerializeField] private List<GachaPool_Data> onOpenPool;
      /// <summary>所有角色的列表数据</summary>
      [SerializeField] private RoleItem_ListData allRole;
      /// <summary>所有武器的列表数据</summary>
      [SerializeField] private WeaponItem_ListData allWeapon;

      [Header("结果缓存")]
      /// <summary>抽卡结果的临时存储</summary>
      [SerializeField] private string[] getItems = new string[10];

      #endregion
      /// <summary> 有延迟的事件正在进行中 </summary>
      bool delayEventOnProgress;
      private int nowPoolIndex; // 当前选择的卡池索引

      #region 初始化启动

      /// <summary>
      /// 初始化方法，负责调用UI配置初始化和创建物品数据字典
      /// </summary>
      private void Awake()
      {
#if UNITY_EDITOR
         Time.timeScale = multiSpeed;
#endif

         uiConfig.Initialize();
         CreateSequenceNumberDictionary(allRole.allRole.ToArray(), allWeapon.allwep.ToArray());

      }

      /// <summary>
      /// Unity默认的启动协程，初始化卡池按钮并切换到默认卡池
      /// </summary>
      private async void Start()
      {
         StartCoroutine(AwakeTimeline());
         AwakeSetPoolBtn(); // 创建卡池按钮

         // 自动选中第一个卡池按钮并触发点击事件
         if (poolBtnParent.GetChild(0).TryGetComponent(out TeaBaseButton teaBtn))
         {
            Debug.Log(teaBtn);
            teaBtn.clickEvent?.Invoke();
         }


         await Task.Delay(20); // 替代协程的等待，用于异步延时
                               // 切换到第一个卡池
         PoolSwitch(0);

         // 音量淡入（启动时播放音频）
         delayEventOnProgress = true;
         await audioSource.AudioVolumeLerpAsync(lerpTime, 0, alVolumeTarget, lerpCurve);
         delayEventOnProgress = false;

      }
      private IEnumerator AwakeTimeline()
      {
         timelineInput.Play();
         yield return new WaitForSeconds((float)timelineInput.duration);
         timelineIdle.Play();
      }

      /// <summary>
      /// 初始化卡池按钮，移除旧按钮并根据卡池列表生成新按钮
      /// </summary>
      private void AwakeSetPoolBtn()
      {
         // 删除父对象中的所有子对象
         for (int i = poolBtnParent.childCount - 1; i >= 0; i--)
         {
            Destroy(poolBtnParent.GetChild(i).gameObject);
         }

         // 遍历卡池列表，动态生成对应的按钮
         for (int i = 0; i < onOpenPool.Count; i++)
         {
            var instPool = Instantiate(poolButtonPrefab, poolBtnParent);
            var index = i;

            // 为按钮添加点击事件监听
            if (instPool.TryGetComponent(out TeaBaseButton teaBtn))
            {
               teaBtn.clickEvent.AddListener(() => PoolSwitch(index));
            }

            // 设置按钮的UI显示
            instPool.transform.PoolUISet(onOpenPool[i]);
         }

         // 调整父对象的高度以适应按钮数量
         var size = (poolBtnParent as RectTransform).sizeDelta;
         size.y = onOpenPool.Count * 140;
         (poolBtnParent as RectTransform).sizeDelta = size;
      }

      #endregion

      #region 输入
      /// <summary>
      /// 切换卡池并更新UI
      /// </summary>
      /// <param name="index">目标卡池的索引</param>
      private void PoolSwitch(int index)
      {
         poolBtnParent.PoolUISwitch(index, onOpenPool[index].sprite_PoolBg);
         onOpenPool[index].SetSelectPool();
      }

      /// <summary>
      /// 单抽功能，触发抽卡逻辑和播放动画
      /// </summary>
      public void OnGacha01()
      {
         if (delayEventOnProgress) { Debug.Log("正在抽卡，请稍后"); return; }
         getItems = Gacha_CoreOperation.OnGacha01(out bool onGetV5);
         var tl = onGetV5 ? gachaSingleRare : gachaSingle;
         delayEventOnProgress = true;
         StartCoroutine(GachaShow(tl));
      }

      /// <summary>
      /// 十连抽功能，触发抽卡逻辑和播放动画
      /// </summary>
      public void OnGacha10()
      {
         if (delayEventOnProgress) { Debug.Log("正在抽卡，请稍后"); return; }
         getItems = Gacha_CoreOperation.OnGacha10(out bool onGetV5);
         var tl = onGetV5 ? gachaTenRare : gachaTen;
         delayEventOnProgress = true;
         StartCoroutine(GachaShow(tl));
      }

      /// <summary> 显示菜单结束 </summary>
      public void GachaShowOver()
      {
         Gacha_UIOperation.ShowLotteryOver();
         timelineIdle.time = 0;
         timelineIdle.Play();
         delayEventOnProgress = false;
      }

      public void Button_UIEvent(string _event)
      {
         Debug.Log("设置ui事件" + _event);
         Gacha_UIOperation.btnEvent = _event;
      }
      #endregion

      #region 抽卡结算后续

      /// <summary>
      /// 抽卡展示动画的协程，负责解析抽卡结果并更新UI
      /// </summary>
      /// <param name="pd">对应的动画PlayableDirector</param>
      /// <returns>协程</returns>
      public IEnumerator GachaShow(PlayableDirector pd)
      {
         pd.Play();

         // 用于设置石板的稀有度组
         List<int> slateRaritys = new();
         List<GachaUIItemData> datas = new();

         for (int i = 0; i < getItems.Length; i++)
         {
            // 若元素为空，则跳过数据处理
            if (string.IsNullOrEmpty(getItems[i])) break;
            // 拆分并获取id
            var msg = getItems[i].Split(Gacha_CoreOperation.sep);

            // ======== 设置石板参数 ========
            // 先找稀有度并设置
            int.TryParse(msg[1], out var rarity);
            // 找到up参数并设置
            bool.TryParse(msg[2], out var r5UP);
            if (rarity == 5 && r5UP) rarity++;
            slateRaritys.Add(rarity);

            // 找id
            int.TryParse(msg[0], out var id);

            // 获取id对应数据
            if (itemDataDic.TryGetValue(id, out var getItem))
               datas.Add(new(getItem, uiConfig));
         }

         // 设置石板材质
         SetSlateMaterial(slateRaritys);

         // 等待抽卡动画播放完毕
         yield return new WaitForSeconds((float)(pd.duration - 0.5f));

         // 更新抽卡结果UI
         Gacha_UIOperation.SetLotteryResultSprite(datas.ToArray());
         pd.Stop();

         yield return Gacha_UIOperation.ShowLotterySingle(gachaShowSingle);

         // 显示抽卡动画和结算界面
         yield return Gacha_UIOperation.ShowLotteryResult();
      }

      private void SetSlateMaterial(List<int> items)
      {
         // 检测是否有6，若有将第一个6移至第0位
         int index = items.IndexOf(6);
         if (index > 0)
         {
            int temp = items[index];
            items.RemoveAt(index);
            items.Insert(0, temp);
         }
         else
         {
            // 若没有6，检测是否有5，若有将第一个5移至第0位
            index = items.IndexOf(5);
            if (index > 0)
            {
               int temp = items[index];
               items.RemoveAt(index);
               items.Insert(0, temp);
            }
         }

         for (int i = 0; i < items.Count; i++)
         {
            Material slateMaterial = slateMatV2;
            switch (items[i])
            {
               case 6: slateMaterial = slateMatV5Up; break;
               case 5: slateMaterial = slateMatV5; break;
               case 4: slateMaterial = slateMatV4; break;
               case 3: slateMaterial = slateMatV3; break;
               default: break;
            }

            slates[i].sharedMaterial = slateMaterial;
         }
      }



      #endregion

      #region 字典
      /// <summary>
      /// 全局物品数据字典，用于快速查找物品
      /// </summary>
      public static Dictionary<int, Base_ItemData> itemDataDic = new();

      /// <summary>
      /// 创建全局的物品ID与数据映射字典
      /// </summary>
      /// <param name="itemDataMatrix">所有物品的二维数组</param>
      public static void CreateSequenceNumberDictionary(params Base_ItemData[][] itemDataMatrix)
      {
         itemDataDic.Clear(); // 清空旧数据

         for (int i = 0; i < itemDataMatrix.Length; i++)
         {
            var itemDataArraw = itemDataMatrix[i];
            for (int j = 0; j < itemDataArraw.Length; j++)
            {
               var item = itemDataArraw[j];
               if (!itemDataDic.TryGetValue(item.itemID, out _))
                  itemDataDic.Add(item.itemID, item);
            }
         }
      }
      #endregion


      // 插值时间范围：[0.5f, 4]秒，控制音量变化的时间
      [SerializeField][Range(0.5f, 4)] private float lerpTime = 1;

      // 音频源，用于播放音频
      [SerializeField] private AudioSource audioSource;

      // 音量目标值，音频淡入/淡出时的目标音量
      [SerializeField][Range(0, 1)] private float alVolumeTarget = 0.7f;

      // 插值曲线，控制音量变化的动画曲线
      [SerializeField] private AnimationCurve lerpCurve;
      public async void Button_Back()
      {
         // 判断是否有延迟事件正在进行中
         if (!delayEventOnProgress)
         {
            Debug.Log("start按键按下");

            // 设置插值时间
            lerpTime = 1;

            // 开始加载场景
            LoadingControl.OnLoad(1);
            delayEventOnProgress = true;
            // 开始音量淡出（停止播放音频）
            await audioSource.AudioVolumeLerpAsync(0.8f, alVolumeTarget, 0, lerpCurve);
            delayEventOnProgress = false;
         }
         else
         {
            Debug.Log("有事件正在进行中");
         }
      }
   }
}
