using System;
using System.Collections.Generic;
using UnityEngine;

namespace DC.Items
{
    ///<summary>
    ///装备栏UI管理
    ///</summary>
    public class EquipmentPanel : MonoBehaviour
    {
        [SerializeField] private Transform equipmentSlotsParent = null;            //装备栏父物体位置
        [SerializeField] public EquipmentSlot[] equipmentSlots = null;            //存储装备栏插槽

        public event Action<BaseItemSlot> OnPointerEnterEvent;
        public event Action<BaseItemSlot> OnPointerExitEvent;
        public event Action<BaseItemSlot> OnRightClickEvent;
        public event Action<BaseItemSlot> OnBeginDragEvent;
        public event Action<BaseItemSlot> OnEndDragEvent;
        public event Action<BaseItemSlot> OnDragEvent;
        public event Action<BaseItemSlot> OnDropEvent;

        private void Start()
        {
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                equipmentSlots[i].OnPointerEnterEvent += slot=> OnPointerEnterEvent(slot);
                equipmentSlots[i].OnPointerExitEvent += slot => OnPointerExitEvent(slot);
                equipmentSlots[i].OnRightClickEvent += slot => OnRightClickEvent(slot);
                equipmentSlots[i].OnBeginDragEvent += slot => OnBeginDragEvent(slot);
                equipmentSlots[i].OnEndDragEvent += slot => OnEndDragEvent(slot);
                equipmentSlots[i].OnDragEvent += slot => OnDragEvent(slot);
                equipmentSlots[i].OnDropEvent += slot => OnDropEvent(slot);
            }
        }

        private void OnValidate()
        {
            equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
        }

        /// <summary>
        /// 向装备栏添加符合装备类型的物品
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddItem(EquippableItem item, out EquippableItem previousItem)
        {
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                if (equipmentSlots[i].EquipmentType == item.EquipmentType)
                {
                    previousItem = (EquippableItem)equipmentSlots[i].Item;
                    equipmentSlots[i].Item = item;
                    equipmentSlots[i].Amount = 1;
                    return true;
                }
            }
            previousItem = null;
            return false;
        }

        /// <summary>
        /// 在装备栏中卸下装备类型的物品
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool RemoveItem(EquippableItem item)
        {
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                if (equipmentSlots[i].Item == item)
                {
                    equipmentSlots[i].Item = null;
                    equipmentSlots[i].Amount = 0;
                    return true;
                }
            }
            return false;
        }


    }
}
