using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using BuildingComponents;

public class ObjectsCost
{
    Dictionary<ResourceType, int> inv = new Dictionary<ResourceType, int>();
    public ObjectsCost(int IronAmt, int NickelAmt, int CobaltAmt,

    int PlatinumAmt, int GoldAmt, int TechnetiumAmt, int TungstenAmt, int IridiumAmt, int TechPointAmt)
    {
        if (IronAmt != 0)
            inv.Add(ResourceType.Iron, IronAmt);
        if (NickelAmt != 0)
            inv.Add(ResourceType.Nickel, NickelAmt);
        if (CobaltAmt != 0)
            inv.Add(ResourceType.Cobalt, CobaltAmt);
        if (PlatinumAmt != 0)
            inv.Add(ResourceType.Platinum, PlatinumAmt);
        if (GoldAmt != 0)
            inv.Add(ResourceType.Gold, GoldAmt);
        if (TechnetiumAmt != 0)
            inv.Add(ResourceType.Technetium, TechnetiumAmt);
        if (TungstenAmt != 0)
            inv.Add(ResourceType.Tungsten, TungstenAmt);
        if (IridiumAmt != 0)
            inv.Add(ResourceType.Iridium, IridiumAmt);
        if (TechPointAmt != 0)
            inv.Add(ResourceType.TechPoint, TechPointAmt);
    }

    public Dictionary<ResourceType, int> getCostDictionary()
    {
        return inv;
    }

    public bool hasTechCost()
    {
        return inv.ContainsKey(ResourceType.TechPoint);
    }

    public override string ToString()
    {
        string result = "Object's Costs:\n";
        foreach (var kvp in inv)
        {
            result += $"{kvp.Key}: {kvp.Value}\n";
        }
        return result;
    }
}
