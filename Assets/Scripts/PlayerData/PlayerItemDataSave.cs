using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC.Items
{
    ///<summary>
    ///玩家物品信息存储
    ///</summary>
    public class PlayerItemDataSave : MonoBehaviour
    {
        [SerializeField] private ItemSaveDatabase itemSaveDatabase = null;
        private const string InventoryFileName = "Inventory.txt";
        private const string EquipmentFileName = "Equipment.txt";
        private const string QuickSlotFileName = "QuickSlot.txt";
        private const string RebirthPositionFileName = "RebirthPosition.txt";
        private const string SignPositionFileName = "SignPosition.txt";

        private void SaveItemData(IList<ItemSlot> itemSlots, string fileName)
        {
            var saveData = new ItemContainerSaveData(itemSlots.Count);      //创建物品插槽等数量的数组存储已有物品信息 

            for (int i = 0; i < saveData.SavedSlots.Length; i++)
            {
                ItemSlot itemSlot = itemSlots[i];

                if (itemSlot.Item == null)                                  //若库存物品插槽为空则将存放物品数组位置设置为空
                {
                    saveData.SavedSlots[i] = null;
                }
                else                                                        //否则将物品存放信息存放到数组中
                {
                    saveData.SavedSlots[i] = new ItemSlotSaveData(itemSlot.Item.ID, itemSlot.Amount);
                }
            }

            SaveDataManager.SaveFile<ItemContainerSaveData>(fileName, saveData);
        }

        public void SaveInventory(Character character)
        {
            SaveItemData(character.Inventory.ItemSlots, InventoryFileName);
        }

        public void LoadInventory(Character character)
        {
            ItemContainerSaveData savedSlots = SaveDataManager.LoadFile<ItemContainerSaveData>(InventoryFileName);
            if (savedSlots == null) return;
            character.Inventory.Clear();

            for (int i = 0; i < savedSlots.SavedSlots.Length; i++)
            {
                ItemSlot itemSlot = character.Inventory.ItemSlots[i];
                ItemSlotSaveData savedSlot = savedSlots.SavedSlots[i];

                if (savedSlot == null)
                {
                    itemSlot.Item = null;
                    itemSlot.Amount = 0;
                }
                else
                {
                    itemSlot.Item = itemSaveDatabase.GetItemCopy(savedSlot.ItemID);
                    itemSlot.Amount = savedSlot.Amount;
                }
            }
        }

        public void SaveEquipment(Character character)
        {
            SaveItemData(character.EquipmentPanel.equipmentSlots, EquipmentFileName);
        }

        public void LoadEquipment(Character character)
        {
            ItemContainerSaveData savedSlots = SaveDataManager.LoadFile<ItemContainerSaveData>(EquipmentFileName);
            if (savedSlots == null) return;

            IList<ItemSlot> itemSlots = character.EquipmentPanel.equipmentSlots;        //清空载入前物品栏所有装备
            for (int i = 0; i < itemSlots.Count; i++)
            {
                itemSlots[i].Item = null;
            }

            foreach (ItemSlotSaveData savedSlot in savedSlots.SavedSlots)
            {
                if (savedSlot == null)
                {
                    continue;
                }

                Item item = itemSaveDatabase.GetItemCopy(savedSlot.ItemID);
                character.Inventory.AddItem(item);
                character.Equip(character.LoadDataWithWeapon((EquippableItem)item));
            }
        }

        public void SaveQuickSlot(Character character)
        {
            SaveItemData(character.QuickSlotsPanel.ItemSlots, QuickSlotFileName);
        }

        public void LoadQuickSlot(Character character)
        {
            ItemContainerSaveData savedSlots = SaveDataManager.LoadFile<ItemContainerSaveData>(QuickSlotFileName);
            if (savedSlots == null) return;

            IList<ItemSlot> itemSlots = character.QuickSlotsPanel.ItemSlots;        //清空载入前物品栏所有装备
            for (int i = 0; i < itemSlots.Count; i++)
            {
                itemSlots[i].Item = null;
            }

            for (int i = 0; i < savedSlots.SavedSlots.Length; i++)
            {
                ItemSlot itemSlot = itemSlots[i];
                ItemSlotSaveData savedSlot = savedSlots.SavedSlots[i];

                if (savedSlot == null)
                {
                    itemSlot.Item = null;
                    itemSlot.Amount = 0;
                }
                else
                {
                    itemSlot.Item = itemSaveDatabase.GetItemCopy(savedSlot.ItemID);
                    itemSlot.Amount = savedSlot.Amount;
                }
            }
        }

        public void SaveRebirthPosition(SaveLoadPos pos)
        {
            SerializableVector3[] saveData = new SerializableVector3[pos.currentPos.Length];
            for (int i = 0; i < pos.currentPos.Length; i++)
            {
                saveData[i] = pos.currentPos[i];
            }
            SaveDataManager.SaveFile< SerializableVector3[]> (RebirthPositionFileName, saveData);
        }

        public void LoadRebirthPosition(SaveLoadPos pos)
        {
            SerializableVector3[] saveData = SaveDataManager.LoadFile<SerializableVector3[]>(RebirthPositionFileName);
            if (saveData == null) return;
            for (int i = 0; i < saveData.Length; i++)
            {
                pos.currentPos[i] = saveData[i];
            }
        }

        public void SaveSignPosition(SaveLoadPos pos)
        {
            SerializableVector3[] saveData = new SerializableVector3[pos.savePosList.Count];
            for (int i = 0; i < pos.savePosList.Count; i++)
            {
                saveData[i] = pos.savePosList[i];
            }
            SaveDataManager.SaveFile<SerializableVector3[]>(SignPositionFileName, saveData);
        }

        public void LoadSignPosition(SaveLoadPos pos)
        {
            SerializableVector3[] saveData = SaveDataManager.LoadFile<SerializableVector3[]>(SignPositionFileName);
            if (saveData == null) return;
            pos.savePosList.Clear();
            for (int i = 0; i < saveData.Length; i++)
            {
                pos.savePosList.Add(saveData[i]);
            }
        }
    }
}
