using System;
using System.Collections.Generic;
using UnityEngine;

namespace DC.Items
{
    ///<summary>
    ///物品容器
    ///</summary>
    public abstract class ItemContainer : MonoBehaviour, IItemContainer
    {
        public List<ItemSlot> ItemSlots;        //存储物品插槽数组

		public event Action<BaseItemSlot> OnPointerEnterEvent;
        public event Action<BaseItemSlot> OnPointerExitEvent;
        public event Action<BaseItemSlot> OnRightClickEvent;
		public event Action<BaseItemSlot> OnLeftClickEvent;
        public event Action<BaseItemSlot> OnBeginDragEvent;
        public event Action<BaseItemSlot> OnEndDragEvent;
        public event Action<BaseItemSlot> OnDragEvent;
        public event Action<BaseItemSlot> OnDropEvent;


        protected virtual void Awake()
        {
            for (int i = 0; i < ItemSlots.Count; i++)
            {
                ItemSlots[i].OnPointerEnterEvent += slot => EventHelper(slot, OnPointerEnterEvent);
                ItemSlots[i].OnPointerExitEvent += slot => EventHelper(slot, OnPointerExitEvent);
                ItemSlots[i].OnRightClickEvent += slot => EventHelper(slot, OnRightClickEvent);
				ItemSlots[i].OnLeftClickEvent += slot => EventHelper(slot, OnLeftClickEvent);
                ItemSlots[i].OnBeginDragEvent += slot => EventHelper(slot, OnBeginDragEvent);
                ItemSlots[i].OnEndDragEvent += slot => EventHelper(slot, OnEndDragEvent);
                ItemSlots[i].OnDragEvent += slot => EventHelper(slot, OnDragEvent);
                ItemSlots[i].OnDropEvent += slot => EventHelper(slot, OnDropEvent);
            }
        }

		protected virtual void OnValidate()
		{
			GetComponentsInChildren(includeInactive: true, result: ItemSlots);
		}

		/// <summary>
		/// 事件绑定方法
		/// </summary>
		/// <param name="itemSlot"></param>
		/// <param name="action"></param>
		private void EventHelper(BaseItemSlot itemSlot, Action<BaseItemSlot> action)
        {
            if (action != null)
                action(itemSlot);
        }

		/// <summary>
		/// 判断能否添加物品
		/// </summary>
		/// <param name="item"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		public virtual bool CanAddItem(Item item, int amount = 1)
		{
			int freeSpaces = 0;

			foreach (ItemSlot itemSlot in ItemSlots)
			{
				if (itemSlot.Item == null || itemSlot.Item.ID == item.ID)		//判断库存是否有空闲的插槽和未堆栈满的相同的物品
				{
					freeSpaces += item.MaximumStacks - itemSlot.Amount;
				}
			}
			return freeSpaces >= amount;
		}

		/// <summary>
		/// 添加物品
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual bool AddItem(Item item)
		{
			for (int i = 0; i < ItemSlots.Count; i++)
			{
				if (ItemSlots[i].CanAddStack(item))
				{
					ItemSlots[i].Item = item;
					ItemSlots[i].Amount++;
					return true;
				}
			}

			for (int i = 0; i < ItemSlots.Count; i++)
			{
				if (ItemSlots[i].Item == null)
				{
					ItemSlots[i].Item = item;
					ItemSlots[i].Amount++;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 根据物品移除物品
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual bool RemoveItem(Item item)
		{
			for (int i = 0; i < ItemSlots.Count; i++)
			{
				if (ItemSlots[i].Item == item)
				{
					ItemSlots[i].Amount--;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 根据物品id移除物品
		/// </summary>
		/// <param name="itemID"></param>
		/// <returns></returns>
		public virtual Item RemoveItem(string itemID)
		{
			for (int i = 0; i < ItemSlots.Count; i++)
			{
				Item item = ItemSlots[i].Item;
				if (item != null && item.ID == itemID)
				{
					ItemSlots[i].Amount--;
					return item;
				}
			}
			return null;
		}

		/// <summary>
		/// 计算物品的数量
		/// </summary>
		/// <param name="itemID"></param>
		/// <returns></returns>
		public virtual int ItemCount(string itemID)
		{
			int number = 0;

			for (int i = 0; i < ItemSlots.Count; i++)
			{
				Item item = ItemSlots[i].Item;
				if (item != null && item.ID == itemID)
				{
					number += ItemSlots[i].Amount;
				}
			}
			return number;
		}

		/// <summary>
		/// 清空物品插槽中的物品
		/// </summary>
		public void Clear()
		{
			for (int i = 0; i < ItemSlots.Count; i++)
			{
				if (ItemSlots[i].Item != null && Application.isPlaying)
				{
					ItemSlots[i].Item.Destory();
				}
				ItemSlots[i].Item = null;
				ItemSlots[i].Amount = 0;
			}
		}
	}
}
