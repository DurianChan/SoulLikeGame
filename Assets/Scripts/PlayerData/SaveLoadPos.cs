using DC.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///存储当前存档点位置
    ///</summary>
    [Serializable]
    public class SaveLoadPos : MonoBehaviour
    {
        public PlayerItemDataSave PlayerItemDataSave = null;
        public Vector3[] currentPos;
        public List<Vector3> savePosList = new List<Vector3>();

        private void Awake()
        {
            PlayerItemDataSave.LoadSignPosition(this);
            Transform[] grandFa = GetComponentsInChildren<Transform>();
            SaveLoadFire lighten = null;

            foreach (Transform child in grandFa)
            {
                if (child.name.Contains("Spawn points"))
                {
                    if (savePosList.Contains(child.position))
                    {
                        lighten = child.parent.GetComponent<SaveLoadFire>();
                        lighten.isFirstSave = false;
                    }
                }
            }
        }
    }
}
