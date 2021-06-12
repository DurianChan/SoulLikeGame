using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC.Items
{
    ///<summary>
    ///无限库存
    ///</summary>
    public class InfiniteInventory : Inventory
    {
        [SerializeField] private GameObject itemSlotPrefab = null;                     //物品插槽预制体

        [SerializeField] private int maxSlots;                                         //最大插槽数
        public int MaxSlots
        {
            get { return maxSlots; }
            set { SetMaxSlots(value); }
        }

        protected override void Awake()
        {
            SetMaxSlots(maxSlots);
            base.Awake();
        }

        /// <summary>
        /// 设置库存最大值
        /// </summary>
        /// <param name="value"></param>
        private void SetMaxSlots(int value)
        {
            if (value <= 0)
            {
                maxSlots = 1;
            }
            else
            {
                maxSlots = value;
            }

            if (maxSlots < ItemSlots.Count)                     //当库存插槽大于库存容量时，移除多余得插槽
            {
                for (int i = maxSlots; i < ItemSlots.Count; i++)
                {
                    Destroy(ItemSlots[i].transform.parent.gameObject);
                }
                int diff = ItemSlots.Count - maxSlots;
                ItemSlots.RemoveRange(maxSlots, diff);
            }
            else if (maxSlots > ItemSlots.Count)              //当库存插槽小于库存容量时，添加缺少的得插槽
            {
                int diff = maxSlots - ItemSlots.Count;

                for (int i = 0; i < diff; i++)
                {
                    GameObject itemSlotGameObj = Instantiate(itemSlotPrefab);
                    itemSlotGameObj.transform.SetParent(itemsParent, worldPositionStays: false);
                    ItemSlots.Add(itemSlotGameObj.GetComponentInChildren<ItemSlot>());
                }
            }
        }

    }
}
