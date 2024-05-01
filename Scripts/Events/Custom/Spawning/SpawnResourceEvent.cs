using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New SpawnResource Event", menuName = "Events/Custom/Spawning/SpawnResource Event")]

public class SpawnResourceEvent : BaseGameEvent<packet.ResourceGameObjectPacket>
{

}
