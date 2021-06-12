using UnityEngine;

namespace DC
{
    ///<summary>
    ///玩家数据保存
    ///</summary>
    [System.Serializable]
    public class SaveObject
    {
        public string playerName;
        public float x;
        public float y;
        public float z;

        /// <summary>
        /// 设置玩家位置
        /// </summary>
        /// <param name="vector3"></param>
        public void SetPlayerPos(Vector3 vector3)
        {
            x = vector3.x;
            y = vector3.y;
            z = vector3.z;
        }

        /// <summary>
        /// 获取玩家位置
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPlayerPos()
        {
            return new Vector3(x, y, z);
        }
    }

    ///<summary>
    ///保存物品插槽物品数据
    ///</summary>
    [System.Serializable]
    public class ItemSlotSaveData
    {
        public string ItemID;           //物品id
        public int Amount;              //物品数量

        public ItemSlotSaveData(string id, int amount)
        {
            ItemID = id;
            Amount = amount;
        }
    }

    /// <summary>
    /// 保存物品存储容器数据
    /// </summary>
    [System.Serializable]
    public class ItemContainerSaveData
    {
        public ItemSlotSaveData[] SavedSlots;

        public ItemContainerSaveData(int numItems)
        {
            SavedSlots = new ItemSlotSaveData[numItems];
        }
    }
}
