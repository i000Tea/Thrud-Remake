using UnityEngine;
using UnityEngine.UI;
namespace TeaFramework
{
   public class EnemyEntity_UI : MonoBehaviour
   {
      public Image sign;
      private EnemyUIData_Element uiIData;
      public void SetUIData(EnemyUIData_Element elementData)
      {
         uiIData = elementData;
         sign.sprite = uiIData.Sign;
      }

      // 用LateUpdate, 在每一帧的最后调整Canvas朝向
      void LateUpdate()
      {
         if (PlayerControl.I.config.camera != null)
         {
            // 这里我的角色朝向和UI朝向是相反的，如果直接用LookAt()还需要把每个UI元素旋转过来。
            // 为了简单，用了下面这个方法。它实际上是一个反向旋转，可以简单理解为“负负得正”吧
            transform.rotation = Quaternion.LookRotation(transform.position - PlayerControl.I.config.camera.transform.position);
         }
      }
   }
}