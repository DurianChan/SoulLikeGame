using UnityEngine;
using UnityEngine.UI;
using System.Text;

namespace DC.Items
{
    ///<summary>
    ///物品提示信息工具面板
    ///</summary>
    public class ItemTooltis : MonoBehaviour
    {
        [SerializeField] private Text ItemNameText = null;                 //物品名字
        [SerializeField] private Text ItemTypeText = null;                 //物品类型
        [SerializeField] private Text ItemDescriptionText = null;          //物品属性
        [Space]
        public float posx;
        public float posy;

        /// <summary>
        /// 显示提示工具面板
        /// </summary>
        /// <param name="item"></param>
        public void ShowTooltip(Item item, GameObject positionGO)
        {
            ItemNameText.text = item.ItemName;                                      //设置物品名字
            ItemTypeText.text = item.GetItemType();                                 //设置物品类型
            ItemDescriptionText.text = item.GetItemDescription();                   //获取物品描述

            if (gameObject.activeSelf == false)
                gameObject.SetActive(true);
        }

        /// <summary>
        /// 隐藏提示工具面板
        /// </summary>
        public void HideTooltip()
        {
            if (gameObject.activeSelf == true)
                gameObject.SetActive(false);
        }


    }
}
