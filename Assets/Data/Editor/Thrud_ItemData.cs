using Codice.Client.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Drawing2D;
using TeaFramework.editor;
using UnityEditor;
using UnityEngine;
namespace TeaFramework.editor
{
   public static class Thrud_ItemData
   {
      public static string ResPath => Thrud_ReadExcel.resPath;

      #region 角色 Thrud
      public static void Thrud_SetItemList(this RoleItem_ListData itemData, DataRowCollection excelData, int columnNum, int rowNum)
      {
         itemData.allRole.RemoveEmptyListItem();

         for (int i = 1; i < rowNum; i++)
         {
            // 如果该行是空行，不计算
            if (excelData[i].IsEmptyRow(columnNum)) continue;

            var roleDataList = itemData.allRole;
            var excelRow = excelData[i];
            RoleItem_Data roleData = null;
            // 查找角色是否已存在，如果存在，则更新数据
            for (int j = 0; j < roleDataList.Count; j++)
            {
               if (roleDataList[j].itemName.Equals(excelRow[0].ToString()))
               {
                  roleData = roleDataList[j];
                  break;
               }
            }
            // 如果角色不存在，则创建一个新的角色数据
            roleData = roleData != null ? roleData : ScriptableObject.CreateInstance<RoleItem_Data>();

            excelRow.Thrud_SetItemData(ref roleData);

            // 在编辑器中保存 ScriptableObject
            if (!roleDataList.Contains(roleData))
            {
               string assetPath =
                  $"{ResPath}/RoleItemDatas/{roleData.itemID}-{(roleData.enName == "未设置" ? roleData.itemPinyin : roleData.enName)}.asset";

               AssetDatabase.CreateAsset(roleData, assetPath); // 保存角色数据为资源文件
               roleDataList.Add(roleData); // 如果角色数据不存在，则添加
            }
            else
            {
               EditorUtility.SetDirty(roleData);
            }
            // 保存已获取的武器数据
            AssetDatabase.SaveAssets();  // 保存所有更改
         }
      }

      /// <summary>
      /// 对一个单独对象设置值
      /// </summary>
      /// <param name="excelRow"></param>
      /// <param name="roleData"></param>
      private static void Thrud_SetItemData(this DataRow excelRow, ref RoleItem_Data roleData)
      {
         int index = 0;
         #region 内容设置

         #region 名 id 简介
         // 填充角色数据
         string value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) roleData.itemName = value; index++;

         // ID行
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (int.TryParse(value, out int roleID)) roleData.itemID = roleID; index++;

         // 拼音行
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) roleData.itemPinyin = value; index++;

         // 英文名
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) roleData.enName = value; index++;

         // 获取途径
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : "default";
         if (value.Equals("限定池")) roleData.inPoolType = 2;
         else if (value.Equals("常驻池")) roleData.inPoolType = 1;
         else roleData.inPoolType = 0;
         //Debug.Log(value + roleData.inPoolType);
         index++;

         // 简介
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) roleData.profile = value; index++;

         #endregion

         #region 职业 元素 稀有度

         // 定位职业
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) roleData.roleDefinition = value.StringToEnum<RoleSpecialty>(); index++;

         // 伤害元素
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) roleData.damageElement = value.StringToEnum<DamageElement>(); index++;

         index++; // 跳过一个字段（武器）

         // 稀有度
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (int.TryParse(value, out int rarity)) roleData.Rarity = rarity; index++;

         #endregion

         #region 文案信息
         // 出生地
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) roleData.birthplace = value; index++;

         // 势力 派系
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) roleData.onFactions = value; index++;

         // 小队
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) roleData.onTeam = value; index++;

         // 生日
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) roleData.birthday = value; index++;

         // 年龄
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (value.ExtractDigitsAndConvertToInt(out int age)) roleData.Age = age; index++;

         // 身高
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (value.ExtractDigitsAndDotAndMultiplyBy100(out int height)) roleData.height = height; index++;

         // 特长爱好
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) roleData.SpecialtyAndHobby = value; index++;

         index++; // 跳过一个字段（外号）

         // 配音
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) roleData.CV = value; index++;

         // 喜好菜式
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) roleData.favoriteCuisine = value;

         #endregion

         #region 图片素材
         // Keywords：角色相关的关键词
         string[] Keywords = new string[] {
               roleData.itemName,
               roleData.enName,
               roleData.itemPinyin,
               roleData.itemID.ToString(),
            };

         var middlePath = "ui_hero_pic/";
         roleData.sprite_listIcon = $"{middlePath}ListIcon".GetSpriteWithKeyword(Keywords);
         roleData.sprite_Lottery = $"{middlePath}Lottery".GetSpriteWithKeyword(Keywords);
         roleData.sprite_LotteryLeader = $"{middlePath}LotteryResult_leader".GetSpriteWithKeyword(Keywords);
         roleData.sprite_roleUI = $"{middlePath}Role".GetSpriteWithKeyword(Keywords);

         #endregion

         #endregion
      }
      #endregion

      #region 武器 weapon
      public static void Weapon_SetItemList(this WeaponItem_ListData itemData, DataRowCollection excelData, int columnNum, int rowNum)
      {
         itemData.allwep.RemoveEmptyListItem();

         for (int i = 1; i < rowNum; i++)
         {
            // 如果该行是空行，不计算
            if (excelData[i].IsEmptyRow(columnNum)) continue;
            if (excelData[i][0].ToString().ToLower().Equals("end")) break;

            var wepDataList = itemData.allwep;
            var excelRow = excelData[i];
            WeaponItem_Data wepData = null;

            // 查找武器是否已存在，如果存在，则更新数据
            for (int j = 0; j < wepDataList.Count; j++)
            {
               if (wepDataList[j].itemName.Equals(excelRow[0].ToString()))
               {
                  wepData = wepDataList[j];
                  break;
               }
            }

            // 如果武器不存在，则创建一个新的武器数据
            if (wepData == null)
            {
               wepData = ScriptableObject.CreateInstance<WeaponItem_Data>();
               Debug.Log(wepData);
               wepDataList.Add(wepData); // 添加新武器到列表
            }

            // 更新武器数据
            excelRow.Weapon_SetItemData(ref wepData);

            // 在编辑器中保存 ScriptableObject
            string assetPath = $"{ResPath}/WepItemDatas/{wepData.itemID}-{wepData.itemPinyin}.asset";
            if (!AssetDatabase.LoadAssetAtPath<WeaponItem_Data>(assetPath)) // 如果该资源不存在，则创建
            {
               AssetDatabase.CreateAsset(wepData, assetPath); // 保存武器数据为资源文件
            }
            else
            {
               // 标记为脏对象，表示该对象已被修改
               EditorUtility.SetDirty(wepData);
            }

            Debug.Log("保存更改");
            AssetDatabase.SaveAssets();  // 保存所有更改
         }
      }
      private static void Weapon_SetItemData(this DataRow excelRow, ref WeaponItem_Data wepData)
      {
         int index = 0;

         #region id 名称 拼音

         // 名字
         string value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) wepData.itemName = value; index++;

         // ID行
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (int.TryParse(value, out int wepID)) wepData.itemID = wepID; index++;

         // 拼音行
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) wepData.itemPinyin = value; index++;
         #endregion

         #region 稀有度 武器类型 子类
         // 稀有度
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (int.TryParse(value, out int rarity)) wepData.Rarity = rarity; index++;

         // 武器类型
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) wepData.weaponType = value.StringToEnum<WeaponType>(); index++;

         // 武器子类型
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) wepData.weaponSubType = value.StringToEnum<WeaponSubType>(); index++;
         #endregion

         // 获取途径
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : "";
         if (value.Equals("限定共鸣")) wepData.getType = 2;
         else if (value.Equals("通常共鸣")) wepData.getType = 1;
         else wepData.getType = 0;
         index++;


         // 旧未设置的获取途径
         index++;

         #region 介绍 技能

         // 介绍
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) wepData.profile = value; index++;

         // 技能
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) wepData.skillName = value; index++;

         // 技能描述
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) wepData.skillDescribe = value; index++;
         #endregion

         #region 基础数值

         // 技能描述
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         value.SetGroupIntValue(out wepData.damage); index++;

         // 技能描述
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         value.SetGroupIntValue(out wepData.gunshot); index++;

         // 技能描述
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         value.SetGroupFloatValue(out wepData.reload); index++;

         // 技能描述
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         value.SetGroupIntValue(out wepData.magazineSize); index++;

         // 技能描述
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         value.SetGroupIntValue(out wepData.firingRate); index++;

         // 技能描述
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         value.SetGroupIntValue(out wepData.stability); index++;


         #endregion

         #region 图片

         // Keywords：角色相关的关键词
         string[] Keywords = new string[] { wepData.itemID.ToString() };

         var middlePath = "ui_weapon_pic/";
         // 抽卡显示大图
         wepData.sprite_Square = $"{middlePath}ui weapon pic_texture".GetSpriteWithKeyword(Keywords);
         // 抽卡结算的竖条
         wepData.sprite_LotteryResult = $"{middlePath}LotteryResult_weapon".GetSpriteWithKeyword(Keywords);
         // 角色装配页的横图
         wepData.sprite_Line = $"{middlePath}ui weapon texture".GetSpriteWithKeyword(Keywords);

         #endregion
      }

      private static void SetGroupIntValue(this string value, out int output)
      {
         var values = value.Split('~');
          int.TryParse(values[0], out output);

      }
      private static void SetGroupFloatValue(this string value, out float output)
      {
         var values = value.Split('~');
         float.TryParse(values[0], out output);
      }

      #endregion

      #region Gacha Pool
      public static void Pool_SetItemList(this GachaPool_ListData itemData, DataRowCollection excelData, int columnNum, int rowNum)
      {
         itemData.allPool.RemoveEmptyListItem();
         for (int i = 1; i < rowNum; i++)
         {
            // 如果该行是空行，不计算
            if (excelData[i].IsEmptyRow(columnNum)) continue;

            var poolDataList = itemData.allPool;
            var excelRow = excelData[i];
            GachaPool_Data poolData = null;
            excelRow[2].ToString().ExtractDigitsAndConvertToInt(out var poolID);
            // 查找角色是否已存在，如果存在，则更新数据
            for (int j = 0; j < poolDataList.Count; j++)
            {
               if (poolDataList[j].itemID.Equals(poolID))
               {
                  poolData = poolDataList[j];
                  break;
               }
            }
            // 如果角色不存在，则创建一个新的角色数据
            poolData = poolData != null ? poolData : ScriptableObject.CreateInstance<GachaPool_Data>();

            excelRow.Pool_SetItemData(ref poolData);

            var rolelist = Thrud_ReadExcel.resPath.GetOrCreateDataAsset<RoleItem_ListData>("rolelist");
            var weplist = Thrud_ReadExcel.resPath.GetOrCreateDataAsset<WeaponItem_ListData>("rolelist");
            Pool_SetInItemList(ref poolData, rolelist.allRole, weplist.allwep);
            // 在编辑器中保存 ScriptableObject
            if (!poolDataList.Contains(poolData))
            {
               string assetPath =
                  $"{ResPath}/GachaPoolDatas/{poolData.itemID}.asset";

               AssetDatabase.CreateAsset(poolData, assetPath); // 保存角色数据为资源文件
               poolDataList.Add(poolData); // 如果角色数据不存在，则添加
            }
            else
            {
               // 标记为脏对象，表示该对象已被修改
               EditorUtility.SetDirty(poolData);
            }
            // 保存已获取的武器数据
            AssetDatabase.SaveAssets();  // 保存所有更改
         }
      }
      private static void Pool_SetItemData(this DataRow excelRow, ref GachaPool_Data poolData)
      {
         int index = 0;

         // 卡池名
         string value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : poolData.itemID.ToString();
         if (!string.IsNullOrEmpty(value)) poolData.itemName = value; index++;

         // 拼音
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : "未设置拼音";
         if (!string.IsNullOrEmpty(value)) poolData.itemPinyin = value; index++;

         // ID行
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : "999999";
         if (value.ExtractDigitsAndConvertToInt(out int poolID)) poolData.itemID = poolID;
         //index++;

         string[] Keywords = new string[] {
               poolData.itemID.ToString(),
            };

         var middlePath = "GachaResource/GachaUIResource/";

         // 抽卡显示大图
         poolData.sprite_PoolBg = $"{middlePath}gacha_pool_bg".GetSpriteWithKeyword(Keywords);
         poolData.sprite_PoolBtnBg = $"{middlePath}gacha_pool_btnbg".GetSpriteWithKeyword(Keywords);
      }

      private static void Pool_SetInItemList(ref GachaPool_Data poolData, List<RoleItem_Data> rolelist, List<WeaponItem_Data> weplist)
      {
         poolData.poolV5_ItemData.Clear();
         poolData.poolV4_ItemData.Clear();
         poolData.poolV3_ItemData.Clear();
         poolData.poolV2_ItemData.Clear();

         for (int i = 0; i < rolelist.Count; i++)
         {
            if (rolelist[i].Rarity == 5) { poolData.poolV5_ItemData.Add(rolelist[i]); }
            else if (rolelist[i].Rarity == 4) { poolData.poolV4_ItemData.Add(rolelist[i]); }
         }
         for (int i = 0; i < weplist.Count; i++)
         {
            if (weplist[i].Rarity == 5) { poolData.poolV5_ItemData.Add(weplist[i]); }
            else if (weplist[i].Rarity == 4) { poolData.poolV4_ItemData.Add(weplist[i]); }
            else if (weplist[i].Rarity == 3) { poolData.poolV3_ItemData.Add(weplist[i]); }
            else if (weplist[i].Rarity == 2) { poolData.poolV2_ItemData.Add(weplist[i]); }
         }
      }
      #endregion
   }
}