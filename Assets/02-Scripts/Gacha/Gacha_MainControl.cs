using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeaFramework
{
   public class Gacha_MainControl : MonoBehaviour
   {
      [SerializeField] private List<GachaPool_Data> onOpenPool;
      [SerializeField] private RoleItem_ListData allRole;
      [SerializeField] private WeaponItem_ListData allWeapon;
      private void Start()
      {
         GachaCoreOperation.Initiakize(onOpenPool);
      }
   }
}
