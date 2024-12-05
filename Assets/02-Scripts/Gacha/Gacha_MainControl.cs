using System.Collections;
using System.Collections.Generic;
using Tea;
using UnityEngine;
using UnityEngine.UI;

namespace TeaFramework
{
   public class Gacha_MainControl : MonoBehaviour
   {
      [SerializeField] PoolTag _tag;
      [SerializeField] private Sprite[] tagSprite;

      [SerializeField] private Transform poolBtnParent;
      [SerializeField] private GameObject poolBtnPrfab;
      [SerializeField] private Sprite poolBtnBG;
      [SerializeField] private Sprite poolBtnBGSelect;

      [SerializeField] private Image poolMainBG;

      [SerializeField] private List<GachaPool_Data> onOpenPool;
      [SerializeField] private RoleItem_ListData allRole;
      [SerializeField] private WeaponItem_ListData allWeapon;

      private int nowPoolIndex;
      private void Awake()
      {
         GachaCoreOperation.Initiakize(onOpenPool);
         AwakeSetPoolBtn();
      }
      private IEnumerator Start()
      {
         yield return new WaitForFixedUpdate();

         if (poolBtnParent.GetChild(0).TryGetComponent(out GachaPoolButton teaBtn))
         {
            Debug.Log(teaBtn);
            teaBtn.clickEvent?.Invoke();
         }
      }
      private void AwakeSetPoolBtn()
      {
         // 使用循环移除 poolBtnParent 的所有子对象
         for (int i = poolBtnParent.childCount - 1; i >= 0; i--) { Destroy(poolBtnParent.GetChild(i).gameObject); }

         for (int i = 0; i < onOpenPool.Count; i++)
         {
            PoolSet(
               Instantiate(poolBtnPrfab, poolBtnParent).transform,
               onOpenPool[i],
               i);
         }

         var size = (poolBtnParent as RectTransform).sizeDelta;
         size.y = onOpenPool.Count * 140;
         (poolBtnParent as RectTransform).sizeDelta = size;

      }

      private void PoolSet(Transform pool, GachaPool_Data data, int index)
      {
         if (pool.TryGetComponent(out GachaPoolButton teaBtn))
         {
            teaBtn.clickEvent.AddListener(() => PoolSwitch(index));
         }

         if (pool.GetChild(1).TryGetComponent(out Image adImg))
         {
            adImg.sprite = data.sprite_PoolBtnBg;
         }
         if (pool.GetChild(2).TryGetComponent(out Image tagImg))
         {
            var tagindex = (int)data.pooltag;
            Debug.Log(tagindex);
            tagImg.sprite = tagSprite[(int)data.pooltag];
         }
      }
      private void PoolSwitch(int index)
      {
         for (int i = 0; i < poolBtnParent.childCount; i++)
         {
            if (poolBtnParent.GetChild(i).GetChild(0).TryGetComponent(out Image btnImg))
            {
               btnImg.sprite = index == i ? poolBtnBGSelect : poolBtnBG;
            }
         }
         poolMainBG.sprite = onOpenPool[index].sprite_PoolBg;
      }

      public void OnGacha01() => GachaCoreOperation.OnGacha01();
      public void OnGacha10() => GachaCoreOperation.OnGacha10();
   }
}
