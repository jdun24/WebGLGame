using UnityEngine;
public class RobotBuddy : BaseBuilding
{
    private BuildingComponents.BuildingType buildingType = BuildingComponents.BuildingType.RobotBuddy;

    //Gameplay related member vars
    private float currentCharge = 600;
    //Changing the maxCharge value requires you to change the maxCharge value in RobotBuddyUIManager as well
    private readonly float maxCharge = 600;

    // Abstract method to get the cost dictionary
    public RobotBuddy()
    {
        buildingData = LoadBuildingData("RobotBuddy");
        //BuildingComponents.BuildingObject thisObject = BuildingComponents.
    }

    public BuildingComponents.BuildingType GetBuildingType()
    {
        return buildingType;
    }

    public float ReduceCharge(float reduceBy)
    {
        currentCharge -= reduceBy;

        return currentCharge;
    }

    public float GainCharge(float gain)
    {
        currentCharge += gain;
        return currentCharge;
    }

    public float GiveFullCharge()
    {
        currentCharge = maxCharge;
        return currentCharge;
    }

    public float GetCurrentCharge()
    {
        return currentCharge;
    }

    public int GetCurrentTechTier()
    {
        return currentTier;
    }

    public int UpdateTechTier()
    {
        return currentTier = InventoryManager.Instance.GetTechTier(buildingType);
    }
    public bool IsChargeEmpty(){
        if(currentCharge <= 0){
            return true;
        }
        return false;
    }
}
