using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///角色控制
    ///</summary>
    [RequireComponent(typeof(Rigidbody))]
    public class ActorController : MonoBehaviour
    {
        #region  字段

        public GameObject model;
        public CameraController camcon;
        public IUserInput pi;
        public float walkSpeed = 2.0f;
        public float runMultiplier = 2.0f;
        public float jumpVelocity = 5.0f;   //上跳的冲量
        public float rollVelocity = 1.0f;   //翻滚的冲量
        public float jabMultiplier = 1.5f;  //后跳的冲量

        [Space(10)]
        [Header("=== Friction Settings ===")]
        public PhysicMaterial frictionOne;
        public PhysicMaterial frictionZero;

        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public Rigidbody rigid;
        private CapsuleCollider col;
        private ActorManager am;
        public Vector3 PlanarVec;
        private Vector3 thrustVec;
        private bool canAttack;             //判断能否攻击
        public Vector3 deltaPos;

        private bool lockPlanar = false;        //锁死平面速度
        private bool trackDirectiion = false;   //锁定追踪方向

        public bool leftIsShield = true;       //判断左手是否持盾

        public bool isItem = false;            //判断是否为物体

        private bool canShamina = true;

        public delegate void OnActionDelegate();
        public event OnActionDelegate OnAction;

        #endregion

        private void Awake()
        {
            IUserInput[] inputs = GetComponents<IUserInput>();
            foreach (var input in inputs)
            {
                if (input.enabled == true)
                {
                    pi = input;
                    break;
                }
            }
            am = GetComponent<ActorManager>();
            anim = model.GetComponent<Animator>();
            rigid = GetComponent<Rigidbody>();
            col = GetComponent<CapsuleCollider>();
        }

        private void Update()
        {
            if (!isItem)
            {
                if (pi.lockon && am.sm.currentHealth > 0)
                {
                    camcon.LockUnlock();
                }

                if (camcon.lockState == false)
                {
                    anim.SetFloat("forward", Mathf.Lerp(anim.GetFloat("forward"), pi.Dmag * ((pi.run) ? 2.0f : 1.0f), 0.5f));      //设置动画移动的量，使用Lerp缓慢改变值     
                    anim.SetFloat("right", 0);
                }
                else
                {
                    Vector3 localDVec = transform.InverseTransformVector(pi.Dvec);
                    anim.SetFloat("forward", localDVec.z * ((pi.run) ? 2.0f : 1.0f));
                    anim.SetFloat("right", localDVec.x * ((pi.run) ? 2.0f : 1.0f));
                }

                if (camcon.lockState == false)      //没有锁定目标时照常旋转移动
                {
                    if (pi.inputEnabled == true)     //当玩家可以操控时
                    {
                        if (pi.Dmag > 0.1f)
                        {
                            model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.3f);      //改变玩家方向,使用Slerp缓慢改变值
                        }
                    }
                    if (lockPlanar == false)
                    {
                        if(camcon.isAI)
                            PlanarVec = pi.Dmag * model.transform.forward * walkSpeed * ((pi.run) ? runMultiplier : 1.0f);        //改变玩家位置
                        else
                            PlanarVec = pi.Dmag * model.transform.forward * walkSpeed * ((pi.run && am.sm.currentStamina > 0) ? runMultiplier : 1.0f);

                        if (!camcon.isAI && pi.run && am.sm.currentStamina > 0)
                        {
                            if (canShamina)
                            {                     
                                StartCoroutine(ChangeShamina());
                            }
                        }                         
                    }
                }
                else
                {
                    if (trackDirectiion == false)
                    {
                        model.transform.forward = transform.forward;
                    }
                    else
                    {
                        model.transform.forward = PlanarVec.normalized;
                    }
                    if (lockPlanar == false)
                    {
                        if(camcon.isAI)
                            PlanarVec = pi.Dvec * walkSpeed * ((pi.run) ? runMultiplier : 1.0f);
                        else
                            PlanarVec = pi.Dvec * walkSpeed * ((pi.run && am.sm.currentStamina > 0) ? runMultiplier : 1.0f);

                        if (!camcon.isAI && pi.run && am.sm.currentStamina > 0)
                        {
                            if (canShamina)
                            {
                                StartCoroutine(ChangeShamina());
                            }
                        }
                    }

                }

                if (pi.roll || rigid.velocity.magnitude > 7f)
                {
                    anim.SetTrigger("roll");
                    canAttack = false;
                    if (!camcon.isAI)
                    {
                        am.sm.ChangeStamina(-2);
                    }
                }

                if (pi.jump)
                {
                    anim.SetTrigger("jump");
                    canAttack = false;
                    if (!camcon.isAI)
                    {
                        am.sm.ChangeStamina(-2);
                    }
                }

                if (leftIsShield)
                {
                    if (CheckState("ground") || CheckState("blocked"))
                    {
                        anim.SetLayerWeight(anim.GetLayerIndex("defense"), 1);
                        anim.SetBool("defense", pi.defense);
                    }
                    else
                    {
                        anim.SetBool("defense", false);
                        anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);
                    }
                }
                else
                {
                    anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);
                }

                if (EventSystem.current.IsPointerOverGameObject())          //防止UI操作影响角色
                    return;

                if ((pi.rb || pi.lb) && (CheckState("ground") || CheckStateTag("attackR") || CheckStateTag("attackL")) && canAttack)
                {
                    if (pi.rb)
                    {
                        if (camcon.isAI)
                        {
                            anim.SetBool("R0L1", false);
                            anim.SetTrigger("attack");
                        }
                        else if (!camcon.isAI && am.sm.currentStamina > 0)
                        {
                            am.sm.ChangeStamina(-3);

                            anim.SetBool("R0L1", false);
                            anim.SetTrigger("attack");
                        }
                        //if (am.sm.currentStamina > 0)
                        //{
                        //    am.sm.ChangeStamina(-3);

                        //    anim.SetBool("R0L1", false);
                        //    anim.SetTrigger("attack");
                        //}
                    }
                    else if (pi.lb && !leftIsShield)        //当左手不为盾的时候才可以攻击
                    {
                        if (camcon.isAI)
                        {
                            anim.SetBool("R0L1", true);
                            anim.SetTrigger("attack");
                        }
                        else if (!camcon.isAI && am.sm.currentStamina > 0)
                        {
                            am.sm.ChangeStamina(-3);

                            anim.SetBool("R0L1", true);
                            anim.SetTrigger("attack");
                        }

                        //if (am.sm.currentStamina > 0)
                        //{
                        //    am.sm.ChangeStamina(-3);

                        //    anim.SetBool("R0L1", true);
                        //    anim.SetTrigger("attack");
                        //}
                    }

                }

                if ((pi.rt || pi.lt) && (CheckState("ground") || CheckStateTag("attackR") || CheckStateTag("attackL")) && canAttack)
                {
                    if (pi.rt)      //按下右手重攻击
                    {

                    }
                    else
                    {
                        if (!leftIsShield)     //左手不为盾牌且按下左手重攻击
                        {

                        }
                        else
                        {
                            if (camcon.isAI)
                            {
                                anim.SetTrigger("counterBack");
                            }else if(am.sm.currentStamina > 0)
                            {
                                am.sm.ChangeStamina(-3);

                                anim.SetTrigger("counterBack");
                            }
                        }
                    }
                }
            }

            if (pi.action)
            {
                OnAction.Invoke();
            }
        }

        private IEnumerator ChangeShamina()
        {
            canShamina = false;
            am.sm.ChangeStamina(-2);
            yield return new WaitForSeconds(1.0f);
            canShamina = true;
        }

        private void FixedUpdate()
        {
            if (!isItem)
            {
                rigid.position += deltaPos;
                rigid.velocity = new Vector3(PlanarVec.x, rigid.velocity.y, PlanarVec.z) + thrustVec;
                thrustVec = Vector3.zero;
                deltaPos = Vector3.zero;
            }
        }

        /// <summary>
        /// 查找某一层中的动画进行状态
        /// </summary>
        /// <param name="stateName">检测状态名称</param>
        /// <param name="layerName">检测层级</param>
        /// <returns></returns>
        public bool CheckState(string stateName, string layerName = "Base Layer")
        {
            return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName);   //查找该层是否进行该动画状态
        }

        /// <summary>
        /// 查找某一层带有某个标签的动画状态
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public bool CheckStateTag(string tagName, string layerName = "Base Layer")
        {
            return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsTag(tagName);
        }

        /// <summary>
        /// 设置某个trigger动画
        /// </summary>
        /// <param name="triggerName"></param>
        public void IssueTrigger(string triggerName)
        {
            anim.SetTrigger(triggerName);
        }

        /// <summary>
        /// 设置某个bool动画
        /// </summary>
        /// <param name="boolName"></param>
        public void SetBool(string boolName, bool value)
        {
            anim.SetBool(boolName, value);
        }

        /// <summary>
        /// 禁用角色控制
        /// </summary>
        public void InputDisable()
        {
            pi.inputEnabled = false;
        }
        /// <summary>
        /// 启用用角色控制
        /// </summary>
        public void InputEnable()
        {
            pi.inputEnabled = true;
        }

        #region  动画消息接收处理

        public void OnJumpEnter()
        {
            pi.inputEnabled = false;
            lockPlanar = true;
            thrustVec = new Vector3(0, jumpVelocity, 0);
            trackDirectiion = true;
        }

        public void IsGround()
        {
            anim.SetBool("isGround", true);
        }

        public void IsNotGround()
        {
            anim.SetBool("isGround", false);
        }

        public void OnGroundEnter()
        {
            pi.inputEnabled = true;
            lockPlanar = false;
            canAttack = true;
            col.material = frictionOne;
            trackDirectiion = false;
        }

        public void OnGroundExit()
        {
            col.material = frictionZero;
        }

        public void OnFallEnter()
        {
            pi.inputEnabled = false;
            lockPlanar = true;
        }

        public void OnRollEnter()
        {
            thrustVec = new Vector3(0, rollVelocity, 0);
            pi.inputEnabled = false;
            lockPlanar = true;
            trackDirectiion = true;
        }

        public void OnJabEnter()
        {
            pi.inputEnabled = false;
            lockPlanar = true;
        }

        public void OnJabUpdate()
        {
            thrustVec = model.transform.forward * anim.GetFloat("javVelocity") * jabMultiplier;
        }

        public void OnAttack1hAEnter()
        {
            pi.inputEnabled = false;
            //lerpTarget = 1.0f;
        }

        public void OnAttackExit()
        {
            model.SendMessage("WeaponDisable");
        }

        public void OnAttack1hAUpdate()
        {
            thrustVec = model.transform.forward * anim.GetFloat("attack1hAVelocity") * jabMultiplier;
            //float currentWeight = Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack")), lerpTarget, 0.4f);  //获取攻击层权重逐渐增加到1
            //anim.SetLayerWeight(anim.GetLayerIndex("attack"), currentWeight);
        }

        public void OnUpdateRM(object _deltaPos)
        {
            if (CheckState("attack1hC"))
                deltaPos += (0.8f * deltaPos + 0.2f * (Vector3)_deltaPos) / 1.0f;
        }

        public void OnHitEnter()
        {
            pi.inputEnabled = false;
            PlanarVec = Vector3.zero;
            model.SendMessage("WeaponDisable");
        }

        public void OnBlockedEnter()
        {
            pi.inputEnabled = false;
        }

        public void OnDieEnter()
        {
            pi.inputEnabled = false;
            PlanarVec = Vector3.zero;
            model.SendMessage("WeaponDisable");
        }

        public void OnStunnedEnter()
        {
            pi.inputEnabled = false;
            PlanarVec = Vector3.zero;
        }

        public void OnCounterBackEnter()
        {
            pi.inputEnabled = false;
            PlanarVec = Vector3.zero;
        }

        public void OnCounterBackExit()
        {
            model.SendMessage("CounterBackDisable");
        }

        public void OnSpringbackEnter()
        {
            pi.inputEnabled = false;
            PlanarVec = Vector3.zero;
        }

        public void OnLockEnter()
        {
            pi.inputEnabled = false;
            PlanarVec = Vector3.zero;
            model.SendMessage("WeaponDisable");
        }

        #endregion

    }
}