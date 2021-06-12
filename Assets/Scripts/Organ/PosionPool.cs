using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///毒池
    ///</summary>
    public class PosionPool : MonoBehaviour
    {


        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                other.gameObject.GetComponent<ActorManager>().Die();
            }
        }


    }
}
