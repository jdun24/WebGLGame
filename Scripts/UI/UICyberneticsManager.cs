using UnityEngine;
using UnityEngine.UI;


public class UICyberneticsManager : MonoBehaviour
{
    [Header("Cybernetic UI Overlays")]
    [SerializeField] private GameObject TierZeroOverlay;
    [SerializeField] private GameObject TierOneOverlay;
    [SerializeField] private GameObject TierThreeOverlay;
    [SerializeField] private GameObject TotalOverlay;
    [Header("Cybernetic UI Components")]
    [SerializeField] private Image TierZeroCharge1;

    [SerializeField] private Image TierOneCharge1;
    [SerializeField] private Image TierOneCharge2;
    [SerializeField] private Image TierOneCharge3;

    [SerializeField] private Image TierThreeCharge1;
    [SerializeField] private Image TierThreeCharge2;
    [SerializeField] private Image TierThreeCharge3;
    [SerializeField] private Image TierThreeCharge4;
    [SerializeField] private Image TierThreeCharge5;
    [SerializeField] private Image TierThreeCharge6;

    // Not for display
    private int currentTier = 0;
    private Color onColor = new Color32(183, 117, 11, 255);
    //private Color onColor = new Color32(100, 255, 85, 255);
    private Color offColor = new Color32(66, 32, 59, 222);
    public void Awake(){
        TierZeroOverlay.SetActive(false);
        TierOneOverlay.SetActive(false);
        TierThreeOverlay.SetActive(false);
        TotalOverlay.SetActive(false);
    }


    //Events
    public void OnBuildObjectEvent(BuildingComponents.BuildingType building){
        if(building == BuildingComponents.BuildingType.Cybernetics){
            currentTier = InventoryManager.Instance.GetTechTier(BuildingComponents.BuildingType.Cybernetics);
            UpdateUIOverlay();
            TotalOverlay.SetActive(true);
        }
    }
    public void OnTechUpEvent(packet.TechUpPacket packet){
        if(packet.building == BuildingComponents.BuildingType.Cybernetics){
            currentTier = InventoryManager.Instance.GetTechTier(BuildingComponents.BuildingType.Cybernetics);
            UpdateUIOverlay();
        }
    }

    public void OnUpdateCharge(int charge){
        UpdateUIComponents(charge);
    }

    // Class Specific functions
    private void UpdateUIOverlay(){
        switch(currentTier){
            case 0:
                TierZeroOverlay.SetActive(true);
            break;
            case 1:
                TierZeroOverlay.SetActive(false);
                TierOneOverlay.SetActive(true);
            break;
            case 2:
                TierZeroOverlay.SetActive(false);
                TierOneOverlay.SetActive(true);
            break;
            case 3:
                TierZeroOverlay.SetActive(false);
                TierOneOverlay.SetActive(false);
                TierThreeOverlay.SetActive(true);
            break;
        }
        if(currentTier == 0){
            TierZeroCharge1.color = onColor;
        }else if(currentTier == 1 || currentTier == 2){
            TierOneCharge1.color = onColor;
        }else if(currentTier == 3){
            TierThreeCharge1.color = onColor;
        }
    }

    private void UpdateUIComponents(int numCharges){
        
        switch(currentTier){
            case 0:
                SetChargeUIZero(numCharges);
            break;
            case 1:
                SetChargeUITwo(numCharges);
            break;
            case 2:
                SetChargeUITwo(numCharges);
            break;
            case 3:
                SetChargeUIThree(numCharges);
            break;
        }
    }

    private void SetChargeUIZero(int numCharges){
        if(numCharges == 0){
            TierZeroCharge1.color = offColor;
        }else if(numCharges == 1){
            TierZeroCharge1.color = onColor;
        }else{
            Debug.LogError("[UICyberneticsManager] -: UpdateUIComponents() -> SetChargeUIZero(): numCharges is above the number that its current tech level allows for numCharges= "  + numCharges);
        }
    }
    private void SetChargeUITwo(int numCharges){
        switch(numCharges){
            case 0:
                SetTierTwoImages(offColor, offColor, offColor);
            break;
            case 1:
                SetTierTwoImages(onColor, offColor, offColor);
            break;
            case 2:
                SetTierTwoImages(onColor, onColor, offColor);
            break;
            case 3:
                SetTierTwoImages(onColor, onColor, onColor);
            break;
            default:
                Debug.LogError("[UICyberneticsManager] -: UpdateUIComponents() -> SetChargeUIZero(): numCharges is above the number that its current tech level allows for numCharges= "  + numCharges);
            break;
        }
    }
    private void SetChargeUIThree(int numCharges){
        switch(numCharges){
            case 0:
                SetTierThreeImages(offColor, offColor, offColor, offColor, offColor, offColor);
            break;
            case 1:
                SetTierThreeImages(onColor, offColor, offColor, offColor, offColor, offColor);
            break;
            case 2:
                SetTierThreeImages(onColor, onColor, offColor, offColor, offColor, offColor);
            break;
            case 3:
                SetTierThreeImages(onColor, onColor, onColor, offColor, offColor, offColor);
            break;
            case 4:
                SetTierThreeImages(onColor, onColor, onColor, onColor, offColor, offColor);
            break;
            case 5:
                SetTierThreeImages(onColor, onColor, onColor, onColor, onColor, offColor);
            break;
            case 6:
                SetTierThreeImages(onColor, onColor, onColor, onColor, onColor, onColor);
            break;
            default:
                Debug.LogError("[UICyberneticsManager] -: UpdateUIComponents() -> SetChargeUIZero(): numCharges is above the absolute max: numCharges= " + numCharges);
            break;
        }
    }
    
    private void SetTierTwoImages(Color col1, Color col2, Color col3){
        TierOneCharge1.color = col1;
        TierOneCharge2.color = col2;
        TierOneCharge3.color = col3;
    }

    private void SetTierThreeImages(Color col1, Color col2, Color col3, Color col4, Color col5, Color col6){
        TierThreeCharge1.color = col1;
        TierThreeCharge2.color = col2;
        TierThreeCharge3.color = col3;
        TierThreeCharge4.color = col4;
        TierThreeCharge5.color = col5;
        TierThreeCharge6.color = col6;
    }
}