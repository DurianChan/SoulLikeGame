using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DC
{
    ///<summary>
    ///耐力条
    ///</summary>
    public class StaminaBar : MonoBehaviour
    {
        public Slider slider;       //耐力滑动条
        public Gradient gradient;   //颜色渐变
        public Image fill;          //图片填充

        public void SetMaxStamina(float stamina)
        {
            slider.maxValue = stamina;
            slider.value = stamina;
            fill.color = gradient.Evaluate(1f);
        }

        public void SetShamina(float stamina)
        {
            slider.value = stamina;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
}
