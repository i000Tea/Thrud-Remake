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
      // 设置本地存储路径
      public const string path = "Assets/Resources/data";
      public const string folderPath = "Assets/05-Textures/ui_hero_pic/ListIcon"; // 你的图片文件夹路径
      public string[] guids = AssetDatabase.FindAssets("t:texture", new[] { folderPath });

      #region 读取表格设置角色

      [MenuItem("Thrud/Excel/读取表格设置斯露德信息")]
      public static void ReadExcelGetThrudData()
      {
         // 获取指定路径下所有的 ScriptableObject 文件
         string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { path });
         AllRoleItem_Data newData = null;

         if (guids.Length == 0)
         {
            // 如果没有找到任何文件，则创建一个新的 AllRoleItem_Data 对象
            newData = ScriptableObject.CreateInstance<AllRoleItem_Data>();
            string filePath = Path.Combine(path, "AllRoleItem_Data.asset");  // 可以根据实际需求修改文件名
            AssetDatabase.CreateAsset(newData, filePath);
            AssetDatabase.SaveAssets();

            Debug.Log($"未找到文件，已创建新的 {filePath}！");
         }
         else
         {
            // 如果找到了文件，则加载第一个找到的文件
            string firstFoundPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            newData = AssetDatabase.LoadAssetAtPath<AllRoleItem_Data>(firstFoundPath);
            Debug.Log($"找到第一个文件：{firstFoundPath}");
         }

         // 调用数据填充方法
         newData.FromExcelSetData();
         EditorUtility.SetDirty(newData); // 标记 newData 为脏数据，保存修改
         AssetDatabase.SaveAssets(); // 保存修改
      }

      #endregion
   }

   public static class Thrud_ReadExcel_StaticMethod
   {
      public static void FromExcelSetData(this AllRoleItem_Data newData)
      {
         var excelData = ReadExcel(Thrud_ReadExcel.path, out int columnNum, out int rowNum, 0);
         Debug.Log($"{columnNum} {rowNum}");


         for (int i = 1; i < rowNum; i++)
         {
            // 如果该行是空行，不计算
            if (IsEmptyRow(excelData[i], columnNum)) continue;
            newData.allRole = excelData[i].SetDataRow(newData.allRole); // 更新 allRole 数据
         }

         newData.allRole.FindSpriteToData();
      }

      public static List<RoleItem_Data> SetDataRow(this DataRow excelRow, List<RoleItem_Data> roleDataList)
      {
         RoleItem_Data roleData = null;

         // 查找角色是否已存在，如果存在，则更新数据
         for (int i = 0; i < roleDataList.Count; i++)
         {
            if (roleDataList[i].roleName.Equals(excelRow[0].ToString()))
            {
               roleData = roleDataList[i];
               break;
            }
         }

         // 如果角色不存在，则创建一个新的角色数据
         roleData ??= ScriptableObject.CreateInstance<RoleItem_Data>();

         int index = 0;

         #region 内容设置
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

         // 在编辑器中保存 ScriptableObject
         if (!roleDataList.Contains(roleData))
         {
            string assetPath =
               $"Assets/Resources/Data/RoleItemDatas/{roleData.roleID}-{(roleData.enName == "未设置" ? roleData.rolePinyin : roleData.enName)}.asset";

            AssetDatabase.CreateAsset(roleData, assetPath); // 保存角色数据为资源文件
            AssetDatabase.SaveAssets();
            roleDataList.Add(roleData); // 如果角色数据不存在，则添加
         }

         return roleDataList;
      }

      private static List<RoleItem_Data> FindSpriteToData(this List<RoleItem_Data> roleDataList)
      {
         // 加载Sprite数组
         var ListIcon = "ListIcon".LoadFirstSpritesFromTextures();
         var Lottery = "Lottery".LoadFirstSpritesFromTextures();
         var LotteryResult_leader = "LotteryResult_leader".LoadFirstSpritesFromTextures();
         var Role = "Role".LoadFirstSpritesFromTextures();

         for (int i = 0; i < roleDataList.Count; i++)
         {
            var getData = roleDataList[i];

            // Keywords：角色相关的关键词
            string[] Keywords = new string[] {
               getData.roleName,
               getData.enName,
               getData.rolePinyin,
               getData.roleID.ToString(),
            };

            // 初始化Sprite
            getData.sprite_listIcon = FindMatchingSprite(Keywords, ListIcon);
            getData.sprite_Lottery = FindMatchingSprite(Keywords, Lottery);
            getData.sprite_LotteryLeader = FindMatchingSprite(Keywords, LotteryResult_leader);
            getData.sprite_roleUI = FindMatchingSprite(Keywords, Role);
         }

         return roleDataList;
      }

      private static Sprite FindMatchingSprite(string[] Keywords, Sprite[] sprites)
      {
         foreach (var sprite in sprites)
         {
            if (Keywords.Any(keyword => sprite.name.Contains(keyword)))
            {
               return sprite;
            }
         }
         return default; // 如果没有找到匹配的Sprite，返回默认值
      }


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

      /// <summary>
      /// 读取excel文件内容获取行数 列数 方便保存
      /// </summary>
      /// <param name="filePath">文件路径</param>
      /// <param name="columnNum">行数</param>
      /// <param name="rowNum">列数</param>
      /// <returns></returns>
      public static DataRowCollection ReadExcel(string filePath, out int columnNum, out int rowNum, int sheet)
      {
         FileStream stream = File.Open(filePath + "/斯露德重制计划-数据留存.xlsx", FileMode.Open, FileAccess.Read, FileShare.Read);
         IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

         DataSet result = excelReader.AsDataSet();
         //Tables[0] 下标0表示excel文件中第一张表的数据
         columnNum = result.Tables[sheet].Columns.Count;
         rowNum = result.Tables[sheet].Rows.Count;
         return result.Tables[sheet].Rows;
      }

      // 判断是否是空行
      public static bool IsEmptyRow(this DataRow collect, int columnNum)
      {
         for (int i = 0; i < columnNum; i++) { if (!collect.IsNull(i)) return false; }
         return true;
      }


      public static Sprite[] LoadFirstSpritesFromTextures(this string folder)
      {
         string folderPath = "Assets/05-Textures/ui_hero_pic/" + folder; // 图片文件夹路径
         string[] guids = AssetDatabase.FindAssets("t:texture", new[] { folderPath });

         List<Sprite> sprites = new List<Sprite>();

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
         return sprites.ToArray();
      }
   }
}
