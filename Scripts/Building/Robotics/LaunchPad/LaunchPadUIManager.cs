using UnityEngine;
using BuildingComponents;
using System.IO;
using TMPro;
using System.Collections;

public class LaunchPadUIManager : MonoBehaviour{

    [SerializeField] public GameObject BuildRocketModuleUI;
    [SerializeField] VoidEvent buttonPressEvent;
    [SerializeField] GameObject youWonOverlay;
    [SerializeField] private TextMeshProUGUI EngineCostText;
    [SerializeField] private TextMeshProUGUI ChasisCostText;
    [SerializeField] private TextMeshProUGUI CockpitCostText;
    [SerializeField] private TextMeshProUGUI ExternalTankCostText;
    private ObjectsCost engineCosts = new ObjectsCost(100,100, 50,0,20,10,0,5,0);
    private ObjectsCost chasisCosts = new ObjectsCost(100,100,75,20,0,10,10,10,0);
    private ObjectsCost cockpitCosts = new ObjectsCost(100,100,75,50,50,25,25,25,0);
    private ObjectsCost externalTankCosts = new ObjectsCost(100,100,75,50,50,50,50,40,0);
    [SerializeField] private GameObject EndCreditsOverlay;
    [SerializeField] private GameObject SideOverlay;
    public void OnTriggerRocketModuleUIOverlay(bool setTo){
        BuildRocketModuleUI.SetActive(setTo);
    }
    public void RocketModuleButtonPressed(){
        buttonPressEvent.Raise();
    }
    public void OnWin(){
        youWonOverlay.SetActive(true);
        StartCoroutine(LoadEndScene());
    }
    public void Awake(){
        LoadIntoUI(engineCosts, EngineCostText);
        LoadIntoUI(chasisCosts, ChasisCostText);
        LoadIntoUI(cockpitCosts, CockpitCostText);
        LoadIntoUI(externalTankCosts, ExternalTankCostText);
    }
    private void LoadIntoUI(ObjectsCost item, TextMeshProUGUI costText){
        costText.text = "";
        int i = 0;
        foreach(var costNode in item.getCostDictionary()){
            if (System.Convert.ToInt32(costNode.Value) > 0)
                if(i % 2 == 0){
                    costText.text += $"{costNode.Key}: {costNode.Value}\t";
                }else{
                    costText.text += $"{costNode.Key}: {costNode.Value}\n";
                }

                i++;
        }
    }


    private IEnumerator LoadEndScene(){
        yield return new WaitForSeconds(3f);
        SideOverlay.SetActive(false);
        EndCreditsOverlay.SetActive(true);
        SoundFXManager.Instance.StopSoundsOfType(typeof(SFX.Music.Asteroid));
        SoundFXManager.Instance.PlayRandomSoundOfType(typeof(SFX.Music.Asteroid), gameObject.transform, 0.5f);
        yield return new WaitForSeconds(40f);
        SideOverlay.SetActive(true);
        EndCreditsOverlay.SetActive(false);
    }
}
