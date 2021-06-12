using System.Collections;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///敌人AI类
    ///</summary>
    public class EnemyAI : EnemyAIBase
    {
        [Header("计时")]
        public bool canToNext = true;
        public float lastTime = 0;

        [Header("攻防频率占比")]
        [Range(1, 10)]
        public int proportion;          //比例
        public float attackTime;        //攻击时间
        public float defenseTime;       //防御时间

        protected override void Start()
        {
            base.Start();
        }

        private void Update()
        {

            OpenSearchRays = anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex("Base Layer")).IsName("Patrol");         //判断是否为巡逻状态
            isPlayerAlive = plsyerSM.currentHealth <= 0;                                                                  //判断玩家是否还活着
            isEnemyAlive = enemySM.currentHealth <= 0;                                                                    //判断敌人自身是否还活着
            if (OpenSearchRays && target == null)       //当敌人处于巡逻状态且未获取到玩家位置时
            {
                if (isEnemyAlive)
                {
                    anim.SetBool("die", true);
                    OpenDetectionPlayer = false;
                }
                else if (!isPlayerAlive)
                {
                    target = CheckPlayer();             //检测并获取保存玩家的物体
                }

                if (target != null)
                {
                    OpenDetectionPlayer = true;
                }
            }
            else if (isEnemyAlive)      //当敌人死亡时，关闭检测敌人与玩家之间的距离的开关
            {
                anim.SetBool("die", true);
                OpenDetectionPlayer = false;
            }          
            else if (isPlayerAlive)       //玩家死亡设置回巡逻状态
            {
                SetPatrol();
            }
            else if (OpenDetectionPlayer && target != null)         //检测到玩家后根据距离执行ai行为 
            {
                distanceA = Vector3.Distance(transform.position, player.transform.position);
                if (distanceA > chaseDistance)
                {
                    SetPatrol();
                }
                else
                {
                    if (distanceA <= chaseDistance && distanceA >= combatDistance)
                        anim.SetTrigger("chase");
                    else if (distanceA <= combatDistance && distanceA >= attactDistance)
                        anim.SetTrigger("combat");
                    else
                    {
                        int wayNumber = Random.Range(1, 11);
                        if (canToNext)
                        {
                            if (wayNumber <= proportion)
                                StartCoroutine(ContinuousBehavior("attack", attackTime));
                            else
                                StartCoroutine(ContinuousBehavior("defense", defenseTime));
                        }     
                    }
                        
                }
            }           
        }

        IEnumerator ContinuousBehavior(string behavior, float durationTime)
        {
            for (int i = 0; i < 3; i++)
            {
                canToNext = false;
                anim.SetTrigger(behavior);
                yield return new WaitForSeconds(durationTime);
                continue;
            }
            canToNext = true;
        }
    }
}
