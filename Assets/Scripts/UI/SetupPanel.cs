using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///暂停菜单
    ///</summary>
    public class SetupPanel : MonoBehaviour
    {
        public GameObject openPanel;
        private IUserInput playerInput;
        private bool isOpen;

        private void Start()
        {
            playerInput = GameManager._instance.playerAM.ac.pi;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isOpen)
                {
                    HideSetupPanel();

                }
                else
                {
                    OpenSetupPanel();
                }
            }
        }

        public void OpenSetupPanel()
        {
            isOpen = true;
            ShowMouseCursor();
            playerInput.mouseEnable = false;
            openPanel.SetActive(true);
        }

        public void HideSetupPanel()
        {
            isOpen = false;
            HideMouseCursor();
            playerInput.mouseEnable = true;
            openPanel.SetActive(false);
        }

        public void QuickGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
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
