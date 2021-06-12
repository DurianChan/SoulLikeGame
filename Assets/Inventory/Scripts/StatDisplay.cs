using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DC.Items
{
    ///<summary>
    ///状态信息展示
    ///</summary>
    public class StatDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private CharacterStat _stat;
        public CharacterStat Stat
        {
            get { return _stat; }
            set
            {
                _stat = value;
                UpdateStatValue();
            }
        }

        private string _name;
        public string Name 
        {
            get { return _name; }
            set
            {
                _name = value;
                nameText.text = _name;                    //转换为小写
            }
        }

        [SerializeField] private Text nameText;
        [SerializeField] private Text valueText;
        [SerializeField] private StatTooltip tooltip;

        private void OnValidate()
        {
            Text[] texts = GetComponentsInChildren<Text>();
            nameText = texts[0];
            valueText = texts[1];

            if (tooltip == null)
                tooltip = FindObjectOfType<StatTooltip>();
        }

        /// <summary>
        /// 更新属性数值
        /// </summary>
        public void UpdateStatValue()
        {
            valueText.text = _stat.Value.ToString();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tooltip.ShowTooltip(Stat,Name);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltip.HideTooltip();
        }

    }
}
