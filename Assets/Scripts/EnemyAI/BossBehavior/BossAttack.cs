using UnityEngine;

namespace DC
{
    ///<summary>
    ///boss右攻击
    ///</summary>
    public class BossAttack : BossBaseFSM
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            enemyAC.isItem = false;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            input.lockon = true;
            input.Dup = 0;
            input.Dright = 0;
            input.Dvec = Vector3.zero;
            input.rb = true;
        }


        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            input.rb = false;
        }
    }
}
