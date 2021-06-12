using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC.Items
{
    ///<summary>
    ///快捷键物品栏
    ///</summary>
    public class QuickSlotsPanel : ItemContainer
    {
        [SerializeField] private Character character;
        [SerializeField] private KeyCode openKeyCode = KeyCode.I;
        [SerializeField] private KeyCode[] quickKeys = null;
        [SerializeField] protected Transform itemsParent = null;     //物品父物体位置  

        private bool isInventoryOpen;

        protected override void OnValidate()
        {
            if (itemsParent != null)
                itemsParent.GetComponentsInChildren(includeInactive: true, result: ItemSlots);

            if (character == null)
                character = FindObjectOfType<Character>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(openKeyCode))
            {
                isInventoryOpen = !isInventoryOpen;
                if (isInventoryOpen)
                    character.OpenQuickSlot(this);
                else
                    character.CloseQuickSlot(this);
            }

            for (int i = 0; i < quickKeys.Length; i++)
            {
                if (Input.GetKeyDown(quickKeys[i]))
                {
                    if (ItemSlots[i].Item is UsableItem)
                    {
                        UsableItem usableItem = (UsableItem)ItemSlots[i].Item;
                        usableItem.Use(character);


                        if (usableItem.IsConsumable)                         //判断可使用物品是否为消耗品
                        {
                            RemoveItem(usableItem);
                            usableItem.Destory();
                        }
                    }
                }
            }
        }

    }
}
