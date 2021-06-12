using DC.Items;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///存档点
    ///</summary>
    public class SaveLoadFire : MonoBehaviour
    {
        private SaveLoadPos pos;            //存储位置的地方
        public GameObject fireEffect;      //火焰
        public Vector3 savePoint;           //玩家存档重生的位置
        public bool isCanSave;              //判断玩家进入存档范围
        public bool isFirstSave = true;     //是否为第一次存档
        public TipTextPanel tipPanel;       //提示类
        public bool isSaveTips = true;      //是否已生成提示
        private Character c;                //角色物品管理类

        private void Start()
        {
            pos = transform.parent.GetComponent<SaveLoadPos>();
            fireEffect = transform.Find("FireEffect").gameObject;
            savePoint = transform.DeepFind("Spawn points").position;
            tipPanel = GetComponent<Guide>().tipPanel;

            if (isFirstSave)
                fireEffect.SetActive(false);

            if (c == null)
                c = FindObjectOfType<Character>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                isCanSave = true;
            }
        }

        private void Update()
        {
            if (isCanSave)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    if (isFirstSave)                    //第一次存档时，将火焰点亮，并将位置插入队列中
                    {
                        isFirstSave = false;
                        fireEffect.SetActive(true);
                        pos.savePosList.Add(savePoint);
                        pos.PlayerItemDataSave.SaveSignPosition(pos);
                    }
                    if (isSaveTips)                     //判断能否显示已存档信息
                    {
                        isSaveTips = false;
                        tipPanel.ShowTipsSave(pos.savePosList);
                    }
                    pos.currentPos[0] = savePoint;      //将当前玩家位置设置为当前存档重生点的位置
                    pos.PlayerItemDataSave.SaveRebirthPosition(pos);
                    SaveCharacterPanel();
                }
            }
        }

        public void SaveCharacterPanel()
        {
            pos.PlayerItemDataSave.SaveInventory(c);
            pos.PlayerItemDataSave.SaveEquipment(c);
            pos.PlayerItemDataSave.SaveQuickSlot(c);
        }

        private void OnTriggerExit(Collider other)
        {
            tipPanel.HideTipsSave();
            isSaveTips = true;
            isCanSave = false;
        }
    }
}
