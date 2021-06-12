using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DC.Items
{
    ///<summary>
    ///属性提示工具面板
    ///</summary>
    public class StatTooltip : MonoBehaviour
    {
        [SerializeField] private Text StatNameText = null;                          //属性名字
        [SerializeField] private Text StatModifiersLabelText = null;                //属性种类
        [SerializeField] private Text StatModifiersText = null;                     //属性描述

        private StringBuilder sb = new StringBuilder();

        /// <summary>
        /// 显示属性面板提示
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="statName"></param>
        public void ShowTooltip(CharacterStat stat,string statName)
        {
            StatNameText.text = GetStatTopText(stat,statName);                   //设置属性名字

            StatModifiersLabelText.text = GetStatModifiersText(stat);            //设置属性加成数值以及物品

            StatModifiersText.text = "";

            gameObject.SetActive(true);
        }

        /// <summary>
        /// 隐藏属性面板提示
        /// </summary>
        public void HideTooltip()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 获得属性面板首字段
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="statName"></param>
        private string GetStatTopText(CharacterStat stat,string statName)
        {
            sb.Length = 0;
            sb.Append(statName);
            sb.Append(" ");
            sb.Append(stat.Value);

            if(stat.Value != stat.BaseValue)
            {
                sb.Append(" (");
                sb.Append(stat.BaseValue);

                if (stat.Value > stat.BaseValue)                    //判断是否大于基本数值，是则为正数
                    sb.Append("+");

                sb.Append(System.Math.Round(stat.Value - stat.BaseValue,4));
                sb.Append(")");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取属性加成数值以及物品
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        private string GetStatModifiersText(CharacterStat stat)
        {
            sb.Length = 0;

            foreach (StatModifier mod in stat.StatModifiers)
            {
                if (sb.Length > 0)
                    sb.AppendLine();

                if (mod.Value > 0)                                          //数值为正则加号
                    sb.Append("+");

                if(mod.Type == StatModType.Flat)
                {
                    sb.Append(mod.Value);
                }
                else
                {
                    sb.Append(mod.Value * 100);
                    sb.Append("%");
                }

                Item item = mod.Source as Item;         //获取数值来源的物品 

                if (item!=null)
                {
                    sb.Append(" ");
                    sb.Append(item.ItemName);                               //获取物品的名字
                }
                else
                {
                    Debug.LogError("Modifier is not an Item!");
                }
            }

            return sb.ToString();
        }

    }
}
