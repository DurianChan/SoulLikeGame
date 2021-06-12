using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DC
{
    ///<summary>
    ///引导类
    ///</summary>
    public class Guide : MonoBehaviour
    {
        public string tipsButton;       //提示按键文本
        public string tipsText;         //提示文本
        public KeyCode playerEnter = KeyCode.E;     //玩家按键触发
        public TipTextPanel tipPanel;   //提示类
        public bool isCanTip;

        private void OnValidate()
        {
            if (tipPanel == null)
            {
                tipPanel = FindObjectOfType<TipTextPanel>();
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                isCanTip = true;
                tipPanel.ShowTipsButton(tipsButton);
            }      
        }

        protected virtual void Update()
        {
            if (isCanTip && Input.GetKeyDown(playerEnter))
            {
                tipPanel.ShowTipsText(tipsText);
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            isCanTip = false;
            tipPanel.HideTipsButton();
            tipPanel.HideTipsText();
        }

        public void HideTips()
        {
            isCanTip = false;
            tipPanel.HideTipsButton();
            tipPanel.HideTipsText();
        }
    }
}