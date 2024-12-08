using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TeaFramework
{
   public class MainCityControl : MonoBehaviour
   {
      /// <summary> 有延迟的事件正在进行中 </summary>
      bool delayEventOnProgress = true;

      // 插值时间范围：[0.5f, 4]秒，控制音量变化的时间
      [SerializeField][Range(0.5f, 4)] private float lerpTime = 1;

      // 音频源，用于播放音频
      [SerializeField] private AudioSource audioSource;

      // 音量目标值，音频淡入/淡出时的目标音量
      [SerializeField][Range(0, 1)] private float alVolumeTarget = 0.7f;

      // 插值曲线，控制音量变化的动画曲线
      [SerializeField] private AnimationCurve lerpCurve;
      private async void Awake()
      {

         // 音量淡入（启动时播放音频）
         await audioSource.AudioVolumeLerpAsync(lerpTime, 0, alVolumeTarget, lerpCurve);
         delayEventOnProgress = false;
      }
      public async void OnLoadToGacha()
      {

         // 判断是否有延迟事件正在进行中
         if (!delayEventOnProgress)
         {
            Debug.Log("start按键按下");

            // 设置插值时间
            lerpTime = 1;

            // 开始加载场景
            LoadingControl.OnLoad(2);
            delayEventOnProgress = true;
            // 开始音量淡出（停止播放音频）
            await audioSource.AudioVolumeLerpAsync(0.8f, alVolumeTarget, 0, lerpCurve);
            delayEventOnProgress = false;
         }
         else
         {
            Debug.Log("有事件正在进行中");
         }
      }
   }
}
