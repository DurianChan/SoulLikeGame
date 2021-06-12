using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace DC.Items
{
    ///<summary>
    ///物品插槽基类
    ///</summary>
    public class BaseItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected Image image = null;               //物品图片
        [SerializeField] protected Text amountText = null;           //物品堆叠数量文本

        public GameObject positioningImage = null;

        public event Action<BaseItemSlot> OnPointerEnterEvent;
        public event Action<BaseItemSlot> OnPointerExitEvent;
        public event Action<BaseItemSlot> OnRightClickEvent;
        public event Action<BaseItemSlot> OnLeftClickEvent;

        protected bool isPointerOver;

        private Color unworkedColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        private Color normalColor = Color.white;

        protected Item _item;
        public Item Item
        {
            get { return _item; }
            set
            {
                _item = value;
                if (_item == null && Amount != 0) Amount = 0;

                if (_item == null)
                {
                    image.sprite = null;
                    image.color = unworkedColor;
                }
                else
                {
                    image.sprite = _item.Icon;
                    image.color = normalColor;
                }

                if (isPointerOver)
                {
                    OnPointerExit(null);
                    OnPointerEnter(null);
                }
            }
        }

        private int _amount;        //物品数量
        public int Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                if (_amount < 0) _amount = 0;
                if (_amount == 0 && Item != null) Item = null;

                if (amountText != null)
                {
                    amountText.enabled = _item != null && _amount > 1;
                    if (amountText.enabled)
                    {
                        amountText.text = _amount.ToString();
                    }
                }
            }
        }

        protected virtual void OnValidate()         //挂载脚本时执行
        {
            if (image == null)
                image = GetComponent<Image>();

            try { positioningImage = transform.parent.gameObject.transform.parent.gameObject; } catch(Exception e) { e.ToString(); }
            

            if (amountText == null)
                amountText = GetComponentInChildren<Text>();

            Item = _item;
            Amount = _amount;
        }

        protected virtual void OnDisable()
        {
            if (isPointerOver)
            {
                OnPointerExit(null);
            }
        }

        /// <summary>
        /// 判断能否进行物品堆叠
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public virtual bool CanAddStack(Item item, int amount = 1)
        {
            return Item != null && Item.ID == item.ID;
        }

        /// <summary>
        /// 判断是否接收到物品类型
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool CanReceiveItem(Item item)
        {
            return true;
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData != null && eventData.button == PointerEventData.InputButton.Left)
            {
                if (OnLeftClickEvent != null)
                    OnLeftClickEvent(this);
            }

            if (eventData != null && eventData.button == PointerEventData.InputButton.Right)
            {
                if (OnRightClickEvent != null)
                    OnRightClickEvent(this);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isPointerOver = true;

            if (OnPointerEnterEvent != null)
                OnPointerEnterEvent(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isPointerOver = false;

            if (OnPointerExitEvent != null)
                OnPointerExitEvent(this);
        }

    }
}
