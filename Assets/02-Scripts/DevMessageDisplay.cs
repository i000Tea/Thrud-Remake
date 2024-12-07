using TMPro;
using UnityEngine;

public class DevMessageDisplay : MonoBehaviour
{
   public string msg = "开发中版本:";
   public TMP_Text version;
   public TMP_Text frame;

   /// <summary> 上一次更新帧率的时间 </summary>
   private float m_lastUpdateShowTime = 0f;
   /// <summary> 更新显示帧率的时间间隔 </summary>
   private readonly float m_updateTime = 0.05f;
   /// <summary> 帧数 </summary>
   private int m_frames = 0;
   private float m_FPS = 0;
   void Start()
   {
      Application.targetFrameRate = 120;
      version.text = msg + Application.version;
      m_lastUpdateShowTime = Time.realtimeSinceStartup;
   }
   private void Update()
   {
      Debug.Log(Application.targetFrameRate);

      m_frames++;
      if (Time.realtimeSinceStartup - m_lastUpdateShowTime >= m_updateTime)
      {
         m_FPS = m_frames / (Time.realtimeSinceStartup - m_lastUpdateShowTime);
         m_frames = 0;
         m_lastUpdateShowTime = Time.realtimeSinceStartup;
      }
      frame.text = "FPS: " + m_FPS.ToString("#0.000");
   }
}
