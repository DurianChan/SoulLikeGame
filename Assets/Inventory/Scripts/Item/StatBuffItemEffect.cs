using System.Collections;
using UnityEngine;

namespace DC.Items
{
    ///<summary>
    ///影响Buff状态
    ///</summary>
    [CreateAssetMenu(menuName ="Item Effects/Stat Buff")]
    public class StatBuffItemEffect : UsableItemEffect
    {
        public int AgilityBuff;         //敏捷buff
        public float Duration;          //Buff持续时间


        public override void ExecuteEffect(UsableItem parentItem, Character character)
        {
            StatModifier statModifier = new StatModifier(AgilityBuff, StatModType.Flat, parentItem);
            character.Agility.AddModifier(statModifier);
            character.StartCoroutine(RemoveBuff(character,statModifier,Duration));
            character.UpdateStatValues();
        }

        public override string GetItemDescription()
        {
            return "Grants " + AgilityBuff + "Agility for " + Duration + " seconds.";
        }

        /// <summary>
        /// 持续时间施加buff数值
        /// </summary>
        /// <param name="character"></param>
        /// <param name="statModifier"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        private static IEnumerator RemoveBuff(Character character, StatModifier statModifier, float duration)
        {
            yield return new WaitForSeconds(duration);
            character.Agility.RemoveModifier(statModifier);             //当超过buff持续时间后消除增加buff的数值
            character.UpdateStatValues();
        }
    }
}
