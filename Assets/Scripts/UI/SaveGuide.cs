using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///存档引导指示
    ///</summary>
    public class SaveGuide : Guide
    {
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
        }

        protected override void Update()
        {
            if (isCanTip && Input.GetKeyDown(playerEnter))
            {
                StartCoroutine(ShowText());
            }
        }

        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
        }

        IEnumerator ShowText()
        {
            tipPanel.ShowTipsText(tipsText);
            yield return new WaitForSeconds(1f);
            tipPanel.HideTipsText();
        }
    }
}
