using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    /// <summary>
    /// 敌人巡逻行为类
    /// </summary>
    public class Patrol : EnemyBaseFSM
    {
        int currentWP;              //巡逻点数组标记
        float accuracy = 0.8f;      //巡逻点距离计算误差值
        bool isArrive;              //判断是否到达巡逻点
        float rotSpeed = 3f;        //旋转速度

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            currentWP = 0;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (waypoints.Length == 0) return;
            isArrive = Vector3.Distance(waypoints[currentWP].transform.position, Enemy.transform.position) < accuracy;
            if (isArrive)
            {
                input.Dup = 0.4f;
                currentWP++;
                if (currentWP >= waypoints.Length)
                    currentWP = 0;
            }
            //rotate towards target
            var direction = (waypoints[currentWP].transform.position - Enemy.transform.position).normalized;
            Enemy.transform.rotation = Quaternion.Slerp(Enemy.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
            input.Dup = 0.6f;
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }
    }
}
