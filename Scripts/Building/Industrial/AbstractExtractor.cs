using UnityEngine;
using BuildingComponents;
using System.IO;
using Newtonsoft.Json;
public abstract class AbstractExtractor : BaseBuilding
{
    protected GameObject prefab;
    protected float mineInterval;
    protected int amountToMine;
    protected float baseBreakChance;
    protected float AsteroidReach;
    protected BuildingComponents.BuildingType buildingType;
    // Abstract method to get the cost dictionary
    protected void SetVarsFromJsonData(float mineInterval, int amountToMine, float baseBreakChance, float AsteroidReach)
    {
        this.mineInterval = mineInterval;
        this.amountToMine = amountToMine;
        this.baseBreakChance = baseBreakChance;
        this.AsteroidReach = AsteroidReach;
    }

    public BuildingComponents.BuildingType GetBuildingType()
    {
        return buildingType;
    }
    public float GetMineInterval()
    {
        return mineInterval;
    }
    public int GetAmountToMine()
    {
        return amountToMine;
    }
    public float GetBaseBreakChance()
    {
        return baseBreakChance;
    }
    public float GetAsteroidReach()
    {
        return AsteroidReach;
    }
}
