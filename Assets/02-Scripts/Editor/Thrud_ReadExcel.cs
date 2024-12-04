using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace TeaFramework.editor
{
   public class Thrud_ReadExcel : Editor
   {
      public const string resPath = "Assets/Resources/Data";

      #region 读取表格设置角色

      [MenuItem("Thrud/读取表格设置/设置【斯露德】信息")]
      public static void ReadExcelGetThrudData()
      {
         var getData = resPath.GetOrCreateDataAsset<RoleItem_ListData>("AllRoleItem_Data");

         // 调用数据填充方法
         getData.FromExcelSetData("/斯露德重制计划-数据留存.xlsx", 0);

         EditorUtility.SetDirty(getData); // 标记 newData 为脏数据，保存修改
         AssetDatabase.SaveAssets(); // 保存修改
      }

      [MenuItem("Thrud/读取表格设置/设置【武器】信息")]
      public static void ReadExcelGetWeaponData()
      {
         var getData = resPath.GetOrCreateDataAsset<WeaponItem_ListData>("AllWeaponItem_Data");

         // 调用数据填充方法
         getData.FromExcelSetData("/斯露德重制计划-数据留存.xlsx", 4);

         EditorUtility.SetDirty(getData); // 标记 newData 为脏数据，保存修改
         AssetDatabase.SaveAssets(); // 保存修改

      }
      [MenuItem("Thrud/读取表格设置/设置【卡池】信息")]
      public static void ReadExcelGetPoolData() { }

      [MenuItem("Thrud/读取表格设置/设置【道具】信息")]
      public static void ReadExcelGetPropData()
      {
         Debug.Log("暂未设置");
      }

      #endregion
   }

   public static class Thrud_ReadExcel_StaticMethod
   {
      private static Dictionary<string, Sprite[]> SpriteListDic = new();

      public static string ResPath => Thrud_ReadExcel.resPath;

      /// <summary>
      /// 获取指定so文件
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="resPath"></param>
      /// <param name="newFileName"></param>
      /// <returns></returns>
      public static T GetOrCreateDataAsset<T>(this string resPath, string newFileName) where T : Base_AllItemData
      {
         // 获取指定路径下所有的 ScriptableObject 文件
         string[] datas = AssetDatabase.FindAssets("t:ScriptableObject", new[] { resPath });
         T getData;

         if (datas.Length == 0)
         {
            // 如果没有找到任何文件，则创建一个新的 AllRoleItem_Data 对象
            getData = ScriptableObject.CreateInstance<T>();
            string filePath = Path.Combine(resPath, $"{newFileName}.asset");  // 可以根据实际需求修改文件名
            AssetDatabase.CreateAsset(getData, filePath);
            AssetDatabase.SaveAssets();

            Debug.Log($"未找到文件，已创建新的 {filePath}！");
         }
         else
         {
            // 如果找到了文件，则加载第一个找到的文件
            string firstFoundPath = AssetDatabase.GUIDToAssetPath(datas[0]);
            getData = AssetDatabase.LoadAssetAtPath<T>(firstFoundPath);
            Debug.Log($"找到第一个文件：{firstFoundPath}");
         }

         return getData;
      }

      /// <summary>
      /// 获取表格转换为的二维数组
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="itemData"></param>
      /// <param name="path"></param>
      /// <param name="page"></param>
      public static void FromExcelSetData<T>(this T itemData, string path, int page) where T : Base_AllItemData
      {
         // 获取指定路径下的excel中获取的二维数组内容
         var excelData = ReadExcel(path, out int columnNum, out int rowNum, page);
         Debug.Log($"{columnNum} {rowNum}");

         if (typeof(T) == typeof(RoleItem_ListData))
         {
            // 现在可以安全地使用 roleItemData 进行操作

            Thrud_SetItemList(itemData as RoleItem_ListData, excelData, columnNum, rowNum);
         }
         else if (typeof(T) == typeof(WeaponItem_ListData))
         {
            Weapon_SetItemList(itemData as WeaponItem_ListData, excelData, columnNum, rowNum);
         }
         else if (typeof(T) == typeof(WeaponItem_ListData))
         {

         }
         else if (typeof(T) == typeof(WeaponItem_ListData))
         {

         }
      }

      #region 角色 Thrud
      public static void Thrud_SetItemList(RoleItem_ListData itemData, DataRowCollection excelData, int columnNum, int rowNum)
      {
         itemData.allRole.RemoveEmptyListItem();

         for (int i = 1; i < rowNum; i++)
         {
            // 如果该行是空行，不计算
            if (IsEmptyRow(excelData[i], columnNum)) continue;

            var roleDataList = itemData.allRole;
            var excelRow = excelData[i];
            RoleItem_Data roleData = null;
            // 查找角色是否已存在，如果存在，则更新数据
            for (int j = 0; j < roleDataList.Count; j++)
            {
               if (roleDataList[j].roleName.Equals(excelRow[0].ToString()))
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
                  $"{ResPath}/RoleItemDatas/{roleData.roleID}-{(roleData.enName == "未设置" ? roleData.rolePinyin : roleData.enName)}.asset";

               AssetDatabase.CreateAsset(roleData, assetPath); // 保存角色数据为资源文件
               AssetDatabase.SaveAssets();
               roleDataList.Add(roleData); // 如果角色数据不存在，则添加
            }
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
         if (!string.IsNullOrEmpty(value)) roleData.roleName = value; index++;

         // ID行
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (int.TryParse(value, out int roleID)) roleData.roleID = roleID; index++;

         // 拼音行
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) roleData.rolePinyin = value; index++;

         // 英文名
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) roleData.enName = value; index++;

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
         Debug.Log($"{roleData.roleName} {roleData.birthday}");

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
               roleData.roleName,
               roleData.enName,
               roleData.rolePinyin,
               roleData.roleID.ToString(),
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
      private static void Weapon_SetItemList(WeaponItem_ListData itemData, DataRowCollection excelData, int columnNum, int rowNum)
      {
         itemData.allwep.RemoveEmptyListItem();

         for (int i = 1; i < rowNum; i++)
         {
            // 如果该行是空行，不计算
            if (IsEmptyRow(excelData[i], columnNum)) continue;

            var wepDataList = itemData.allwep;
            var excelRow = excelData[i];
            WeaponItem_Data wepData = null;
            // 查找角色是否已存在，如果存在，则更新数据
            for (int j = 0; j < wepDataList.Count; j++)
            {
               if (wepDataList[j].wepName.Equals(excelRow[0].ToString()))
               {
                  wepData = wepDataList[j];
                  break;
               }
            }
            // 如果武器不存在，则创建一个新的角色数据
            wepData = wepData != null ? wepData : ScriptableObject.CreateInstance<WeaponItem_Data>();

            excelRow.Weapon_SetItemData(ref wepData);

            // 在编辑器中保存 ScriptableObject
            if (!wepDataList.Contains(wepData))
            {
               string assetPath =
                  $"{ResPath}/RoleItemDatas/wep-{wepData.wepID}.asset";

               AssetDatabase.CreateAsset(wepData, assetPath); // 保存角色数据为资源文件
               AssetDatabase.SaveAssets();
               wepDataList.Add(wepData); // 如果角色数据不存在，则添加
            }
         }
      }
      private static void Weapon_SetItemData(this DataRow excelRow, ref WeaponItem_Data wepData)
      {
         int index = 0;

         // ID行
         string value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (int.TryParse(value, out int wepID)) wepData.wepID = wepID; index++;

         // 名字
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) wepData.wepName = value; index++;

         // 拼音行
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) wepData.wepPinyin = value; index++;

         // 稀有度
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (int.TryParse(value, out int rarity)) wepData.Rarity = rarity; index++;

         // 武器类型
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) wepData.weaponType = value.StringToEnum<WeaponType>(); index++;

         // 武器子类型
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) wepData.weaponSubType = value.StringToEnum<WeaponSubType>(); index++;

         // 获取途径
         index++;

         // 介绍
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) wepData.profile = value; index++;

         // 技能
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) wepData.skillName = value; index++;

         // 技能描述
         value = excelRow[index] != DBNull.Value ? excelRow[index].ToString() : null;
         if (!string.IsNullOrEmpty(value)) wepData.skillDescribe = value; 
         //index++;

      }
      #endregion

      #region 通用方法

      #region 图片获取
      /// <summary>
      /// 获取具有特定关键词的图片
      /// </summary>
      /// <param name="middlePath"></param>
      /// <param name="Keywords"></param>
      /// <returns></returns>
      private static Sprite GetSpriteWithKeyword(this string endPath, string[] Keywords)
      {
         if (!SpriteListDic.TryGetValue(endPath, out Sprite[] Sprites))
         {
            Sprites = endPath.LoadFirstSpritesFromTextures().ToArray();
            SpriteListDic.Add(endPath, Sprites);
         }
         foreach (var sprite in Sprites) { if (Keywords.Any(keyword => sprite.name.Contains(keyword))) { return sprite; } }
         return default; // 如果没有找到匹配的Sprite，返回默认值
      }

      /// <summary>
      /// 加载一个路径文件夹下 所有图片素材中的第一个Sprite图
      /// </summary>
      /// <param name="folder"></param>
      /// <returns></returns>
      private static List<Sprite> LoadFirstSpritesFromTextures(this string folder)
      {
         string folderPath = "Assets/05-Textures/" + folder; // 图片文件夹路径
         string[] guids = AssetDatabase.FindAssets("t:texture", new[] { folderPath });

         List<Sprite> sprites = new();

         foreach (string guid in guids)
         {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);

            if (texture != null)
            {
               // 如果纹理是图集，则获取图集中的第一个Sprite
               if (texture.mipmapCount > 1 || texture.filterMode != FilterMode.Point)
               {
                  // 获取该纹理下的所有Sprite
                  string[] spriteGuids = AssetDatabase.FindAssets("t:sprite", new[] { folderPath });

                  foreach (string spriteGuid in spriteGuids)
                  {
                     string spriteAssetPath = AssetDatabase.GUIDToAssetPath(spriteGuid);
                     Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spriteAssetPath);

                     if (sprite != null && sprite.texture == texture)
                     {
                        // 将第一个Sprite加入列表并跳出循环
                        sprites.Add(sprite);
                        break;
                     }
                  }
               }
               else
               {
                  // 如果是单一的Texture2D，创建一个Sprite并添加到列表
                  Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                  sprites.Add(sprite);
               }
            }
         }

         Debug.Log("Total sprites loaded: " + sprites.Count);

         // 返回所有Sprite作为Sprite[]数组
         return sprites;
      }

      #endregion

      /// <summary>
      /// 删除列表中的空项
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="newDatas"></param>
      public static void RemoveEmptyListItem<T>(this List<T> newDatas) where T : ScriptableObject
      {
         for (int i = newDatas.Count - 1; i >= 0; i--) if (newDatas[i] == null) newDatas.RemoveAt(i);
      }

      #region 数值转置

      /// <summary>
      /// 从字符串中提取数字并转换为 int 类型。
      /// </summary>
      /// <param name="input">输入字符串</param>
      /// <returns>返回转换后的 int，如果没有找到数字则返回 0</returns>
      public static bool ExtractDigitsAndConvertToInt(this string input, out int value)
      {
         value = 0;
         // 使用正则表达式提取字符串中的数字
         string digits = Regex.Replace(input, @"\D", ""); // \D 表示非数字字符，替换为 ""

         // 如果找到了数字，则将其转换为 int，否则返回 0
         if (!string.IsNullOrEmpty(digits))
         {
            value = int.Parse(digits);
            return true;
         }

         return false; // 如果没有数字，返回 0
      }

      /// <summary>
      /// 从字符串中提取数字和小数点，转换为 float 类型并乘以 100。
      /// </summary>
      /// <param name="input">输入字符串</param>
      /// <returns>返回转换后的 float 值，如果没有找到有效数字则返回 0</returns>
      public static bool ExtractDigitsAndDotAndMultiplyBy100(this string input, out int value)
      {
         value = 0;
         // 使用正则表达式提取数字和小数点
         string result = Regex.Replace(input, @"[^0-9.]", "");  // 保留数字和小数点

         // 如果提取结果非空且有效数字格式
         if (!string.IsNullOrEmpty(result))
         {
            // 尝试将提取的字符串转换为 float
            if (float.TryParse(result, out float number))
            {
               value = (int)(number * 100);
               return true; // 返回乘以 100 后的结果
            }
         }

         return false; // 如果没有找到有效数字，则返回 0
      }

      #endregion

      #region 表格相关
      /// <summary>
      /// 读取excel文件内容获取行数 列数 方便保存
      /// </summary>
      /// <param name="filePath">文件路径</param>
      /// <param name="columnNum">行数</param>
      /// <param name="rowNum">列数</param>
      /// <returns></returns>
      public static DataRowCollection ReadExcel(this string filePath, out int columnNum, out int rowNum, int sheet)
      {
         FileStream stream = File.Open(ResPath + filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
         IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

         DataSet result = excelReader.AsDataSet();
         //Tables[0] 下标0表示excel文件中第一张表的数据
         columnNum = result.Tables[sheet].Columns.Count;
         rowNum = result.Tables[sheet].Rows.Count;
         return result.Tables[sheet].Rows;
      }

      /// <summary>
      /// 判断是否是空行
      /// </summary>
      /// <param name="collect"></param>
      /// <param name="columnNum"></param>
      /// <returns></returns>
      public static bool IsEmptyRow(this DataRow collect, int columnNum)
      {
         for (int i = 0; i < columnNum; i++) { if (!collect.IsNull(i)) return false; }
         return true;
      }

      #endregion

      #endregion
   }
}
