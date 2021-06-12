using UnityEngine;
using DC.Items;

namespace DC
{
    ///<summary>
    ///数据存储测试
    ///</summary>
    public class SaveTest : MonoBehaviour
    {
        [SerializeField] private Character c;
        [SerializeField] private PlayerItemDataSave PlayerItemDataSave = null;

        private void OnValidate()
        {
            if (c == null)
            {
                c = FindObjectOfType<Character>();
            }

            if (PlayerItemDataSave == null)
            {
                PlayerItemDataSave = GetComponent<PlayerItemDataSave>();
            }
        }

        private void Update()
        {
            //save
            if (Input.GetKeyDown(KeyCode.N))
            {
                PlayerItemDataSave.SaveInventory(c);
                PlayerItemDataSave.SaveEquipment(c);
                PlayerItemDataSave.SaveQuickSlot(c);
                print("save!");
            }

            //load
            if (Input.GetKeyDown(KeyCode.M))
            {
                PlayerItemDataSave.LoadInventory(c);
                PlayerItemDataSave.LoadEquipment(c);
                PlayerItemDataSave.LoadQuickSlot(c);
                print("load!");
            }
        }
    }
}
