using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///Boss行为FSM基础类
    ///</summary>
    public class BossBaseFSM : StateMachineBehaviour
    {
        public GameObject Enemy;                //敌人物体
        public GameObject opponent;             //目标
        public DummyIUserInput input;           //输入信号
        public ActorController enemyAC;        //敌人动作控制

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Enemy = animator.gameObject;
            enemyAC = animator.gameObject.GetComponent<ActorController>();
            input = Enemy.GetComponent<BossAI>().GetInput();
            opponent = Enemy.GetComponent<BossAI>().GetPlayer();
        }
    }
}
