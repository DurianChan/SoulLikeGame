using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC.Items
{
    ///<summary>
    ///存储物品的箱子
    ///</summary>
    public class ItemStash : ItemContainer
    {
        [SerializeField] private Transform itemsParent = null;              //物品父物体位置 
        [SerializeField] private Character character;
        [SerializeField] private KeyCode openKeyCode = KeyCode.E;

        private bool isOpen;                                                //判读是否打开
        private bool isInRange;                                             //是否在范围内

        protected override  void OnValidate()
        {


            if (itemsParent != null)
                 itemsParent.GetComponentsInChildren(includeInactive: true, result: ItemSlots);

            if (character == null)
                character = FindObjectOfType<Character>();


        }

        protected override void Awake()
        {
            base.Awake();
            itemsParent.gameObject.SetActive(false);
        }

        private void Update()
        {
           if(isInRange && Input.GetKeyDown(openKeyCode))
            {
                isOpen = !isOpen;
                itemsParent.gameObject.SetActive(isOpen);

                if (isOpen)
                    character.OpenItemContainer(this);
                else
                    character.CloseItemContainer(this);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckCollision(other.gameObject, true);
        }

        private void OnTriggerExit(Collider other)
        {
            CheckCollision(other.gameObject, false);
        }

        /// <summary>
        /// 检测玩家触发
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="state"></param>
        private void CheckCollision(GameObject gameObject, bool state)
        {
            if (gameObject.CompareTag("Player"))
            {
                isInRange = state;
                Character c = character;

                if (!isInRange && isOpen)
                {
                    isOpen = false;
                    itemsParent.gameObject.SetActive(false);
                    c.CloseItemContainer(this);
                }

                if (isInRange)
                    c = character;
                else
                    c = null;
            }
                
        }
    }
}
