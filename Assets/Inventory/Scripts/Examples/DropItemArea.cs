using UnityEngine.EventSystems;
using UnityEngine;
using System;

namespace DC.Items
{
    ///<summary>
    ///丢弃物品区域
    ///</summary>
    public class DropItemArea : MonoBehaviour, IDropHandler
    {
        public event Action OnDropEvent;

        public void OnDrop(PointerEventData eventData)
        {
            if (OnDropEvent != null)
                OnDropEvent();
        }
    }
}
