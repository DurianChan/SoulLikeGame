using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DC
{
    ///<summary>
    ///相机控制
    ///</summary>
    public class CameraController : MonoBehaviour
    {
        private IUserInput pi;
        public float horizontalSpeed = 100.0f;      //相机水平旋转速度
        public float verticalSpeed = 80.0f;         //相机竖直旋转速度
        public float cameraDampValue = 0.05f;       //相机跟随速度
        public Image lockDot;                       //锁定提示
        public bool lockState;                      //锁定状态
        public bool isAI = false;                   //判断是否为AI
        
        private GameObject playerHandle;
        private GameObject cameraHandle;
        private float tempEulerX;
        private GameObject model;
        private GameObject cameraFollow;
        private Vector3 cameraDampVelocity;
        [SerializeField] private LockTarget lockTarget;      //锁定目标

        private void Start()
        {
            cameraHandle = transform.parent.gameObject;
            playerHandle = cameraHandle.transform.parent.gameObject;
            tempEulerX = 20;
            ActorController ac = playerHandle.GetComponent<ActorController>();
            model = ac.model;
            pi = ac.pi;
            if (!isAI)      
            {
                cameraFollow = Camera.main.gameObject;
                lockDot.enabled = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            lockState = false;
        }

        private void FixedUpdate()
        {
            if (lockTarget == null)
            {
                Vector3 tempModelEuler = model.transform.eulerAngles;

                playerHandle.transform.Rotate(Vector3.up, pi.Jright * horizontalSpeed * Time.fixedDeltaTime);
                tempEulerX -= pi.Jup * verticalSpeed * Time.fixedDeltaTime;
                tempEulerX = Mathf.Clamp(tempEulerX, -40, 30);  //限制上下相机的角度
                cameraHandle.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);
                model.transform.eulerAngles = tempModelEuler;   //将模型的欧拉角赋值回来
            }
            else
            {
                Vector3 tempForward = lockTarget.obj.transform.position - model.transform.position;
                tempForward.y = 0;
                playerHandle.transform.forward = tempForward;
                if(!isAI)
                    cameraFollow.transform.LookAt(lockTarget.obj.transform);
            }

            if (!isAI)
            {
                cameraFollow.transform.position = Vector3.SmoothDamp(cameraFollow.transform.position, transform.position, ref cameraDampVelocity, cameraDampValue);
                //camera.transform.eulerAngles = transform.eulerAngles;
                cameraFollow.transform.LookAt(cameraHandle.transform);
            }
        }

        private void Update()
        {
            if (lockTarget != null)     //设置锁定提示为目标点的半高处
            {
                if (!isAI)
                {
                    lockDot.rectTransform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position + new Vector3(0, lockTarget.halfHeight, 0));
                }             
                if (Vector3.Distance(model.transform.position, lockTarget.obj.transform.position) > 10.0f)
                {
                    LockProcessA(null,false, false, isAI);
                }
                if (lockTarget != null && lockTarget.am!=null && lockTarget.am.sm.isDie)       //锁定目标已死亡即可解锁
                {
                    LockProcessA(null, false, false, isAI);
                }
            }              
        }

        private void LockProcessA(LockTarget _lockTarget, bool _lockDotEnable, bool _lockState,bool _isAI)
        {
            lockTarget = _lockTarget;
            if (!_isAI)
            {
                lockDot.enabled = _lockDotEnable;
            }
            lockState = _lockState;
        }

        //private void OnDrawGizmosSelected()
        //{
        //    Gizmos.color = Color.green;
        //    Vector3 modelOrigin = Vector3.zero;
        //    if (model != null)
        //        modelOrigin = model.transform.position + new Vector3(0, 1, 0);
        //    Gizmos.DrawWireSphere(modelOrigin, 3.5f);
        //}

        /// <summary>
        /// 切换锁定敌人相机状态
        /// </summary>
        public void LockUnlock()
        {
            Vector3 modelOrigin = model.transform.position + new Vector3(0, 1, 0);
            Collider[] cols = Physics.OverlapSphere(modelOrigin, 5f, LayerMask.GetMask(isAI?"Player":"Enemy"));
            bool isLock = false;        //检测是否锁定目标
            if (cols.Length == 0)
            {
                LockProcessA(null, false, false, isAI);
            }
            else
            {
                foreach (var col in cols)
                {
                    if (lockTarget != null && lockTarget.obj == col.gameObject)     //如果锁定目标为同一个则继续检测是否有下一个目标可以锁定
                        continue;

                    LockProcessA(new LockTarget(col.gameObject, col.bounds.extents.y), true, true, isAI);
                    isLock = true;
                    break;
                }
                if (isLock == false)            //如果当前已锁定目标，且再次锁定目标时有且只有这一个时，退出锁定
                {
                    LockProcessA(null, false, false, isAI);
                }
            }
        }
    }

    /// <summary>
    /// 锁定目标类
    /// </summary>
    public class LockTarget
    {
        public GameObject obj;      //锁定的目标物体
        public float halfHeight;    //锁定物体的半高
        public ActorManager am;     //锁定物体的角色管理

        public LockTarget(GameObject _obj,float _halfHeight)
        {
            obj = _obj;
            halfHeight = _halfHeight;
            am = _obj.GetComponent<ActorManager>();
        }
    }

}


