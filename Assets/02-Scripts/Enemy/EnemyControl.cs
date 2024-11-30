using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   public class EnemyControl : Singleton<EnemyControl>
   {
      public static List<EnemyEntity_Main> enemys = new();
      public EnemyUIData uiData;
      protected override void OnDestroy()
      {
         enemys.Clear();
         base.OnDestroy();
      }
   }
   public static class EnemyControlExpand
   {
      private static EnemyUIData UIData => EnemyControl.I.uiData;
      public static EnemyEntity_UI NewEnemyEntityUI(this EnemyEntity_Main enemyEntity, DamageElement element, float yOffset = 1)
      {
         var uiInst = Object.Instantiate(EnemyControl.I.uiData.UIPrefab, enemyEntity.transform).transform;
         uiInst.localPosition = Vector3.up * yOffset;

         uiInst.TryGetComponent(out EnemyEntity_UI ui);
         EnemyUIData_Element elementData = default;
         switch (element)
         {
            case DamageElement.none:
               elementData = UIData.uiIData_None;
               break;
            case DamageElement.Ice:
               elementData = UIData.uiIData_Ice;
               break;
            case DamageElement.Fire:
               elementData = UIData.uiIData_Fire;
               break;
            case DamageElement.Wind:
               elementData = UIData.uiIData_Wind;
               break;
            case DamageElement.Electric:
               elementData = UIData.uiIData_Electric;
               break;
            default:
               break;
         }
         ui.SetUIData(elementData);
         return ui;
      }
   }
}