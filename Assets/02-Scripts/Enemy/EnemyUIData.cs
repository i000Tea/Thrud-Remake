using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
   public class EnemyUIData : ScriptableObject
   {
      public GameObject UIPrefab;
      public EnemyUIData_Element uiIData_None;
      public EnemyUIData_Element uiIData_Ice;
      public EnemyUIData_Element uiIData_Fire;
      public EnemyUIData_Element uiIData_Wind;
      public EnemyUIData_Element uiIData_Electric;
   }
   [System.Serializable]
   public class EnemyUIData_Element
   {
      public Sprite Sign;
   }
}
