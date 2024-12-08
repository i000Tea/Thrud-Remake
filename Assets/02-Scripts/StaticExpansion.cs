using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace TeaFramework
{
   public static class StaticExpansion
   {
      public static T FindParentComponent<T>(this Transform target) where T : Component
      {
         for (int i = 0; i < 9; i++)
         {
            if (target == null) break;
            if (target.TryGetComponent(out T component))
            {
               return component;
            }
            else target = target.parent;
         }
         return null;
      }


      public static async Task AudioVolumeLerpAsync(this AudioSource audioSource, float lerpTime, float startVolume, float endVolume, AnimationCurve lerpCurve)
      {
         // 设置音频源的初始音量
         audioSource.volume = startVolume;

         // 若未播放，则开始播放音频
         if (!audioSource.isPlaying) audioSource.Play();

         // 用于记录已经经过的时间
         float elapsedTime = 0;

         // 逐渐改变音量直到达到目标
         while (elapsedTime < lerpTime)
         {
            await Task.Delay(20); // 替代协程的等待，用于异步延时

            // 每次增加0.02秒
            elapsedTime += 0.02f;

            // 计算当前时间的归一化值（0到1）
            float t = Mathf.Clamp01(elapsedTime / lerpTime);

            // 计算音量差值
            float diffValue = Mathf.Abs(audioSource.volume - endVolume);
            Debug.Log(diffValue);

            // 如果差值小于阈值，直接设置音量并跳出循环
            if (diffValue < 0.02f)
            {
               audioSource.volume = endVolume; // 直接设置为目标音量
               break; // 跳出循环
            }

            // 根据动画曲线插值计算新的音量值
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, lerpCurve.Evaluate(t));
         }

         // 确保音量最终达到目标值
         audioSource.volume = endVolume;
      }
   }
}
