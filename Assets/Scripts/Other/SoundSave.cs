using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC
{

    public class SoundSave : MonoBehaviour
    {
        public static SoundSave instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        public SoundAudioClip[] soundAudioClipArray;


        [System.Serializable]
        public class SoundAudioClip
        {
            public SoundManager.Sound sound;
            public AudioClip audioClip;

            [Range(0f,1f)]
            public float volume;

            public bool loop;
        }
    }
}
