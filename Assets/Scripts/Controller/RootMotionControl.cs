using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///设置模型RootMotion
    ///</summary>
    public class RootMotionControl : MonoBehaviour
    {
        private Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        private void OnAnimatorMove()
        {
            SendMessageUpwards("OnUpdateRM",(object)anim.deltaPosition);
        }

    }
}
