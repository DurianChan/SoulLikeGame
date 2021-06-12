using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DC
{
    ///<summary>
    ///测试敌人导航寻路
    ///</summary>
    public class TestEnemyAgent : MonoBehaviour
    {
        [SerializeField] private DummyIUserInput input;
        [SerializeField] private Transform followTarget;

        public float a = 0;
        public float b = 1f;
        public float x = 0;
        public float speed;
        public float startTime;
        private void Update()
        {
            if (x == 1)
                gameObject.SetActive(false);
            if (x >= 0.99)
            {
                Debug.Log(x);
                x = 1;
            }
            else
            {
                x = Mathf.Lerp(a,b,(Time.time- startTime) *speed);
                Debug.Log(x);
            }
        }

        //private void Start()
        //{
        //    input = GetComponent<DummyIUserInput>();
        //}

        //private void Update()
        //{
        //    PhysicsPlayer();
        //}

        //private void OnDrawGizmosSelected()
        //{
        //    Gizmos.color = Color.green;
        //    Vector3 modelOrigin = Vector3.zero;
        //    modelOrigin = transform.position + new Vector3(0, 1, 0);
        //    Gizmos.DrawWireSphere(modelOrigin, 8f);
        //}

        //private void PhysicsPlayer()
        //{
        //    Vector3 modelOrigin = transform.position + new Vector3(0, 1, 0);
        //    Collider[] cols = Physics.OverlapSphere(modelOrigin, 8f, LayerMask.GetMask("Player"));
        //    if (cols.Length == 0)
        //    {
        //        input.Dup = 0;
        //        input.Dright = 0;
        //        return;
        //    }
        //    else
        //    {
        //        foreach (var col in cols)
        //        {
        //            followTarget = col.transform;
        //        if (Vector3.Distance(transform.position,followTarget.position)<2f)
        //            {
        //                input.lockon = true;
        //                input.Dup = 0;
        //                input.Dright = 0;
        //                input.rb = true;
        //            }
        //            else if(Vector3.Distance(transform.position, followTarget.position) <3f)
        //            {
        //                input.lockon = true;
        //                input.Dup = 0.5f;
        //                input.Dright = 0;
        //            }
        //            else if(Vector3.Distance(transform.position, followTarget.position) < 5f)
        //            {
        //                input.lockon = true;
        //                input.Dup = 0.8f;
        //                input.Dright = 0;
        //            }
        //            else
        //            {
        //                input.lockon = true;
        //                input.Dup = 1f;                       
        //                input.Dright = 0;
        //                input.Jright = 1.0f;
        //                input.Jup = 0f;
        //                input.run = true;
        //            }
        //        }
        //    }
        //}
    }
}
