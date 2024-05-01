using UnityEngine;
using BuildingComponents;
public class CollectableResourceGen
{

    public CollectableResourceGen()
    {

    }
    public ResourceType GetRandomResource(AsteroidClass type){
        int resourceSeed = generateResourceSeed(type);
        return initResource(resourceSeed);
    }

    public int GetRandomAmount(ResourceType resource){
        return GenerateRandomAmount(resource);
    }
    //--------------------< Resource Methods > --------------------//
    public ResourceType initResource(int seed)
    {
        switch (seed)
        {
            case 0:
                return ResourceType.Iron;
            case 1:
                return ResourceType.Nickel;
            case 2:
                return ResourceType.Cobalt;
            case 3:
                return ResourceType.Platinum;
            case 4:
                return ResourceType.Gold;
            case 5:
                return ResourceType.Technetium;
            case 6:
                return ResourceType.Tungsten;
            case 7:
                return ResourceType.Iridium;
            default:
                return ResourceType.Iron;
        }
    }

    public Rarity getResourceRarity(int seed)
    {
        switch (seed)
        {
            case 0: //Iron
                return Rarity.Common;
            case 1: //Nickel
                return Rarity.Common;
            case 2: //Cobalt
                return Rarity.Common;
            case 3: //Plat
                return Rarity.Commodity;
            case 4: //Gold
                return Rarity.Commodity;
            case 5: //Tech
                return Rarity.Commodity;
            case 6: //Tung
                return Rarity.Exotic;
            case 7://Ir
                return Rarity.Exotic;
            default: //Iron
                return Rarity.Common;
        }
    }

    //--------------------< Asteroid Type Methods > --------------------//
    private int generateResourceSeed(AsteroidClass type)
    {
        RandomNumGen generator = new RandomNumGen();
        switch (type)
        {
            case AsteroidClass.S_Class:
                double[] Sprobabilities = { 0.0, 0.0, 0.0, 0.10, 0.10, 0.10, 0.35, 0.35, 0.0 };
                return generator.GenerateBiasedNumber(Sprobabilities);

            case AsteroidClass.A_Class:
                double[] Aprobabilities = { 0.125, 0.125, 0.125, 0.125, 0.125, 0.125, 0.125, 0.125, 0.0 };
                return generator.GenerateBiasedNumber(Aprobabilities);

            case AsteroidClass.B_Class:
                double[] Bprobabilities = { 0.05, 0.05, 0.05, 0.30, 0.30, 0.20, 0.03, 0.02, 0.0 };
                return generator.GenerateBiasedNumber(Bprobabilities);

            case AsteroidClass.C_Class:
                double[] Cprobabilities = { 0.35, 0.30, 0.25, 0.03, 0.03, 0.02, 0.01, 0.01, 0.0 };
                return generator.GenerateBiasedNumber(Cprobabilities);

            case AsteroidClass.D_Class:
                //Spawns any Common resource with much lower chance of Plat or Gold, never Technetium
                double[] Dprobabilities = { 0.35, 0.35, 0.15, 0.10, 0.05, 0.0, 0.0, 0.0, 0.0 };
                return generator.GenerateBiasedNumber(Dprobabilities);
            default:
                Debug.LogError("CollectableResourceGen.cs --: generateResourceSeed() :-- Failed to recognize " + type);
                break;
        }
        return 0;
    }

    public int GenerateRandomAmount(ResourceType resource){
        switch (resource)
        {
            case ResourceType.Iron:
                return UnityEngine.Random.Range(15, 25);
            case ResourceType.Nickel:
                return UnityEngine.Random.Range(15, 25);
            case ResourceType.Cobalt:
                return UnityEngine.Random.Range(8, 18);
            case ResourceType.Platinum:
                return UnityEngine.Random.Range(7, 18);
            case ResourceType.Gold:
                return UnityEngine.Random.Range(6, 12);
            case ResourceType.Technetium:
                return UnityEngine.Random.Range(1, 11);
            case ResourceType.Tungsten:
                return UnityEngine.Random.Range(1, 8);
            case ResourceType.Iridium:
                return UnityEngine.Random.Range(1, 8);

            default:
                Debug.LogError("GenerateRandomAmount.cs --: generateResourceSeed() :-- Failed to recognize " + resource);
                break;
        }
        return 0;
    }

}
