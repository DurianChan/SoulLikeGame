using UnityEngine;

namespace DC.Events
{
    ///<summary>
    ///游戏事件监听接口
    ///</summary>
    public interface IGameEventListener<T>
    {
        void OnEventRaised(T item);


    }
}
