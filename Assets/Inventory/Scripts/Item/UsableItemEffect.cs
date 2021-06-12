using UnityEngine;

namespace DC.Items
{
    ///<summary>
    ///使用物品效果
    ///</summary>
    public abstract class UsableItemEffect : ScriptableObject
    {
        /// <summary>
        /// 物品执行效果
        /// </summary>
        /// <param name="parentItem"></param>
        /// <param name="character"></param>
        public abstract void ExecuteEffect(UsableItem parentItem,Character character);

        /// <summary>
        /// 获取描述
        /// </summary>
        /// <returns></returns>
        public abstract string GetItemDescription();
    }
}
