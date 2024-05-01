using UnityEngine;
using UnityEngine.Events;

// Correct the generic constraints syntax and property definition
public abstract class BaseGameEventListener<T, E, UER> : MonoBehaviour, IGameEventListener<T>
    where E : BaseGameEvent<T>
    where UER : UnityEvent<T>
{
    [SerializeField] private E gameEvent;

    public E GameEvent
    {
        get { return gameEvent; }
        set { gameEvent = value; }
    }

    [SerializeField] private UER unityEventResponse;

    private void OnEnable()
    {
        if (gameEvent == null) { return; }

        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        if (gameEvent == null) { return; }

        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(T item)
    {
        // Debug.Log($"Event raised with payload: {item}");
        unityEventResponse?.Invoke(item);
    }
}
