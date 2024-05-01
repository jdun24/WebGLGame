using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New TechUp Event", menuName = "Events/Custom/Build/TechUp Event")]

public class TechUpEvent : BaseGameEvent<packet.TechUpPacket>
{

}
