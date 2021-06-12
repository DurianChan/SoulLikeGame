using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///按键状态判断与设置
    ///</summary>
    public class MyButton
    {
        public bool IsPressing = false;             //判断是否正在被按下
        public bool OnPressed = false;              //判断是否刚刚被按下
        public bool OnReleased = false;             //判断是否刚刚被释放
        public bool IsExtending = false;            //判断是否在计时进行状态
        public bool IsDelaying = false;             //判断是否要长按计时

        public float extendingDuration = 0.15f;     //计时的时间
        public float delayingDuration = 0.15f;       //长按的时间

        private bool curState = false;              //目前的状态
        private bool lastState = false;             //前一次的状态

        private MyTimer extTimer = new MyTimer();
        private MyTimer delayTimer = new MyTimer();

        public void Tick(bool input)
        {
            extTimer.Tick();
            delayTimer.Tick();

            curState = input;

            IsPressing = curState;

            OnPressed = false;
            OnReleased = false;
            IsExtending = false;
            IsDelaying = false;

            if (curState != lastState)   //如果目前状态不等于最后一次状态且当前状态为真则处于刚被按下的状态否则为刚刚释放的状态
            {
                if (curState == true)
                {
                    OnPressed = true;
                    StartTimer(delayTimer, delayingDuration);
                }
                else
                {
                    OnReleased = true;
                    StartTimer(extTimer, extendingDuration);
                }
            }

            if(delayTimer.state == MyTimer.STATE.RUN)
            {
                IsDelaying = true;
            }

            lastState = curState;

            if (extTimer.state == MyTimer.STATE.RUN)
                IsExtending = true;                     //如果处于计时进行状态设置为true否则为false
        }

        /// <summary>
        /// 开启计时
        /// </summary>
        /// <param name=""></param>
        private void StartTimer(MyTimer timer, float duration)
        {
            timer.duration = duration;
            timer.Go();
        }

    }
}
