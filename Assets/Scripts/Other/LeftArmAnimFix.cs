using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///模型动画左手腕修复
    ///</summary>
    public class LeftArmAnimFix : MonoBehaviour
    {
        private Animator anim;
        private ActorController ac;
        public Vector3 a;  //旋转的角度位置

        private void Awake()
        {
            anim = GetComponent<Animator>();
            ac = GetComponentInParent<ActorController>();
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (ac.leftIsShield)
            {
                if (anim.GetBool("defense") == false)
                {
                    Transform leftLowerArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
                    leftLowerArm.localEulerAngles += 0.75f * a;
                    anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(leftLowerArm.localEulerAngles));
                }
            }
        }
    }
}
