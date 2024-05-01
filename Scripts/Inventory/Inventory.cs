using System.Collections.Generic;
using BuildingComponents;

public class Inventory
{
    Dictionary<ResourceType, int> inv = new Dictionary<ResourceType, int>();
    Dictionary<BuildingType, int> currentTechLevels = new Dictionary<BuildingType, int>();
    public Inventory(int IronAmt, int NickelAmt, int CobaltAmt,
    int PlatinumAmt, int GoldAmt, int TechnetiumAmt, int TungstenAmt, int IridiumAmt,
    int TechPointAmt)
    {
        inv.Add(ResourceType.Iron, IronAmt);
        inv.Add(ResourceType.Nickel, NickelAmt);
        inv.Add(ResourceType.Cobalt, CobaltAmt);
        inv.Add(ResourceType.Platinum, PlatinumAmt);
        inv.Add(ResourceType.Gold, GoldAmt);
        inv.Add(ResourceType.Technetium, TechnetiumAmt);
        inv.Add(ResourceType.Tungsten, TungstenAmt);
        inv.Add(ResourceType.Iridium, IridiumAmt);
        inv.Add(ResourceType.TechPoint, TechPointAmt);
        currentTechLevels.Add(BuildingType.Extractor, 1);
        currentTechLevels.Add(BuildingType.CommercialExtractor, 0);
        currentTechLevels.Add(BuildingType.IndustrialExtractor, 0);
        currentTechLevels.Add(BuildingType.Exosuit, 0);
        currentTechLevels.Add(BuildingType.JetPack, 0);
        currentTechLevels.Add(BuildingType.Cybernetics, 0);
        currentTechLevels.Add(BuildingType.RobotBuddy, 0);
        currentTechLevels.Add(BuildingType.Satellite, 0);
        currentTechLevels.Add(BuildingType.LaunchPad, 0);
    }
    public Inventory(Dictionary<ResourceType, int> passedInv)
    {
        inv = passedInv;
        currentTechLevels.Add(BuildingType.Extractor, 1);
        currentTechLevels.Add(BuildingType.CommercialExtractor, 0);
        currentTechLevels.Add(BuildingType.IndustrialExtractor, 0);
        currentTechLevels.Add(BuildingType.Exosuit, 0);
        currentTechLevels.Add(BuildingType.JetPack, 0);
        currentTechLevels.Add(BuildingType.Cybernetics, 0);
        currentTechLevels.Add(BuildingType.RobotBuddy, 0);
        currentTechLevels.Add(BuildingType.Satellite, 0);
        currentTechLevels.Add(BuildingType.LaunchPad, 0);
    }
    public Dictionary<ResourceType, int> GetInvDictionary()
    {
        return inv;
    }
    public bool CheckCost(ObjectsCost resourcesToCheck)
    {
        foreach (var resource in resourcesToCheck.getCostDictionary())
        {
            //amt had < amt needed, if so cant buy
            if (inv[resource.Key] < resource.Value)
            {
                return false;
            }
        }
        //has all resources required
        return true;
    }
    public void PayForObjectWithObjCost(ObjectsCost costs)
    {
        foreach (var entry in costs.getCostDictionary())
        {
            SubResource(entry.Key, entry.Value);
        }
    }
    public void AddResource(ResourceType resourceType, int amt)
    {
        switch (resourceType)
        {
            case (ResourceType.Iron):
                inv[ResourceType.Iron] += amt;
                break;
            case (ResourceType.Nickel):
                inv[ResourceType.Nickel] += amt;
                break;
            case (ResourceType.Cobalt):
                inv[ResourceType.Cobalt] += amt;
                break;
            case (ResourceType.Platinum):
                inv[ResourceType.Platinum] += amt;
                break;
            case (ResourceType.Gold):
                inv[ResourceType.Gold] += amt;
                break;
            case (ResourceType.Technetium):
                inv[ResourceType.Technetium] += amt;
                break;
            case (ResourceType.Tungsten):
                inv[ResourceType.Tungsten] += amt;
                break;
            case (ResourceType.Iridium):
                inv[ResourceType.Iridium] += amt;
                break;
            case (ResourceType.TechPoint):
                inv[ResourceType.TechPoint] += amt;
                break;
        }
    }

    public void SubResource(ResourceType resourceType, int amt)
    {
        switch (resourceType)
        {
            case (ResourceType.Iron):
                inv[ResourceType.Iron] -= amt;
                break;
            case (ResourceType.Nickel):
                inv[ResourceType.Nickel] -= amt;
                break;
            case (ResourceType.Cobalt):
                inv[ResourceType.Cobalt] -= amt;
                break;
            case (ResourceType.Platinum):
                inv[ResourceType.Platinum] -= amt;
                break;
            case (ResourceType.Gold):
                inv[ResourceType.Gold] -= amt;
                break;
            case (ResourceType.Technetium):
                inv[ResourceType.Technetium] -= amt;
                break;
            case (ResourceType.Tungsten):
                inv[ResourceType.Tungsten] -= amt;
                break;
            case (ResourceType.Iridium):
                inv[ResourceType.Iridium] -= amt;
                break;
            case (ResourceType.TechPoint):
                inv[ResourceType.TechPoint] -= amt;
                break;
        }
    }

    //Dictionary<BuildingType, int> currentTechLevels = new Dictionary<BuildingType, int>();
    public void GainTechPoint()
    {
        inv[ResourceType.TechPoint] += 1;
    }
    public void TechUpBuilding(BuildingType building, int level)
    {
        currentTechLevels[building] = level;
    }
    public int GetBuildingTechLevel(BuildingType building)
    {
        return currentTechLevels[building];
    }
}
