using System.Collections.Generic;
using UnityEngine;
using BuildingComponents;
using packet;
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Events")]
    [SerializeField] public BuildObjEvent buildObjEvent;
    [SerializeField] public TechUpEvent techUpEvent;
    [Header("Mutable")]

    [Header("ReadOnly")]

    // Not for display
    Inventory currentInventory;
    [SerializeField] GameObject UIInvManagerObject;
    UIInventoryManager inventoryUI;

    private bool isTest = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //init resource dictionary and add their starting resources
        if(isTest == true){
            currentInventory = new Inventory(999, 999, 999, 999, 999, 999, 999, 999, 25);
        }else{
            currentInventory = new Inventory(45, 45, 25, 0, 0, 0, 0, 0, 0);
        }
        //currentInventory = new Inventory(45, 45, 25, 0, 0, 0, 0, 0, 0);
        //^ above is initial thought on starting resources, below is for testing the building
        //currentInventory = new Inventory(999, 999, 999, 999, 999, 999, 999, 999, 25);
        inventoryUI = UIInvManagerObject.GetComponent<UIInventoryManager>();
        inventoryUI.UpdateInventoryFromDictionary(currentInventory.GetInvDictionary());
    }
    // -------------------------------------------------------------------
    // Handle events

    public void OnCheckInventoryEvent(packet.CheckInventoryPacket packet){
        BuildingType building = packet.building;
        if(CheckAvailResources(packet.objCost) == true){
            if(packet.objCost.hasTechCost() == false){
                if(packet.building == BuildingType.CommercialExtractor || packet.building == BuildingType.IndustrialExtractor){
                    if(currentInventory.GetBuildingTechLevel(packet.building) > 0){
                        buildObjEvent.Raise(building);
                        return;
                    }
                    else return;
                }
                PayForObject(packet.objCost);
                buildObjEvent.Raise(building);
            }else{
                CheckInvTechCase(packet);
            }
        }
    }

    public void OnToInventoryEvent(packet.ResourceToInventory packet){
        currentInventory.AddResource(packet.resourceToChange, packet.amountToChange);
        inventoryUI.UpdateInventoryFromDictionary(currentInventory.GetInvDictionary());
    }

    public void OnDiscoverGeoPhenom(){
        currentInventory.GainTechPoint();
        inventoryUI.UpdateInventoryFromDictionary(GetCurrentInventory());
    }
    public void OnResourceCollect(ResourceType resource, int amount){
        currentInventory.AddResource(resource, amount);
        inventoryUI.UpdateInventoryFromDictionary(GetCurrentInventory());
    }
    // -------------------------------------------------------------------
    // Class Specific
    private void CheckInvTechCase(packet.CheckInventoryPacket packet){

        int newLevel = currentInventory.GetBuildingTechLevel(packet.building) + 1;
        //Only two buildings with 4 levels, using 0 indexing
        if(packet.building == BuildingType.LaunchPad || packet.building == BuildingType.RobotBuddy){
            if(newLevel <= 4){
                PayForObject(packet.objCost);
                currentInventory.TechUpBuilding(packet.building, newLevel);
                techUpEvent.Raise(new TechUpPacket(packet.building, newLevel));
            }else{
                return;
            }
        //Every other building has 3 leves, using 0 indexing
        }else if(newLevel <= 3){
            PayForObject(packet.objCost);
            currentInventory.TechUpBuilding(packet.building, newLevel);
            techUpEvent.Raise(new TechUpPacket(packet.building, newLevel));
            return;
        }
    }
    public Dictionary<ResourceType, int> GetCurrentInventory(){
        return currentInventory.GetInvDictionary();
    }
    public void PayForObject(ObjectsCost cost){
        currentInventory.PayForObjectWithObjCost(cost);
        inventoryUI.UpdateInventoryFromDictionary(GetCurrentInventory());
    }
    private bool CheckAvailResources(ObjectsCost costDictionary){
        return currentInventory.CheckCost(costDictionary);
    }
    // -------------------------------------------------------------------
    // API
    public int GetTechTier(BuildingComponents.BuildingType buildingType){
        return currentInventory.GetBuildingTechLevel(buildingType);
    }
}
