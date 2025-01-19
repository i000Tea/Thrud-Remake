namespace TeaFramework
{
   public abstract class P_0ModularBase
   {
      protected PlayerConfig Config => PlayerControl.I.config;
      protected FaceCanvasConfig UiConfig => PlayerControl.I.canvasConfig;
      protected EntityBattleData[] EntityBattleDatas => PlayerControl.I.entityBattleDatas;
      protected EntityBattleData NowBattleData => 
         PlayerControl.I.entityBattleDatas[PlayerControl.I.config.selectRoleIndedx];
      public virtual void Start() { }
      public virtual void Update() { }
      public virtual void FixedUpdate() { }
      public virtual void LateUpdate() { }
      public virtual void OnDestroy() { }
   }
}