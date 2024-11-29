using TeaFramework;
using UnityEngine;

public class Weapon_Base : MonoBehaviour
{
   public Transform showPoint;
   public GameObject bulletPrefab;

   public float shotSpeed = 50f;

   /// <summary> 射击间隔 </summary>
   public float shotInterval = 0.1f;
   /// <summary> 当前间隔 </summary>
   private float nowInterval;
   private float LockSpeed => PlayerControl.I.config.LockEnemySpeed;  // 旋转速度

   private void Update()
   {
      // 当前间隔存在，随时间减少
      if (nowInterval > 0) nowInterval -= Time.deltaTime;

      // 当控制权限未被接管，且按下左键，且无CD，则射击并附加CD
      else if (!PlayerControl.I.ControlTakeOff && Input.GetMouseButton(0) && nowInterval <= 0)
      {
         OnShot();
         nowInterval = shotInterval;
      }
      TowardsUpdate();
   }
   /// <summary>
   /// 角度更新
   /// </summary>
   private void TowardsUpdate()
   {
      if (PlayerControl.I.config.lockEnemy)
      {
         // 获取目标方向
         Vector3 targetAxis = PlayerControl.I.config.lockEnemy.transform.position - transform.position;

         // 计算目标的旋转（使用Quaternion）
         Quaternion targetRotation = Quaternion.LookRotation(targetAxis);

         // 将旋转值应用到物体
         transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, LockSpeed * Time.deltaTime);
      }
      else
      {
         // 目标旋转为单位Quaternion（不旋转）
         Quaternion targetRotation = Quaternion.identity;

         // 通过插值将当前旋转平滑过渡到目标旋转
         transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, LockSpeed * Time.deltaTime);

         //Debug.Log($"原始旋转: {transform.localRotation} 更新旋转: {targetRotation}");
      }
   }
   public void OnShot()
   {
      if (!bulletPrefab) return;
      var instB = Instantiate(bulletPrefab, showPoint.position, showPoint.rotation);
      Destroy(instB, 5);
      instB.TryGetComponent(out Rigidbody rb);
      rb.velocity = showPoint.forward * shotSpeed;
   }
}
