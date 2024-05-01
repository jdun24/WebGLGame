using UnityEngine.Rendering.Universal;
using UnityEngine;
public class PlayerFlashlight : MonoBehaviour{
    private Light2D flashlight;

    public void Awake(){
        flashlight = GetComponent<Light2D>();
        flashlight.enabled = false;
    }

    public void OnTechUpEvent(packet.TechUpPacket packet){
        if(packet.building == BuildingComponents.BuildingType.Exosuit){
            if(packet.TechToLevel == 1){
                flashlight.enabled = true;
            }
        }
    }

    public void OnFlashLightToggle(){
        if(BuildManager.Instance.HasBuiltExosuit() && BuildingTierManager.Instance.GetTierOf(BuildingComponents.BuildingType.Exosuit) > 0){
            if(flashlight.enabled == true){
                flashlight.enabled = false;
            }else{
                flashlight.enabled = true;
            }
        }
    }
}