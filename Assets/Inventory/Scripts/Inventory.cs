using UnityEngine;
using UnityEngine.Serialization;

namespace DC.Items
{
    ///<summary>
    ///库存
    ///</summary>
    public class Inventory : ItemContainer
    {
        [FormerlySerializedAs("items")]
        [SerializeField] protected Item[] startingItems = null;          //初始存储物品列表
        [SerializeField] protected Transform itemsParent = null;     //物品父物体位置  

        protected override void OnValidate()
        {
            if (itemsParent != null)
                itemsParent.GetComponentsInChildren(includeInactive: true, result: ItemSlots);

            if (!Application.isPlaying)
            {
                SetStartingItems();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            SetStartingItems();
        }

        /// <summary>
        /// 设置开始物品，刷新库存UI
        /// </summary>
        private void SetStartingItems()
        {
            Clear();
            foreach (Item item in startingItems)
            {
                AddItem(item.GetCopy());
            }
        }
    }
}
