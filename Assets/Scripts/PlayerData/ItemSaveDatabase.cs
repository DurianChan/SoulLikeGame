using DC.Items;
using UnityEngine;

namespace DC
{
	///<summary>
	///物品数据基础
	///</summary>
	[CreateAssetMenu(menuName = "Items/Item Save Database")]
    public class ItemSaveDatabase : ScriptableObject
    {
		[SerializeField] Item[] items = null;

		/// <summary>
		/// 获取相同ID的物品
		/// </summary>
		/// <param name="itemID"></param>
		/// <returns></returns>
		public Item GetItemReference(string itemID)
		{
			foreach (Item item in items)
			{
				if (item.ID == itemID)
				{
					return item;
				}
			}
			return null;
		}

		/// <summary>
		/// 获取物品
		/// </summary>
		/// <param name="itemID"></param>
		/// <returns></returns>
		public Item GetItemCopy(string itemID)
		{
			Item item = GetItemReference(itemID);
			return item != null ? item.GetCopy() : null;
		}
	}
}
