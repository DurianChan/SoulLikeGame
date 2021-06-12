using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DC
{
    ///<summary>
    ///提示面板
    ///</summary>
    public class TipTextPanel : MonoBehaviour
    {
        [SerializeField] private GameObject TipsButton;
        [SerializeField] private GameObject TipsText;
        [SerializeField] private GameObject TipsSave;
        private Transform saveParent;
        private IUserInput playerInput;
        [SerializeField] private LevelLoader loader;
        [SerializeField] private SaveLoadPos saveLoadPos;

        private Text buttonText;
        private Text textText;
        private CanvasGroup buttonGroup;
        private CanvasGroup textGroup;
        private CanvasGroup saveGroup;

        public GameObject saveTipsPerfab;
        public string[] saveName;               //传送点名称

        private void OnValidate()
        {
            if (TipsButton == null)
                TipsButton = transform.Find("TipsButton").gameObject;

            if (TipsText == null)
                TipsText = transform.Find("TipsText").gameObject;


            if (TipsSave == null)
                TipsSave = transform.Find("TipsSave").gameObject;

            if (loader == null)
                loader = FindObjectOfType<LevelLoader>();


            if (saveLoadPos == null)
                saveLoadPos = FindObjectOfType<SaveLoadPos>();
        }

        private void Start()
        {
            playerInput = GameManager._instance.playerAM.ac.pi;
            saveParent = TipsSave.transform.Find("TipsSaveBG");

            buttonText = TipsButton.transform.DeepFind("Text").GetComponent<Text>();
            textText = TipsText.transform.DeepFind("Text").GetComponent<Text>();

            buttonGroup = TipsButton.GetComponent<CanvasGroup>();
            textGroup = TipsText.GetComponent<CanvasGroup>();
            saveGroup = TipsSave.GetComponent<CanvasGroup>();
        }

        public void ShowTipsButton(string tipsText)
        {
            buttonText.text = tipsText;
            buttonGroup.alpha = 1;
        }

        public void HideTipsButton()
        {
            buttonText.text = "";
            buttonGroup.alpha = 0;
        }

        public void ShowTipsText(string tipsText)
        {
            textText.text = tipsText;
            textGroup.alpha = 1;
        }

        public void HideTipsText()
        {
            textText.text = "";
            textGroup.alpha = 0;
        }

        /// <summary>
        /// 生成传送点选择面板
        /// </summary>
        /// <param name="savePosition"></param>
        public void ShowTipsSave(List<Vector3> savePosition)
        {
            int i = 0;
            foreach (Vector3 pos in savePosition)
            {
                GameObject obj = GameObject.Instantiate(saveTipsPerfab);
                obj.transform.SetParent(saveParent.transform);
                obj.GetComponent<Button>().onClick.AddListener(()=> TransferSave(pos));               
                obj.GetComponentInChildren<Text>().text = saveName[i];
                i++;
            }

            playerInput.mouseEnable = false;
            ShowMouseCursor();
            saveGroup.alpha = 1;
        }

        /// <summary>
        /// 传送玩家到指定位置
        /// </summary>
        /// <param name="pos"></param>
        private void TransferSave(Vector3 pos)
        {
            saveLoadPos.currentPos[0] = pos;
            saveLoadPos.PlayerItemDataSave.SaveRebirthPosition(saveLoadPos);
            StartCoroutine(loader.RefreshPlayer(pos));
        }

        /// <summary>
        /// 清空子物体并隐藏鼠标和面板
        /// </summary>
        public void HideTipsSave()
        {
            //清除原来的子物体
            for (int i = 0; i < saveParent.childCount; i++)
            {
                Destroy(saveParent.GetChild(i).gameObject);
            }
            playerInput.mouseEnable = true;
            HideMouseCursor();
            saveGroup.alpha = 0;
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
