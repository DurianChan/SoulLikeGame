using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///模拟输入
    ///</summary>
    public class DummyIUserInput : IUserInput
    {
        //IEnumerator Start()
        //{
        //    while (true)
        //    {
        //        Dup = 1.0f;
        //        Dright = 0;
        //        Jright = 1.0f;
        //        Jup = 0f;
        //        run = true;
        //        rb = true;
        //        yield return new WaitForSeconds(3.0f);
        //        Dup = 0f;
        //        Dright = 0;
        //        Jright = 0f;
        //        Jup = 0f;
        //        yield return new WaitForSeconds(1.0f);
        //        lockon = true;
        //        yield return 0;
        //        lb = true;
        //        yield return new WaitForSeconds(1.5f);
        //    }
        //}

        private void Update()
        {
            UpdateDmagDvec(Dup, Dright);
        }


    }
}
