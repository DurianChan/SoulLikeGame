using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///敌人AI类基类
    ///</summary>
    public class EnemyAIBase : MonoBehaviour
    {
        [Header("基础类")]
        protected GameObject player;              //存储玩家物体
        protected DummyIUserInput input;          //信号接口
        protected Animator anim;                  //有限状态机
        protected GameObject model;               //敌人模型
        protected StateManager plsyerSM;          //玩家状态管理
        protected StateManager enemySM;           //敌人状态管理
        protected bool isPlayerAlive;             //玩家是否活着
        protected bool isEnemyAlive;              //敌人自身是否活着

        public GameObject[] waypoints;          //巡逻路点
        public GameObject target;               //目标

        [Header("信号开关")]
        [SerializeField] protected bool OpenSearchRays;           //射线检测玩家是否在搜寻范围内的开关
        [SerializeField] protected bool OpenDetectionPlayer;      //检测敌人与玩家之间的距离的开关

        [Header("状态距离")]
        [SerializeField] protected float distanceA;            //敌人自身与玩家之间的距离
        [SerializeField] protected float distanceB;            //敌人自身与巡逻点的距离
        public float chaseDistance;                          //玩家远离敌人一定距离，转到追踪状态
        public float combatDistance;                         //靠近的距离，转到战斗状态
        public float attactDistance;                         //在可攻击的范围内，转到攻击状态

        protected virtual void Start()
        {
            anim = GetComponent<Animator>();
            player = GameManager._instance.playerAM.gameObject;
            input = GetComponent<DummyIUserInput>();
            model = transform.Find("Enemy").gameObject;
            plsyerSM = player.GetComponent<ActorManager>().sm;
            enemySM = GetComponent<StateManager>();
        }

        /// <summary>
        /// 获取玩家物体
        /// </summary>
        /// <returns></returns>
        public GameObject GetPlayer()
        {
            return player;
        }

        /// <summary>
        /// 获取输入信号接口
        /// </summary>
        /// <returns></returns>
        public DummyIUserInput GetInput()
        {
            return input;
        }

        //绘画检测范围
        //private void OnDrawGizmosSelected()
        //{
        //    Gizmos.color = Color.red;
        //    Vector3 modelOrigin = Vector3.zero;

        //    modelOrigin = model.transform.position + new Vector3(0, 1, 0);
        //    Gizmos.DrawWireSphere(modelOrigin, 3.5f);
        //}

        /// <summary>
        /// 射线检测玩家
        /// </summary>
        /// <returns></returns>
        protected GameObject CheckPlayer()
        {
            Vector3 modelOrigin = model.transform.position + new Vector3(0, 1, 0);
            Collider[] cols = Physics.OverlapSphere(modelOrigin, 5f, LayerMask.GetMask("Player"));
            if (cols.Length == 0)
            {
                return null;
            }
            else
            {
                foreach (var col in cols)
                {
                    if (col.gameObject == player)
                    {
                        target = col.gameObject;
                        break;
                    }
                }
                return target;
            }
        }

        /// <summary>
        /// 设置追踪状态
        /// </summary>
        protected void SetPatrol()
        {
            anim.SetTrigger("patrol");
            target = null;
            OpenDetectionPlayer = false;
        }
    }
}
