using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///敌人行为FSM基础类
    ///</summary>
    public class EnemyBaseFSM : StateMachineBehaviour
    {
        public GameObject Enemy;                //敌人物体
        public GameObject opponent;             //目标
        public DummyIUserInput input;           //输入信号
        public GameObject[] waypoints;          //巡逻路点

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Enemy = animator.gameObject;
            input = Enemy.GetComponent<EnemyAI>().GetInput();
            opponent = Enemy.GetComponent<EnemyAI>().GetPlayer();
            waypoints = Enemy.GetComponent<EnemyAI>().waypoints;
        }

    }
}
