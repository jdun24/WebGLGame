using UnityEngine;
public class ExtractorMining : AbstractExtractorMining
{
    Extractor ExtractorClass;
    void Start(){
        buildingType = BuildingComponents.BuildingType.Extractor;
        DragAndDropExtractor.OnPlacementEvent += LinkToResource;
        gravityBody = GetComponent<GravityBody2D>();
        ExtractorClass = new();
        mineInterval = ExtractorClass.GetMineInterval();
        amountToMine = ExtractorClass.GetAmountToMine();
        baseBreakChance = ExtractorClass.GetBaseBreakChance();
    }

    void Update(){
        MineIfPlaced();
    }
}