using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///数据中心存储
    ///</summary>
    public class DataBase
    {
        private string weaponDatabaseFileName = "weaponData";       //武器信息,设置只能读
        public readonly JSONObject weaponDataBase;              
        
        

        public DataBase()
        {
            TextAsset weaponContent = Resources.Load(weaponDatabaseFileName) as TextAsset;
            weaponDataBase = new JSONObject(weaponContent.text);          
        }

    }
}
