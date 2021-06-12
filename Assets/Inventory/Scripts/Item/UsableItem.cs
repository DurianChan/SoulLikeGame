using UnityEngine;
using System.Collections.Generic;

namespace DC.Items
{
    ///<summary>
    ///使用物品类型
    ///</summary>
    [CreateAssetMenu(menuName = "Items/Usable Item")]
    public class UsableItem : Item
    {
        public bool IsConsumable;                           //判断是否为可消耗物品

        public List<UsableItemEffect> Effects;              //效果列表

        /// <summary>
        /// 使用物品
        /// </summary>
        /// <param name="c"></param>
        public virtual void Use(Character character)
        {
            foreach (UsableItemEffect effect in Effects)
            {
                effect.ExecuteEffect(this, character);
            }
        }

        /// <summary>
        /// 获取物品类型
        /// </summary>
        /// <returns></returns>
        public override string GetItemType()
        {
            return IsConsumable ? "Consumable" : "Usable";
        }

        /// <summary>
        /// 获取物品描述
        /// </summary>
        /// <returns></returns>
        public override string GetItemDescription()
        {
            sb.Length = 0;

            foreach (UsableItemEffect effect in Effects)
            {
                sb.AppendLine(effect.GetItemDescription());
            }

            return sb.ToString();
        }

    }
}
