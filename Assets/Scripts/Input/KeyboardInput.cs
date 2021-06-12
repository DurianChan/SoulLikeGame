using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///键盘输入
    ///</summary>
    public class KeyboardInput : IUserInput
    {
        #region  字段

        //输入按键
        [Header("=== Key settings ===")]
        public string KeyUp = "w";
        public string KeyDown = "s";
        public string KeyLeft = "a";
        public string KeyRight = "d";

        public string keyA;
        public string keyB;
        public string keyC;
        public string keyD;
        public string keyE;
        public string keyF;
        public string keyG;
        public string keyH;

        public MyButton buttonA = new MyButton();
        public MyButton buttonB = new MyButton();
        public MyButton buttonC = new MyButton();
        public MyButton buttonD = new MyButton();
        public MyButton buttonE = new MyButton();
        public MyButton buttonF = new MyButton();
        public MyButton buttonG = new MyButton();
        public MyButton buttonH = new MyButton();

        //控制手柄摄像机移动
        public string KeyJup;
        public string KeyJDown;
        public string KeyJLeft;
        public string KeyJright;

        [Header("=== Mouse Settings ===")]
        public float mouseSensitivityX = 1.0f;      //鼠标X轴移动灵敏度
        public float mouseSensitivityY = 1.5f;      //鼠标Y轴移动灵敏度

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

        #endregion

        private void Update()
        {
            buttonA.Tick(Input.GetKey(keyA));
            buttonB.Tick(Input.GetKey(keyB));
            buttonC.Tick(Input.GetKey(keyC));
            buttonD.Tick(Input.GetKey(keyD));
            buttonE.Tick(Input.GetKey(keyE));
            buttonF.Tick(Input.GetKey(keyF));
            buttonG.Tick(Input.GetKey(keyG));


            if (mouseEnable == true)
            {
                Jup = Input.GetAxis("Mouse Y") * mouseSensitivityY * 3.0f;
                Jright = Input.GetAxis("Mouse X") * mouseSensitivityX * 2.5f;
            }
            else
            {
                Jup = (Input.GetKey(KeyJup) ? 1.0f : 0) - (Input.GetKey(KeyJDown) ? 1.0f : 0);
                Jright = (Input.GetKey(KeyJright) ? 1.0f : 0) - (Input.GetKey(KeyJLeft) ? 1.0f : 0);
            }

            targetDup = (Input.GetKey(KeyUp) ? 1.0f : 0) - (Input.GetKey(KeyDown) ? 1.0f : 0);
            targetDright = (Input.GetKey(KeyRight) ? 1.0f : 0) - (Input.GetKey(KeyLeft) ? 1.0f : 0);

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
            lockon = buttonE.OnPressed;
            //冲刺
            run = (buttonA.IsPressing && !buttonA.IsDelaying) || buttonA.IsExtending;  
            //防御
            defense = buttonF.IsPressing;
            //跳跃
            jump = buttonB.OnPressed && buttonA.IsPressing;
            //动作信号
            action = buttonG.OnPressed;
            //攻击
            rb = buttonD.OnPressed;
            lb = buttonC.OnPressed;
            rt = buttonF.IsPressing && buttonD.OnPressed;
            lt = buttonF.IsPressing && buttonC.OnPressed;
            //翻滚
            roll = buttonB.OnPressed && !buttonA.IsDelaying;
        }

    }
}
