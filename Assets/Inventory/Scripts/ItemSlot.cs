using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DC.Items
{
    ///<summary>
    ///物品插槽
    ///</summary>
    public class ItemSlot : BaseItemSlot, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
    {
        public event Action<BaseItemSlot> OnBeginDragEvent;
        public event Action<BaseItemSlot> OnEndDragEvent;
        public event Action<BaseItemSlot> OnDragEvent;
        public event Action<BaseItemSlot> OnDropEvent;

		private bool isDragging;

		public override bool CanAddStack(Item item, int amount = 1)
		{
			return base.CanAddStack(item, amount) && Amount + amount <= item.MaximumStacks;
		}

		public override bool CanReceiveItem(Item item)
		{
			return true;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			if (isDragging)
			{
				OnEndDrag(null);
			}
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			isDragging = true;

			if (OnBeginDragEvent != null)
				OnBeginDragEvent(this);
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			isDragging = false;

			if (OnEndDragEvent != null)
				OnEndDragEvent(this);
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (OnDragEvent != null)
				OnDragEvent(this);
		}

		public void OnDrop(PointerEventData eventData)
		{
			if (OnDropEvent != null)
				OnDropEvent(this);
		}
	}
}
