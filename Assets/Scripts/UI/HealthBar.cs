using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DC
{
    ///<summary>
    ///生命条
    ///</summary>
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;       //生命滑动条
        public Gradient gradient;   //颜色渐变
        public Image fill;          //图片填充


        public void SetMaxHealth(float health)
        {
            slider.maxValue = health;
            slider.value = health;
            fill.color = gradient.Evaluate(1f);
        }

        public void SetHealth(float health)
        {
            slider.value = health;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
        
    }
}
