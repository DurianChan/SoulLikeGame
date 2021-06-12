using UnityEngine;

namespace DC.Items
{
    ///<summary>
    ///库存输入
    ///</summary>
    public class InventoryInput : MonoBehaviour
    {
        public GameObject TipsCanvas = null;
        [SerializeField] private GameObject InventoryPanelGO = null;
        [SerializeField] private GameObject EquipmentPanelGO = null;
        [SerializeField] private GameObject craftingPanelGameObject = null;
        [SerializeField] private KeyCode[] toggleCharacterPanelKeys = null;

        [SerializeField] private IUserInput playerInput;
        [SerializeField] private bool openingSign = false;

        private void Start()
        {
            HideMouseCursor();
            playerInput = GameManager._instance.playerAM.ac.pi;
        }

        private void Update()
        {
            for (int i = 0; i < toggleCharacterPanelKeys.Length; i++)
            {
                if (Input.GetKeyDown(toggleCharacterPanelKeys[i]))
                {
                    if (InventoryPanelGO.activeSelf && EquipmentPanelGO.activeSelf && craftingPanelGameObject.activeSelf)
                    {
                        ToggleInventoryPanel();
                        ToggleEquipmentPanel();
                        ToggleCraftingPanel();
                    }
                    else if (InventoryPanelGO.activeSelf && EquipmentPanelGO.activeSelf)
                    {
                        ToggleInventoryPanel();
                        ToggleEquipmentPanel();
                    }
                    else if (InventoryPanelGO.activeSelf && craftingPanelGameObject.activeSelf)
                    {
                        ToggleInventoryPanel();
                        ToggleCraftingPanel();
                    }
                    else if (EquipmentPanelGO.activeSelf && craftingPanelGameObject.activeSelf)
                    {
                        ToggleEquipmentPanel();
                        ToggleCraftingPanel();
                    }
                    else if (InventoryPanelGO.activeSelf)
                    {
                        ToggleInventoryPanel();
                    }
                    else if (EquipmentPanelGO.activeSelf)
                    {
                        ToggleEquipmentPanel();
                    }
                    else if (craftingPanelGameObject.activeSelf)
                    {
                        ToggleCraftingPanel();
                    }
                    else
                    {
                        ToggleInventoryPanel();
                        ToggleEquipmentPanel();
                        ToggleCraftingPanel();
                    }

                    openingSign = !openingSign;
                    if (openingSign)
                        playerInput.mouseEnable = false;
                    else
                        playerInput.mouseEnable = true;

                    if (InventoryPanelGO.activeSelf || EquipmentPanelGO.activeSelf || craftingPanelGameObject.activeSelf)
                    {
                        TipsCanvas.SetActive(false);
                        ShowMouseCursor();
                    }
                    else
                    {
                        TipsCanvas.SetActive(true);
                        HideMouseCursor();
                    }
                        

                    break;
                }
            }
        }

        public void ToggleInventoryPanel()
        {
            InventoryPanelGO.SetActive(!InventoryPanelGO.activeSelf);
            if (!craftingPanelGameObject.activeSelf && !EquipmentPanelGO.activeSelf)
            {
                HideMouseCursor();
            }
        }

        public void ToggleEquipmentPanel()
        {
            EquipmentPanelGO.SetActive(!EquipmentPanelGO.activeSelf);
            if (!craftingPanelGameObject.activeSelf && !InventoryPanelGO.activeSelf)
            {
                HideMouseCursor();
            }
        }

        public void ToggleCraftingPanel()
        {
            craftingPanelGameObject.SetActive(!craftingPanelGameObject.activeSelf);
            if (!InventoryPanelGO.activeSelf && !EquipmentPanelGO.activeSelf)
            {
                HideMouseCursor();
            }
        }

        /// <summary>
        /// 显示鼠标光标
        /// </summary>
        public void ShowMouseCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        /// <summary>
        /// 隐藏鼠标光标
        /// </summary>
        public void HideMouseCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

    }
}