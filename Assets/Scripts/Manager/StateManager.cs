using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///状态管理
    ///</summary>
    public class StateManager : IActorManagerInterface
    {
        [Header("=== Health Settings ===")]
        public float currentHealth;
        public float maxHealth = 15.0f;
        public HealthBar healthBar;

        [Header("=== Shamina Settings ===")]
        public float currentStamina;
        public float maxStamina = 50.0f;
        private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
        private Coroutine regen;
        public StaminaBar staminaBar;

        [Header("=== ATK and DEF settings ===")]
        public float ATK = 5.0f;
        public float DEF = 5.0f;

        [Header("=== 1st order states flags ===")]
        public bool isGround;
        public bool isJump;
        public bool isFall;
        public bool isRoll;
        public bool isJab;
        public bool isAttack;
        public bool isHit;
        public bool isDie;
        public bool isBlocked;
        public bool isDefense;
        public bool isCounterBack;
        public bool isCounterBackEnable;

        [Header("=== 2nd order state flag ===")]
        public bool isAllowDefense;
        public bool isImmortal;             //无敌状态
        public bool isCounterBackSuccess;   //判断盾反成功
        public bool isCounterBackFailure;   //判断盾反失败

        private void Start()
        {
            ResetHealth();
            ResetStamina();
            if (healthBar != null)
            {
                healthBar.SetMaxHealth(maxHealth);
            }
            if (staminaBar!=null)
            {
                staminaBar.SetMaxStamina(maxStamina);
            }
        }

        private void Update()
        {
            isGround = am.ac.CheckState("ground");
            isJump = am.ac.CheckState("jump");
            isFall = am.ac.CheckState("fall");
            isRoll = am.ac.CheckState("roll");
            isJab = am.ac.CheckState("jab");
            isAttack = am.ac.CheckStateTag("attackR") || am.ac.CheckStateTag("attackL");
            isHit = am.ac.CheckState("hit");
            isDie = am.ac.CheckState("die");
            isBlocked = am.ac.CheckState("blocked");
            isCounterBack = am.ac.CheckState("counterBack");
            isCounterBackSuccess = isCounterBackEnable;
            isCounterBackFailure = isCounterBack && !isCounterBackEnable;

            isAllowDefense = isGround || isBlocked;
            isDefense = isAllowDefense && am.ac.CheckState("defense1h", "defense");
            isImmortal = isRoll || isJab;                                               //翻滚和后侧为无敌状态

            if (isJump)
                SoundManager.PlaySound(SoundManager.Sound.Jump);
            if(isAttack)
                SoundManager.PlaySound(SoundManager.Sound.Attack);
            if(isBlocked)
                SoundManager.PlaySound(SoundManager.Sound.Blocked);
            if (isDie && !am.ac.camcon.isAI)
                SoundManager.PlaySound(SoundManager.Sound.Die);
            if (isHit)
                SoundManager.PlaySound(SoundManager.Sound.Hit);
            if (isRoll)
                SoundManager.PlaySound(SoundManager.Sound.Roll);

        }

        /// <summary>
        /// 重置HP值
        /// </summary>
        public void ResetHealth()
        {
            ChangeHealth(maxHealth);
        }

        /// <summary>
        /// 重置耐力
        /// </summary>
        public void ResetStamina()
        {
            ChangeStamina(maxStamina);
        }

        /// <summary>
        /// 改变HP的值
        /// </summary>
        /// <param name="value"></param>
        public void ChangeHealth(float value)
        {
            currentHealth += value;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            if (healthBar != null)
            {
                healthBar.SetHealth(currentHealth);
            }
        }

        /// <summary>
        /// 改变耐力值
        /// </summary>
        /// <param name="value"></param>
        public void ChangeStamina(float value)
        {
            currentStamina += value;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            if (staminaBar!=null)
            {
                staminaBar.SetShamina(currentStamina);
            }
            if (regen != null)
                StopCoroutine(regen);

            regen = StartCoroutine(RegenStamina());
        }

        /// <summary>
        /// 恢复耐力值
        /// </summary>
        /// <returns></returns>
        private IEnumerator RegenStamina()
        {
            yield return new WaitForSeconds(2);
            while (currentStamina < maxStamina)
            {
                currentStamina += maxStamina / 50;
                if (staminaBar != null)
                {
                    staminaBar.SetShamina(currentStamina);
                }
                yield return regenTick;
            }
            if(currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
            regen = null;
        }

        /// <summary>
        /// 获取人物基础攻击值
        /// </summary>
        /// <returns></returns>
        public float GetBaseATK()
        {
            return ATK;
        }

        /// <summary>
        /// 获取人物基础防御值
        /// </summary>
        /// <returns></returns>
        public float GetBaseDEF()
        {
            return DEF;
        }
    }
}
