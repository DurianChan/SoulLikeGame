using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///武器控制
    ///</summary>
    public class WeaponController : MonoBehaviour
    {
        public WeaponManager wm;
        public WeaponData wdata;

        private void Awake()
        {
            wdata = GetComponentInChildren<WeaponData>();
        }

        private void Update()
        {
            
        }

        public float GetATK()
        {
            float ATKSum = 0;
            if (wdata != null)
                ATKSum += wdata.ATK;
            ATKSum += wm.am.sm.GetBaseATK();
            return ATKSum;
        }

        public float GetDEF()
        {
            float DEFSum = 0;
            if (wdata != null)
                DEFSum += wdata.ATK;
            DEFSum += wm.am.sm.GetBaseDEF();
            return DEFSum;
        }

    }
}
