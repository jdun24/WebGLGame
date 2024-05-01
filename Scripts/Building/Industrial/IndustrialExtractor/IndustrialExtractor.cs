public class IndustrialExtractor : AbstractExtractor
{
    public IndustrialExtractor(){
        buildingType = BuildingComponents.BuildingType.IndustrialExtractor;
        buildingData = LoadBuildingData("IndustrialExtractor");
        SetVarsFromJsonData(10, 5, .005f, 50);
    }

}