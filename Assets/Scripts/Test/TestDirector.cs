using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

///<summary>
///导演播放
///</summary>
public class TestDirector : MonoBehaviour
{
    public PlayableDirector pd;

    public Animator attacker;
    public Animator victim;

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    foreach (var track in pd.playableAsset.outputs)
        //    {
        //        if(track.streamName == "Attacker Animation")
        //        {
        //            pd.SetGenericBinding(track.sourceObject,attacker);
        //        }
        //        else if(track.streamName == "Victim Animation")
        //        {
        //            pd.SetGenericBinding(track.sourceObject,victim);
        //        }
                
        //    }

        //    //pd.time = 0;
        //    //pd.Stop();
        //    //pd.Evaluate();
        //    pd.Play();
        //}
    }

}
