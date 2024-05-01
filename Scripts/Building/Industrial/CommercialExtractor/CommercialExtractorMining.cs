public class CommercialExtractorMining : AbstractExtractorMining
{
    CommercialExtractor CommercialExtractorClass;
    void Start(){
        DragAndDropExtractor.OnPlacementEvent += LinkToResource;
        gravityBody = GetComponent<GravityBody2D>();
        CommercialExtractorClass = new();
        mineInterval = CommercialExtractorClass.GetMineInterval();
        amountToMine = CommercialExtractorClass.GetAmountToMine();
        baseBreakChance = CommercialExtractorClass.GetBaseBreakChance();
    }

    void Update(){
        MineIfPlaced();
    }
}