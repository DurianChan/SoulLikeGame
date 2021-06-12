using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///处理动画事件
    ///</summary>
    public class TriggerControl : MonoBehaviour
    {
        private Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        /// <summary>
        /// 重置指定Trigger
        /// </summary>
        /// <param name="triggerName"></param>
        public void ResetTrigger(string triggerName)
        {
            anim.ResetTrigger(triggerName);
        }
    }
}
