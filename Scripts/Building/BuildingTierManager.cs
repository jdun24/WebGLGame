using System.Collections.Generic;
using BuildingComponents;
using UnityEngine;

public class BuildingTierManager : MonoBehaviour
{
    public static BuildingTierManager Instance { get; private set; }
    [SerializeField] VoidEvent satelliteUnlockSteeringEvent;
    //Not For Display
    Dictionary<BuildingType, int> buildingTiers = new Dictionary<BuildingType, int>();
    void Awake()
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

        InitBuildingTierManager();
    }
    public void InitBuildingTierManager(){
        buildingTiers.Add(BuildingType.Extractor, 1);
        buildingTiers.Add(BuildingType.CommercialExtractor, 0);
        buildingTiers.Add(BuildingType.IndustrialExtractor, 0);
        buildingTiers.Add(BuildingType.Exosuit, 0);
        buildingTiers.Add(BuildingType.JetPack, 0);
        buildingTiers.Add(BuildingType.Cybernetics, 0);
        buildingTiers.Add(BuildingType.RobotBuddy, 0);
        buildingTiers.Add(BuildingType.Satellite, 0);
        buildingTiers.Add(BuildingType.LaunchPad, 0);
    }

    public void OnTechUpEvent(packet.TechUpPacket packet){
        buildingTiers[packet.building] = packet.TechToLevel;

        if(packet.building == BuildingComponents.BuildingType.Satellite && packet.TechToLevel == 1){
            satelliteUnlockSteeringEvent.Raise();
        }
    }


    //API
    public int GetTierOf(BuildingType building){
        return buildingTiers[building];
    }
}