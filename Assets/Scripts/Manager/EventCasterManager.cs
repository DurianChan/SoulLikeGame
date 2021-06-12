using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///背刺事件管理
    ///</summary>
    public class EventCasterManager : IActorManagerInterface
    {
        public string eventName;
        public Collider col;
        public bool active;
        public Vector3 offset = new Vector3(0,0,1f);

        private void Start()
        {
            col = GetComponent<Collider>();
            if (am == null)
            {
                am = GetComponentInParent<ActorManager>();
            }
        }

    }
}
