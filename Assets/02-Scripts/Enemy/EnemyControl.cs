using System.Collections.Generic;
using TeaFramework;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
   public static List<Enemy_Main> enemys = new();
   private void OnDestroy()
   {
      enemys.Clear();
   }
}
