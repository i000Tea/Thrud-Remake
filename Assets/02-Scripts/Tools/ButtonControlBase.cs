using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering;

namespace Tea
{
   /// <summary>
   /// 自定义的按钮基类(工程需要加载dotween) 目前有拖拽和点击两种事件
   /// </summary>
   public class ButtonControlBase : MonoBehaviour,
       IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler,
       IBeginDragHandler, IDragHandler, IEndDragHandler
   {
      #region Variable    变量
      public UnityEvent clickEvent;
      [SerializeField]
      private ButtonControlSetting m_Setting;
      private float awakeScale;

      [SerializeField]
      protected bool isDebug = false;

      /// <summary>
      /// 鼠标是否在自身身上
      /// </summary>
      protected bool mouseIsIn = false;
      /// <summary>
      /// 是否处于拖拽中
      /// </summary>
      protected bool drawing = false;
      #region Draw 拖拽移动
      /// <summary>
      /// 父集位置
      /// </summary>
      public RectTransform ParentRectTrans => transform.parent as RectTransform;
      /// <summary>
      /// 自身坐标位置
      /// </summary>
      protected RectTransform thisRectTrans => transform as RectTransform;
      /// <summary>
      /// 拖拽偏移
      /// </summary>
      protected Vector2 dragOffsetPoint;

      /// <summary>
      /// 被拖拽的物体
      /// </summary>
      protected virtual Transform DragTrans
      {
         get
         {
            // 当拖拽对象为空 
            if (dragObj == null) { dragObj = thisRectTrans; }
            return dragObj;
         }
      }
      protected RectTransform DragObjRect => dragObj as RectTransform;

      /// <summary>
      /// 被拖拽的某物体(手动配置)
      /// </summary>
      [SerializeField]
      protected Transform dragObj;
      private float dragTime;
      /// <summary>
      /// 初始坐标
      /// </summary>
      protected Vector3 awakePosition;
      /// <summary>
      /// 转换父集后的初始坐标
      /// </summary>
      protected Vector3 awakeNewPosition;

      /// <summary>
      /// 鼠标当前位置
      /// </summary>
      /// <param name="eventData"></param>
      /// <returns></returns>
      protected Vector3 MouseNowPosition(PointerEventData eventData)
      {
         //新建v2
         Vector2 pos;
         // 如果点击 RectTransform 平面，则无论点是否在矩形内，都返回 true。
         if (RectTransformUtility.ScreenPointToLocalPointInRectangle(ParentRectTrans,
             eventData.position,
             eventData.pressEventCamera,
             out pos))
         {
            // 当前移动位置为移动后的值
            movePosition = pos;
            // 若可以移动 则自身坐标为移动后的值
            //if (m_Setting.CanDraw)
            //{
            //   DragTrans.localPosition = pos;
            //}
            return pos;
         }
         else { return default; }
      }
      /// <summary>
      /// 鼠标的当前移动位置
      /// </summary>
      protected Vector3 movePosition;
      #endregion

      Tween inAnim;
      #endregion

      #region Unity void 基础方法

      private void OnValidate()
      {
         //if (!m_Setting)
         //{
         //    m_Setting = ButtonControlSetting.FindSetting();
         //}
      }

      protected virtual void Awake()
      {
         AwakeSet();
         awakeScale = transform.localScale.x;
      }
      #endregion

      /// <summary>
      /// 初始化设置
      /// </summary>
      protected virtual void AwakeSet() { }

      #region 鼠标触碰 点击
      /// <summary>
      /// 鼠标进入
      /// </summary>
      /// <param name="eventData"></param>
      public void OnPointerEnter(PointerEventData eventData)
      {
         if (isDebug)
            Debug.Log("PointerEnter");
         OnEnter();
         if (m_Setting.CanEnter)
         {
            //Debug.Log("鼠标进入");
            if (m_Setting.CanEnterAnim)
            {
               inAnim = OnScale(m_Setting.enter_Scale, m_Setting.enter_Time);
            }
            mouseIsIn = true;
         }
      }
      /// <summary>
      /// 鼠标离开
      /// </summary>
      /// <param name="eventData"></param>
      public void OnPointerExit(PointerEventData eventData)
      {
         if (isDebug)
            Debug.Log("PointerExit");
         OnExit();
         if (m_Setting.CanEnter &&
             // 当不处于拖拽中 或 被拖拽的物体不是自身
             (!drawing || DragTrans != thisRectTrans))
         {
            //Debug.Log("鼠标离开"); 
            if (m_Setting.CanEnterAnim)
            {
               if (inAnim != null)
               {
                  inAnim.Kill();
                  inAnim = null;
               }
               OnScale(1f, m_Setting.enterBack_Time);
            }
            mouseIsIn = false;
         }
      }

      /// <summary>
      /// 鼠标按下
      /// </summary>
      /// <param name="eventData"></param>
      public void OnPointerDown(PointerEventData eventData)
      {
         if (isDebug)
            Debug.Log("PointerDown");
         if (m_Setting.CanClick)
         {
            //Debug.Log("按下鼠标");
            if (m_Setting.CanClickAnim)
            {
               OnScale(m_Setting.click_Scale, m_Setting.click_Time);
            }
         }
      }
      /// <summary>
      /// 鼠标抬起
      /// </summary>
      /// <param name="eventData"></param>
      public void OnPointerUp(PointerEventData eventData)
      {
         if (m_Setting.CanClick)
         {
            //Debug.Log("抬起鼠标");
            if (m_Setting.CanClickAnim)
            {
               if (!mouseIsIn || !m_Setting.CanEnterAnim)
                  OnScale(1f, m_Setting.clickBack_Time);
               else
                  OnScale(m_Setting.enter_Scale, m_Setting.enter_Time);
            }
         }
      }
      /// <summary>
      /// 鼠标点击
      /// </summary>
      /// <param name="eventData"></param>
      void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
      {
         //Debug.Log("鼠标单击");
         if (m_Setting.CanClick)
         {
            OnClick();
         }
      }
      /// <summary>
      /// 点击事件
      /// </summary>
      protected virtual void OnClick()
      {
         if (isDebug) Debug.Log("点击");
         clickEvent?.Invoke();
      }
      /// <summary>
      /// 进入事件
      /// </summary>
      protected virtual void OnEnter()
      {
      }
      /// <summary>
      /// 离开事件
      /// </summary>
      protected virtual void OnExit()
      {
      }

      #endregion

      #region 鼠标拖拽
      /// <summary>
      /// 开始拖拽
      /// </summary>
      /// <param name="eventData"></param>
      public virtual void OnBeginDrag(PointerEventData eventData)
      {
         if (isDebug)
            Debug.Log("StartDrap");

         if (m_Setting.CanDraw)
         {
            drawing = true;
            awakePosition = DragTrans.localPosition;
            if (DragTrans != thisRectTrans)
            {
               DragTrans.SetParent(thisRectTrans.parent);
               awakeNewPosition = DragTrans.localPosition;
            }
            if (m_Setting.DrawMouseIsOffset && dragOffsetPoint == Vector2.zero)
            {
               dragOffsetPoint = MouseNowPosition(eventData) - dragObj.localPosition;
            }
         }
      }
      /// <summary>
      /// 拖拽中
      /// </summary>
      /// <param name="eventData"></param>
      public virtual void OnDrag(PointerEventData eventData)
      {
         var _value = MouseNowPosition(eventData);

         if (m_Setting.CanDraw)
         {
            if (dragTime == default)
            {
               OnDragFrist(eventData);
            }
            if (_value != default)
            {
               if (m_Setting.DrawMouseIsOffset && dragOffsetPoint != Vector2.zero)
               {
                  Vector3 offset = dragOffsetPoint;
                  _value -= offset;
               }
               DrawUpdate(_value);
            }
         }
      }
      protected virtual void DrawUpdate(Vector3 value)
      {
         DragTrans.localPosition = value;
      }
      /// <summary>
      /// 拖拽的第一帧
      /// </summary>
      public virtual void OnDragFrist(PointerEventData eventData)
      {
         dragTime = Time.time;
         awakeNewPosition = DragTrans.localPosition;
      }
      /// <summary>
      /// 结束拖拽
      /// </summary>
      /// <param name="eventData"></param>
      public virtual void OnEndDrag(PointerEventData eventData)
      {
         if (isDebug)
            Debug.Log("DrapOvre");
         if (m_Setting.CanDraw)
         {
            dragTime = default;
            drawing = false;
            awakeNewPosition = default;
            dragOffsetPoint = default;
            if (DragTrans != thisRectTrans)
            {
               DragTrans.SetParent(thisRectTrans);
            }
            StartCoroutine(ButtonDisAppear());
         }
      }
      #endregion

      #region Anim void 具体动画方法
      /// <summary>
      /// 动态改变自身的大小
      /// </summary>
      public virtual Tween OnScale(float Scale, float time = 0.25f)
      {
         return transform.DOScale(Scale * awakeScale, time);
      }

      /// <summary>
      /// 按钮返回
      /// </summary>
      /// <returns></returns>
      protected virtual IEnumerator ButtonDisAppear()
      {
         yield return new WaitForFixedUpdate();
         DragTrans.transform.DOLocalMove(awakePosition /*+ new Vector3(dragOffsetPoint.x,  dragOffsetPoint.y, 0)*/, 0.25f);
         DragTrans.transform.DOScale(1f, 0.25f);
         yield return new WaitForSeconds(0.5f);
      }
      #endregion

      #region Other void
      public void SetDrawObject(Transform drawObject)
      {
         Debug.Log("拖拽对象更新");
         dragObj = drawObject;
      }
      #endregion
   }
}
