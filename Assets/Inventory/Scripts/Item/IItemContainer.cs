namespace DC.Items
{
    ///<summary>
    ///物品容器
    ///</summary>
    public interface IItemContainer
    {
        bool CanAddItem(Item item, int amount = 1);     //判断能否添加物品
        int ItemCount(string itemID);               //计算物品的数量
        Item RemoveItem(string itemID);             //移除物品
        bool RemoveItem(Item item);                 //移除物品
        bool AddItem(Item item);                //添加物品
        void Clear();                           //清空物品插槽

    }
}
