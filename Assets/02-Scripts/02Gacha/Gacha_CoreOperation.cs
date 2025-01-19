using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace TeaFramework
{
   /// <summary>
   /// 抽卡核心操作类
   /// 处理抽卡的主要逻辑，包括概率计算、保底机制等
   /// </summary>
   public static class Gacha_CoreOperation
   {
      /// <summary> 物品信息分隔符 </summary>
      public const char sep = '$';

      /// <summary> 临时存储抽取到的物品信息 </summary>
      private static string[] getItems = new string[10];
      
      /// <summary> 缓存所有卡池的抽卡记录 </summary>
      public static Dictionary<string, PoolRecord> cachePoolRecords = new();
      
      /// <summary> 当前选中卡池的记录 </summary>
      private static PoolRecord NowPoolRecord => cachePoolRecords[onSelectPoolKey];
      
      /// <summary> 当前选中卡池的Key </summary>
      private static string onSelectPoolKey;

      #region 概率相关常量
      /// <summary> 5星基础概率 (0.2%) </summary>
      private static int v5Prob = 200;
      /// <summary> 5星UP角色占5星总概率比例 (50%) </summary>
      private static int v5UpProb = 5000;

      /// <summary> 5星概率提升起始抽数 </summary>
      private static int v5StageUpIndex = 59;
      /// <summary> 每次5星概率提升值 </summary>
      private static int v5StageUpValue = 200;
      /// <summary> 5星保底抽数 </summary>
      private static int v5MaxIndex = 79;

      /// <summary> 4星基础概率 (1.8%) </summary>
      private static int v4Prob = 1800;
      /// <summary> 4星UP物品占4星总概率比例 (50%) </summary>
      private static int v4UpProb = 5000;
      /// <summary> 4星保底抽数 </summary>
      private static int v4MaxIndex = 9;

      /// <summary> 3星概率 (10%) </summary>
      private static int v3Prob = 10000;
      #endregion

      /// <summary> 缓存当前选中的卡池数据 </summary>
      private static GachaPool_Data cacheSelectPool;

      /// <summary>
      /// 设置当前选择池的信息
      /// </summary>
      /// <param name="selectPool"></param>
      public static void SetSelectPool(this GachaPool_Data selectPool)
      {
         // 缓存为空 则查找
         if (cachePoolRecords == null) { cachePoolRecords = new(); }

         // 设置 key 标准池用统一key 其余用单独id
         string poolKey;
         if (selectPool.pooltag == PoolTag.normal)
            poolKey = PoolTag.normal.ToString();
         else
            poolKey = selectPool.itemID.ToString();

         // 在缓存中查找，若没有找到则新建一个
         cachePoolRecords.TryGetValue(poolKey, out PoolRecord getRecord);
         if (getRecord == null)
         {
            getRecord = new PoolRecord();
            cachePoolRecords.Add(poolKey, getRecord);
         }

         onSelectPoolKey = poolKey;
         cacheSelectPool = selectPool;
      }

      public static string[] OnGacha01(out bool onGetV5)
      {
         getItems = new string[10];
         0.OneGacha(out int rarity);
         onGetV5 = rarity == 5;
         return getItems;
      }

      public static string[] OnGacha10(out bool onGetV5)
      {
         onGetV5 = false;
         for (int i = 0; i < 10; i++)
         {
            i.OneGacha(out int rarity);
            if (rarity == 5) onGetV5 = true;
         }
         return getItems;
      }

      public static void OneGacha(this int index, out int rarity)
      {
         rarity = CalculateRarity();
         // 
         var item = rarity.RandomItemByRarity(out bool onGetV5Up);
         string itemName = "未确定类型";
         string itemID = "未确定类型";
         string itemType = "未确定类型";
         if (item.GetType() == typeof(RoleItem_Data))
         {
            itemName = (item as RoleItem_Data).itemName;
            itemID = (item as RoleItem_Data).itemID.ToString();
            itemType = "role";
         }
         else if (item.GetType() == typeof(WeaponItem_Data))
         {
            itemName = (item as WeaponItem_Data).itemName;
            itemID = (item as WeaponItem_Data).itemID.ToString();
            itemType = "wep";
         }
         //Debug.Log($"搜索到{itemName}");
         getItems[index] = $"{itemID}{sep}{rarity}{sep}{onGetV5Up}{sep}{itemName}{sep}{itemType}";
      }

      /// <summary> 计算一个稀有度 </summary>
      private static int CalculateRarity()
      {
         string logStr = default;
         int getRarity = 2;
         // 未出5星已到数量 保底5
         if (NowPoolRecord.pullsSinceLastFiveStar >= v5MaxIndex)
         {
            getRarity = 5;
            logStr += "\n未出5星到限制 保底5星";
         }
         // 未出4星已到数量 保底4
         else if (NowPoolRecord.pullsSinceLastFourStar >= v4MaxIndex)
         {
            getRarity = 4;
            logStr += "\n未出4星到限制 保底4星";
         }
         // 没有保底，开始计算
         else
         {
            logStr += $"\n距离上一个5过去{NowPoolRecord.pullsSinceLastFiveStar}距离上一个4过去{NowPoolRecord.pullsSinceLastFourStar}";
            var randomValue = Random.Range(0, 10000);

            var get5Prob = v5Prob +
             Mathf.Max(0, NowPoolRecord.pullsSinceLastFiveStar - v5StageUpIndex) * v5StageUpValue;
            if (randomValue < get5Prob)
               getRarity = 5;
            else if (randomValue < get5Prob + v4Prob)
               getRarity = 4;
            else if (randomValue < get5Prob + v4Prob + v3Prob)
               getRarity = 3;
            logStr += $"\n随机数:{randomValue}; 5:<{get5Prob} 4:<{get5Prob + v4Prob} 3:<{get5Prob + v4Prob + v3Prob}";
         }
         if (getRarity == 5)
         {
            NowPoolRecord.pullsSinceLastFiveStar = 0;
            NowPoolRecord.pullsSinceLastFourStar = 0;
         }
         else if (getRarity == 4)
         {
            NowPoolRecord.pullsSinceLastFiveStar++;
            NowPoolRecord.pullsSinceLastFourStar = 0;
         }
         else
         {
            NowPoolRecord.pullsSinceLastFiveStar++;
            NowPoolRecord.pullsSinceLastFourStar++;

         }

         logStr = $"获取到稀有度为{getRarity}{logStr}";
         //Debug.Log(logStr);
         return getRarity;
      }

      private static Base_ItemData RandomItemByRarity(this int rarity, out bool onGetV5Up)
      {
         onGetV5Up = false;
         var upValue = Random.Range(0, 10000);
         var itemList = cacheSelectPool.poolV3_ItemData;
         switch (rarity)
         {
            case 5:
               if (cacheSelectPool.poolV5UP_ItemData.Count > 0 && upValue < v5UpProb)
               {
                  onGetV5Up = true;
                  itemList = cacheSelectPool.poolV5UP_ItemData;
               }
               else { itemList = cacheSelectPool.poolV5_ItemData; }
               break;
            case 4:
               if (cacheSelectPool.poolV4UP_ItemData.Count > 0 && upValue < v4UpProb) { itemList = cacheSelectPool.poolV4UP_ItemData; }
               else { itemList = cacheSelectPool.poolV4_ItemData; }
               break;
            case 3:
               itemList = cacheSelectPool.poolV3_ItemData;
               break;
            case 2:
               itemList = cacheSelectPool.poolV2_ItemData;
               break;
            default:
               break;
         }
         return itemList[Random.Range(0, itemList.Count)];
      }
   }

   /// <summary> 卡池抽卡记录类 </summary>
   [System.Serializable]
   public class PoolRecord
   {
      [FormerlySerializedAs("poolKey")]
      public string poolIdentifier; // 更改变量名并保留旧引用
      
      /// <summary> 卡池类型标识 </summary>
      [FormerlySerializedAs("poolTag")]
      public string poolCategory;
      
      /// <summary> 卡池唯一标识ID </summary>
      [FormerlySerializedAs("poolId")]
      public int poolIdentificationNumber;
      
      /// <summary> 该卡池总抽卡次数 </summary>
      [FormerlySerializedAs("allGachaIndex")]
      public int totalPullCount;
      
      /// <summary> 该卡池获得5星总次数 </summary>
      [FormerlySerializedAs("allGetV5Index")]
      public int totalFiveStarCount;
      
      /// <summary> 该卡池获得4星总次数 </summary>
      [FormerlySerializedAs("allGetV4Index")]
      public int totalFourStarCount;
      
      /// <summary> 距离上次获得5星的抽数 </summary>
      [FormerlySerializedAs("unGetV5Index")]
      public int pullsSinceLastFiveStar;
      
      /// <summary> 距离上次获得4星的抽数 </summary>
      [FormerlySerializedAs("unGetV4Index")]
      public int pullsSinceLastFourStar;
      
      /// <summary> 下一个5星是否必定为UP角色(大保底) </summary>
      [FormerlySerializedAs("nextV5IsUP")]
      public bool guaranteedUpFiveStar;
   }
}
