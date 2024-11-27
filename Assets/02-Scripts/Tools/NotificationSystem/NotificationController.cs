using System;
using System.Collections.Generic;
using UnityEngine;

namespace TeaFramework
{
   /// <summary>事件控制</summary>
   public static class NotificationController
   {
      #region key is string 字符串类型

      private static Dictionary<string, Delegate> m_EveryTableString = new Dictionary<string, Delegate>();

      #region 基类
      /// <summary> 添加监听 </summary>
      private static void OnAddSomeList(this string eventType, Delegate callBack)
      {
         //判断事件码是否存在，如果不存在就添加
         if (!m_EveryTableString.ContainsKey(eventType))
         {
            m_EveryTableString.Add(eventType, null);
         }
         //拿到m_EventTable所对应的委托
         Delegate d = m_EveryTableString[eventType];
         //判断要添加的委托与当前事件码对应的委托类型是否一致，一致才可以绑定
         if (d != null && d.GetType() != callBack.GetType())
         {
            throw new Exception(string.Format("尝试为事件码{0}添加不同类型的委托，" +
                "当前事件所对应的委托是{1}，要添加的委托类型是{2}", eventType, d.GetType(), callBack.GetType()));
         }
      }

      /// <summary> 移除监听前 </summary>
      private static void OnRemovingSomeList(this string eventType, Delegate callBack)
      {
         //判断事件码是否存在
         if (m_EveryTableString.ContainsKey(eventType))
         {
            Delegate d = m_EveryTableString[eventType];
            //当前事件码对应的委托是否为空，空无法移除
            if (d == null)
            {
               throw new Exception(string.Format("移除监听错误：事件{0}没有对应的委托", eventType));
            }
            else
            //判断要移除的委托与当前事件码对应的委托类型是否一致，一致才可以移除
            if (d.GetType() != callBack.GetType())
            {
               throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的委托，" +
                   "当前委托类型为{1}，要移除的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
            }
         }
         else
         {
            throw new Exception(string.Format("移除监听错误：没有事件码{0}", eventType));
         }
      }

      /// <summary> 移除监听后 </summary>
      private static void OnRemovedSomeList(this string eventType)
      {
         //判断当前事件码对应的委托是否为空，为空的话事件码无用，移除事件码
         if (m_EveryTableString[eventType] == null) { m_EveryTableString.Remove(eventType); }
      }
      #endregion

      #region Add
      /// <summary> 添加一个任意字符类的事件到列表 </summary>
      public static void OnAddAnotherList(this string keyValue, TeaEvent callBack)
      {
         OnAddSomeList(keyValue, callBack);
         m_EveryTableString[keyValue] = (TeaEvent)m_EveryTableString[keyValue] + callBack;
      }
      /// <summary> 添加一个任意字符类+1参数的事件到列表 </summary>
      public static void OnAddAnotherList<T>(this string keyValue, TeaEvent<T> callBack)
      {
         OnAddSomeList(keyValue, callBack);
         m_EveryTableString[keyValue] = (TeaEvent<T>)m_EveryTableString[keyValue] + callBack;
      }
      /// <summary> 添加一个任意字符类+2参数的事件到列表 </summary>
      public static void OnAddAnotherList<T, T2>(this string keyValue, TeaEvent<T, T2> callBack)
      {
         OnAddSomeList(keyValue, callBack);
         m_EveryTableString[keyValue] = (TeaEvent<T, T2>)m_EveryTableString[keyValue] + callBack;
      }
      /// <summary> 添加一个任意字符类+3参数的事件到列表 </summary>
      public static void OnAddAnotherList<T, T2, T3>(this string keyValue, TeaEvent<T, T2, T3> callBack)
      {
         OnAddSomeList(keyValue, callBack);
         m_EveryTableString[keyValue] = (TeaEvent<T, T2, T3>)m_EveryTableString[keyValue] + callBack;
      }

      #endregion

      #region Remove
      /// <summary> 移除一个任意字符类的事件到列表 </summary>
      public static void OnRemoveAnotherList(this string keyValue, TeaEvent callBack)
      {
         OnRemovingSomeList(keyValue, callBack);
         m_EveryTableString[keyValue] = (TeaEvent)m_EveryTableString[keyValue] - callBack;
         OnRemovedSomeList(keyValue);
      }
      /// <summary> 移除一个任意字符类+1参数的事件到列表 </summary>
      public static void OnRemoveAnotherList<T>(this string keyValue, TeaEvent<T> callBack)
      {
         OnRemovingSomeList(keyValue, callBack);
         m_EveryTableString[keyValue] = (TeaEvent<T>)m_EveryTableString[keyValue] - callBack;
         OnRemovedSomeList(keyValue);
      }
      /// <summary> 移除一个任意字符类+2参数的事件到列表 </summary>
      public static void OnRemoveAnotherList<T, T2>(this string keyValue, TeaEvent<T, T2> callBack)
      {
         OnRemovingSomeList(keyValue, callBack);
         m_EveryTableString[keyValue] = (TeaEvent<T, T2>)m_EveryTableString[keyValue] - callBack;
         OnRemovedSomeList(keyValue);
      }
      /// <summary> 移除一个任意字符类+3参数的事件到列表 </summary>
      public static void OnRemoveAnotherList<T, T2, T3>(this string keyValue, TeaEvent<T, T2, T3> callBack)
      {
         OnRemovingSomeList(keyValue, callBack);
         m_EveryTableString[keyValue] = (TeaEvent<T, T2, T3>)m_EveryTableString[keyValue] - callBack;
         OnRemovedSomeList(keyValue);
      }

      #endregion

      #region Invoke
      /// <summary> 执行一个字符串类型的委托事件 </summary>
      public static void InvokeSomething(this string keyValue)
      {
         if (m_EveryTableString.TryGetValue(keyValue, out Delegate baseDele))
         {
            if (baseDele is TeaEvent dEvent) { dEvent?.Invoke(); }
            else { throw new Exception(string.Format("广播事件错误错误：事件{0}对应的委托具有不同的类型", keyValue)); }
         }
      }
      /// <summary> 执行一个字符串类型的 1参 委托事件 </summary>
      public static void InvokeSomething<T>(this string keyValue, T arg)
      {
         if (m_EveryTableString.TryGetValue(keyValue, out Delegate baseDele))
         {
            if (baseDele is TeaEvent<T> dEvent)
            {
               try { dEvent?.Invoke(arg); }
               catch (Exception e) { Debug.LogError("事件执行失败" + e); throw; }
            }
            else { throw new Exception(string.Format("广播事件错误错误：事件{0}对应的委托具有不同的类型", keyValue)); }
         }
      }
      /// <summary> 执行一个字符串类型的 2参 委托事件 </summary>
      public static void InvokeSomething<T, T2>(this string keyValue, T arg, T2 arg2)
      {
         if (m_EveryTableString.TryGetValue(keyValue, out Delegate baseDele))
         {
            if (baseDele is TeaEvent<T, T2> dEvent)
            {
               try { dEvent?.Invoke(arg, arg2); }
               catch (Exception e) { Debug.LogError("事件执行失败" + e); throw; }
            }
            else { throw new Exception(string.Format("广播事件错误错误：事件{0}对应的委托具有不同的类型", keyValue)); }
         }
      }
      /// <summary> 执行一个字符串类型的 3参 委托事件 </summary>
      public static void InvokeSomething<T, T2, T3>(this string keyValue, T arg, T2 arg2, T3 arg3)
      {
         if (m_EveryTableString.TryGetValue(keyValue, out Delegate baseDele))
         {
            if (baseDele is TeaEvent<T, T2, T3> dEvent)
            {
               try { dEvent?.Invoke(arg, arg2, arg3); }
               catch (Exception e) { Debug.LogError("事件执行失败" + e); throw; }
            }
            else { throw new Exception(string.Format("广播事件错误错误：事件{0}对应的委托具有不同的类型", keyValue)); }
         }
      }
      #endregion

      #endregion

      #region key is enum 枚举型
      /// <summary>任意枚举类的事件 字典</summary>
      private static Dictionary<Enum, Delegate> m_EveryTableEnum = new Dictionary<Enum, Delegate>();

      #region 基类
      /// <summary> 添加监听 </summary>
      private static void OnAddSomeList<_Enum>(this _Enum eventType, Delegate callBack) where _Enum : Enum
      {
         //判断事件码是否存在，如果不存在就添加
         if (!m_EveryTableEnum.ContainsKey(eventType))
         {
            m_EveryTableEnum.Add(eventType, null);
         }
         //拿到m_EventTable所对应的委托
         Delegate d = m_EveryTableEnum[eventType];
         //判断要添加的委托与当前事件码对应的委托类型是否一致，一致才可以绑定
         if (d != null && d.GetType() != callBack.GetType())
         {
            throw new Exception(string.Format("尝试为事件码{0}添加不同类型的委托，" +
                "当前事件所对应的委托是{1}，要添加的委托类型是{2}", eventType, d.GetType(), callBack.GetType()));
         }
      }

      /// <summary> 移除监听前 </summary>
      private static void OnRemovingSomeList<_Enum>(this _Enum eventType, Delegate callBack) where _Enum : Enum
      {
         //判断事件码是否存在
         if (m_EveryTableEnum.ContainsKey(eventType))
         {
            Delegate d = m_EveryTableEnum[eventType];
            //当前事件码对应的委托是否为空，空无法移除
            if (d == null)
            {
               throw new Exception(string.Format("移除监听错误：事件{0}没有对应的委托", eventType));
            }
            else
            //判断要移除的委托与当前事件码对应的委托类型是否一致，一致才可以移除
            if (d.GetType() != callBack.GetType())
            {
               throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的委托，" +
                   "当前委托类型为{1}，要移除的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
            }
         }
         else
         {
            throw new Exception(string.Format("移除监听错误：没有事件码{0}", eventType));
         }
      }

      /// <summary> 移除监听后 </summary>
      private static void OnRemovedSomeList<_Enum>(this _Enum eventType) where _Enum : Enum
      {
         //判断当前事件码对应的委托是否为空，为空的话事件码无用，移除事件码
         if (m_EveryTableEnum[eventType] == null) { m_EveryTableEnum.Remove(eventType); }
      }
      #endregion

      #region Add
      /// <summary> 添加一个任意枚举类的事件到列表 </summary>
      public static void OnAddAnotherList<_Enum>(this _Enum someEnum, TeaEvent callBack) where _Enum : Enum
      {
         OnAddSomeList(someEnum, callBack);
         m_EveryTableEnum[someEnum] = (TeaEvent)m_EveryTableEnum[someEnum] + callBack;
      }
      /// <summary> 添加一个任意枚举类+1参数的事件到列表 </summary>
      public static void OnAddAnotherList<_Enum, T>(this _Enum someEnum, TeaEvent<T> callBack) where _Enum : Enum
      {
         OnAddSomeList(someEnum, callBack);
         m_EveryTableEnum[someEnum] = (TeaEvent<T>)m_EveryTableEnum[someEnum] + callBack;
      }
      /// <summary> 添加一个任意枚举类+2参数的事件到列表 </summary>
      public static void OnAddAnotherList<_Enum, T, T2>(this _Enum someEnum, TeaEvent<T, T2> callBack) where _Enum : Enum
      {
         OnAddSomeList(someEnum, callBack);
         m_EveryTableEnum[someEnum] = (TeaEvent<T, T2>)m_EveryTableEnum[someEnum] + callBack;
      }

      #endregion

      #region Remove
      /// <summary> 移除一个任意枚举类的事件到列表 </summary>
      public static void OnRemoveAnotherList<_Enum>(this _Enum someEnum, TeaEvent callBack) where _Enum : Enum
      {

         OnRemovingSomeList(someEnum, callBack);
         m_EveryTableEnum[someEnum] = (TeaEvent)m_EveryTableEnum[someEnum] - callBack;
         OnRemovedSomeList(someEnum);
      }
      /// <summary> 移除一个任意枚举类+1参数的事件到列表 </summary>
      public static void OnRemoveAnotherList<_Enum, T>(this _Enum someEnum, TeaEvent<T> callBack) where _Enum : Enum
      {
         OnRemovingSomeList(someEnum, callBack);
         m_EveryTableEnum[someEnum] = (TeaEvent<T>)m_EveryTableEnum[someEnum] - callBack;
         OnRemovedSomeList(someEnum);
      }
      /// <summary> 移除一个任意枚举类+2参数的事件到列表 </summary>
      public static void OnRemoveAnotherList<_Enum, T, T2>(this _Enum someEnum, TeaEvent<T, T2> callBack) where _Enum : Enum
      {
         OnRemovingSomeList(someEnum, callBack);
         m_EveryTableEnum[someEnum] = (TeaEvent<T, T2>)m_EveryTableEnum[someEnum] - callBack;
         OnRemovedSomeList(someEnum);
      }

      #endregion

      #region Invoke
      /// <summary> 执行一个委托事件 </summary>
      public static void InvokeSomething<_Enum>(this _Enum someEnum) where _Enum : Enum
      {
         if (m_EveryTableEnum.TryGetValue(someEnum, out Delegate baseDele))
         {
            if (baseDele is TeaEvent dEvent) { dEvent?.Invoke(); }
            else { throw new Exception(string.Format("广播事件错误错误：事件{0}对应的委托具有不同的类型", someEnum)); }
         }
      }
      /// <summary> 执行一个 1参 委托事件 </summary>
      public static void InvokeSomething<_Enum, T>(this _Enum someEnum, T arg) where _Enum : Enum
      {
         if (m_EveryTableEnum.TryGetValue(someEnum, out Delegate baseDele))
         {
            if (baseDele is TeaEvent<T> dEvent)
            {
               try { dEvent?.Invoke(arg); }
               catch (Exception) { Debug.Log("事件执行失败"); throw; }
            }
            else { throw new Exception(string.Format("广播事件错误错误：事件{0}对应的委托具有不同的类型", someEnum)); }
         }
      }
      /// <summary> 执行一个 2参 委托事件 </summary>
      public static void InvokeSomething<_Enum, T, T2>(this _Enum someEnum, T arg, T2 arg2) where _Enum : Enum
      {
         if (m_EveryTableEnum.TryGetValue(someEnum, out Delegate baseDele))
         {
            if (baseDele is TeaEvent<T, T2> dEvent)
            {
               try { dEvent?.Invoke(arg, arg2); }
               catch (Exception) { Debug.Log("事件执行失败"); throw; }
            }
            else { throw new Exception(string.Format("广播事件错误错误：事件{0}对应的委托具有不同的类型", someEnum)); }
         }
      }
      #endregion

      #endregion
   }
}
