using System.Collections;
using System.Collections.Generic;
using TeaFramework;
using UnityEngine;

public class Weapon_Base : MonoBehaviour
{
   public Transform showPoint;
   public GameObject bulletPrefab;

   public float shotSpeed = 50f;

   public float shotInterval = 0.1f;
   private float nowInterval;

   private void Update()
   {
      if (nowInterval > 0) nowInterval -= Time.deltaTime;
      else if (!PlayerControl.I.ControlTakeOff && Input.GetMouseButton(0) && nowInterval <= 0)
      {
         OnShot();
         nowInterval = shotInterval;
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
