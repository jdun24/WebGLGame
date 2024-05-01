using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Void Event", menuName = "Events/Primitive/Void Event")]

public class VoidEvent : BaseGameEvent<Void>
{
    public void Raise() => Raise(new Void());
}
