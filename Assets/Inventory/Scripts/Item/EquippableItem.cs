using UnityEngine;

namespace DC.Items
{
    /// <summary>
    /// 装备类型
    /// </summary>
    public enum EquipmentType
    {
        Helmet,
        Chest,
        WeaponL,
        WeaponR,
        Boot
    }

    ///<summary>
    ///装备类型物品
    ///</summary>
    [CreateAssetMenu(menuName ="Items/Equippable Item")]
    public class EquippableItem : Item
    {
        public int StrengthBonus;           //力量
        public int AgilityBonus;            //敏捷
        public int IntelligenceBonus;       //智力
        public int VitalityBonus;           //体力
        [Space]                                      
        public float StrengthPercentBonus;           //力量百分比 
        public float AgilityPercentBonus;            //敏捷百分比
        public float IntelligencePercentBonus;       //智力百分比
        public float VitalityPercentBonus;           //体力百分比
        [Space]
        public EquipmentType EquipmentType;          //装备类型
        public bool isWeaponHand;                    //是否为手持武器
        public bool isShield;                        //是否为护盾
        public bool isDual;                          //是否为双手武器
        

        /// <summary>
        /// 实例化一个新的物品
        /// </summary>
        /// <returns></returns>
        public override Item GetCopy()
        {
            return Instantiate(this);
        }

        /// <summary>
        /// 销毁物品
        /// </summary>
        public override void Destory()
        {
            Destroy(this);
        }

        /// <summary>
        /// 装备物品
        /// </summary>
        /// <param name="c"></param>
        public void Equip(Character c)
        {
            //装备上时改变角色属性值
            if (StrengthBonus != 0)
                c.Strength.AddModifier(new StatModifier(StrengthBonus, StatModType.Flat,this));
            if (AgilityBonus != 0)
                c.Agility.AddModifier(new StatModifier(AgilityBonus, StatModType.Flat, this));
            if (IntelligenceBonus != 0)
                c.Intelligence.AddModifier(new StatModifier(IntelligenceBonus, StatModType.Flat, this));
            if (VitalityBonus != 0)
                c.Vitality.AddModifier(new StatModifier(VitalityBonus, StatModType.Flat, this));

            //按百分比改变属性值
            if (StrengthPercentBonus != 0)
                c.Strength.AddModifier(new StatModifier(StrengthPercentBonus, StatModType.PercentMult, this));
            if (AgilityPercentBonus != 0)
                c.Agility.AddModifier(new StatModifier(AgilityPercentBonus, StatModType.PercentMult, this));
            if (IntelligencePercentBonus != 0)
                c.Intelligence.AddModifier(new StatModifier(IntelligencePercentBonus, StatModType.PercentMult, this));
            if (VitalityPercentBonus != 0)
                c.Vitality.AddModifier(new StatModifier(VitalityPercentBonus, StatModType.PercentMult, this));
        }

        /// <summary>
        /// 卸下物品
        /// </summary>
        /// <param name="c"></param>
        public void Unequip(Character c)
        {
            //移除物品上的所增加的属性
            c.Strength.RemoveAllModifiersFromSource(this);
            c.Agility.RemoveAllModifiersFromSource(this);
            c.Intelligence.RemoveAllModifiersFromSource(this);
            c.Vitality.RemoveAllModifiersFromSource(this);
        }

        /// <summary>
        /// 获取物品类型
        /// </summary>
        /// <returns></returns>
        public override string GetItemType()
        {
            return EquipmentType.ToString();
        }

        /// <summary>
        /// 获取物品描述
        /// </summary>
        /// <returns></returns>
        public override string GetItemDescription()
        {
            sb.Length = 0;                                                          //设置物品的属性
            AddStat(StrengthBonus, "Strength");
            AddStat(AgilityBonus, "Agility");
            AddStat(IntelligenceBonus, "Intelligence");
            AddStat(VitalityBonus, "Vitality");

            AddStat(StrengthPercentBonus, "Strength", isPercent: true);        //设置物品的属性百分比
            AddStat(AgilityPercentBonus, "Agility", isPercent: true);
            AddStat(IntelligencePercentBonus, "Intelligence", isPercent: true);
            AddStat(VitalityPercentBonus, "Vitality", isPercent: true);
            return sb.ToString();
        }

        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="value"></param>
        /// <param name="statName"></param>
        private void AddStat(float value, string statName, bool isPercent = false)
        {
            if (value != 0)
            {
                if (sb.Length > 0)                                          //当前行已有数据即换行
                    sb.AppendLine();

                if (value > 0)                                              //当数值大于0为加号
                    sb.Append("+");

                if (isPercent)                                              //显示百分比
                {
                    sb.Append(value * 100);
                    sb.Append("% ");
                }
                else
                {
                    sb.Append(value);                                       //正常数值显示
                    sb.Append(" ");
                }

                sb.Append(statName);
            }
        }
    }
}
