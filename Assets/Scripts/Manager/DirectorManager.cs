using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace DC
{
    ///<summary>
    ///导演管理
    ///</summary>
    [RequireComponent(typeof(PlayableDirector))]
    public class DirectorManager : IActorManagerInterface
    {
        public PlayableDirector pd;

        [Header("=== Timeline assets ===")]
        public TimelineAsset frontStab;
        public TimelineAsset openBox;
        public TimelineAsset leverUp;

        [Header("=== Assets Settings ===")]
        public ActorManager playerAm;
        public ActorManager targetAm;

        private void Start()
        {
            pd = GetComponent<PlayableDirector>();
            pd.playOnAwake = false;                 //将默认播放取消
        }

        public bool IsPlaying()
        {
            if (pd.state == PlayState.Playing)
            {
                return true;                 //如果导演正在播放直接返回
            }
            return false;
        }

        public void PlayActionTimeline(string timelineName, ActorManager playerAm, ActorManager targetAm)
        {
            if (timelineName == "frontStab")
            {
                //pd.playableAsset = Instantiate(frontStab);

                //TimelineAsset timeline = (TimelineAsset)pd.playableAsset;

                //foreach (var track in timeline.GetOutputTracks())
                //{
                //    if (track.name == "Attacker Script")
                //    {
                //        pd.SetGenericBinding(track, playerAm);
                //        foreach (var clip in track.GetClips())
                //        {
                //            MySuperPlayableClip myclip = (MySuperPlayableClip)clip.asset;
                //            MySuperPlayableBehaviour mybehav = myclip.template;
                //            mybehav.myFloat = 777;
                //            //myclip.am.exposedName = System.Guid.NewGuid().ToString();
                //            pd.SetReferenceValue(myclip.am.exposedName, playerAm);
                //        }
                //    }
                //    else if (track.name == "Victim Script")
                //    {
                //        pd.SetGenericBinding(track, targetAm);
                //        pd.SetGenericBinding(track, playerAm);
                //        foreach (var clip in track.GetClips())
                //        {
                //            MySuperPlayableClip myclip = (MySuperPlayableClip)clip.asset;
                //            MySuperPlayableBehaviour mybehav = myclip.template;
                //            mybehav.myFloat = 666;
                //            //myclip.am.exposedName = System.Guid.NewGuid().ToString();
                //            pd.SetReferenceValue(myclip.am.exposedName, targetAm);
                //        }
                //    }
                //    else if (track.name == "Attacker Animation")
                //    {
                //        pd.SetGenericBinding(track, playerAm.ac.anim);
                //    }
                //    else if (track.name == "Victim Animation")
                //    {
                //        pd.SetGenericBinding(track, targetAm.ac.anim);
                //    }
                //}
                //pd.Evaluate();
                //pd.Play();
                DoAtion(frontStab, playerAm,"Attacker Animation", "Attacker Script", targetAm, "Victim Animation", "Victim Script");
            }
            else if (timelineName == "openBox")
            {
                //pd.playableAsset = Instantiate(openBox);

                //TimelineAsset timeline = (TimelineAsset)pd.playableAsset;

                //foreach (var track in timeline.GetOutputTracks())
                //{
                //    if (track.name == "Player Script")
                //    {
                //        pd.SetGenericBinding(track, attacker);
                //        foreach (var clip in track.GetClips())
                //        {
                //            MySuperPlayableClip myclip = (MySuperPlayableClip)clip.asset;
                //            MySuperPlayableBehaviour mybehav = myclip.template;
                //            myclip.am.exposedName = System.Guid.NewGuid().ToString();
                //            pd.SetReferenceValue(myclip.am.exposedName, attacker);
                //        }
                //    }
                //    else if (track.name == "Box Script")
                //    {
                //        pd.SetGenericBinding(track, victim);
                //        pd.SetGenericBinding(track, attacker);
                //        foreach (var clip in track.GetClips())
                //        {
                //            MySuperPlayableClip myclip = (MySuperPlayableClip)clip.asset;
                //            MySuperPlayableBehaviour mybehav = myclip.template;
                //            myclip.am.exposedName = System.Guid.NewGuid().ToString();
                //            pd.SetReferenceValue(myclip.am.exposedName, victim);
                //        }
                //    }
                //    else if (track.name == "Player Animation")
                //    {
                //        pd.SetGenericBinding(track, attacker.ac.anim);
                //    }
                //    else if (track.name == "Box Animation")
                //    {
                //        pd.SetGenericBinding(track, victim.ac.anim);
                //    }
                //}
                //pd.Evaluate();
                //pd.Play();
                DoAtion(openBox, playerAm,"Player Animation", "Player Script",targetAm, "Box Animation", "Box Script");
            }
            else if (timelineName == "leverUp")
            {
                //pd.playableAsset = Instantiate(leverUp);

                //TimelineAsset timeline = (TimelineAsset)pd.playableAsset;

                //foreach (var track in timeline.GetOutputTracks())
                //{
                //    if (track.name == "Player Script")
                //    {
                //        pd.SetGenericBinding(track, playerAm);
                //        foreach (var clip in track.GetClips())
                //        {
                //            MySuperPlayableClip myclip = (MySuperPlayableClip)clip.asset;
                //            MySuperPlayableBehaviour mybehav = myclip.template;
                //            myclip.am.exposedName = System.Guid.NewGuid().ToString();
                //            pd.SetReferenceValue(myclip.am.exposedName, playerAm);
                //        }
                //    }
                //    else if (track.name == "Lever Script")
                //    {
                //        pd.SetGenericBinding(track, targetAm);
                //        pd.SetGenericBinding(track, playerAm);
                //        foreach (var clip in track.GetClips())
                //        {
                //            MySuperPlayableClip myclip = (MySuperPlayableClip)clip.asset;
                //            MySuperPlayableBehaviour mybehav = myclip.template;
                //            myclip.am.exposedName = System.Guid.NewGuid().ToString();
                //            pd.SetReferenceValue(myclip.am.exposedName, targetAm);
                //        }
                //    }
                //    else if (track.name == "Player Animation")
                //    {
                //        pd.SetGenericBinding(track, playerAm.ac.anim);
                //    }
                //    else if (track.name == "Lever Animation")
                //    {
                //        pd.SetGenericBinding(track, targetAm.ac.anim);
                //    }
                //}
                //pd.Evaluate();
                //pd.Play();
                DoAtion(leverUp, playerAm,"Player Animation", "Player Script", targetAm,"Lever Animation", "Lever Script");
            }
        }

        /// <summary>
        /// 设置TimeLine动画并播放
        /// </summary>
        /// <param name="timelineAsset"></param>
        /// <param name="playerAm"></param>
        /// <param name="playerAnimationName"></param>
        /// <param name="playerScriptName"></param>
        /// <param name="targetAm"></param>
        /// <param name="targetAnimationName"></param>
        /// <param name="targetScriptName"></param>
        public void DoAtion(TimelineAsset timelineAsset, ActorManager playerAm, string playerAnimationName,string playerScriptName, 
            ActorManager targetAm,string targetAnimationName, string targetScriptName)
        {
            pd.playableAsset = Instantiate(timelineAsset);

            TimelineAsset timeline = (TimelineAsset)pd.playableAsset;

            foreach (var track in timeline.GetOutputTracks())
            {
                if (track.name == playerScriptName)
                {
                    pd.SetGenericBinding(track, playerAm);
                    foreach (var clip in track.GetClips())
                    {
                        MySuperPlayableClip myclip = (MySuperPlayableClip)clip.asset;
                        MySuperPlayableBehaviour mybehav = myclip.template;
                        //mybehav.myFloat = 777;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName, playerAm);
                    }
                }
                else if (track.name == targetScriptName)
                {
                    pd.SetGenericBinding(track, targetAm);
                    pd.SetGenericBinding(track, playerAm);
                    foreach (var clip in track.GetClips())
                    {
                        MySuperPlayableClip myclip = (MySuperPlayableClip)clip.asset;
                        MySuperPlayableBehaviour mybehav = myclip.template;
                        //mybehav.myFloat = 666;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName, targetAm);
                    }
                }
                else if (track.name == playerAnimationName)
                {
                    pd.SetGenericBinding(track, playerAm.ac.anim);
                }
                else if (track.name == targetAnimationName)
                {
                    pd.SetGenericBinding(track, targetAm.ac.anim);
                }
            }
            pd.Evaluate();
            pd.Play();
        }

    }
}
