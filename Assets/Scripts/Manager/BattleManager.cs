using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///战斗管理
    ///</summary>
    [RequireComponent(typeof(CapsuleCollider))]     //防御碰撞框
    public class BattleManager : IActorManagerInterface
    {
        private CapsuleCollider defCol;

        private void Start()
        {
            defCol = GetComponent<CapsuleCollider>();
            defCol.center = Vector3.up * 1.0f;
            defCol.height = 2.0f;
            defCol.radius = 0.5f;
            defCol.isTrigger = true;
        }

        private void OnTriggerEnter(Collider col)
        {
            WeaponController targetWc = col.GetComponentInParent<WeaponController>();

            if (targetWc == null)
            {
                return;
            }

            GameObject attacker = targetWc.wm.am.gameObject;    //攻击者角色管理
            //GameObject receiver = am.gameObject;              
            GameObject receiver = am.ac.model;                  //受击者角色管理

            ////算出攻击者角度范围
            //Vector3 attackingDir = receiver.transform.position - attacker.transform.position;
            //float attackingAngle1 = Vector3.Angle(attacker.transform.forward, attackingDir);

            ////算出盾反角度范围
            //Vector3 counterDir = attacker.transform.position - receiver.transform.position;
            //float counterAngle1 = Vector3.Angle(receiver.transform.forward, counterDir);
            //float counterAngle2 = Vector3.Angle(attacker.transform.forward, receiver.transform.forward);

            //bool attackValid = (attackingAngle1 < 90);                                          //攻击夹角小于45度为成功
            //bool counterValid = (counterAngle1 < 30 && Mathf.Abs(counterAngle2 - 180) < 30);        //盾反夹角小于30度，且攻防两者面向夹角小于30度为成功

            if (col.tag == "Weapon")
            {
                am.TryDoDamage(targetWc, CheckAngleTarget(receiver,attacker,90), CheckAnglePlayer(receiver,attacker,45), CheckAnglePlayer(receiver, attacker, 45));
            }
            else if(col.tag == "Shield")
            {
                am.TryDoDamage(targetWc, false, false, CheckAnglePlayer(receiver, attacker, 45));
            }
        }

        /// <summary>
        /// 检测物体是否在玩家张角内
        /// </summary>
        /// <param name="player"></param>
        /// <param name="target"></param>
        /// <param name="playerAngleLimit"></param>
        /// <returns></returns>
        public static bool CheckAnglePlayer(GameObject player,GameObject target,float playerAngleLimit)
        {
            Vector3 counterDir = target.transform.position - player.transform.position;

            float counterAngle1 = Vector3.Angle(player.transform.forward, counterDir);
            float counterAngle2 = Vector3.Angle(target.transform.forward, player.transform.forward);
                                        
            bool attackValid = (counterAngle1 < playerAngleLimit && Mathf.Abs(counterAngle2 - 180) < playerAngleLimit);
            return attackValid;
        }

        /// <summary>
        /// 检测玩家是否在物体张角内
        /// </summary>
        /// <param name="player"></param>
        /// <param name="target"></param>
        /// <param name="targetAngleLimit"></param>
        /// <returns></returns>
        public static bool CheckAngleTarget(GameObject player,GameObject target,float targetAngleLimit)
        {
            Vector3 attackingDir = player.transform.position - target.transform.position;
            float attackingAngle1 = Vector3.Angle(target.transform.forward, attackingDir);

            bool counterValid = (attackingAngle1 < targetAngleLimit);                                         
            return counterValid;
        }

    }
}
