using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TeaFramework
{
   public class PlayerControl : Singleton<PlayerControl>
   {
      #region 参数变量
      [Range(0.01f, 4f)] public float scale = 1;
      /// <summary> 控制权限被接管(开场动画/大招/暂停) </summary>
      public bool ControlTakeOff;
      public PlayerConfig config;
      public FaceCanvasConfig canvasConfig;
      public EntityBattleData[] entityBattleDatas;

      #region 缓存参数

      private P_0ModularBase[] modulars;

      #endregion

      #endregion

      protected override void Awake()
      {
         base.Awake();

         Time.timeScale = scale;

         modulars = new P_0ModularBase[] {
            new P_Input(),
            new P_ViewModule(),
            new P_Movement(),
            new P_AngleOfView(),
            new P_VisualEffect(),
            new P_Aim(),
            new P_WeaponTrigger(),
         };
      }
      private void Start()
      {
         if (StoreBattleRoleData.I) entityBattleDatas = StoreBattleRoleData.I.battleDatas;
         for (int i = 0; i < modulars.Length; i++) { modulars[i].Start(); }
      }
      private void Update()
      {
         for (int i = 0; i < modulars.Length; i++) { modulars[i].Update(); }
         // 检测 Esc 键按下
         if (Input.GetKeyDown(KeyCode.Escape))
         {
            TogglePause();
         }
      }
      private void FixedUpdate()
      {
         for (int i = 0; i < modulars.Length; i++) { modulars[i].FixedUpdate(); }
      }
      private void LateUpdate()
      {
         for (int i = 0; i < modulars.Length; i++) { modulars[i].LateUpdate(); }
      }
      protected override void OnDestroy()
      {
         for (int i = 0; i < modulars.Length; i++) { modulars[i].OnDestroy(); }
         base.OnDestroy();
      }
      private void TogglePause()
      {
#if UNITY_EDITOR
         if (!EditorApplication.isPaused) EditorApplication.isPaused = true;
#endif
      }
   }
}
