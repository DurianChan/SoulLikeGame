using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///开门引导
    ///</summary>
    public class DoorGuide : Guide
    {
        public Animator animator;
        public bool isOpen = false;

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
        }

        protected override void Update()
        {
            if(isCanTip && !isOpen && Input.GetKeyDown(playerEnter))
            {
                animator.SetBool("open", true);
                isOpen = true;
            }
        }


        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
        }
    }
}
