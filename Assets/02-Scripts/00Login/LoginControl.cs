using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TeaFramework
{
   public class LoginControl : MonoBehaviour
   {
      /// <summary> 是否有延迟的事件正在进行中 </summary>
      bool delayEventOnProgress;

      // 插值时间范围：[0.5f, 4]秒，控制音量变化的时间
      [SerializeField][Range(0.5f, 4)] private float lerpTime = 1;

      // 遮罩图像，用于渐变效果
      [SerializeField] private Image mask;

      // 音频源，用于播放音频
      [SerializeField] private AudioSource audioSource;

      // 音量目标值，音频淡入/淡出时的目标音量
      [SerializeField][Range(0, 1)] private float alVolumeTarget = 0.7f;

      // 插值曲线，控制音量变化的动画曲线
      [SerializeField] private AnimationCurve lerpCurve;

      private async void Awake()
      {
         // 设置初始遮罩颜色为黑色

         // 遮罩从黑色渐变为透明
         mask.DOColor(new Color(0, 0, 0, 0), lerpTime).SetEase(lerpCurve);
         delayEventOnProgress = true;
         // 音量淡入（启动时播放音频）
         await audioSource.AudioVolumeLerpAsync(lerpTime, 0, alVolumeTarget, lerpCurve);
         delayEventOnProgress = false;
      }

      public async void Button_Start()
      {
         // 判断是否有延迟事件正在进行中
         if (!delayEventOnProgress)
         {
            Debug.Log("start按键按下");


            // 开始加载场景
            LoadingControl.OnLoad(1);
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
