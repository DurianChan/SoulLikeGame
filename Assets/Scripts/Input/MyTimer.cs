using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///计时器
    ///</summary>
    public class MyTimer
    {
        public enum STATE
        {
            IDLE,
            RUN,
            FINISHED
        }
        public STATE state;                     //计时状态

        public float duration = 1.0f;           //计时的时间

        private float elapsedTime = 0;          //计时流逝的时间

        public void Tick()
        {
            switch (state)
            {
                case STATE.IDLE:
                    break;
                case STATE.RUN:
                    elapsedTime += Time.deltaTime;
                    if (elapsedTime >= duration)        //判断是否已经到时设置为完成状态
                        state = STATE.FINISHED;
                    break;
                case STATE.FINISHED:
                    break;
                default:
                    Debug.Log("MyTimer Error");
                    break;
            }
        }

        public void Go()
        {
            elapsedTime = 0;
            state = STATE.RUN;
        }
    }
}
