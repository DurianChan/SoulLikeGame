using UnityEngine;
using UnityEngine.Events;

namespace DC.Events
{
    ///<summary>
    ///游戏事件侦听器
    ///</summary>
    public abstract class BaseGameEventListener<T,E,UER> : MonoBehaviour,
        IGameEventListener<T> where E : BaseGameEvent<T> where UER : UnityEvent<T>
    {
        [SerializeField] private E gameEvent;
        public E GameEvent { get { return gameEvent; }  set { gameEvent = value; } }

        public UER UnityEventResponse { get => unityEventResponse; set => unityEventResponse = value; }

        [SerializeField] private UER unityEventResponse;

        private void OnEnable()
        {
            if (gameEvent == null) { return; }

            GameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            if (gameEvent == null) { return; }

            GameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(T item)
        {
            if (UnityEventResponse!=null)
            {
                UnityEventResponse.Invoke(item);
            }
        }
    }
}