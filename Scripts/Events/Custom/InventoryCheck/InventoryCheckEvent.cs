using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New CheckInventory Event", menuName = "Events/Custom/Inventory/CheckInventory Event")]

public class InventoryCheckEvent : BaseGameEvent<packet.CheckInventoryPacket>
{

}
