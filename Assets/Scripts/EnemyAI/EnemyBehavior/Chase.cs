using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    public class Chase : EnemyBaseFSM
    {
        float rotSpeed = 3f;        //旋转速度

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var direction = (opponent.transform.position - Enemy.transform.position).normalized;
            Enemy.transform.rotation = Quaternion.Slerp(Enemy.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
            input.Dup = 1f;
            input.run = true;
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            input.run = false;
        }
    }
}
