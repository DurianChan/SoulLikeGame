using UnityEngine;
using UnityEngine.UI;

namespace DC.Items
{
    ///<summary>
    ///角色管理
    ///</summary>
    public class Character : MonoBehaviour
    {
        [Header("=== 库存装备栏 ===")]
        public Inventory Inventory = null;                            //库存
        public EquipmentPanel EquipmentPanel = null;                  //装备面板
        public QuickSlotsPanel QuickSlotsPanel = null;                //快捷栏
        [Space]
        public ActorManager am = null;                                                  //角色管理
        [SerializeField] private StatPanel statPanel = null;                            //属性面板 
        [SerializeField] private ItemTooltis itemTooltip = null;                        //物品提示面板 
        [SerializeField] private CraftingWindow craftingWindow = null;                  //制表窗口
        [SerializeField] private ItemContainer openItemContainer;                       //物品容器
        [SerializeField] private ItemContainer QickSlotItemContainer;                   //快捷键栏
        [SerializeField] private DropItemArea dropItemArea = null;                      //丢弃物品区域
        [SerializeField] private QuestionDialog questionDialog = null;                  //问题提示
        [SerializeField] private SaveLoadPos playerSavePos;                             //重置位置
        [SerializeField] private Vector3 InitSavePos = new Vector3(4, 1, 11);           //初始存档点位置


        [Header("=== 角色属性 ===")]
        public CharacterStat Strength;
        public CharacterStat Agility;
        public CharacterStat Intelligence;
        public CharacterStat Vitality;

        [Header("=== 成员变量 ===")]
        [SerializeField] private Image draggableItem = null;                           //移动的物品图片
        [SerializeField] private BaseItemSlot dragItemSlot;                            //被拖拽移动插槽

        [Space]
        public GameObject InventoryPanelGO;
        public GameObject EquipmentPanelGO;
        public PlayerItemDataSave PlayerItemDataSave;

        private int sign = 1;       //载入存档初始化

        private void OnValidate()
        {
            if (itemTooltip == null)
                itemTooltip = FindObjectOfType<ItemTooltis>();

            //if (am == null)
            //    am = GameManager._instance.playerAM;

            if (playerSavePos == null)
                playerSavePos = FindObjectOfType<SaveLoadPos>();
        }

        public void Awake()
        {
            //print(Application.persistentDataPath);
            statPanel.SetStats(Strength, Agility, Intelligence, Vitality);          //设置属性
            statPanel.UpdateStatValues();                                           //更新属性值

            //Setup Events:
            //Left/Right Click
            Inventory.OnLeftClickEvent += InventoryLeftClick;
            Inventory.OnRightClickEvent += InventoryRightClick;
            EquipmentPanel.OnRightClickEvent += EquipmentPanelRightClick;
            //Pointer Enter
            Inventory.OnPointerEnterEvent += ShowTooltip;
            EquipmentPanel.OnPointerEnterEvent += ShowTooltip;
            craftingWindow.OnPointerEnterEvent += ShowTooltip;
            //Pointer Exit
            Inventory.OnPointerExitEvent += HideTooltip;
            EquipmentPanel.OnPointerExitEvent += HideTooltip;
            craftingWindow.OnPointerExitEvent += HideTooltip;
            //Begin Drag
            Inventory.OnBeginDragEvent += BeginDrag;
            EquipmentPanel.OnBeginDragEvent += BeginDrag;
            //End Drag
            Inventory.OnEndDragEvent += EndDrag;
            EquipmentPanel.OnEndDragEvent += EndDrag;
            //Drag
            Inventory.OnDragEvent += Drag;
            EquipmentPanel.OnDragEvent += Drag;
            //Drop
            Inventory.OnDropEvent += Drop;
            EquipmentPanel.OnDropEvent += Drop;
            dropItemArea.OnDropEvent += DropItemOutsideUI;
        }

        private void LateUpdate()
        {
            if (sign == 1)
            {
                sign = 0;
                InventoryPanelGO.SetActive(true);
                EquipmentPanelGO.SetActive(true);
                //载入玩家装备信息
                PlayerItemDataSave.LoadInventory(this);
                PlayerItemDataSave.LoadEquipment(this);
                PlayerItemDataSave.LoadQuickSlot(this);
                //载入玩家位置
                PlayerItemDataSave.LoadRebirthPosition(playerSavePos);
                if (playerSavePos.currentPos[0] == Vector3.zero)
                {
                    playerSavePos.currentPos[0] = InitSavePos;
                    PlayerItemDataSave.SaveRebirthPosition(playerSavePos);
                    am.gameObject.transform.position = InitSavePos;                
                }                 
                else
                {
                    am.gameObject.transform.position = playerSavePos.currentPos[0];
                }
                   

                InventoryPanelGO.SetActive(false);
                EquipmentPanelGO.SetActive(false);
            }
        }

        /// <summary>
        /// 库存右击物品使用事件方法
        /// </summary>
        /// <param name="itemSlot"></param>
        private void InventoryRightClick(BaseItemSlot itemSlot)
        {
            if (itemSlot.Item is EquippableItem)
            {
                EquippableItem equippableItem = itemSlot.Item as EquippableItem;
                if (equippableItem.isWeaponHand)
                    equippableItem.EquipmentType = EquipmentType.WeaponR;
            }

            if (itemSlot.Item is EquippableItem)                    //判断该事物若是装备时，右击则触发装备
            {
                Equip((EquippableItem)itemSlot.Item);
            }
            else if (itemSlot.Item is UsableItem)                   //若为可使用型物品，则右击触发使用
            {
                UsableItem usableItem = (UsableItem)itemSlot.Item;
                usableItem.Use(this);

                if (usableItem.IsConsumable)                         //判断可使用物品是否为消耗品
                {
                    Inventory.RemoveItem(usableItem);
                    usableItem.Destory();
                }
            }
        }

        private void InventoryLeftClick(BaseItemSlot itemSlot)
        {
            if (itemSlot.Item is EquippableItem)
            {
                EquippableItem equippableItem = itemSlot.Item as EquippableItem;
                if (equippableItem.isWeaponHand && !equippableItem.isDual)
                    equippableItem.EquipmentType = EquipmentType.WeaponL;
            }

            if (itemSlot.Item is EquippableItem)                    //判断该事物若是装备时，右击则触发装备
            {
                Equip((EquippableItem)itemSlot.Item);
            }
        }

        /// <summary>
        /// 装备栏右击使用事件方法
        /// </summary>
        /// <param name="itemSlot"></param>
        private void EquipmentPanelRightClick(BaseItemSlot itemSlot)
        {
            if (itemSlot.Item is EquippableItem)                    //判断该事物若是装备时，右击则触发装备
            {
                Unequip((EquippableItem)itemSlot.Item);
            }

        }

        /// <summary>
        /// 穿上装备
        /// </summary>
        /// <param name="item"></param>
        public void Equip(EquippableItem item)
        {
            if (Inventory.RemoveItem(item))         //从库存中移除物品
            {
                EquippableItem previousItem;
                if (EquipmentPanel.AddItem(item, out previousItem))      //将物品装上装备栏
                {
                    if (previousItem != null)                            //如果原装备栏上有物品
                    {
                        Inventory.AddItem(previousItem);                //将原物品添加到库存中

                        UnequipWeapon(previousItem);

                        previousItem.Unequip(this);                     //通过卸下上物品改变人物属性
                        statPanel.UpdateStatValues();
                    }

                    EquipWeapon(item);
                    EquippableItem equippableItem;
                    if (item.isDual && EquipmentPanel.equipmentSlots[1].Item != null)
                    {
                        equippableItem = (EquippableItem)EquipmentPanel.equipmentSlots[1].Item;         //左手武器
                        Unequip(equippableItem);
                        UnequipWeapon(equippableItem);
                        statPanel.UpdateStatValues();
                    }
                    else if (item.EquipmentType == EquipmentType.WeaponL && EquipmentPanel.equipmentSlots[2].Item != null)
                    {
                        equippableItem = (EquippableItem)EquipmentPanel.equipmentSlots[2].Item;         //右手武器
                        if (equippableItem.isDual)
                        {
                            Unequip(equippableItem);
                            UnequipWeapon(equippableItem);
                            statPanel.UpdateStatValues();
                        }
                    }

                    item.Equip(this);                                   //通过装备上物品改变人物属性
                    statPanel.UpdateStatValues();
                }
                else
                {
                    Inventory.AddItem(item);                            //无法装备上的物品添加回物品
                }
            }
        }

        /// <summary>
        /// 卸下装备
        /// </summary>
        public void Unequip(EquippableItem item)
        {
            if (Inventory.CanAddItem(item) && EquipmentPanel.RemoveItem(item))     //当库存不满且从装备栏中移除物品
            {
                item.Unequip(this);                             //通过卸下上物品改变人物属性

                UnequipWeapon(item);

                statPanel.UpdateStatValues();
                Inventory.AddItem(item);                                    //将卸下的物品添加回库存
            }
        }

        /// <summary>
        /// 显示提示
        /// </summary>
        /// <param name="itemSlot"></param>
        private void ShowTooltip(BaseItemSlot itemSlot)
        {
            if (itemSlot.Item != null)
            {
                itemTooltip.ShowTooltip(itemSlot.Item, itemSlot.positioningImage);
            }
        }

        /// <summary>
        /// 隐藏提示
        /// </summary>
        /// <param name="itemSlot"></param>
        private void HideTooltip(BaseItemSlot itemSlot)
        {
            itemTooltip.HideTooltip();
        }

        /// <summary>
        /// 开始拖拽物品方法
        /// </summary>
        /// <param name="itemSlot"></param>
        private void BeginDrag(BaseItemSlot itemSlot)
        {
            if (itemSlot.Item != null)
            {
                dragItemSlot = itemSlot;
                draggableItem.sprite = itemSlot.Item.Icon;
                draggableItem.transform.position = Input.mousePosition;
                draggableItem.enabled = true;
            }
        }

        /// <summary>
        /// 结束拖拽方法
        /// </summary>
        /// <param name="itemSlot"></param>
        private void EndDrag(BaseItemSlot itemSlot)
        {
            dragItemSlot = null;
            draggableItem.enabled = false;
        }

        /// <summary>
        /// 拖拽物品方法
        /// </summary>
        /// <param name="itemSlot"></param>
        private void Drag(BaseItemSlot itemSlot)
        {
            if (draggableItem.enabled)
            {
                draggableItem.transform.position = Input.mousePosition;
            }
        }

        /// <summary>
        /// 推动物品方法
        /// </summary>
        /// <param name="itemSlot"></param>
        private void Drop(BaseItemSlot dropItemSlot)
        {
            if (dragItemSlot == null) return;

            if (dropItemSlot.CanAddStack(dragItemSlot.Item))//can add stack of dragItemSlot.Item to dropItemSlot 
            {
                AddStacks(dropItemSlot);
            }
            //检查是否物品为物品且物品是否为空，并转换为装备类型
            else if (dropItemSlot.CanReceiveItem(dragItemSlot.Item) && dragItemSlot.CanReceiveItem(dropItemSlot.Item))
            {
                SwapItems(dropItemSlot);
            }
        }

        /// <summary>
        /// 将物品拖放到UI外
        /// </summary>
        private void DropItemOutsideUI()
        {
            if (dragItemSlot == null) return;

            questionDialog.Show();
            BaseItemSlot baseItemSlot = dragItemSlot;
            questionDialog.OnYesEvent += () => DestoryItemInSlot(baseItemSlot);
        }

        /// <summary>
        /// 销毁插槽中的物品
        /// </summary>
        /// <param name="baseItemSlot"></param>
        private void DestoryItemInSlot(BaseItemSlot baseItemSlot)
        {
            // If the item is equiped, unequip first
            if (baseItemSlot is EquipmentSlot)
            {
                EquippableItem equippableItem = (EquippableItem)baseItemSlot.Item;
                equippableItem.Unequip(this);
                UnequipWeapon(equippableItem);
            }

            baseItemSlot.Item.Destory();
            baseItemSlot.Item = null;
        }

        /// <summary>
        /// 交换两物品位置
        /// </summary>
        /// <param name="dropItemSlot"></param>
        private void SwapItems(BaseItemSlot dropItemSlot)
        {
            EquippableItem dragItem = dragItemSlot.Item as EquippableItem;
            EquippableItem dropItem = dropItemSlot.Item as EquippableItem;

            if (dragItemSlot is EquipmentSlot)
            {
                //正在拖拽移动中，卸下物品的装备,并装备上放下的物品
                if (dragItem != null)
                {
                    dragItem.Unequip(this);
                    UnequipWeapon(dragItem);
                }

                if (dropItem != null)
                {
                    dropItem.Equip(this);
                    EquipWeapon(dropItem);
                }
            }
            if (dropItemSlot is EquipmentSlot)
            {
                if (dragItem != null)
                {
                    dragItem.Equip(this);
                    EquipWeapon(dragItem);
                }
                if (dropItem != null)
                {
                    dropItem.Unequip(this);
                    UnequipWeapon(dropItem);
                }
            }
            statPanel.UpdateStatValues();                           //更新数值

            Item draggedItem = dragItemSlot.Item;
            int draggedItemAmount = dragItemSlot.Amount;

            dragItemSlot.Item = dropItemSlot.Item;
            dragItemSlot.Amount = dropItemSlot.Amount;

            dropItemSlot.Item = draggedItem;
            dropItemSlot.Amount = draggedItemAmount;
        }

        /// <summary>
        /// 更变物品堆叠数
        /// </summary>
        /// <param name="dropItemSlot"></param>
        private void AddStacks(BaseItemSlot dropItemSlot)
        {
            //Add stacks until dropItemSlot is full
            //Remove the same number of stacks from dragItemSlot
            int numAddaboleStacks = dropItemSlot.Item.MaximumStacks - dropItemSlot.Amount;          //检查拖放物品剩余堆叠数
            int stacksToAdd = Mathf.Min(numAddaboleStacks, dragItemSlot.Amount);                    //取剩余堆叠数以及拖放数的最小值

            dropItemSlot.Amount += stacksToAdd;                                                     //添加堆叠数
            dragItemSlot.Amount -= stacksToAdd;                                                     //减去拖拽物品的数量
        }

        /// <summary>
        /// 更新状态数值
        /// </summary>
        public void UpdateStatValues()
        {
            statPanel.UpdateStatValues();
        }

        /// <summary>
        /// 打开物品存储容器
        /// </summary>
        /// <param name="itemContainer"></param>
        public void OpenItemContainer(ItemContainer itemContainer)
        {
            openItemContainer = itemContainer;

            Inventory.OnLeftClickEvent -= InventoryLeftClick;
            Inventory.OnRightClickEvent -= InventoryRightClick;
            Inventory.OnRightClickEvent += TransferToItemContainer;

            itemContainer.OnRightClickEvent += TransferToInventory;

            itemContainer.OnPointerEnterEvent += ShowTooltip;
            itemContainer.OnPointerExitEvent += HideTooltip;
            itemContainer.OnBeginDragEvent += BeginDrag;
            itemContainer.OnEndDragEvent += EndDrag;
            itemContainer.OnDragEvent += Drag;
            itemContainer.OnDropEvent += Drop;
        }

        /// <summary>
        /// 关闭物品存储容器
        /// </summary>
        /// <param name="itemContainer"></param>
        public void CloseItemContainer(ItemContainer itemContainer)
        {
            openItemContainer = null;

            Inventory.OnLeftClickEvent += InventoryLeftClick;
            Inventory.OnRightClickEvent += InventoryRightClick;
            Inventory.OnRightClickEvent -= TransferToItemContainer;

            itemContainer.OnRightClickEvent -= TransferToInventory;

            itemContainer.OnPointerEnterEvent -= ShowTooltip;
            itemContainer.OnPointerExitEvent -= HideTooltip;
            itemContainer.OnBeginDragEvent -= BeginDrag;
            itemContainer.OnEndDragEvent -= EndDrag;
            itemContainer.OnDragEvent -= Drag;
            itemContainer.OnDropEvent -= Drop;
        }

        /// <summary>
        /// 将库存物品转移到其他物品容器中
        /// </summary>
        /// <param name="baseItemSlot"></param>
        private void TransferToItemContainer(BaseItemSlot itemSlot)
        {
            Item item = itemSlot.Item;
            if (item != null && openItemContainer.CanAddItem(item))
            {
                Inventory.RemoveItem(item);
                openItemContainer.AddItem(item);
            }
        }

        /// <summary>
        /// 将其他容器中物品转移到库存中
        /// </summary>
        /// <param name="baseItemSlot"></param>
        private void TransferToInventory(BaseItemSlot itemSlot)
        {
            Item item = itemSlot.Item;
            if (item != null && Inventory.CanAddItem(item))
            {
                openItemContainer.RemoveItem(item);
                Inventory.AddItem(item);
            }
        }

        /// <summary>
        /// 打开快捷栏
        /// </summary>
        /// <param name="itemContainer"></param>
        public void OpenQuickSlot(ItemContainer itemContainer)
        {
            QickSlotItemContainer = itemContainer;
            Inventory.OnLeftClickEvent -= InventoryRightClick;
            Inventory.OnLeftClickEvent += TransferToQuickSlot;

            itemContainer.OnRightClickEvent += QuickSlotTransferToInventory;

            itemContainer.OnPointerEnterEvent += ShowTooltip;
            itemContainer.OnPointerExitEvent += HideTooltip;
            itemContainer.OnBeginDragEvent += BeginDrag;
            itemContainer.OnEndDragEvent += EndDrag;
            itemContainer.OnDragEvent += Drag;
            itemContainer.OnDropEvent += Drop;
        }

        /// <summary>
        /// 关闭打开快捷栏
        /// </summary>
        /// <param name="itemContainer"></param>
        public void CloseQuickSlot(ItemContainer itemContainer)
        {
            QickSlotItemContainer = null;
            Inventory.OnLeftClickEvent += InventoryRightClick;
            Inventory.OnLeftClickEvent -= TransferToQuickSlot;

            itemContainer.OnRightClickEvent -= QuickSlotTransferToInventory;

            itemContainer.OnPointerEnterEvent -= ShowTooltip;
            itemContainer.OnPointerExitEvent -= HideTooltip;
            itemContainer.OnBeginDragEvent -= BeginDrag;
            itemContainer.OnEndDragEvent -= EndDrag;
            itemContainer.OnDragEvent -= Drag;
            itemContainer.OnDropEvent -= Drop;
        }

        /// <summary>
        /// 将库存物品转移到快捷栏
        /// </summary>
        /// <param name="baseItemSlot"></param>
        private void TransferToQuickSlot(BaseItemSlot itemSlot)
        {
            Item item = itemSlot.Item;
            if (item is UsableItem)
            {
                if (item != null && QickSlotItemContainer.CanAddItem(item))
                {
                    Inventory.RemoveItem(item);
                    QickSlotItemContainer.AddItem(item);
                }
            }
        }

        /// <summary>
        /// 将快捷栏中物品转移到库存中
        /// </summary>
        /// <param name="baseItemSlot"></param>
        private void QuickSlotTransferToInventory(BaseItemSlot itemSlot)
        {
            Item item = itemSlot.Item;
            if (item != null && Inventory.CanAddItem(item))
            {
                QickSlotItemContainer.RemoveItem(item);
                Inventory.AddItem(item);
            }
        }

        /// <summary>
        /// 同步装备武器
        /// </summary>
        /// <param name="equippableItem"></param>
        private void EquipWeapon(EquippableItem equippableItem)
        {
            if (am != null)
            {
                if (equippableItem.EquipmentType == EquipmentType.WeaponL)
                    am.wm.UpdateLeftWeapon(GameManager._instance.weaponFact, equippableItem.ItemName, equippableItem.isDual, equippableItem.isShield);
                else if (equippableItem.EquipmentType == EquipmentType.WeaponR)
                    am.wm.UpdateRightWeapon(GameManager._instance.weaponFact, equippableItem.ItemName, equippableItem.isDual);
            }
        }

        /// <summary>
        /// 同步卸下武器
        /// </summary>
        /// <param name="equippableItem"></param>
        private void UnequipWeapon(EquippableItem equippableItem)
        {
            if (am != null)
            {
                if (equippableItem.EquipmentType == EquipmentType.WeaponL)
                    am.wm.UnloadWeapon("L");
                else if (equippableItem.EquipmentType == EquipmentType.WeaponR)
                    am.wm.UnloadWeapon("R");
            }
        }

        /// <summary>
        /// 加载装备存储数据
        /// </summary>
        /// <param name="equippableItem"></param>
        public EquippableItem LoadDataWithWeapon(EquippableItem equippableItem)
        {
            foreach (EquipmentSlot equipmentSlot in EquipmentPanel.equipmentSlots)
            {
                if (equipmentSlot.name.Equals("WeaponL Slot") && equipmentSlot.Item != null)        //左手槽已满，将武器改为右手属性
                {
                    equippableItem.EquipmentType = EquipmentType.WeaponR;
                }
                if (equipmentSlot.name.Equals("WeaponR Slot") && equipmentSlot.Item != null)     //左手槽已满，将武器改为左手属性
                {
                    equippableItem.EquipmentType = EquipmentType.WeaponL;
                }
            }
            return equippableItem;
        }
    }
}
