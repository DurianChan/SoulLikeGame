using UnityEngine;

namespace DC.Items
{
    ///<summary>
    ///状态统计面板
    ///</summary>
    public class StatPanel : MonoBehaviour
    {
        [SerializeField] private StatDisplay[] statDisplays = null;
        [SerializeField] private string[] statNames = null;

        private CharacterStat[] stats;

        private void OnValidate()
        {
            statDisplays = GetComponentsInChildren<StatDisplay>();
            UpdateStatNames();
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        public void SetStats(params CharacterStat[] charStats)
        {
            stats = charStats;

            if (stats.Length > statDisplays.Length)                         //角色属性数组长度大于属性存放数组
            {
                Debug.LogError("Not Enough Stat Displays!");
                return;
            }

            for (int i = 0; i < statDisplays.Length; i++)
            {
                statDisplays[i].gameObject.SetActive(i<stats.Length);       //启用现有的属性否则禁用

                if (i < stats.Length)
                {
                    statDisplays[i].Stat = stats[i];                        //设置展示属性的值
                }
            }
        } 

        /// <summary>
        /// 更新角色属性数值
        /// </summary>
        public void UpdateStatValues()
        {
            for (int i = 0; i < stats.Length; i++)
            {
                statDisplays[i].UpdateStatValue();
            }
        }

        /// <summary>
        /// 更新角色属性名字
        /// </summary>
        public void UpdateStatNames()
        {
            for (int i = 0; i < statNames.Length; i++)
            {
                statDisplays[i].Name = statNames[i];
            }
        }
    }
}
