using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{
    ///<summary>
    ///
    ///</summary>
    public class TestBones : MonoBehaviour
    {
        public SkinnedMeshRenderer srcMeshRenderer;
        public SkinnedMeshRenderer tgtMeshRenderer;

        private void Start()
        {
            tgtMeshRenderer.bones = srcMeshRenderer.bones;
        }


    }
}
