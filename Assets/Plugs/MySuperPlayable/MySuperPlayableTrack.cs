using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using DC;

[TrackColor(0.675679f, 0.8962264f, 0.4734781f)]
[TrackClipType(typeof(MySuperPlayableClip))]
[TrackBindingType(typeof(ActorManager))]
public class MySuperPlayableTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<MySuperPlayableMixerBehaviour>.Create (graph, inputCount);
    }
}
