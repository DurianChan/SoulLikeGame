using System;
using System.Collections.Generic;
using UnityEngine;

namespace DC.Items
{
    [Serializable]
    public struct ItemAmount
    {
        public Item Item;       //消耗物品
        [Range(1, 999)]          //限制1-999数量之间
        public int Amount;      //消耗物品的数量
    }

    ///<summary>
    ///制作物品：制作配方
    ///</summary>
    [CreateAssetMenu]
    public class CraftingRecipe : ScriptableObject
    {
        public List<ItemAmount> Materials;                    //制作材料
        public List<ItemAmount> Results;                      //制作结果

        /// <summary>
        /// 判断能否进行制作物品
        /// </summary>
        /// <param name="inventory"></param>
        /// <returns></returns>
        public bool CanCraft(IItemContainer itemContainer)
        {
            return HasMaterials(itemContainer) && HasSpace(itemContainer);
        }

        /// <summary>
        /// 检查制作物品是否满足条件
        /// </summary>
        /// <param name="itemContainer"></param>
        /// <returns></returns>
        private bool HasMaterials(IItemContainer itemContainer)
        {
            foreach (ItemAmount itemAmount in Materials)
            {
                if (itemContainer.ItemCount(itemAmount.Item.ID) < itemAmount.Amount)           //库存中的物品数量是否满足制作的数量
                {
                    Debug.Log("You don't have the required materials.");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 检查是否库存有足够的空间
        /// </summary>
        /// <param name="itemContainer"></param>
        /// <returns></returns>
        private bool HasSpace(IItemContainer itemContainer)
        {
            foreach (ItemAmount itemAmount in Results)
            {
                if (!itemContainer.CanAddItem(itemAmount.Item, itemAmount.Amount))      //判断制作出的物品是否有足够的库存存放物品
                {
                    Debug.LogError("Your inventory is full.");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 移除需要制作物品的材料
        /// </summary>
        /// <param name="itemContainer"></param>
        private void RemoveMaterials(IItemContainer itemContainer)
        {
            foreach (ItemAmount itemAmount in Materials)
            {
                for (int i = 0; i < itemAmount.Amount; i++)
                {
                    Item oldItem = itemContainer.RemoveItem(itemAmount.Item.ID);      //移除对应制作消耗的物品
                    oldItem.Destory();
                }
            }
        }

        /// <summary>
        /// 制作物品
        /// </summary>
        /// <param name="inventory"></param>
        public void Craft(IItemContainer itemContainer)
        {
            if (CanCraft(itemContainer))
            {
                RemoveMaterials(itemContainer);
                AddResults(itemContainer);
            }
        }

        /// <summary>
        /// 向库存中添加制作的物品
        /// </summary>
        /// <param name="itemContainer"></param>
        private void AddResults(IItemContainer itemContainer)
        {
            foreach (ItemAmount itemAmount in Results)
            {
                for (int i = 0; i < itemAmount.Amount; i++)
                {
                    itemContainer.AddItem(itemAmount.Item.GetCopy());         //添加制作的物品
                }
            }
        }
    }
}
