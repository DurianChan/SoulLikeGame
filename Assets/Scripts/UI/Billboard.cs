using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///
    ///</summary>
    public class Billboard : MonoBehaviour
    {
        private Transform cam;

        private void Start()
        {
            cam = Camera.main.gameObject.transform;
        }

        private void LateUpdate()
        {
            transform.LookAt(transform.position + cam.forward);
        }

    }
}
