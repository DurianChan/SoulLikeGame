using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///控制Boss门的关闭
    ///</summary>
    public class DoorClose : MonoBehaviour
    {
        public Animator animator;
        public GameObject boss;
        public StateManager bossSM;
        public GameObject passGame;
        public bool isOpenn;        //判断门是否可以打开
        public bool isAlive;        //判断boss是否活着
        public bool isShow;         //判断能否显示通关界面

        private void Start()
        {
            bossSM = boss.GetComponent<StateManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && animator.GetBool("open"))
                isOpenn = true;
        }

        private void Update()
        {
            isAlive = bossSM.currentHealth <= 0;

            if (isOpenn)
            {
                isOpenn = false;
                animator.SetBool("open", false);
                boss.SetActive(true);
                SoundManager.StopSound(SoundManager.Sound.Background);
                SoundManager.PlaySound(SoundManager.Sound.BossBG);
            }

            if (isAlive && !isShow)
            {
                StartCoroutine(ShowPassGame());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            isOpenn = false;
        }

        
        IEnumerator ShowPassGame()
        {
            isShow = true;
            GetComponent<Collider>().enabled = false;
            passGame.SetActive(true);
            animator.SetBool("open", true);
            SoundManager.StopSound(SoundManager.Sound.BossBG);
            yield return new WaitForSeconds(2.5f);
            passGame.SetActive(false);
            SoundManager.PlaySound(SoundManager.Sound.Background);
        }
    }
}