using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace TeaFramework
{
   public class TeaBaseButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
   {
      public UnityEvent clickEvent;

      public float clickChangeScale = 0.95f;
      public void OnPointerClick(PointerEventData eventData)
      {
         clickEvent?.Invoke();
      }

      public void OnPointerDown(PointerEventData eventData)
      {
         if (clickChangeScale != 1)
            transform.DOScale(0.95f, 0.1f);
      }

      public void OnPointerUp(PointerEventData eventData)
      {
         if (clickChangeScale != 1)
            transform.DOScale(1f, 0.1f);
      }
   }
}
