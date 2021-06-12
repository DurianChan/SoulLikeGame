using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///武器工厂
    ///</summary>
    public class WeaponFactory
    {
        private DataBase weaponDB;

        public WeaponFactory(DataBase _weaponDB)
        {
            weaponDB = _weaponDB;
        }

        /// <summary>
        /// 创建武器
        /// </summary>
        /// <param name="weaponName"></param>
        public GameObject CreateWeapon(string weaponName, Vector3 pos, Quaternion rot)
        {
            GameObject prefab = Resources.Load(weaponName) as GameObject;
            GameObject obj = GameObject.Instantiate(prefab, pos, rot);

            //SetWeaponData(obj, weaponName);

            return obj;
        }

        public Collider CreateWeapon(string weaponName, string side, WeaponManager wm)
        {
            WeaponController wc;
            if (side == "L")
            {
                wc = wm.wcL;
            }
            else if (side == "R")
            {
                wc = wm.wcR;
            }
            else
            {
                return null; 
            }

            GameObject prefab = Resources.Load(weaponName) as GameObject;
            GameObject obj = GameObject.Instantiate(prefab);
            obj.transform.parent = wc.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;

            SetWeaponData(obj, weaponName,wc);

            if (side == "L")
            {
                if(obj.transform.Find(weaponName + "_R").gameObject != null)
                    obj.transform.Find(weaponName + "_R").gameObject.SetActive(false);
                if(obj.transform.Find(weaponName + "_L").gameObject != null)
                    obj.transform.Find(weaponName + "_L").gameObject.SetActive(true);
            }
            else if (side == "R")
            {
                //has done it
            }

            return obj.GetComponentInChildren<Collider>();
        }

        private void SetWeaponData(GameObject obj, string weaponName, WeaponController wc)
        {
            WeaponData wdata = obj.AddComponent<WeaponData>();
            wdata.ATK = weaponDB.weaponDataBase[weaponName]["ATK"].f;
            wdata.DEF = weaponDB.weaponDataBase[weaponName]["DEF"].f;
            wc.wdata = wdata;
        }
    }
}
