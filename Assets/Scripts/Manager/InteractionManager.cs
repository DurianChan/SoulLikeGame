using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///互动管理
    ///</summary>
    public class InteractionManager : IActorManagerInterface
    {
        private CapsuleCollider interCol;

        public List<EventCasterManager> overlapEcastms = new List<EventCasterManager>();

        private void Start()
        {
            interCol = GetComponent<CapsuleCollider>();
        }

        private void OnTriggerEnter(Collider col)
        {
            EventCasterManager[] ecastms = col.GetComponents<EventCasterManager>();
            foreach (var ecastm in ecastms)
            {
                if (!overlapEcastms.Contains(ecastm))
                {
                    overlapEcastms.Add(ecastm);
                }
            }
        }

        private void OnTriggerStay(Collider col)
        {

        }

        private void OnTriggerExit(Collider col)
        {
            EventCasterManager[] ecastms = col.GetComponents<EventCasterManager>();
            foreach (var ecastm in ecastms)
            {
                if (overlapEcastms.Contains(ecastm))
                {
                    overlapEcastms.Remove(ecastm);
                }
            }
        }

    }
}
