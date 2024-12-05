using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Tea
{
   /// <summary>
   /// 配置文件
   /// </summary>
   [CreateAssetMenu(fileName = "新的按钮配置", menuName = "按钮配置")]
   /// <summary>
   /// 配置文件
   /// </summary>
   public class ButtonControlSetting : ScriptableObject
   {
      #region 点击
      [Header("Click  点击")]

      public bool CanClick;
      public bool CanClickAnim;
      /// <summary>
      /// 点击 动画缩放倍率
      /// </summary>
      public float click_Scale = 0.6f;
      /// <summary>
      /// 点击 动画缩放时间
      /// </summary>
      public float click_Time = 0.1f;

      public bool click_UseColor;
      public Color click_Color;
      /// <summary>
      /// 点击 动画缩放时间
      /// </summary>
      public float clickBack_Time = 0.25f;
      #endregion

      #region 进入离开

      [Space(10)]
      public bool CanEnter;
      public bool CanEnterAnim;
      /// <summary>
      /// 进入 动画缩放时间
      /// </summary>
      public float enter_Scale = 1.5f;
      /// <summary>
      /// 进入 动画缩放时间
      /// </summary>
      public float enter_Time = 1;
      public bool enter_UseColor;
      public Color enter_Color;

      public float enterBack_Time = 1;
      #endregion

      #region 拖拽

      /// <summary>
      /// 可以被拖拽
      /// </summary>
      [Header("Draw   拖拽")]
      public bool CanDraw;
      /// <summary>
      /// 拖拽时鼠标偏移
      /// </summary>
      public bool DrawMouseIsOffset;
      public bool EndDragBack = true;
      [Range(0.01f, 1)]
      /// <summary>
      /// 进入动画缩放时间
      /// </summary>
      public float draw_Speed = 1;

      public float drawBack_Speed = 1;
      #endregion

#if UNITY_EDITOR
      /// <summary>
      /// 查找配置文件
      /// </summary>
      public static ButtonControlSetting FindSetting()
      {
         ButtonControlSetting newBCS;
         // 查找设置文件 若没有 则生成一个
         try
         {
            //Get settings
            newBCS = (ButtonControlSetting)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:ButtonControllerSetting")[0]), typeof(ButtonControlSetting));
         }
         catch
         {
            //If no settings file, create and assign
            // 新建配置文件
            ButtonControlSetting created = CreateInstance<ButtonControlSetting>();
            // 本地生成配置文件
            AssetDatabase.CreateAsset(created, "Assets/Resources/New ButtonControlSetting.asset");
            // 当前窗口的配置文件为刚刚生成的那一个
            newBCS = AssetDatabase.LoadAssetAtPath<ButtonControlSetting>("Assets/Resources/New ButtonControlSetting.asset"); //CreateAsset doesn't return anything :(
         }
         return newBCS;
      }
#endif
   }
}
