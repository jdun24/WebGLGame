
public class LaunchPad : BaseBuilding{
    private BuildingComponents.BuildingType buildingType = BuildingComponents.BuildingType.LaunchPad;

    //Building focused vars
    private bool hasBuiltEngines = false;
    private bool hasBuiltChasis = false;
    private bool hasBuiltCockpit = false;
    private bool hasBuiltExternalTank = false;

    // Abstract method to get the cost dictionary
    public LaunchPad(){
        buildingData = LoadBuildingData("LaunchPad");
    }
    public BuildingComponents.BuildingType GetBuildingType(){
        return buildingType;
    }
    public bool isEngineBuilt(){
        return hasBuiltEngines;
    }
    public void SetEngineBuilt(){
        hasBuiltEngines = true;
    }
    public bool isChasisBuilt(){
        return hasBuiltChasis;
    }
    public void SetChasisBuilt(){
        hasBuiltChasis = true;
    }
    public bool isCockpitBuilt(){
        return hasBuiltCockpit;
    }
    public void SetCockpitBuilt(){
        hasBuiltCockpit = true;
    }
    public bool isExternalTankBuilt(){
        return hasBuiltExternalTank;
    }
    public void SetExternalTankBuilt(){
        hasBuiltExternalTank = true;
    }

}