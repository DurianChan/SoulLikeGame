using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///Boss类AI
    ///</summary>
    public class BossAI : EnemyAIBase
    {
        [Header("计时")]
        public bool canDoNext = true;
        public float lastTime = 0;

        [Header("左右频率占比")]
        [Range(1, 10)]
        public int proportion;              //比例
        public float attackTime;            //攻击时间
        public float pauseTime;             //暂停时间
        private bool isPause;               //是否为暂停

        protected override void Start()
        {
            base.Start();
        }

        private void Update()
        {
            isEnemyAlive = enemySM.currentHealth <= 0;
            distanceA = Vector3.Distance(transform.position,player.transform.position);
            if (isEnemyAlive)
            {
                anim.SetBool("die", true);
            }
            else
            {
                if (distanceA >= attactDistance && !isPause)
                {
                    anim.SetTrigger("walk");
                }
                else if (distanceA < attactDistance)
                {
                    int wayNumber = Random.Range(1, 11);
                    if (canDoNext)
                    {
                        if (wayNumber <= proportion)
                            StartCoroutine(ContinuousBehavior("attack", attackTime));
                        else
                        {
                            isPause = true;
                            StartCoroutine(ContinuousBehavior("pause", pauseTime));
                        }
                            
                    }
                }
            }
        }

        IEnumerator ContinuousBehavior(string behavior, float durationTime)
        {
            for (int i = 0; i < 3; i++)
            {
                canDoNext = false;
                anim.SetTrigger(behavior);
                yield return new WaitForSeconds(durationTime);
                continue;
            }
            canDoNext = true;
            isPause = false;
        }
    }
}
