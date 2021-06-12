using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DC
{
    ///<summary>
    ///死亡界面
    ///</summary>
    public class DeathPanel : MonoBehaviour
    {
        public Text deathReason;
        private Animator deathAnimator;

        private void Start()
        {
            deathAnimator = GetComponent<Animator>();
            deathReason = GetComponentInChildren<Text>();
        }

        public void ShowDeathPanel(string reason)
        {
            deathReason.text = reason;
            deathAnimator.SetTrigger("die");
        }
    }
}
