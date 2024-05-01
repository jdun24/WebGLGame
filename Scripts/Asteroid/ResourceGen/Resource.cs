using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum Rarity
{
    Common, Commodity, Exotic
}

public class Resource
{
    //Resource info
    public string Name { get; set; }
    public Vector2 Position { get; set; }
    public Color Color { get; set; }
    public Rarity rarity;

    //Deposit info
    public Vector2 depositSize { get; set; }
    public int depositAmount { get; set; }
    public bool isDepleted;
    public BuildingComponents.ResourceType resourceType;
    public int originalAmount;
    protected float xSize;
    protected float ySize;
    //public GameObject prefab; //for when each resource has its own prefab

    public Resource(Vector2 pos)
    {
        Position = pos;
        depositSize = randomizeSize();
        //amount of resources in an ore deposit is related to its size
        depositAmount = (int)(depositSize.x + depositSize.y) * 25;
    }

    protected Vector2 randomizeSize()
    {
        float xSize = Random.Range(0.5f, 5f);
        float ySize = Random.Range(0.5f, 5f);

        Vector2 valToReturn = new Vector3(xSize, ySize);
        return valToReturn;
    }
    public BuildingComponents.ResourceType GetResourceType(){
        return resourceType;
    }
    public int GetDepositAmount(){
        return depositAmount;
    }
    public void ReduceDepositAmount(int minedAmount){
        depositAmount -= minedAmount;
    }
    public Vector2 ReduceSize(int AfterAmount, int changeAmt, GameObject resource){
        float percentDecrease = (float)AfterAmount / originalAmount;
        Debug.Log($"percentDecrease: {percentDecrease}");

        Vector2 newScale = resource.transform.localScale * percentDecrease;
        depositSize = newScale;
        return newScale;
    }
}

//----------------------<COMMON RESOURCES>----------------------//
public class Iron : Resource
{
    public Iron(Vector2 pos) : base(pos)
    {
        Name = "Iron";
        Color = Color.red;
        resourceType = BuildingComponents.ResourceType.Iron;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 70;
        rarity = Rarity.Common;
        originalAmount = depositAmount;
    }
}

public class Nickel : Resource
{
    public Nickel(Vector2 pos) : base(pos)
    {
        Name = "Nickel";
        Color = Color.gray;
        resourceType = BuildingComponents.ResourceType.Nickel;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 70;
        rarity = Rarity.Common;
        originalAmount = depositAmount;
    }
}

public class Cobalt : Resource
{
    public Cobalt(Vector2 pos) : base(pos)
    {
        Name = "Cobalt";
        resourceType = BuildingComponents.ResourceType.Cobalt;
        Color = Color.white;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 65;
        rarity = Rarity.Common;
        originalAmount = depositAmount;
    }
}

//----------------------<COMMODITY RESOURCES>----------------------//
public class Platinum : Resource
{
    public Platinum(Vector2 pos) : base(pos)
    {
        Name = "Platinum";
        resourceType = BuildingComponents.ResourceType.Platinum;
        Color = Color.cyan;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 50;
        rarity = Rarity.Commodity;
        originalAmount = depositAmount;
    }
}

public class Gold : Resource
{
    public Gold(Vector2 pos) : base(pos)
    {
        Name = "Gold";
        resourceType = BuildingComponents.ResourceType.Gold;
        Color = Color.yellow;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 50;
        rarity = Rarity.Commodity;
        originalAmount = depositAmount;
    }
}

public class Technetium : Resource
{
    public Technetium(Vector2 pos) : base(pos)
    {
        Name = "Technetium";
        resourceType = BuildingComponents.ResourceType.Technetium;
        Color = Color.green;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 50;
        rarity = Rarity.Commodity;
        originalAmount = depositAmount;
    }
}

//----------------------<EXOTIC RESOURCES>----------------------//
public class Tungsten : Resource
{
    public Tungsten(Vector2 pos) : base(pos)
    {
        Name = "Tungsten";
        resourceType = BuildingComponents.ResourceType.Tungsten;
        Color = Color.blue;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 20;
        rarity = Rarity.Exotic;
        originalAmount = depositAmount;
    }
}

public class Iridium : Resource
{
    public Iridium(Vector2 pos) : base(pos)
    {
        Name = "Iridium";
        resourceType = BuildingComponents.ResourceType.Iridium;
        Color = Color.black;
        Position = pos;
        depositSize = base.randomizeSize();
        depositAmount = (int)(depositSize.x + depositSize.y) * 25;
        rarity = Rarity.Exotic;
        originalAmount = depositAmount;
    }
}