using UnityEngine;

namespace DC.Items
{
    public enum StatType { Strength, Agility, Intelligence, Vitality }

    ///<summary>
    ///状态类型
    ///</summary>
    public class StatBuff : UsableItemEffect
    {
        public StatType StatType;               //状态类型
        public int BuffAmount;                  //buff数值
        public float Duration;                  //buff持续时间

        public override void ExecuteEffect(UsableItem parentItem, Character character)
        {
            switch (StatType)
            {
                case StatType.Strength:
                    break;
                case StatType.Agility:
                    break;
                case StatType.Intelligence:
                    break;
                case StatType.Vitality:
                    break;
                default:
                    break;
            }
        }

        public override string GetItemDescription()
        {
            throw new System.NotImplementedException();
        }
    }
}
