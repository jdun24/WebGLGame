using UnityEngine;
using BuildingComponents;
using Unity.VisualScripting;
public class UIBuildManager : MonoBehaviour
{
    [SerializeField] GameObject buildChildOverlay;
    [SerializeField] private bool isOverlayActive;
    [SerializeField] public InventoryCheckEvent checkInventory;
    [SerializeField] private GameObject IndustryButtonOverlay;
    [SerializeField] private GameObject SuitButtonOverlay;
    [SerializeField] private GameObject RoboticsButtonOverlay;
    [SerializeField] private TechUpEvent techEvent;
    private GameObject currentOverlay;
    [SerializeField] private Sprite filledStar;
    private ObjectsCost techCost = new ObjectsCost(0,0,0,0,0,0,0,0,1);
    //<------------------------------------ <Industry Vars> ------------------------------------>
    //<---- <Extractor Text Fields> ---->
    [SerializeField] private GameObject Extractor_Tier1Star;
    [SerializeField] private GameObject Extractor_Tier2Star;
    [SerializeField] private GameObject Extractor_Tier3Star;
    //<---- <Commercial Extractor Text Fields> ---->
    [SerializeField] private GameObject CommercialExtractor_Tier1Star;
    [SerializeField] private GameObject CommercialExtractor_Tier2Star;
    [SerializeField] private GameObject CommercialExtractor_Tier3Star;
    //<---- <Industrial Extractor Text Fields> ---->
    [SerializeField] private GameObject IndustrialExtractor_Tier1Star;
    [SerializeField] private GameObject IndustrialExtractor_Tier2Star;
    [SerializeField] private GameObject IndustrialExtractor_Tier3Star;

    //<------------------------------------ <Suit Vars> --------------------------------------->

    //<---- <Exosuit Text Fields> ---->
    [SerializeField] private GameObject Exosuit_Tier1Star;
    [SerializeField] private GameObject Exosuit_Tier2Star;
    [SerializeField] private GameObject Exosuit_Tier3Star;
    //<---- <Jetpack Text Fields> ---->
    [SerializeField] private GameObject JetPack_Tier1Star;
    [SerializeField] private GameObject JetPack_Tier2Star;
    [SerializeField] private GameObject JetPack_Tier3Star;
    //<---- <Cybernetics Text Fields> ---->
    [SerializeField] private GameObject Cybernetics_Tier1Star;
    [SerializeField] private GameObject Cybernetics_Tier2Star;
    [SerializeField] private GameObject Cybernetics_Tier3Star;

    //<------------------------------------ <Robotic Vars> ------------------------------------>
    //<---- <RoboBuddy Text Fields> ---->
    [SerializeField] private GameObject RobotBuddy_Tier1Star;
    [SerializeField] private GameObject RobotBuddy_Tier2Star;
    [SerializeField] private GameObject RobotBuddy_Tier3Star;
    [SerializeField] private GameObject RobotBuddy_Tier4Star;
    //<---- <Satellite Text Fields> ---->
    [SerializeField] private GameObject Satellite_Tier1Star;
    [SerializeField] private GameObject Satellite_Tier2Star;
    [SerializeField] private GameObject Satellite_Tier3Star;
    //<---- <LaunchPad Text Fields> ---->
    [SerializeField] private GameObject LaunchPad_Tier1Star;
    [SerializeField] private GameObject LaunchPad_Tier2Star;
    [SerializeField] private GameObject LaunchPad_Tier3Star;
    [SerializeField] private GameObject LaunchPad_Tier4Star;

    //<------------------- <========<>========> ------------------->
    private void Start()
    {
        isOverlayActive = false;
        buildChildOverlay.SetActive(false);
    }


    // -------------------------------------------------------------------
    // Handle events
    public bool IsActive(){
        return isOverlayActive;
    }
    public void OnPlayerBuildOverlay()
    {
        if(CaveManager.Instance.GetIsPlayerInCave() == false){
            isOverlayActive = !isOverlayActive;
            buildChildOverlay.SetActive(isOverlayActive);

            currentOverlay = IndustryButtonOverlay;
            IndustryButtonOverlay.SetActive(true);
            SuitButtonOverlay.SetActive(false);
            RoboticsButtonOverlay.SetActive(false);
        }
    }
    //Makes it so the player isn't able to go into caves with the build menu open
    public void OnPlayerInteract(){
        isOverlayActive = false;
        buildChildOverlay.SetActive(isOverlayActive);
    }
    public void OnUpdateControlState(){
        isOverlayActive = false;
        buildChildOverlay.SetActive(isOverlayActive);
    }

    //  <Suit> | <Industry> | <Robotics>
    //Always Starts Industry, cylce left continuously
    public void OnPlayerBuildOverlayCycleLeft(){
        if (Input.GetKeyDown(KeyCode.V) && isOverlayActive)
        {
            // Hide the current overlay
            currentOverlay.SetActive(false);

            // Switch to the next overlay
            if (currentOverlay == IndustryButtonOverlay){
                currentOverlay = SuitButtonOverlay;
            }
            else if (currentOverlay == SuitButtonOverlay){
                currentOverlay = RoboticsButtonOverlay;
            }
            else if (currentOverlay == RoboticsButtonOverlay){
                currentOverlay = IndustryButtonOverlay;
            }

            // Show the new overlay
            currentOverlay.SetActive(true);
        }
    }
    //  <Suit> | <Industry> | <Robotics>
    public void OnPlayerBuildOverlayCycleRight(){
        if (Input.GetKeyDown(KeyCode.N) && isOverlayActive)
        {
            // Hide the current overlay
            currentOverlay.SetActive(false);

            // Switch to the next overlay
            if (currentOverlay == IndustryButtonOverlay){
                currentOverlay = RoboticsButtonOverlay;
            }
            else if (currentOverlay == SuitButtonOverlay){
                currentOverlay = IndustryButtonOverlay;
            }
            else if (currentOverlay == RoboticsButtonOverlay){
                currentOverlay = SuitButtonOverlay;
            }

            // Show the new overlay
            currentOverlay.SetActive(true);
        }
    }

    // -------------------------------------------------------------------
    public void OnTechUpEvent(packet.TechUpPacket packet){
        //tierManager.UpdateBuildingTier(packet.building, packet.TechToLevel);
        //changes the stars
        switch(packet.building){
            case(BuildingComponents.BuildingType.Extractor):
                if(packet.TechToLevel == 2){
                    Extractor_Tier2Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 3){
                    Extractor_Tier3Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }
                break;
            case(BuildingComponents.BuildingType.CommercialExtractor):
                if(packet.TechToLevel == 1){
                    CommercialExtractor_Tier1Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 2){
                    CommercialExtractor_Tier2Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 3){
                    CommercialExtractor_Tier3Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }
                break;
            case(BuildingComponents.BuildingType.IndustrialExtractor):
                if(packet.TechToLevel == 1){
                    IndustrialExtractor_Tier1Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 2){
                    IndustrialExtractor_Tier2Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 3){
                    IndustrialExtractor_Tier3Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }
                break;
            case(BuildingComponents.BuildingType.Exosuit):
                if(packet.TechToLevel == 1){
                    Exosuit_Tier1Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 2){
                    Exosuit_Tier2Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 3){
                    Exosuit_Tier3Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }
                break;
            case(BuildingComponents.BuildingType.JetPack):
                if(packet.TechToLevel == 1){
                    JetPack_Tier1Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 2){
                    JetPack_Tier2Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 3){
                    JetPack_Tier3Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }
                break;
            case(BuildingComponents.BuildingType.Cybernetics):
                if(packet.TechToLevel == 1){
                    Cybernetics_Tier1Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 2){
                    Cybernetics_Tier2Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 3){
                    Cybernetics_Tier3Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }
                break;
            case(BuildingComponents.BuildingType.RobotBuddy):
                if(packet.TechToLevel == 1){
                    RobotBuddy_Tier1Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 2){
                    RobotBuddy_Tier2Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 3){
                    RobotBuddy_Tier3Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 4){
                    RobotBuddy_Tier4Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }
                break;
            case(BuildingComponents.BuildingType.Satellite):
                if(packet.TechToLevel == 1){
                    Satellite_Tier1Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 2){
                    Satellite_Tier2Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 3){
                    Satellite_Tier3Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }
                break;
            case(BuildingComponents.BuildingType.LaunchPad):
                if(packet.TechToLevel == 1){
                    LaunchPad_Tier1Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 2){
                    LaunchPad_Tier2Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 3){
                    LaunchPad_Tier3Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }else if(packet.TechToLevel == 4){
                    LaunchPad_Tier4Star.GetComponent<SpriteRenderer>().sprite = filledStar;
                }
                break;
        }
    }

    //----------< UIBuild/Sound Functions >-----------//
    private void PlayClick(){
        SoundFXManager.Instance.PlaySound(SFX.UI.Click, this.transform, 1f);
    }
    //<------------------------------------ <Industry Functions> ------------------------------------>
    public void TryBuildExtractor(){
        PlayClick();
        Extractor newExtractor = new();
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, newExtractor.GetBuildingType(), newExtractor.GetCostDictionary()));
    }
    public void TryTechUpExtractor(){
        PlayClick();
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingType.Extractor, techCost));
    }
    public void TryBuildCommercialExtractor(){
        PlayClick();
        CommercialExtractor newExtractor = new();
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, newExtractor.GetBuildingType(), newExtractor.GetCostDictionary()));
    }
    public void TryTechUpCommercialExtractor(){
        PlayClick();
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingType.CommercialExtractor, techCost));
    }
    public void TryBuildIndustrialExtractor(){
        PlayClick();
        IndustrialExtractor newExtractor = new();
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, newExtractor.GetBuildingType(), newExtractor.GetCostDictionary()));
    }
    public void TryTechUpIndustrialExtractor(){
        PlayClick();
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingType.IndustrialExtractor, techCost));
    }
    //<------------------------------------ <Suit Functions> ------------------------------------>
    public void TryBuildExosuit(){
        PlayClick();
        Exosuit tempExosuit = new();
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, tempExosuit.GetBuildingType(), tempExosuit.GetCostDictionary()));
    }
    public void TryTechUpExosuit(){
        PlayClick();
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingComponents.BuildingType.Exosuit, techCost));
    }
    public void TryBuildJetPack(){
        PlayClick();
        JetPack tempJetPack = new();
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, tempJetPack.GetBuildingType(), tempJetPack.GetCostDictionary()));
    }
    public void TryTechUpJetPack(){
        PlayClick();
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingComponents.BuildingType.JetPack, techCost));
    }
    public void TryBuildCybernetics(){
        PlayClick();
        if(CyberneticsManager.Instance.IsBuilt() == false){
            Cybernetics tempCyber = new();
            checkInventory.Raise(new packet.CheckInventoryPacket(
                this.gameObject, BuildingComponents.BuildingType.Cybernetics, tempCyber.GetCostDictionary()));
        }
    }
    public void TryTechUpCybernetics(){
        PlayClick();
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingComponents.BuildingType.Cybernetics, techCost));
    }
    //<------------------------------------ <Robotics Functions> ------------------------------------>
    public void TryBuildRobotBuddy(){
        PlayClick();
        RobotBuddy tempRoboBuddy = new();
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingType.RobotBuddy, tempRoboBuddy.GetCostDictionary()));
    }
    public void TryTechUpRobotBuddy(){
        PlayClick();
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingComponents.BuildingType.RobotBuddy, techCost));
    }
    public void TryBuildSatellite(){
        PlayClick();
        ObjectsCost costToSend;
        if(BuildingTierManager.Instance.GetTierOf(BuildingType.Satellite) < 2){
            Satellite tempSatellite = new();
            costToSend = tempSatellite.GetCostDictionary();
            checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingType.Satellite, costToSend));
        }else if (BuildingTierManager.Instance.GetTierOf(BuildingType.Satellite) == 2){
            costToSend = new ObjectsCost(150, 150, 100, 50, 50, 4, 1, 2, 0);
            checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingType.Satellite, costToSend));
        }else if(BuildingTierManager.Instance.GetTierOf(BuildingType.Satellite) >= 3){
            costToSend = new ObjectsCost(150, 150, 100, 50, 30, 4, 1, 0, 0);
            checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingType.Satellite, costToSend));
        }
        
    }
    public void TryTechUpSatellite(){
        PlayClick();
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingComponents.BuildingType.Satellite, techCost));
    }
    public void TryBuildLaunchPad(){
        PlayClick();
        LaunchPad tempLaunchPad = new();
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingType.LaunchPad, tempLaunchPad.GetCostDictionary()
        ));
    }
    public void TryTechUpLaunchPad(){
        PlayClick();
        checkInventory.Raise(new packet.CheckInventoryPacket(
            this.gameObject, BuildingComponents.BuildingType.LaunchPad, techCost));
    }

}
