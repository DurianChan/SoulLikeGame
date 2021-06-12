using UnityEngine;

namespace DC.Items
{
    ///<summary>
    ///影响生命值物品
    ///</summary>
    [CreateAssetMenu(menuName = "Item Effects/Heal Effect")]
    public class HealthItemEffect : UsableItemEffect
    {
        public int HealthAmount;                        //更变的生命值
        public override void ExecuteEffect(UsableItem parentItem, Character character)
        {
            character.am.sm.ChangeHealth(HealthAmount);
        }

        public override string GetItemDescription()
        {
            return "Heals for " + HealthAmount + " health.";
        }
    }
}
