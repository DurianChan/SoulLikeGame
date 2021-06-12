using System.Collections.Generic;
using UnityEngine;
using static DC.SoundSave;

namespace DC
{
    ///<summary>
    ///声音管理类
    ///</summary>
    public static class SoundManager
    {
        public enum Sound
        {
            Attack,
            Jump,
            Blocked,
            Roll,
            Hit,
            Die,
            Menu,
            BossBG,
            Background
        }

        private static Dictionary<Sound, float> soundTimerDictionary;       //使用字典作为播放音效的计时器，防止在update中每帧多次播放
        private static GameObject oneShotGameObject;
        private static AudioSource oneShotAudioSource;

        /// <summary>
        /// 初始化字典
        /// </summary>
        public static void Initialize()
        {
            soundTimerDictionary = new Dictionary<Sound, float>();
            soundTimerDictionary[Sound.Jump] = 0f;
            soundTimerDictionary[Sound.Attack] = 0f;
            soundTimerDictionary[Sound.Blocked] = 0f;
            soundTimerDictionary[Sound.Hit] = 0f;
            soundTimerDictionary[Sound.Roll] = 0f;
            soundTimerDictionary[Sound.Die] = 0f;
            soundTimerDictionary[Sound.Menu] = 0f;
            soundTimerDictionary[Sound.Background] = 0f;
            soundTimerDictionary[Sound.BossBG] = 0f;
        }

        /// <summary>
        /// 播放3D音效
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="position"></param>
        public static void PlaySound(Sound sound,Vector3 position) 
        {
            if (CanPlaySound(sound))
            {
                GameObject soundGameObject = new GameObject("Sound");
                soundGameObject.transform.position = position;
                AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
                audioSource.clip = GetAudioClip(sound).audioClip;
                audioSource.maxDistance = 100f;
                audioSource.spatialBlend = 1f;
                audioSource.rolloffMode = AudioRolloffMode.Linear;
                audioSource.dopplerLevel = 0f;
                audioSource.Play();

                Object.Destroy(soundGameObject, audioSource.clip.length);
            }
        }

        /// <summary>
        /// 停止播放对应音乐
        /// </summary>
        /// <param name="sound"></param>
        public static void StopSound(Sound sound)
        {
            if (oneShotGameObject == null)
            {
                oneShotGameObject = new GameObject("One Shot Sound");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            }
            SoundAudioClip soundAudio = GetAudioClip(sound);
            oneShotAudioSource.clip = soundAudio.audioClip;
            oneShotAudioSource.Stop();
        }

        /// <summary>
        /// 播放对应的音效
        /// </summary>
        /// <param name="sound"></param>
        public static void PlaySound(Sound sound)
        {
            if (CanPlaySound(sound))
            {
                if (oneShotGameObject == null)
                {
                    oneShotGameObject = new GameObject("One Shot Sound");
                    oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
                }
                SoundAudioClip soundAudio = GetAudioClip(sound);
                oneShotAudioSource.volume = soundAudio.volume;
                oneShotAudioSource.loop = soundAudio.loop;
                oneShotAudioSource.PlayOneShot(soundAudio.audioClip);
            }
        }

        /// <summary>
        /// 判断能否播放该音效:使用字典记录最后一次播放时间进行延迟播放
        /// </summary>
        /// <param name="sound"></param>
        /// <returns></returns>
        private static bool CanPlaySound(Sound sound)
        {
            switch (sound)
            {
                default:
                    return true;
                case Sound.Jump:
                    return SettingSoundTime(sound, 0.5f);
                case Sound.Attack:
                    return SettingSoundTime(sound, 1.25f);
                case Sound.Blocked:
                    return SettingSoundTime(sound, 1f);
                case Sound.Hit:
                    return SettingSoundTime(sound, 1f);
                case Sound.Roll:
                    return SettingSoundTime(sound, 1.25f);
                case Sound.Die:
                    return SettingSoundTime(sound, 5f);
            }
        }

        /// <summary>
        /// 设置音乐播放时间间隔
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="playerTimerMax"></param>
        /// <returns></returns>
        private static bool SettingSoundTime(Sound sound, float playerTimerMax)
        {
            if (soundTimerDictionary.ContainsKey(sound))
            {
                float lastTimePlayed = soundTimerDictionary[sound];
                if (lastTimePlayed + playerTimerMax < Time.time)     //当前时间大于最后一个播放时间与延迟播放时间的总量才能再次播放
                {
                    soundTimerDictionary[sound] = Time.time;            //更新最后一次播放时间
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 通过枚举类型获取数组中对应的资源
        /// </summary>
        /// <param name="sound"></param>
        /// <returns></returns>
        private static SoundAudioClip GetAudioClip(Sound sound)
        {
            foreach (SoundSave.SoundAudioClip soundAudioClip in SoundSave.instance.soundAudioClipArray)
            {
                if(soundAudioClip.sound == sound)
                {
                    return soundAudioClip;
                }
            }
            Debug.LogError("Sound "+sound+" not found!");
            return null;
        }
    }
}
