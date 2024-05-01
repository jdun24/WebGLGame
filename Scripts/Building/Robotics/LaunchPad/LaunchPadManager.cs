using UnityEngine;
using BuildingComponents;
using UnityEngine.SceneManagement;
using System.Collections;

[DefaultExecutionOrder(100)]
public class LaunchPadManager : MonoBehaviour{

    private bool isPlaced = false;
    private LaunchPad launchpad;
    private int TechTier = 0;
    [SerializeField] BoolEvent triggerRocketModuleUIOverlay;
    [SerializeField] VoidEvent winningEvent;
    //building based vars
    [SerializeField] public InventoryCheckEvent checkInventory;
    private ObjectsCost engineCosts = new ObjectsCost(100,100, 50,0,20,10,0,5,0);
    private ObjectsCost chasisCosts = new ObjectsCost(100,100,75,20,0,10,10,10,0);
    private ObjectsCost cockpitCosts = new ObjectsCost(100,100,75,50,50,25,25,25,0);
    private ObjectsCost externalTankCosts = new ObjectsCost(100,100,75,50,50,50,50,40,0);
    [SerializeField] private GameObject enginesPrefab;
    [SerializeField] private GameObject chasisPrefab;
    [SerializeField] private GameObject cockpitPrefab;
    [SerializeField] private GameObject externalTankPrefab;
    
    public void Awake(){
        launchpad = new();
        QueryTechLevel();
    }
    public void OnUIButtonPress(){
        TryBuildNextRocketPart();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & LayerMask.GetMask("Player")) != 0 && isPlaced && !launchpad.isExternalTankBuilt())
        {
            triggerRocketModuleUIOverlay.Raise(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        //Turn off current price
        if (((1 << collision.gameObject.layer) & LayerMask.GetMask("Player")) != 0 && isPlaced)
        {
            triggerRocketModuleUIOverlay.Raise(false);
        }
    }
    public void SetPlaced(){
        isPlaced = true;
    }

    public void OnTechUpEvent(packet.TechUpPacket packet){
        if(packet.building == BuildingType.LaunchPad){
            TechTier = packet.TechToLevel;
        }
    }

    public void TryBuildNextRocketPart(){
        if(launchpad.isEngineBuilt()){
            if(launchpad.isChasisBuilt()){
                if(launchpad.isCockpitBuilt()){
                    if(!launchpad.isExternalTankBuilt()){
                        TryBuildExternalTank();
                    }
                }else{
                    TryBuildCockpit();
                }
            }else{
                TryBuildChasis();
            }
        }else{
            TryBuildEngine();
        }
    }

    public void OnBuildObjEvent(BuildingComponents.BuildingType type){
        switch(type){
            case(BuildingComponents.BuildingType.Engines):
                SpawnEngines();
            break;
            case(BuildingComponents.BuildingType.Chasis):
                SpawnChasis();
            break;
            case(BuildingComponents.BuildingType.Cockpit):
                SpawnCockpit();
            break;
            case(BuildingComponents.BuildingType.ExternalTank):
                SpawnExternalTank();
            break;
        }
    }

    private void SpawnEngines(){
        enginesPrefab.SetActive(true);
        launchpad.SetEngineBuilt();
        SoundFXManager.Instance.PlaySound(SFX.Player.Work, this.gameObject.transform, 1f);
    }
    private void SpawnChasis(){
        chasisPrefab.SetActive(true);
        launchpad.SetChasisBuilt();
        SoundFXManager.Instance.PlaySound(SFX.Player.Work, this.gameObject.transform, 1f);
    }
    private void SpawnCockpit(){
        cockpitPrefab.SetActive(true);
        launchpad.SetCockpitBuilt();
        SoundFXManager.Instance.PlaySound(SFX.Player.Work, this.gameObject.transform, 1f);
    }
    private void SpawnExternalTank(){
        externalTankPrefab.SetActive(true);
        launchpad.SetExternalTankBuilt();
        SoundFXManager.Instance.PlaySound(SFX.Player.Work, this.gameObject.transform, 1f);
        //They have won the game


        triggerRocketModuleUIOverlay.Raise(false);
        winningEvent.Raise();
    }

    private void TryBuildEngine(){
        checkInventory.Raise(new packet.CheckInventoryPacket(
                this.gameObject, BuildingType.Engines, engineCosts
                ));
    }
    private void TryBuildChasis(){
        //if has techtier 2, else no checkInventory
        if(TechTier >= 2){
            checkInventory.Raise(new packet.CheckInventoryPacket(
                this.gameObject, BuildingType.Chasis, chasisCosts
                ));
        }
    }
    private void TryBuildCockpit(){
        if(TechTier >= 3){
            checkInventory.Raise(new packet.CheckInventoryPacket(
                this.gameObject, BuildingType.Cockpit, cockpitCosts
                ));
        }
    }
    private void TryBuildExternalTank(){
        if(TechTier >= 4){
            checkInventory.Raise(new packet.CheckInventoryPacket(
                this.gameObject, BuildingType.ExternalTank, externalTankCosts
                ));
        }
    }
    protected void QueryTechLevel(){
        TechTier = InventoryManager.Instance.GetTechTier(launchpad.GetBuildingType());
    }
    //Events
    //------------------------------------------------------------------

}