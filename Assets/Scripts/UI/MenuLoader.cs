using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DC
{
    ///<summary>
    ///菜单加载过渡
    ///</summary>
    public class MenuLoader : MonoBehaviour
    {
        public Animator transition;
        public float transitionTime = 1f;
        private static string directoryName = "SaveData";            //存储数据文件夹

        private void Awake()
        {
            SoundManager.PlaySound(SoundManager.Sound.Menu);
        }

        /// <summary>
        /// 删除所有存档重新开始
        /// </summary>
        public void StartGame()
        {
            string path = Application.persistentDataPath + "/" + directoryName;
            DeleteAllFile(path);
            LoadNextLevel();
        }

        /// <summary>
        /// 删除指定文件目录下的所有文件
        /// </summary>
        /// <param name="fullPath">文件路径</param>
        public bool DeleteAllFile(string fullPath)
        {
            //获取指定路径下面的所有资源文件  然后进行删除
            if (Directory.Exists(fullPath))
            {
                DirectoryInfo direction = new DirectoryInfo(fullPath);
                FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

                //Debug.Log(files.Length);

                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name.EndsWith(".meta"))
                    {
                        continue;
                    }
                    string FilePath = fullPath + "/" + files[i].Name;
                    //print(FilePath);
                    File.Delete(FilePath);
                }
                return true;
            }
            return false;
        }


        /// <summary>
        /// 加载下一个关卡(1)
        /// </summary>
        public void LoadNextLevel()
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
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

        public void QuickGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

    }
}