public class Extractor : AbstractExtractor
{
    public Extractor(){
        buildingType = BuildingComponents.BuildingType.Extractor;
        //buildingData = LoadBuildingData();
        buildingData = LoadBuildingData("Extractor");

        //BuildingComponents.BuildingObject thisObject = FindBuildingObjectByID("Extractor");
        SetVarsFromJsonData(5f, 2, .02f, 20f);
        currentTier = 1;
        //thisCosts = InitObjCost(buildingData);
    }


}