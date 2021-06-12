﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///胶囊射线检测地面
    ///</summary>
    public class OnGroundSensor : MonoBehaviour
    {
        //胶囊碰撞体
        public CapsuleCollider capcol;
        public float offset = 0.1f;

        //头顶与脚底两个胶囊的位置
        private Vector3 point1;
        private Vector3 point2;
        private float radius;


        private void Awake()
        {
            radius = capcol.radius - 0.05f;
        }

        private void FixedUpdate()
        {
            point1 = transform.position + transform.up * (radius-offset);
            point2 = transform.position + transform.up * (capcol.height-offset) - transform.up * radius;

            Collider[] outputCols = Physics.OverlapCapsule(point1, point2, radius,LayerMask.GetMask("Ground"));
            if (outputCols.Length != 0)
            {
                SendMessageUpwards("IsGround");
            }
            else
            {
                SendMessageUpwards("IsNotGround");
            }
        }

    }
}
