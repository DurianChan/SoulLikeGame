using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DC.Items
{
    ///<summary>
    ///物品
    ///</summary>
    [CreateAssetMenu(menuName ="Items/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] string id;             //物品id
        public string ID { get { return id; } }
        public string ItemName;                 //物品名字
        public Sprite Icon;                     //物品图片
        [Range(1,999)]                          //限制在1到999之间
        public int MaximumStacks = 1;           //最大堆叠数量

        protected static readonly StringBuilder sb = new StringBuilder();             //处理字符串

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            string path = AssetDatabase.GetAssetPath(this);
            id = AssetDatabase.AssetPathToGUID(path);
        }
#endif


        /// <summary>
        /// 实例化一个新的物品
        /// </summary>
        /// <returns></returns>
        public virtual Item GetCopy()
        {
            return this;
        }

        /// <summary>
        /// 销毁物品
        /// </summary>
        public virtual void Destory()
        {

        }

        /// <summary>
        /// 获取物品类型
        /// </summary>
        /// <returns></returns>
        public virtual string GetItemType()
        {
            return "";
        }

        /// <summary>
        /// 获取物品描述
        /// </summary>
        /// <returns></returns>
        public virtual string GetItemDescription()
        {
            return "";
        }

    }
}
