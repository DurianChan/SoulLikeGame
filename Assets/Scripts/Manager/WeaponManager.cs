using UnityEngine;

namespace DC
{
    ///<summary>
    ///武器管理
    ///</summary>
    public class WeaponManager : IActorManagerInterface
    {
        private Collider weaponColL;    //左手武器碰撞器
        private Collider weaponColR;    //右手武器碰撞器

        public GameObject whL;          //左手武器控制
        public GameObject whR;          //右手武器控制

        public WeaponController wcL;    //左手控制
        public WeaponController wcR;    //右手控制


        private void Start()
        {
            try
            {
                whL = transform.DeepFind("weaponHandleL").gameObject;
                wcL = BindWeaponController(whL);
                weaponColL = whL.GetComponentInChildren<Collider>();
                weaponColL = whL.transform.GetChild(0).GetComponentInChildren<Collider>();
            }
            catch (System.Exception ex)
            {
                ex.ToString();             
            }

            try
            {
                whR = transform.DeepFind("weaponHandleR").gameObject;
                wcR = BindWeaponController(whR);
                weaponColR = whR.transform.GetChild(0).GetComponentInChildren<Collider>();
            }
            catch (System.Exception ex)
            {
                ex.ToString();
            }
        }

        /// <summary>
        /// 绑定WeaponController
        /// </summary>
        /// <param name="targetObj"></param>
        public WeaponController BindWeaponController(GameObject targetObj)
        {
            WeaponController tempWc;
            tempWc = targetObj.GetComponent<WeaponController>();
            if (tempWc == null)
            {
                tempWc = targetObj.AddComponent<WeaponController>();
            }
            tempWc.wm = this;
            return tempWc;
        }

        /// <summary>
        /// 更新武器碰撞器
        /// </summary>
        public void UpdateWeaponCollider(string side, Collider col)
        {
            if (side == "L")
            {
                weaponColL = col;
            }
            else if (side == "R")
            {
                weaponColR = col;
            }
        }

        /// <summary>
        /// 更新左手装备
        /// </summary>
        /// <param name="weaponName">装备名称</param>
        /// <param name="isDual">是否为双手武器</param>
        /// <param name="isShield">是否为盾</param>
        public void UpdateLeftWeapon(WeaponFactory weaponFact, string weaponName,bool isDual, bool isShield)
        {
            if (isDual)
            {
                UnloadWeapon("L");
                UnloadWeapon("R");
                am.ac.leftIsShield = true;
            }
            else if (am.ac.anim.runtimeAnimatorController == am.twoHandAnim && !isDual)
            {
                UnloadWeapon("L");
                UnloadWeapon("R");
                am.ac.leftIsShield = isShield ? true : false;
            }
            else
            {
                UnloadWeapon("L");
                am.ac.leftIsShield = isShield ? true : false;
            }

            UpdateWeaponCollider("L", weaponFact.CreateWeapon(weaponName, "L", this));
            ChangeDualHands(isDual);
        }

        /// <summary>
        /// 更新右手手装备
        /// </summary>
        /// <param name="weaponName">装备名称</param>
        /// <param name="isDual">是否为双手武器</param>
        public void UpdateRightWeapon(WeaponFactory weaponFact,string weaponName, bool isDual)
        {
            if (isDual)
            {
                UnloadWeapon("L");
                UnloadWeapon("R");
                am.ac.leftIsShield = true;
            }
            else if (am.ac.anim.runtimeAnimatorController == am.twoHandAnim && !isDual)
            {
                UnloadWeapon("L");
                UnloadWeapon("R");
                am.ac.leftIsShield = false;
            }
            else
            {
                UnloadWeapon("R");
            }
            UpdateWeaponCollider("R", weaponFact.CreateWeapon(weaponName, "R", this));
            ChangeDualHands(isDual);
        }

        /// <summary>
        /// 卸下武器
        /// </summary>
        public void UnloadWeapon(string side)
        {
            if (side == "L")
            {
                weaponColL = null;
                wcL.wdata = null;
                foreach (Transform tran in whL.transform)
                {
                    Destroy(tran.gameObject);
                }
            }
            else if (side == "R")
            {
                weaponColR = null;
                wcR.wdata = null;
                foreach (Transform tran in whR.transform)
                {
                    Destroy(tran.gameObject);
                }
            }
        }

        /// <summary>
        /// 开启武器碰撞器
        /// </summary>
        public void WeaponEnable()
        {
            if (am.ac.CheckStateTag("attackL"))
            {
                if(weaponColL!=null)
                    weaponColL.enabled = true;
            }
            else
            {
                if(weaponColR!=null)
                    weaponColR.enabled = true;
            }
        }

        /// <summary>
        /// 关闭武器碰撞器
        /// </summary>
        public void WeaponDisable()
        {
            if (weaponColL != null)
                 weaponColL.enabled = false;
            if (weaponColR != null)
                weaponColR.enabled = false;
        }

        /// <summary>
        /// 切换为双手武器动作动画
        /// </summary>
        /// <param name="dualOn"></param>
        public void ChangeDualHands(bool dualOn)
        {
            am.ChangeDualHands(dualOn);
        }

        /// <summary>
        /// 设置盾反开启
        /// </summary>
        public void CounterBackEnable()
        {
            am.SetIsCounterBack(true);
        }

        /// <summary>
        /// 设置盾反关闭
        /// </summary>
        public void CounterBackDisable()
        {
            am.SetIsCounterBack(false);
        }
    }
}
