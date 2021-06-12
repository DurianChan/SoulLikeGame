using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///角色管理
    ///</summary>
    public class ActorManager : MonoBehaviour
    {
        public ActorController ac;

        [Header("=== Auto Generate if Null ===")]
        public BattleManager bm;
        public WeaponManager wm;
        public StateManager sm;
        public DirectorManager dm;
        public InteractionManager im;

        [Header("=== Override Animators ===")]
        public AnimatorOverrideController oneHandAnim;
        public AnimatorOverrideController twoHandAnim;

        /// <summary>
        /// //用于显示死亡提示
        /// </summary>       
        public LevelLoader level;                  //过渡类
        public DeathPanel panel;                   //死亡界面
        public string deathReason = "菜";          //死亡原因

        private void OnValidate()
        {
            if (level == null)
                level = FindObjectOfType<LevelLoader>();
            if (panel == null)
                panel = FindObjectOfType<DeathPanel>();
        }

        private void Awake()
        {
            ac = GetComponent<ActorController>();
            GameObject model = ac.model;
            GameObject sensor = null;

            if (!ac.isItem)
                sensor = transform.Find("sensor").gameObject;

            bm = Bind<BattleManager>(sensor);
            wm = Bind<WeaponManager>(model);
            sm = Bind<StateManager>(gameObject);
            dm = Bind<DirectorManager>(gameObject);
            im = Bind<InteractionManager>(sensor);

            ac.OnAction += DoAction;
        }

        /// <summary>
        /// 获取指定的类，并互相绑定到角色管理类上
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        private T Bind<T>(GameObject go) where T : IActorManagerInterface     //将传入的值类型为为IActorManagerInterface的这一类或子类，否则报错
        {
            if (go == null)
            {
                return null;
            }
            T tempInstance = go.GetComponent<T>();
            if (tempInstance == null)
            {
                tempInstance = go.AddComponent<T>();
            }
            tempInstance.am = this;
            return tempInstance;
        }

        /// <summary>
        /// 受伤行为
        /// </summary>
        public void TryDoDamage(WeaponController targetWc, bool attackValid, bool counterValid, bool defenseValid)
        {
            if (sm.isCounterBackSuccess)
            {
                if (counterValid)
                    targetWc.wm.am.Stunned();
            }
            else if (sm.isCounterBackFailure)
            {
                if (attackValid)
                    HitOrDie(targetWc, false, false);
            }
            else if (sm.isImmortal)
            {
                //Do nothing!
            }
            else if (sm.isDefense && defenseValid)
            {
                Blocked(targetWc.GetATK());
                targetWc.wm.am.ac.IssueTrigger("springback");
            }
            else
            {
                if (attackValid)
                    HitOrDie(targetWc, true, false);
            }
        }

        /// <summary>
        /// 设置受伤死亡更变值的方法
        /// </summary>
        public void HitOrDie(WeaponController targetWc, bool doHitAnimation, bool doBackstab)
        {
            if (sm.currentHealth <= 0)
            {
                //Alerady dead
            }
            else if (doBackstab)
            {
                float doDamage = -1 * targetWc.GetATK() * 3 + -1 * targetWc.wm.am.sm.GetBaseATK() + sm.GetBaseDEF();
                if (doDamage > 0)
                    doDamage = -1;
                sm.ChangeHealth(doDamage);
                if (sm.currentHealth <= 0)
                {
                    Die();
                }
            }
            else
            {
                float doDamage = -1 * targetWc.GetATK() + -1 * targetWc.wm.am.sm.GetBaseATK() + sm.GetBaseDEF();
                if (doDamage > 0)
                    doDamage = -1;
                sm.ChangeHealth(doDamage);
                if (sm.currentHealth > 0)
                {
                    if (doHitAnimation)
                        Hit();
                }
                else
                {
                    Die();
                }
            }
        }

        /// <summary>
        /// 设置受伤状态
        /// </summary>
        public void Hit()
        {
            ac.IssueTrigger("hit");
        }

        /// <summary>
        /// 设置破防状态
        /// </summary>
        public void Stunned()
        {
            transform.Find("caster").GetComponent<EventCasterManager>().active = true;
            ac.IssueTrigger("stunned");
            StartCoroutine(ResetCasterCol());
        }

        private IEnumerator ResetCasterCol()
        {
            yield return new WaitForSeconds(2.8f);
            transform.Find("caster").GetComponent<EventCasterManager>().active = false;
        }

        /// <summary>
        /// 设置防御状态
        /// </summary>
        public void Blocked(float attackerATK)
        {

            if (attackerATK - sm.GetBaseDEF() > 0)
            {
                sm.ChangeStamina(-8);
                if (sm.currentStamina == 0)
                {
                    ac.IssueTrigger("stunned");
                    if (ac.camcon.isAI)
                    {
                        wm.am.Stunned();
                    }
                    return;
                }

            }
            else
            {
                sm.ChangeStamina(-3);
                if (sm.currentStamina == 0)
                {
                    ac.IssueTrigger("stunned");
                    if (ac.camcon.isAI)
                    {
                        wm.am.Stunned();
                    }
                    return;
                }
            }

            ac.IssueTrigger("blocked");
        }

        /// <summary>
        /// 设置死亡方法
        /// </summary>
        public void Die()
        {
            ac.IssueTrigger("die");
            ac.InputDisable();
            if (ac.camcon.isAI)
            {
                ac.rigid.isKinematic = true;
                sm.healthBar.gameObject.SetActive(false);
            }
            if (ac.camcon.lockState == true)
            {
                ac.camcon.LockUnlock();
            }
            ac.camcon.enabled = false;
            if (!ac.camcon.isAI)
            {
                level.playerSavePos.GetComponentInChildren<SaveLoadFire>().SaveCharacterPanel();
                StartCoroutine(DeathTips());
            }
        }

        /// <summary>
        /// 显示死亡提示
        /// </summary>
        /// <returns></returns>
        public IEnumerator DeathTips()
        {
            yield return new WaitForSeconds(1f);
            panel.ShowDeathPanel(deathReason);
            yield return new WaitForSeconds(1f);
            StartCoroutine(level.RefreshPlayer());
        }

        /// <summary>
        /// 玩家重生
        /// </summary>
        public void Rebirth()
        {
            ac.IssueTrigger("rebirth");
            sm.ResetHealth();
            sm.ResetStamina();
            ac.InputEnable();
            ac.camcon.enabled = true;
        }

        /// <summary>
        /// 修改盾反开关状态
        /// </summary>
        /// <param name="value"></param>
        public void SetIsCounterBack(bool value)
        {
            sm.isCounterBackEnable = value;
        }

        /// <summary>
        /// 锁住与解锁状态机
        /// </summary>
        public void LockUnLockActorController(bool value)
        {
            ac.SetBool("lock", value);
        }

        /// <summary>
        /// 切换为双手武器动作动画
        /// </summary>
        /// <param name="dualOn"></param>
        public void ChangeDualHands(bool dualOn)
        {
            if (dualOn)
            {
                ac.anim.runtimeAnimatorController = twoHandAnim;
            }
            else
            {
                ac.anim.runtimeAnimatorController = oneHandAnim;
            }

        }

        /// <summary>
        /// 动作事件
        /// </summary>
        public void DoAction()
        {
            if (im.overlapEcastms.Count != 0)
            {
                if (im.overlapEcastms[0].active == true && !dm.IsPlaying())
                {
                    if (im.overlapEcastms[0].eventName == "frontStab")
                    {
                        dm.PlayActionTimeline("frontStab", this, im.overlapEcastms[0].am);
                        im.overlapEcastms[0].active = false;
                        im.overlapEcastms[0].am.HitOrDie(wm.wcR, false, true);
                    }
                    else if (im.overlapEcastms[0].eventName == "openBox")
                    {
                        if (BattleManager.CheckAnglePlayer(ac.model, im.overlapEcastms[0].am.gameObject, 45))     //在张角45度内才能开箱子
                        {
                            im.overlapEcastms[0].active = false;
                            //修正角色开完箱子的位置
                            transform.position = im.overlapEcastms[0].am.transform.position + im.overlapEcastms[0].am.transform.TransformVector(im.overlapEcastms[0].offset);
                            ac.model.transform.LookAt(im.overlapEcastms[0].am.transform, Vector3.up);
                            dm.PlayActionTimeline("openBox", this, im.overlapEcastms[0].am);
                        }
                    }
                    else if (im.overlapEcastms[0].eventName == "leverUp")
                    {
                        if (BattleManager.CheckAnglePlayer(ac.model, im.overlapEcastms[0].am.gameObject, 45))     //在张角15度内才能开箱子
                        {
                            im.overlapEcastms[0].active = false;
                            //修正角色开完箱子的位置
                            transform.position = im.overlapEcastms[0].am.transform.position + im.overlapEcastms[0].am.transform.TransformVector(im.overlapEcastms[0].offset);
                            ac.model.transform.LookAt(im.overlapEcastms[0].am.transform, Vector3.up);
                            dm.PlayActionTimeline("leverUp", this, im.overlapEcastms[0].am);
                        }
                    }
                }
            }
        }

    }
}
