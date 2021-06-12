using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///手柄输入
    ///</summary>
    public class JoystickInput : IUserInput
    {
        #region  字段

        [Header("=== Joystick Settings ===")]
        public string axisX = "axisX";
        public string axisY = "axisY";
        public string axisJright = "axis4";
        public string axisJup = "axis5";
        public string btnA = "btn1";            //B
        public string btnB = "btn0";            //A
        public string btnC = "btn2";            //X
        public string btnD = "btn3";            //Y
        public string btnLB = "btn4";           //LB
        public string btnLT = "axis3";          //LT
        public string btnRB = "btn5";           //RB
        public string btnRT = "axis3";          //RT
        public string btnJstick = "btn9";       //RP

        //axis3 x 和 y 轴 对应 LT 和 RT
        private bool axis3XValue;
        private bool axis3YValue;

        public MyButton buttonA = new MyButton();
        public MyButton buttonB = new MyButton();
        public MyButton buttonC = new MyButton();
        public MyButton buttonD = new MyButton();
        public MyButton buttonLB = new MyButton();
        public MyButton buttonRB = new MyButton();
        public MyButton buttonJstick = new MyButton();

        [Header("=== Mouse Settings ===")]
        public float mouseSensitivityX = 1.0f;      //鼠标X轴移动灵敏度
        public float mouseSensitivityY = 1.5f;      //鼠标Y轴移动灵敏度

        #endregion

        private void Start()
        {
            mouseEnable = true;
        }

        #region  PS3 controller update
        //PS3 Controller Mapping
        //public string axisX = "axisX";
        //public string axisY = "axisY";
        //public string axisJright = "axis5";
        //public string axisJup = "axis4";
        //public string btnA = "btn0";            
        //public string btnB = "btn1";
        //public string btnC = "btn2";
        //public string btnD = "btn3";
        //public string btnLB = "btn4";
        //public string btnLT = "btn6";
        //public string btnRB = "btn5";
        //public string btnRT = "btn7";
        //public string btnJstick = "btn11";

        //public MyButton buttonA = new MyButton();
        //public MyButton buttonB = new MyButton();
        //public MyButton buttonC = new MyButton();
        //public MyButton buttonD = new MyButton();
        //public MyButton buttonLB = new MyButton();
        //public MyButton buttonLT = new MyButton();
        //public MyButton buttonRB = new MyButton();
        //public MyButton buttonRT = new MyButton();
        //public MyButton buttonJstick = new MyButton();

        ////输出信号
        //[Header("=== Output signals ===")]
        //public float Dup;
        //public float Dright;
        //public float Dmag;
        //public Vector3 Dvec;

        //public float Jup;
        //public float Jright;

        ////1.按压型信号
        //public bool run;    //跑步信号
        ////2.触发型信号
        //public bool jump;       //跳跃信号
        //public bool attack;     //攻击信号
        //private bool lastJump;  //最后一次跳跃信号
        //private bool lastAttack; //最后一次攻击信号
        ////3.双击触发型信号

        //[Header("=== Others ===")]
        //public bool inputEnabled = true;    //输入开关

        //private float targetDup;
        //private float targetDright;
        //private float velocityDup;
        //private float velocityDright;


        //private void Update()
        //{
        //    buttonA.Tick(Input.GetButton(btnA));
        //    buttonB.Tick(Input.GetButton(btnB));
        //    buttonC.Tick(Input.GetButton(btnC));
        //    buttonD.Tick(Input.GetButton(btnD));
        //    buttonLB.Tick(Input.GetButton(btnLB));
        //    buttonLT.Tick(Input.GetButton(btnLT));
        //    buttonRB.Tick(Input.GetButton(btnRB));
        //    buttonRT.Tick(Input.GetButton(btnRT));
        //    buttonJstick.Tick(Input.GetButton(btnJstick));

        //    Jup = (-1) * Input.GetAxis(axisJup);
        //    Jright = Input.GetAxis(axisJright);

        //    targetDup = Input.GetAxis(axisY);
        //    targetDright = Input.GetAxis(axisX);


        //    if (inputEnabled == false)
        //    {
        //        targetDup = 0;
        //        targetDright = 0;
        //    }

        //    //使用SmoothDamp使得信号值缓动
        //    Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        //    Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        //    Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
        //    float Dright2 = tempDAxis.x;
        //    float Dup2 = tempDAxis.y;

        //    UpdateDmagDvec(Dup2, Dright2);

        //    //锁定
        //    lockon = buttonJstick.OnPressed;
        //    //冲刺
        //    run = (buttonA.IsPressing && !buttonA.IsDelaying) || buttonA.IsExtending;
        //    //防御
        //    defense = buttonLB.IsPressing;
        //    //跳跃
        //    jump = buttonA.OnPressed && buttonA.IsExtending;
        //    //动作信号
        //    action = buttonC.OnPressed;
        //    //攻击
        //    rb = buttonRB.OnPressed;
        //    rt = buttonRT.OnPressed;
        //    lb = buttonRB.OnPressed;
        //    lt = buttonLT.OnPressed;
        //    //翻滚
        //    roll = buttonA.OnReleased && buttonA.IsDelaying;
        //}
        #endregion

        private void Update()
        {
            buttonA.Tick(Input.GetButton(btnA));
            buttonB.Tick(Input.GetButton(btnB));
            buttonC.Tick(Input.GetButton(btnC));
            buttonD.Tick(Input.GetButton(btnD));
            buttonLB.Tick(Input.GetButton(btnLB));
            buttonRB.Tick(Input.GetButton(btnRB));
            buttonJstick.Tick(Input.GetButton(btnJstick));

            if (mouseEnable == true)
            {
                Jup = (-1) * Input.GetAxis(axisJup) * mouseSensitivityY * 3.0f;
                Jright = Input.GetAxis(axisJright) * mouseSensitivityX * 2.5f;
            }

            //Jup = (-1) * Input.GetAxis(axisJup);        //右摇杆Y轴
            //Jright = Input.GetAxis(axisJright);         //右摇杆X轴

            targetDup = Input.GetAxis(axisY);           //左摇杆Y轴
            targetDright = Input.GetAxis(axisX);        //左摇杆X轴


            axis3XValue = Input.GetAxis(btnLT) > 0 ? true : false;
            axis3YValue = Input.GetAxis(btnRT) < 0 ? true : false;

            if (inputEnabled == false)
            {
                targetDup = 0;
                targetDright = 0;
            }

            //使用SmoothDamp使得信号值缓动
            Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
            Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

            Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
            float Dright2 = tempDAxis.x;
            float Dup2 = tempDAxis.y;

            UpdateDmagDvec(Dup2, Dright2);

            //锁定
            lockon = buttonJstick.OnPressed;
            //冲刺
            run = (buttonA.IsPressing && !buttonA.IsDelaying) || buttonA.IsExtending;
            //防御
            defense = buttonLB.IsPressing;
            //跳跃
            jump = buttonA.OnPressed && buttonA.IsExtending;
            //动作信号
            action = buttonC.OnPressed;
            //攻击
            rb = buttonRB.OnPressed;
            lb = buttonLB.OnPressed;
            rt = axis3XValue;
            lt = axis3YValue;
            //翻滚
            roll = buttonA.OnReleased && buttonA.IsDelaying;
        }
    }
}