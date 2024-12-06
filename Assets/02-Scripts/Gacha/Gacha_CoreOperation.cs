using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

namespace TeaFramework
{
   public static class Gacha_CoreOperation
   {
      public const char sep = '$';

      private static string[] getItems = new string[10];
      public static Dictionary<string, PoolRecord> cachePoolRecords = new();
      private static PoolRecord NowPoolRecord => cachePoolRecords[onSelectPoolKey];
      private static string onSelectPoolKey;

      /// <summary> 5星概率 </summary>
      private static int v5Prob = 200;
      /// <summary> 5星up占比 </summary>
      private static int v5UpProb = 5000;

      /// <summary> 5星阶梯提升开始次数 </summary>
      private static int v5StageUpIndex = 59;
      /// <summary> 5星阶梯提升量 </summary>
      private static int v5StageUpValue = 200;
      /// <summary> 5星保底数 </summary>
      private static int v5MaxIndex = 79;

      /// <summary> 4星概率 </summary>
      private static int v4Prob = 1800;
      /// <summary> 4星up占比 </summary>
      private static int v4UpProb = 5000;
      /// <summary> 4星保底数 </summary>
      private static int v4MaxIndex = 9;

      /// <summary> 3星概率 </summary>
      private static int v3Prob = 10000;

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
            poolKey = selectPool.poolID.ToString();

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
         var item = rarity.RandomItemByRarity();
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
            itemName = (item as WeaponItem_Data).wepName;
            itemID = (item as WeaponItem_Data).wepID.ToString();
            itemType = "wep";
         }
         Debug.Log($"搜索到{itemName}");
         getItems[index] = $"{itemID}{sep}{rarity}{sep}{itemName}{sep}{itemType}";
      }

      /// <summary> 计算一个稀有度 </summary>
      private static int CalculateRarity()
      {
         string logStr = default;
         int getRarity = 2;
         // 未出5星已到数量 保底5
         if (NowPoolRecord.unGetV5Index >= v5MaxIndex)
         {
            getRarity = 5;
            logStr += "\n未出5星到限制 保底5星";
         }
         // 未出4星已到数量 保底4
         else if (NowPoolRecord.unGetV4Index >= v4MaxIndex)
         {
            getRarity = 4;
            logStr += "\n未出4星到限制 保底4星";
         }
         // 没有保底，开始计算
         else
         {
            logStr += $"\n距离上一个5过去{NowPoolRecord.unGetV5Index}距离上一个4过去{NowPoolRecord.unGetV4Index}";
            var randomValue = Random.Range(0, 10000);

            var get5Prob = v5Prob +
             Mathf.Max(0, NowPoolRecord.unGetV5Index - v5StageUpIndex) * v5StageUpValue;
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
            NowPoolRecord.unGetV5Index = 0;
            NowPoolRecord.unGetV4Index = 0;
         }
         else if (getRarity == 4)
         {
            NowPoolRecord.unGetV5Index++;
            NowPoolRecord.unGetV4Index = 0;
         }
         else
         {
            NowPoolRecord.unGetV5Index++;
            NowPoolRecord.unGetV4Index++;

         }

         logStr = $"获取到稀有度为{getRarity}{logStr}";
         Debug.Log(logStr);
         return getRarity;
      }

      private static Base_ItemData RandomItemByRarity(this int rarity)
      {
         var upValue = Random.Range(0, 10000);
         var itemList = cacheSelectPool.poolV3_ItemData;
         switch (rarity)
         {
            case 5:
               if (cacheSelectPool.poolV5UP_ItemData.Count > 0 && upValue < v5UpProb) { itemList = cacheSelectPool.poolV5UP_ItemData; }
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

   /// <summary> 卡池记录 </summary>
   [System.Serializable]
   public class PoolRecord
   {
      public string poolKey;
      /// <summary> 卡池类型 </summary>
      public string poolTag;
      /// <summary> 卡池id </summary>
      public int poolId;
      /// <summary> 总抽次数 </summary>
      public int allGachaIndex;
      /// <summary> 总抽v5次数 </summary>
      public int allGetV5Index;
      /// <summary> 总抽v4次数 </summary>
      public int allGetV4Index;
      /// <summary> 未出5次数 </summary>      
      public int unGetV5Index;
      /// <summary> 未出4次数 </summary>      
      public int unGetV4Index;
      /// <summary> 下一个5是否是大保底 </summary>      
      public bool nextV5IsUP;
   }
}
