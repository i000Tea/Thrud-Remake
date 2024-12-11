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
      public const string resPath = "Assets/Data";
      public const string file = "/斯露德重制计划-数据留存.xlsx";

      #region 读取表格设置角色

      [MenuItem("Thrud/读取表格设置/0.一键设置所有")]
      public static void ReadExcelGetAllData()
      {
         ReadExcelGetThrudData();
         ReadExcelGetWeaponData();
         ReadExcelGetPoolData();
      }

      [MenuItem("Thrud/读取表格设置/1.设置【斯露德】信息")]
      public static void ReadExcelGetThrudData()
      {
         // 获取so文件
         var getData = resPath.GetOrCreateDataAsset<RoleItem_ListData>("AllRoleItem_Data");

         // 调用数据填充方法
         getData.FromExcelSetData(file, 0);
      }

      [MenuItem("Thrud/读取表格设置/2.设置【武器】信息")]
      public static void ReadExcelGetWeaponData()
      {
         var getData = resPath.GetOrCreateDataAsset<WeaponItem_ListData>("AllWeaponItem_Data");
         // 调用数据填充方法
         getData.FromExcelSetData(file, 2);
      }

      [MenuItem("Thrud/读取表格设置/3.设置【卡池】信息")]
      public static void ReadExcelGetPoolData()
      {
         var getData = resPath.GetOrCreateDataAsset<GachaPool_ListData>("AllPoolItem_Data");
         // 调用数据填充方法
         getData.FromExcelSetData(file, 4);
      }

      [MenuItem("Thrud/读取表格设置/4.设置【道具】信息")]
      public static void ReadExcelGetPropData()
      {
         Debug.Log("暂未设置");
      }

      #endregion
   }

   public static class Thrud_ReadExcel_StaticMethod
   {
      private static readonly Dictionary<string, Sprite[]> SpriteListDic = new();

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
         string[] datas = AssetDatabase.FindAssets($"t:{typeof(T)}", new[] { resPath });
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

         // 通过不同的类判断使用方法
         if (typeof(T) == typeof(RoleItem_ListData))
         {
            (itemData as RoleItem_ListData).Thrud_SetItemList(excelData, columnNum, rowNum);
         }
         else if (typeof(T) == typeof(WeaponItem_ListData))
         {
            (itemData as WeaponItem_ListData).Weapon_SetItemList(excelData, columnNum, rowNum);
         }
         else if (typeof(T) == typeof(GachaPool_ListData))
         {
            (itemData as GachaPool_ListData).Pool_SetItemList(excelData, columnNum, rowNum);
         }
         else if (typeof(T) == typeof(PropItem_ListData))
         {

         }

         // 刷新缓存，避免重复加载
         SpriteListDic.Clear(); // 清空缓存，防止之前的缓存影响新的加载
         AssetDatabase.SaveAssets(); // 保存修改
         EditorUtility.SetDirty(itemData); // 标记 newData 为脏数据，保存修改
      }

      #region 通用方法

      #region 图片获取
      /// <summary>
      /// 获取具有特定关键词的图片
      /// </summary>
      /// <param name="middlePath"></param>
      /// <param name="Keywords"></param>
      /// <returns></returns>
      public static Sprite GetSpriteWithKeyword(this string endPath, string[] Keywords)
      {
         if (Keywords == null || Keywords.Length == 0)
         {
            throw new ArgumentException("Keywords cannot be null or empty", nameof(Keywords));
         }

         if (!SpriteListDic.TryGetValue(endPath, out Sprite[] Sprites))
         {
            Sprites = endPath.LoadFirstSpritesFromTextures().ToArray();
            SpriteListDic.Add(endPath, Sprites);
         }

         foreach (var sprite in Sprites)
         {
            if (Keywords.Any(keyword => sprite.name.Contains(keyword)))
            {
               return sprite;
            }
         }

         for (int i = 0; i < Sprites.Length; i++)
         {
            // Debug.Log(Sprites[i].name + " " + Keywords[0]);
         }

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
         Debug.Log($"加载的sprites图总数：{sprites.Count}\n{folder}");


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
