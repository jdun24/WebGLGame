public class IndustrialExtractorMining : AbstractExtractorMining
{
    IndustrialExtractor IndustrialExtractorClass;
    void Start(){
        DragAndDropExtractor.OnPlacementEvent += LinkToResource;
        gravityBody = GetComponent<GravityBody2D>();
        IndustrialExtractorClass = new();
        mineInterval = IndustrialExtractorClass.GetMineInterval();
        amountToMine = IndustrialExtractorClass.GetAmountToMine();
        baseBreakChance = IndustrialExtractorClass.GetBaseBreakChance();
    }

    void Update(){
        MineIfPlaced();
    }
}