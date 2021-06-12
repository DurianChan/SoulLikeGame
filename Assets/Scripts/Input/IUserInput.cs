using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///输入抽象类
    ///</summary>
    public abstract class IUserInput : MonoBehaviour
    {

        //输出信号
        public float Dup;
        public float Dright;
        public float Dmag;
        public Vector3 Dvec;
        public float Jup;
        public float Jright;

        //1.按压型信号
        public bool run;                //跑步信号
        public bool defense;            //防御信号
        //2.触发型信号
        public bool jump;               //跳跃信号
                                        
        public bool roll;               //翻滚信号
        public bool lockon;             //锁定信号
        public bool action;             //动作信号

        //手柄对应按键信号
        public bool lb;
        public bool lt;
        public bool rb;
        public bool rt;
        protected bool lastJump;        //最后一次跳跃信号
        protected bool lastAttack;      //最后一次攻击信号
         //3.双击触发型信号

        public bool mouseEnable = false;     //移动视角鼠标开关
        public bool inputEnabled = true;    //输入开关

        protected float targetDup;
        protected float targetDright;
        protected float velocityDup;
        protected float velocityDright;


        /// <summary>
        /// 该数学公式：限制于玩家斜方向运动速度小于等于1
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected Vector2 SquareToCircle(Vector2 input)
        {
            Vector2 output = Vector2.zero;

            output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
            output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

            return output;
        }

        /// <summary>
        /// 更新Dmag和Dvec数值
        /// </summary>
        /// <param name="Dup2"></param>
        /// <param name="Dright2"></param>
        protected void UpdateDmagDvec(float Dup2, float Dright2)
        {
            Dmag = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));
            Dvec = Dright2 * transform.right + Dup2 * transform.forward;
        }

    }
}
