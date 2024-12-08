using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

namespace TeaFramework
{
   public static class LoadingControl
   {
      public static GameObject loadGroupPrefab;
      private static GameObject loadGroupEntity;

      public static void OnLoad(int sceneIndex)
      {
         if (loadGroupEntity == null) { loadGroupEntity = LoadAndInstantiateLoadingGroup(); }


         // 开始淡入效果
         if (loadGroupEntity != null && loadGroupEntity.transform.GetChild(0).TryGetComponent<CanvasGroup>(out var canvasGroup))
         {
            FadeInCanvasGroup(canvasGroup, 1.0f, 500, sceneIndex).ConfigureAwait(false);
         }
      }

      public static GameObject LoadAndInstantiateLoadingGroup()
      {
         // 从 Resources 目录加载 prefab
         GameObject prefab = Resources.Load<GameObject>("Prefabs/LoadingGroup");
         if (prefab == null)
         {
            Debug.LogError("Prefab /LoadingGroup 未找到预制件");
            return null;
         }

         // 实例化 prefab
         GameObject instance = Object.Instantiate(prefab);
         Object.DontDestroyOnLoad(instance);
         return instance;
      }

      private static async Task FadeInCanvasGroup(CanvasGroup canvasGroup, float targetAlpha, int fadeInDurationMs, int sceneIndex)
      {

         float startAlpha = canvasGroup.alpha;
         float elapsedTime = 0f;

         // 淡入效果
         while (elapsedTime < fadeInDurationMs)
         {
            await Task.Delay(10);
            elapsedTime += 10;
            float t = Mathf.Clamp01(elapsedTime / fadeInDurationMs);
            float easeOutT = 1 - Mathf.Pow(1 - t, 3);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, easeOutT);
         }

         // 开始预加载场景并禁止立即切换场景
         AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync(sceneIndex);
         sceneLoadOperation.allowSceneActivation = false;

         canvasGroup.alpha = targetAlpha;

         await Task.Delay(500); // 每 100 毫秒检查加载进度

         // 等待场景加载完成
         while (!sceneLoadOperation.isDone)
         {
            if (sceneLoadOperation.progress >= 0.9f) sceneLoadOperation.allowSceneActivation = true; // 禁止立即切换场景
            Debug.Log($"场景加载进度: {sceneLoadOperation.progress}  {sceneLoadOperation.isDone}");
            await Task.Delay(200); // 每 100 毫秒检查加载进度
         }

         await Task.Delay(500); // 每 100 毫秒检查加载进度

         // 场景加载完成后淡出
         elapsedTime = 0f;
         float fadeOutDurationMs = 500;
         startAlpha = canvasGroup.alpha;
         targetAlpha = 0f;

         while (elapsedTime < fadeOutDurationMs)
         {
            await Task.Delay(10);
            elapsedTime += 10;
            float t = Mathf.Clamp01(elapsedTime / fadeOutDurationMs);
            float easeInT = Mathf.Pow(t, 3); // 使用 easeInCubic 淡出效果
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, easeInT);
         }
         if (targetAlpha <= 0)
         {
            targetAlpha = 0.0002f;
         }
         canvasGroup.alpha = targetAlpha; // 确保最终值精确
      }
   }
}
