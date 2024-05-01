using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseBuilding
{
    protected LoadableBuildingObject buildingData;
    protected ObjectsCost thisCosts;
    protected int currentTier = 0;
    LoadableBuildingObject extractor;
    LoadableBuildingObject commercialExtractor;
    LoadableBuildingObject industrialExtractor;
    LoadableBuildingObject exosuit;
    LoadableBuildingObject cybernetics;
    LoadableBuildingObject satellite;
    LoadableBuildingObject launchPad;
    LoadableBuildingObject robotBuddy;

    public ObjectsCost GetCostDictionary()
    {
        return thisCosts;
    }

    protected LoadableBuildingObject LoadBuildingData(string id)
    {
        InitObjCost(FindBuildingObjectByID(id));
        return FindBuildingObjectByID(id);
    }

    public LoadableBuildingObject FindBuildingObjectByID(string id)
    {
        InitItems();
        switch (id)
        {
            case "Extractor":
                return extractor;
            case "CommercialExtractor":
                return commercialExtractor;
            case "IndustrialExtractor":
                return industrialExtractor;
            case "Exosuit":
                return exosuit;
            case "Cybernetics":
                return cybernetics;
            case "Satellite":
                return satellite;
            case "RobotBuddy":
                return robotBuddy;
            case "LaunchPad":
                return launchPad;
            default:
                Debug.Log("BaseBuilding missed switch cases, returning extractor");
                return extractor;
        }
    }

    protected void InitObjCost(LoadableBuildingObject thisObject)
    {
        thisCosts = new ObjectsCost(
                thisObject.Costs["Iron"],
                thisObject.Costs["Nickel"],
                thisObject.Costs["Cobalt"],
                thisObject.Costs["Platinum"],
                thisObject.Costs["Gold"],
                thisObject.Costs["Technetium"],
                thisObject.Costs["Tungsten"],
                thisObject.Costs["Iridium"],
                0
            );
    }

    public void InitItems()
    {
        extractor = new LoadableBuildingObject(
            "Extractor",
            "Industrial",
            "Extractor",
            "Mines shallow resources, could maybe potentially break very easily",
            2,
            5,
            20,
            0.1f,
            new Dictionary<string, int>
            {
                { "Iron", 15 },
                { "Nickel", 15 },
                { "Cobalt", 5 },
                { "Platinum", 0 },
                { "Gold", 0 },
                { "Technetium", 0 },
                { "Tungsten", 0 },
                { "Iridium", 0 }
            },
            "",
            new List<string>
            {
                "Increase mining amount by 20%, reduce break chance and increase extractor reach",
                "Increase mining amount by 40%, further reduce break chance and increase reach",
                "Peak Technology Reached!!!"
            }
        );

        commercialExtractor = new LoadableBuildingObject(
            "CommercialExtractor",
            "Industrial",
            "Commercial Extractor",
            "Mines deep resources, could potentially break",
            5,
            5,
            30,
            0.05f,
            new Dictionary<string, int>
            {
                { "Iron", 25 },
                { "Nickel", 25 },
                { "Cobalt", 10 },
                { "Platinum", 10 },
                { "Gold", 0 },
                { "Technetium", 2 },
                { "Tungsten", 0 },
                { "Iridium", 0 }
            },
            "",
            new List<string>
            {
                "Unlock this building",
                "Increase mining amount by 20%, reduce break chance",
                "Increase mining amount by 40%, further reduce break chance",
                "Peak Technology Reached!!!"
            }
        );

        industrialExtractor = new LoadableBuildingObject(
            "IndustrialExtractor",
            "Industrial",
            "Industrial Extractor",
            "A Robust piece of mining equipment that reaches that deepest of minerals",
            10,
            5,
            50,
            0.01f,
            new Dictionary<string, int>
            {
                { "Iron", 75 },
                { "Nickel", 75 },
                { "Cobalt", 50 },
                { "Platinum", 15 },
                { "Gold", 0 },
                { "Technetium", 1 },
                { "Tungsten", 1 },
                { "Iridium", 0 }
            },
            "",
            new List<string>
            {
                "Unlock this building",
                "Increase mining amount by 20%, reduce break chance and increase extractor reach",
                "Increase mining amount by 40%, further reduce break chance and increase reach",
                "Peak Technology Reached!!!"
            }
        );

        exosuit = new LoadableBuildingObject(
            "Exosuit",
            "Suit",
            "ExoSuit",
            "Increase movement by 10%",
            10,
            0,
            0,
            0,
            new Dictionary<string, int>
            {
                { "Iron", 50 },
                { "Nickel", 50 },
                { "Cobalt", 30 },
                { "Platinum", 1 },
                { "Gold", 10 },
                { "Technetium", 1 },
                { "Tungsten", 0 },
                { "Iridium", 0 }
            },
            "",
            new List<string>
            {
                "Hydraulic glute supporters allow for higher jumping",
                "Increase movement speed by another 10% (total: 20%)",
                "Mine cave resources with no minigame",
                "Peak Technology Reached!!!"
            }
        );

        cybernetics = new LoadableBuildingObject(
            "Cybernetics",
            "Suit",
            "Suit Cybernetics",
            "Gain a charge every 7 minutes that can charge your Robot Friends :)",
            0,
            0,
            0,
            0,
            new Dictionary<string, int>
            {
                { "Iron", 75 },
                { "Nickel", 75 },
                { "Cobalt", 50 },
                { "Platinum", 10 },
                { "Gold", 15 },
                { "Technetium", 10 },
                { "Tungsten", 0 },
                { "Iridium", 1}
            },
            "",
            new List<string>
            {
                "Gain a charge every 5 minutes, can hold 3 at a time",
                "Can now use a charge to instantly repair a module",
                "Hold up to 6 charges with a 3.5 minute charge time",
                "Peak Technology Reached!!!"
            }
        );
        robotBuddy = new LoadableBuildingObject(
            "RobotBuddy",
            "Robotics",
            "Robo Buddy",
            "A friendly interface in this lonely asteroid cloud... can repair modules(Max 2)",
            0,
            0,
            0,
            0,
            new Dictionary<string, int>
            {
                { "Iron", 35 },
                { "Nickel", 35 },
                { "Cobalt", 15 },
                { "Platinum", 10 },
                { "Gold", 20 },
                { "Technetium", 5 },
                { "Tungsten", 0 },
                { "Iridium", 0 }
            },
            "",
            new List<string>
            {
                "Tech to level 4 to make your robot buddies never run out of battery",
                "Tech to level 4 to make your robot buddies never run out of battery",
                "Tech to level 4 to make your robot buddies never run out of battery",
                "Through sheer will Robot buddy will never run out of power again...",
                "Peak Technology Reached!!!"
            }
        );
        satellite = new LoadableBuildingObject(
        "Satellite",
        "Robotics",
        "Satellite",
        "Use to reveal resources (Max 1 per asteroid)",
        0,
        0,
        0,
        0,
        new Dictionary<string, int>
        {
            { "Iron", 150 },
            { "Nickel", 150 },
            { "Cobalt", 150 },
            { "Platinum", 75 },
            { "Gold", 50 },
            { "Technetium", 4 },
            { "Tungsten",1 },
            { "Iridium", 2 }
        },
        "",
        new List<string>
        {
            "Gain the ability to steer satellites",
            "Future Satellites cost 50 less Cobalt and 25 less platinum",
            "Future Satellites cost 20 less Gold and no Iridium",
            "Peak Technology Reached!!!"
        }
    );

        launchPad = new LoadableBuildingObject(
            "LaunchPad",
            "Robotics",
            "Rocket Launch Pad",
            "Fly home with all your mined resources!!!",
            0,
            0,
            0,
            0,
            new Dictionary<string, int>
            {
            { "Iron", 250 },
            { "Nickel", 250 },
            { "Cobalt", 150 },
            { "Platinum", 75 },
            { "Gold", 50 },
            { "Technetium", 50 },
            { "Tungsten", 35 },
            { "Iridium", 35 }
            },
            "",
            new List<string>
            {
            "Can now build Rocket Engines",
            "Can now build Rocket Chassis",
            "Can now build Rocket Cockpit",
            "Can now build Rocket Computer Wiring",
            "Peak Technology Reached!!!"
            }
        );
    }
}

public class LoadableBuildingObject
{
    public LoadableBuildingObject(
        string id,
        string type,
        string itemTitle,
        string itemDescription,
        int amountToMine,
        int intervalMine,
        int asteroidReach,
        double baseBreakChance,
        Dictionary<string, int> costs,
        string image,
        List<string> techUpTexts)
    {
        ID = id;
        Type = type;
        ItemTitle = itemTitle;
        ItemDescription = itemDescription;
        AmountToMine = amountToMine;
        IntervalMine = intervalMine;
        AsteroidReach = asteroidReach;
        BaseBreakChance = (float)baseBreakChance;
        Costs = costs;
        Image = image;
        TechUpTexts = techUpTexts;
    }
    public string ID { get; set; }
    public string Type { get; set; }
    public string ItemTitle { get; set; }
    public string ItemDescription { get; set; }
    public int AmountToMine { get; set; }
    public int IntervalMine { get; set; }
    public float AsteroidReach { get; set; }
    public float BaseBreakChance { get; set; }
    public Dictionary<string, int> Costs { get; set; }
    public string Image { get; set; }
    public List<string> TechUpTexts { get; set; }
}
