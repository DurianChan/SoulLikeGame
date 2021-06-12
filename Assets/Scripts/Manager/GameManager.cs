using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///游戏管理
    ///</summary>
    public class GameManager : MonoBehaviour
    {
        public ActorManager playerAM;

        public static GameManager _instance;
        private DataBase weaponDB;
        public WeaponFactory weaponFact;

        private void Awake()
        {
            CheckGameObject();
            CheckSingle();
            SoundManager.Initialize();
            SoundManager.PlaySound(SoundManager.Sound.Background);
        }

        private void Start()
        {
            InitWeaponDB();
            InitWeaponFactory();
        }

        /// <summary>
        /// 获取任意键值
        /// </summary>
        /// <returns></returns>
        public KeyCode getKeyDownCode()
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        return keyCode;
                    }
                }
            }
            return KeyCode.None;
        }

        /// <summary>
        /// 初始化武器信息
        /// </summary>
        private void InitWeaponDB()
        {
            weaponDB = new DataBase();
        }

        /// <summary>
        /// 初始化武器工厂
        /// </summary>
        private void InitWeaponFactory()
        {
            weaponFact = new WeaponFactory(weaponDB);
        }

        /// <summary>
        /// 检测游戏物体是否符合
        /// </summary>
        private void CheckGameObject()
        {
            if(tag == "GM")
            {
                return;
            }
            else
            {
                Destroy(this);
            }
        }

        /// <summary>
        /// 检测是否为唯一：单例模式
        /// </summary>
        private void CheckSingle()
        {
            if (_instance == null)
            {
                _instance = this;
                //DontDestroyOnLoad(gameObject);
                return;
            }
            //Destroy(this);
        }

    }
}
