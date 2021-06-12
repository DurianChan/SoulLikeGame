using UnityEngine;

namespace DC.Items
{
    ///<summary>
    ///物品箱子
    ///</summary>
    public class ItemChest : MonoBehaviour
    {
        [SerializeField] private Item item = null;
        [SerializeField] private int amount = 1;
        [SerializeField] private Inventory inventory = null;
        [SerializeField] private KeyCode itemPickupKeycode = KeyCode.E;

        public bool isInRange;                                             //是否在范围内
        public bool isEmpty;                                               //是否物品为空

        private void OnValidate()
        {
            if (inventory == null)
                inventory = FindObjectOfType<Inventory>();
        }

        private void Update()
        {
            if (isInRange && Input.GetKeyDown(itemPickupKeycode))
            {
                if (!isEmpty)
                {
                    Item itemCopy = item.GetCopy();
                    if (inventory.AddItem(itemCopy))
                    {
                        amount--;
                        if (amount == 0)
                        {
                            //isEmpty = true;
                            Invoke("HideTips", 0.5f);
                            Invoke("ItemDestory", 1f);
                        }
                    }
                    else
                    {
                        itemCopy.Destory();
                    }
                }
            }
        }

        private void HideTips()
        {
            GetComponent<Guide>().HideTips();
        }

        private void ItemDestory()
        {
            Destroy(gameObject);
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
                isInRange = state;
        }

    }
}
