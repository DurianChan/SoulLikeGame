using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DC
{
    ///<summary>
    ///关卡加载过渡动画控制
    ///</summary>
    public class LevelLoader : MonoBehaviour
    {
        public Animator transition;
        public ActorManager playerAM;
        public float transitionTime = 1f;
        public SaveLoadPos playerSavePos;  //重置位置

        private void OnValidate()
        {
            if (playerSavePos == null)
                playerSavePos = FindObjectOfType<SaveLoadPos>();
        }

        private void Start()
        {
            playerAM = GameManager._instance.playerAM;
        }

        /// <summary>
        /// 重载本关卡
        /// </summary>
        public void LoadThisLevel()
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
        }

        /// <summary>
        /// 加载上一个关卡
        /// </summary>
        public void LoadLastLevel()
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
        }

        /// <summary>(2)
        /// 加载下一个关卡
        /// </summary>
        /// <param name="levelIndex"></param>
        /// <returns></returns>
        IEnumerator LoadLevel(int levelIndex)
        {
            //Play animation
            transition.SetTrigger("start");
            //Wait
            yield return new WaitForSeconds(transitionTime);
            //Load scene
            SceneManager.LoadScene(levelIndex);
        }

        /// <summary>
        /// 重生玩家：恢复血量并将位置设置为保存点位置
        /// </summary>
        /// <param name="refreshPos"></param>
        /// <returns></returns>
        public IEnumerator RefreshPlayer(Vector3 refreshPos)
        {
            //Play animation
            transition.SetTrigger("start");
            //Wait
            yield return new WaitForSeconds(transitionTime);
            transition.SetTrigger("end");
            InitPlayerPosition(refreshPos);
            playerAM.Rebirth();
        }

        /// <summary>
        /// 重生玩家：恢复血量并将位置设置为保存点位置
        /// </summary>
        /// <param name="refreshPos"></param>
        /// <returns></returns>
        public IEnumerator RefreshPlayer()
        {
            //Play animation
            transition.SetTrigger("start");
            //Wait
            yield return new WaitForSeconds(transitionTime);
            LoadThisLevel();
        }

        /// <summary>
        /// 重置玩家位置
        /// </summary>
        /// <param name="pos"></param>
        public void InitPlayerPosition(Vector3 pos)
        {
            playerAM.gameObject.transform.position = pos;        //重置位置
        }
    }
}
