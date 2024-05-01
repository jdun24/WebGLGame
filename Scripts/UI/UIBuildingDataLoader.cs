using System.IO;
using Newtonsoft.Json;
using BuildingComponents;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;
public class UIBuildingDataLoader : MonoBehaviour
{
    //This class is responsible for loading in the building data into the UI,
    //as well as updating the text to red or green based on whether or not the object is buyable
    private string filePath = "Assets/Resources/BuildingData.json"; // Adjust the path as per your project structure

    //<------------------------------------ <Industry Vars> ------------------------------------>
    //<---- <Extractor Text Fields> ---->
    [SerializeField] private TextMeshProUGUI ExtractorCostText;
    [SerializeField] private TextMeshProUGUI ExtractorHelpText;
    [SerializeField] private TextMeshProUGUI ExtractorTechUpText;
    //<---- <Commercial Extractor Text Fields> ---->
    [SerializeField] private TextMeshProUGUI CommercialExtractorCostText;
    [SerializeField] private TextMeshProUGUI CommercialExtractorHelpText;
    [SerializeField] private TextMeshProUGUI CommercialExtractorTechUpText;
    //<---- <Industrial Extractor Text Fields> ---->
    [SerializeField] private TextMeshProUGUI IndustrialExtractorCostText;
    [SerializeField] private TextMeshProUGUI IndustrialExtractorHelpText;
    [SerializeField] private TextMeshProUGUI IndustrialExtractorTechUpText;

    //<------------------------------------ <Suit Vars> --------------------------------------->

    //<---- <Exosuit Text Fields> ---->
    [SerializeField] private TextMeshProUGUI ExosuitCostText;
    [SerializeField] private TextMeshProUGUI ExosuitHelpText;
    [SerializeField] private TextMeshProUGUI ExosuitTechUpText;
    //<---- <Jetpack Text Fields> ---->
    [SerializeField] private TextMeshProUGUI JetpackCostText;
    [SerializeField] private TextMeshProUGUI JetpackHelpText;
    [SerializeField] private TextMeshProUGUI JetpackTechUpText;
    //<---- <Cybernetics Text Fields> ---->
    [SerializeField] private TextMeshProUGUI CyberneticsCostText;
    [SerializeField] private TextMeshProUGUI CyberneticsHelpText;
    [SerializeField] private TextMeshProUGUI CyberneticsTechUpText;

    //<------------------------------------ <Robotic Vars> ------------------------------------>
    //<---- <RoboBuddy Text Fields> ---->
    [SerializeField] private TextMeshProUGUI RoboBuddyCostText;
    [SerializeField] private TextMeshProUGUI RoboBuddyHelpText;
    [SerializeField] private TextMeshProUGUI RoboBuddyTechUpText;
    //<---- <Satellite Text Fields> ---->
    [SerializeField] private TextMeshProUGUI SatelliteCostText;
    [SerializeField] private TextMeshProUGUI SatelliteHelpText;
    [SerializeField] private TextMeshProUGUI SatelliteTechUpText;
    //<---- <LaunchPad Text Fields> ---->
    [SerializeField] private TextMeshProUGUI LaunchPadCostText;
    [SerializeField] private TextMeshProUGUI LaunchPadHelpText;
    [SerializeField] private TextMeshProUGUI LaunchPadTechUpText;
    //Json Vars
    //-----------------------------------------------------------------
    private BuildingData buildingData;
    private BaseBuilding buildingBase;
    private List<LoadableBuildingObject> allData = new List<LoadableBuildingObject>();
    private void Start()
    {
        buildingBase = new();
        allData.Add(buildingBase.FindBuildingObjectByID("Extractor"));
        allData.Add(buildingBase.FindBuildingObjectByID("CommercialExtractor"));
        allData.Add(buildingBase.FindBuildingObjectByID("IndustrialExtractor"));
        allData.Add(buildingBase.FindBuildingObjectByID("Cybernetics"));
        allData.Add(buildingBase.FindBuildingObjectByID("Exosuit"));
        allData.Add(buildingBase.FindBuildingObjectByID("Satellite"));
        allData.Add(buildingBase.FindBuildingObjectByID("RobotBuddy"));
        allData.Add(buildingBase.FindBuildingObjectByID("LaunchPad"));
        LoadBuildingUI(allData);
    }
    private void LoadBuildingUI(List<LoadableBuildingObject> data){
        foreach (LoadableBuildingObject buildingObject in data)
        {
            SetUIBuildingItem(buildingObject);
        }
    }
    private void SetUIBuildingItem(LoadableBuildingObject item){
        string itemID = item.ID;
        
        switch(itemID){
            case "Extractor":
                LoadIntoUI(item, ExtractorCostText, ExtractorHelpText, ExtractorTechUpText);
            break;
            case "CommercialExtractor":
                LoadIntoUI(item, CommercialExtractorCostText, CommercialExtractorHelpText, CommercialExtractorTechUpText);
            break;
            case "IndustrialExtractor":
                LoadIntoUI(item, IndustrialExtractorCostText, IndustrialExtractorHelpText, IndustrialExtractorTechUpText);
            break;
            case "Exosuit":
                LoadIntoUI(item, ExosuitCostText, ExosuitHelpText, ExosuitTechUpText);
            break;
            case "JetPack":
                LoadIntoUI(item, JetpackCostText, JetpackHelpText, JetpackTechUpText);
            break;
            case "Cybernetics":
                LoadIntoUI(item, CyberneticsCostText, CyberneticsHelpText, CyberneticsTechUpText);
            break;
            case "RobotBuddy":
                LoadIntoUI(item, RoboBuddyCostText, RoboBuddyHelpText, RoboBuddyTechUpText);
            break;
            case "Satellite":
                LoadIntoUI(item, SatelliteCostText, SatelliteHelpText, SatelliteTechUpText);
            break;
            case "LaunchPad":
                LoadIntoUI(item, LaunchPadCostText, LaunchPadHelpText, LaunchPadTechUpText);
            break;
        }
    }
    private void LoadIntoUI(LoadableBuildingObject item, TextMeshProUGUI costText, TextMeshProUGUI helpText, TextMeshProUGUI techUpText){
        costText.text = "";
        int i = 0;
        foreach(var costNode in item.Costs){
            if (Convert.ToInt32(costNode.Value) > 0)
                if(i % 2 == 0){
                    costText.text += $"{costNode.Key}: {costNode.Value}\t";
                }else{
                    costText.text += $"{costNode.Key}: {costNode.Value}\n";
                }
                
                i++;
        }
        helpText.text = item.ItemDescription;
        techUpText.text = item.TechUpTexts[0];
    }

    public void OnTechUpEvent(packet.TechUpPacket packet){
        LoadableBuildingObject item = GetItemByBuilding(packet.building);
        switch(packet.building){
            case(BuildingComponents.BuildingType.Extractor):
                ExtractorTechUpText.text = item.TechUpTexts[packet.TechToLevel - 1];
            break;
            case(BuildingComponents.BuildingType.CommercialExtractor):
                CommercialExtractorTechUpText.text = item.TechUpTexts[packet.TechToLevel];
            break;
            case(BuildingComponents.BuildingType.IndustrialExtractor):
                IndustrialExtractorTechUpText.text = item.TechUpTexts[packet.TechToLevel];
            break;
            case(BuildingComponents.BuildingType.Exosuit):
                ExosuitTechUpText.text = item.TechUpTexts[packet.TechToLevel];
            break;
            case(BuildingComponents.BuildingType.JetPack):
                JetpackTechUpText.text = item.TechUpTexts[packet.TechToLevel];
            break;
            case(BuildingComponents.BuildingType.Cybernetics):
                CyberneticsTechUpText.text = item.TechUpTexts[packet.TechToLevel];
            break;
            case(BuildingComponents.BuildingType.RobotBuddy):
                RoboBuddyTechUpText.text = item.TechUpTexts[packet.TechToLevel];
            break;
            case(BuildingComponents.BuildingType.Satellite):
                SatelliteTechUpText.text = item.TechUpTexts[packet.TechToLevel];
                if(packet.TechToLevel == 2){
                    ObjectsCost newCost = new ObjectsCost(150, 150, 100, 50, 50, 4, 1, 2, 0);
                    UpdateCostUI(newCost, SatelliteCostText);
                }else if (packet.TechToLevel == 3){
                    ObjectsCost newCost = new ObjectsCost(150, 150, 100, 50, 30, 4, 1, 0, 0);
                    UpdateCostUI(newCost, SatelliteCostText);
                }

            break;
            case(BuildingComponents.BuildingType.LaunchPad):
                LaunchPadTechUpText.text = item.TechUpTexts[packet.TechToLevel];
            break;
        }
    }
    private void UpdateCostUI(ObjectsCost item, TextMeshProUGUI costText){
        costText.text = "";
        int i = 0;
        foreach(var costNode in item.getCostDictionary()){
            if (Convert.ToInt32(costNode.Value) > 0)
                if(i % 2 == 0){
                    costText.text += $"{costNode.Key}: {costNode.Value}\t";
                }else{
                    costText.text += $"{costNode.Key}: {costNode.Value}\n";
                }
                
                i++;
        }
    }

    private LoadableBuildingObject GetItemByBuilding(BuildingComponents.BuildingType building){
        foreach (var buildingObject in allData)
        {
            if(buildingObject.ID == BuildingTypeToString(building)){
                return buildingObject;
            }
        }
        Debug.LogError("UIBuildingDataLoader could not find json entry for building " + BuildingTypeToString(building));
        return null;
    }
    private String BuildingTypeToString(BuildingComponents.BuildingType building){
        switch(building){
            case(BuildingComponents.BuildingType.Extractor):
                return "Extractor";
            case(BuildingComponents.BuildingType.CommercialExtractor):
                return "CommercialExtractor";
            case(BuildingComponents.BuildingType.IndustrialExtractor):
                return "IndustrialExtractor";
            case(BuildingComponents.BuildingType.Exosuit):
                return "Exosuit";
            case(BuildingComponents.BuildingType.JetPack):
                return "JetPack";
            case(BuildingComponents.BuildingType.Cybernetics):
                return "Cybernetics";
            case(BuildingComponents.BuildingType.RobotBuddy):
                return "RobotBuddy";
            case(BuildingComponents.BuildingType.Satellite):
                return "Satellite";
            case(BuildingComponents.BuildingType.LaunchPad):
                return "LaunchPad";
            default:
                Debug.LogError("BuildingTypeToString failed to determine correct building type while teching up!!  Returning \"Extractor\"");
                return "Extractor";
        }
    }
}
