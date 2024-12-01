using UnityEngine;

namespace TeaFramework
{
   public class EnemyEntity_Main : MonoBehaviour
   {
      /// <summary> 当前对象在摄像机中 </summary>
      public bool onCame;
      /// <summary> 敌人元素 </summary>
      [SerializeField] private DamageElement element;
      [SerializeField] private Transform beLockPoint;

      [SerializeField] private float shotCDMin = 3;
      [SerializeField] private float shotCDMax = 5;
      private float shotCD;

      #region ui
      [SerializeField][Range(0f, 100f)] private float uiYOffset = 1f;
      private EnemyEntity_UI UIInst;
      #endregion

      #region numerical value 数值
      [SerializeField] private float BaseHP = 100;
      private float nowHP;
      #endregion

      #region attack 战斗系统

      #endregion

      private void Awake()
      {
         shotCD = Random.Range(shotCDMin, shotCDMax);
         nowHP = BaseHP;
         if (!beLockPoint) beLockPoint = transform;
      }
      private void Start()
      {
         if (!UIInst)
         {
            UIInst = this.NewEnemyEntityUI(element, uiYOffset);
         }
         EnemyControl.enemys.Add(this);
      }
      private void Update()
      {
         onCame = IsTargetVisible(beLockPoint, PlayerControl.I.config.camera);
         OnShotDetection();
      }

      /// <summary>
      /// 检查目标是否在摄像机视野内
      /// </summary>
      /// <param name="transform">目标 Transform</param>
      /// <param name="camera">检测的 Camera</param>
      /// <returns>是否可见</returns>
      public bool IsTargetVisible(Transform transform, Camera camera)
      {
         if (transform == null || camera == null)
         {
            Debug.LogError("Transform 或 Camera 不能为空！");
            return false;
         }

         // 将目标的世界坐标转换为屏幕坐标
         Vector3 screenPoint = camera.WorldToViewportPoint(transform.position);

         // 检查屏幕坐标是否在 [0, 1] 的范围内，以及 z 坐标是否大于 0（在摄像机前方）
         return screenPoint.z > 0 && screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;
      }
      public void BeHit(float damage = 1)
      {
         nowHP -= damage;
         if (nowHP <= 0)
         {
            Debug.Log("die");
         }
         EnvEffectControl.I.NewDamageText(transform.position, (int)damage);
         UIInst.HealthUpdate(nowHP / BaseHP);
      }

      public void OnShotDetection()
      {
         if (shotCD > 0)
         {
            shotCD -= Time.deltaTime;
         }
         else
         {
            shotCD = Random.Range(shotCDMin, shotCDMax);

            var bullet = Instantiate(EnemyControl.I.enemyButtle);
            bullet.transform.position = transform.position;
            bullet.transform.LookAt(PlayerControl.I.transform);
            bullet.TryGetComponent(out Rigidbody rb);
            rb.velocity = bullet.transform.forward * 3;
         }
      }
   }
}
