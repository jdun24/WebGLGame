
public class JetPack : BaseBuilding{
    private BuildingComponents.BuildingType buildingType = BuildingComponents.BuildingType.JetPack;

    // Abstract method to get the cost dictionary
    public JetPack(){
        buildingData = LoadBuildingData("Jetpack");
    }
    public BuildingComponents.BuildingType GetBuildingType(){
        return buildingType;
    }
}