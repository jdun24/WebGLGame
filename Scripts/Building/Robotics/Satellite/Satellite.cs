using BuildingComponents;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine;

public class Satellite : BaseBuilding
{
    private BuildingComponents.BuildingType buildingType = BuildingComponents.BuildingType.Satellite;

    // Abstract method to get the cost dictionary
    public Satellite()
    {
        buildingData = LoadBuildingData("Satellite");
    }

    public BuildingComponents.BuildingType GetBuildingType()
    {
        return buildingType;
    }
}
