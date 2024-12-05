using DG.Tweening;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Tea;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
namespace TeaFramework
{
   public class GachaPoolButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
   {
      public UnityEvent clickEvent;
      public void OnPointerClick(PointerEventData eventData)
      {
         clickEvent?.Invoke();
      }

      public void OnPointerDown(PointerEventData eventData)
      {
         transform.DOScale(0.95f, 0.1f);
      }

      public void OnPointerUp(PointerEventData eventData)
      {
         transform.DOScale(1f, 0.1f);
      }
   }
}
