using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///Traansform帮助类
    ///</summary>
    public static class TransformHelpers { 

        /// <summary>
        /// 深度搜索父类下指定某个物体
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="targetName"></param>
        /// <returns></returns>
        public static Transform DeepFind(this Transform parent,string targetName)   //this Transform用法 挂载到Transform上
        {
            Transform tempTrans = null;

            foreach (Transform chid in parent)
            {
                if (chid.name == targetName)
                {
                    return chid;
                }
                else
                {
                    tempTrans = DeepFind(chid, targetName);
                    if (tempTrans != null)
                        return tempTrans;
                }
                    
            }
            return null;
        }
    }
}
