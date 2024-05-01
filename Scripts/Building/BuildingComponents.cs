using System;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingComponents
{

    public enum ResourceType
    {
        Iron,
        Nickel,
        Cobalt,
        Platinum,
        Gold,
        Technetium,
        Tungsten,
        Iridium,
        TechPoint
    }

    public enum BuildingType
    {
        Extractor,
        CommercialExtractor,
        IndustrialExtractor,
        Exosuit,
        Satellite,
        JetPack,
        Cybernetics,
        LaunchPad,
        RobotBuddy,
        Engines,
        Chasis,
        Cockpit,
        ExternalTank,
        RobotBuddyAlpha,
        RobotBuddyBeta
    }
    
    //JSON Serialization
    //----------------------------------------------------------------------------------------
    public class BuildingObject
    {
        public string ID;
        public string Type;
        public string ItemTitle;
        public string ItemDescription;
        public int AmountToMine;
        public float IntervalMine;
        public float percentAsteroidReach;
        public float baseBreakChance;
        public Dictionary<string, int> Costs;
        public string Image;
        public String[] TechUpTexts;
    }

    [System.Serializable]
    public class BuildingData
    {
        public List<BuildingObject> BuildingObject;
    }
    
}
