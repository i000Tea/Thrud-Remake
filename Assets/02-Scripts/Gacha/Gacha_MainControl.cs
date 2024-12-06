using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace TeaFramework
{
   public class Gacha_MainControl : MonoBehaviour
   {
      #region MyRegion
      [Header("场景对象与预制件")]
      [SerializeField] private Transform poolBtnParent;
      [SerializeField] private GameObject poolBtnPrfab;

      [SerializeField] private PlayableDirector g01;
      [SerializeField] private PlayableDirector g01v5;
      [SerializeField] private PlayableDirector g10;
      [SerializeField] private PlayableDirector g10v5;

      [Header("石板对象与材质")]
      [SerializeField] private MeshRenderer[] slates = new MeshRenderer[10];
      [SerializeField] private Material slateMatV2;
      [SerializeField] private Material slateMatV3;
      [SerializeField] private Material slateMatV4;
      [SerializeField] private Material slateMatV5;
      [SerializeField] private Material slateMatV5Up;

      [Header("UI索引")]
      [SerializeField] private Gacha_UIOperation_Config uiConfig;

      [Header("卡池索引")]
      [SerializeField] private List<GachaPool_Data> onOpenPool;
      [SerializeField] private RoleItem_ListData allRole;
      [SerializeField] private WeaponItem_ListData allWeapon;

      [Header("结果缓存")]
      [SerializeField] private string[] getItems = new string[10];


      #endregion

      private int nowPoolIndex;
      private void Awake()
      {
         uiConfig.Initialize();
         CreateSequenceNumberDictionary(allRole.allRole.ToArray(), allWeapon.allwep.ToArray());
      }
      private IEnumerator Start()
      {
         AwakeSetPoolBtn();
         if (poolBtnParent.GetChild(0).TryGetComponent(out TeaBaseButton teaBtn))
         {
            Debug.Log(teaBtn);
            teaBtn.clickEvent?.Invoke();
         }
         yield return null;
         PoolSwitch(0);
      }
      private void AwakeSetPoolBtn()
      {
         // 使用循环移除 poolBtnParent 的所有子对象
         for (int i = poolBtnParent.childCount - 1; i >= 0; i--) { Destroy(poolBtnParent.GetChild(i).gameObject); }

         for (int i = 0; i < onOpenPool.Count; i++)
         {
            var instPool = Instantiate(poolBtnPrfab, poolBtnParent);
            var index = i;
            // 挂载按钮事件
            if (instPool.TryGetComponent(out TeaBaseButton teaBtn)) { teaBtn.clickEvent.AddListener(() => PoolSwitch(index)); }
            instPool.transform.PoolUISet(onOpenPool[i]);
         }

         var size = (poolBtnParent as RectTransform).sizeDelta;
         size.y = onOpenPool.Count * 140;
         (poolBtnParent as RectTransform).sizeDelta = size;

      }
      private void PoolSwitch(int index)
      {
         poolBtnParent.PoolUISwitch(index, onOpenPool[index].sprite_PoolBg);
         onOpenPool[index].SetSelectPool();
      }

      public void OnGacha01()
      {
         getItems = Gacha_CoreOperation.OnGacha01(out bool onGetV5);
         var tl = onGetV5 ? g01 : g01v5;
         StartCoroutine(GachaShow(tl));
      }
      public void OnGacha10()
      {
         getItems = Gacha_CoreOperation.OnGacha10(out bool onGetV5);
         var tl = onGetV5 ? g10 : g10v5;
         StartCoroutine(GachaShow(tl));
      }

      public IEnumerator GachaShow(PlayableDirector pd)
      {
         //pd.Play();
         //yield return new WaitForSeconds((float)(pd.duration - 0.5f));

         int[] rarity = new int[10];
         Sprite[] items = new Sprite[10];
         for (int i = 0; i < getItems.Length; i++)
         {
            if (string.IsNullOrEmpty(getItems[i])) break;
            var msg = getItems[i].Split(Gacha_CoreOperation.sep);
            int.TryParse(msg[0], out var id);
            Debug.Log(id);
            itemDataDic.TryGetValue(id, out var getItem);
            Debug.Log(getItem);
            var item = itemDataDic[int.Parse(msg[0])];

            if (item.GetType() == typeof(WeaponItem_Data))
            {
               items[i] = (item as WeaponItem_Data).sprite_LotteryResult;
            }
            rarity[i] = int.Parse(msg[1]);
         }
         Gacha_UIOperation.SetLotteryResultSprite(rarity, items);
         // 显示单张
         // 显示结算
         yield return Gacha_UIOperation.ShowLotteryResult();

      }



      public static Dictionary<int, Base_ItemData> itemDataDic = new();
      public static void CreateSequenceNumberDictionary(params Base_ItemData[][] itemDataMatrix)
      {
         itemDataDic.Clear();
         for (int i = 0; i < itemDataMatrix.Length; i++)
         {
            var itemDataArraw = itemDataMatrix[i];
            for (int j = 0; j < itemDataArraw.Length; j++)
            {
               var item = itemDataArraw[j];
               Debug.Log(itemDataDic);
               Debug.Log(item);
               if (!itemDataDic.TryGetValue(item.itemID, out _))
                  itemDataDic.Add(item.itemID, item);
            }
         }
      }
   }
}
