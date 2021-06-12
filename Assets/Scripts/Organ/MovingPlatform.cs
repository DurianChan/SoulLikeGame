using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///移动平台
    ///</summary>
    public class MovingPlatform : MonoBehaviour
    {
        public Vector3[] points;            //存储移动路点坐标
        public int point_number = 0;        //路点数量
        public float tolerance;             //误差值
        public float speed;                 //移动速度
        public float delay_time;            //延迟移动时间
        public bool autonatic;              //控制是否自动移动

        private Vector3 current_target;     //当前移动目标坐标
        private float delay_start;          //延迟开始时间

        private void Start()
        {
            if (points.Length > 0)
            {
                for(int i = 0; i < points.Length; i++)
                {
                    points[i] += transform.position;
                }
                current_target = points[0];
            }
            tolerance = speed * Time.deltaTime;
        }

        private void FixedUpdate()
        {
            if (transform.position != current_target)
            {
                MoveTarget();             //若平台未移动到当前目标点的坐标，则执行移动方法
            }
            else
            {
                UpdateTarget();     //若已经到达目标点坐标，则更新移动目标
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            other.transform.parent = transform;
        }

        private void OnTriggerExit(Collider other)
        {
            other.transform.parent = null;
        }

        /// <summary>
        /// 项目标点移动
        /// </summary>
        private void MoveTarget()
        {
            Vector3 heading = current_target - transform.position;
            transform.position += (heading / heading.magnitude) * speed * Time.deltaTime;
            if (heading.magnitude < tolerance)
            {
                transform.position = current_target;    //当平台移动到接近误差值时，直接将位置赋值给当前平台
                delay_start = Time.time;                //延迟下次移动时间
            }
                
        }

        /// <summary>
        /// 更新移动目标点
        /// </summary>
        private void UpdateTarget()
        {
            if (autonatic)  //是否到达目标点后自动移动
            {
                if(Time.time - delay_start > delay_time)
                {
                    NextTarget();
                }
            }
        }

        /// <summary>
        /// 平台继续移动
        /// </summary>
        private void NextTarget()
        {
            point_number++;         //标记移动至下个路点
            if(point_number >= points.Length)
            {
                point_number = 0;   //重置路点
            }
            current_target = points[point_number];
        }
    }
}
