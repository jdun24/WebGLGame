using System.Collections.Generic;
using UnityEngine;

public class AsteroidResourceManager: MonoBehaviour{

    private Dictionary<GameObject, Resource> resourceMasterDict = new Dictionary<GameObject, Resource>();


    //Events
    public void OnResourceSpawnEvent(packet.ResourceGameObjectPacket packet){
        AddResourceDictEntry(packet.GameObjectResource, packet.resource);
        //Debug.LogWarning("Adding " + packet.resource.Name + " amt:" + packet.resource.GetDepositAmount());
    }

    public void OnMiningEvent(packet.ResourceToInventory packet){
        GameObject obj = packet.obj;
        Resource resourceToChange = resourceMasterDict[obj];
        resourceToChange.ReduceDepositAmount(packet.amountToChange);
        //Debug.Log("Deposit amonut: " + resourceToChange.GetDepositAmount());
        if(resourceToChange.GetDepositAmount() < 0){
            Destroy(obj);
            RemoveResourceDictEntry(obj);
            return;
        }

        obj.transform.localScale = resourceToChange.ReduceSize(resourceToChange.GetDepositAmount(), packet.amountToChange, obj);
    }
    //class specific functions
    private void AddResourceDictEntry(GameObject gameObject, Resource resource){
        resourceMasterDict.Add(gameObject, resource);
    }
    private void RemoveResourceDictEntry(GameObject gameObject){
        resourceMasterDict.Remove(gameObject);
    }
}