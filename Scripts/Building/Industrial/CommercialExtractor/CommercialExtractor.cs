public class CommercialExtractor : AbstractExtractor
{
    public CommercialExtractor(){
        buildingType = BuildingComponents.BuildingType.CommercialExtractor;
        buildingData = LoadBuildingData("CommercialExtractor");
        SetVarsFromJsonData(5f, 5, .01f, 30f);
    }

}