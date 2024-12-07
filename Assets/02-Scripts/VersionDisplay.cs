using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VersionDisplay : MonoBehaviour
{
   public string msg = "开发中版本:";
   private void Awake()
   {
      Application.targetFrameRate = 60;
   }
   void Start()
   {
      if (transform.TryGetComponent(out TMP_Text text))
      {
         text.text = msg + Application.version;
      }
   }
   private void Update()
   {
      Debug.Log(Application.targetFrameRate);
   }

}
