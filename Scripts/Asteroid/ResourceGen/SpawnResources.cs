using UnityEngine;

public class SpawnResources
{
    public int numberOfObjectsToSpawn;
    public float spawnRadius;
    private Asteroid asteroid;

    public SpawnResources(float spawnRadius, int numberOfObjectsToSpawn, Asteroid asteroid)
    {
        this.spawnRadius = spawnRadius;
        this.numberOfObjectsToSpawn = numberOfObjectsToSpawn;
        this.asteroid = asteroid;
        SpawnObjects();
    }
    private void SpawnObjects()
    {
        for (int iter = 0; iter < numberOfObjectsToSpawn; iter++)
        {
            int infLoopProtector = 0;
            //Resource seed is dependant on asteroid class
            int resourceSeed = generateResourceSeed();

            // Calculate a valid random position relative to the SimpleGen's GameObject position
            Vector2 randomPosition = getRand_Pos_OffRarity(getResourceRarity(resourceSeed));
            while (asteroid.IsValidPosition(randomPosition, iter) == false && infLoopProtector < 100 && iter != 0)
            {
                //randomPosition = determineRandPosMethod(getResourceRarity(resourceSeed));
                randomPosition = getRand_Pos_OffRarity(getResourceRarity(resourceSeed));

                infLoopProtector += 1;
                if (infLoopProtector == 100)
                    Debug.Log("spawnResources.cs --: SpawnObjects() :-- While Loop Failed to find a Unique Position to spawn a resource!");
            }

            Resource addedResource = initResource(resourceSeed, randomPosition);
            asteroid.SpawnResource(addedResource, iter);
        }
    }

    //--------------------< Resource Methods > --------------------//
    public Resource initResource(int seed, Vector2 Pos)
    {
        switch (seed)
        {
            case 0:
                return new Iron(Pos);
            case 1:
                return new Nickel(Pos);
            case 2:
                return new Cobalt(Pos);
            case 3:
                return new Platinum(Pos);
            case 4:
                return new Gold(Pos);
            case 5:
                return new Technetium(Pos);
            case 6:
                return new Tungsten(Pos);
            case 7:
                return new Iridium(Pos);
            default:
                return new Iron(Pos);
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
    private int generateResourceSeed()
    {
        RandomNumGen generator = new RandomNumGen();
        switch (asteroid.asteroidClass)
        {
            case AsteroidClass.S_Class:
                double[] Sprobabilities = { 0.0, 0.0, 0.0, 0.10, 0.10, 0.10, 0.35, 0.35, 0.0 };
                return generator.GenerateBiasedNumber(Sprobabilities);

            case AsteroidClass.A_Class:
                double[] Aprobabilities = { 0.12, 0.12, 0.125, 0.125, 0.135, 0.125, 0.125, 0.125, 0.0 };
                return generator.GenerateBiasedNumber(Aprobabilities);

            case AsteroidClass.B_Class:
                double[] Bprobabilities = { 0.05, 0.05, 0.05, 0.30, 0.30, 0.20, 0.03, 0.02, 0.0 };
                return generator.GenerateBiasedNumber(Bprobabilities);

            case AsteroidClass.C_Class:
                double[] Cprobabilities = { 0.28, 0.24, 0.20, 0.1, 0.07, 0.05, 0.03, 0.03, 0.0 };
                return generator.GenerateBiasedNumber(Cprobabilities);

            case AsteroidClass.D_Class:
                //Spawns any Common resource with much lower chance of Plat or Gold, rarely exotics Technetium
                double[] Dprobabilities = { 0.32, 0.32, 0.15, 0.09, 0.05, 0.03, 0.02, 0.02, 0.0 };
                return generator.GenerateBiasedNumber(Dprobabilities);

            default:
                Debug.LogError("SpawnResources.cs --: generateResourceSeed() :-- Failed to recognize " + asteroid.asteroidClass);
                break;
        }
        return 0;
    }

    private int determineRandPosMethod(Rarity rarity)
    {
        switch (asteroid.asteroidClass)
        {
            case AsteroidClass.S_Class:
                getRandomPositionNoRestriction();
                break;
            case AsteroidClass.A_Class:
                getRand_Pos_OffRarity(rarity);
                break;
            case AsteroidClass.B_Class:

                break;
            case AsteroidClass.C_Class:

                break;
            case AsteroidClass.D_Class:

                break;
            default:
                Debug.LogError("SpawnResources.cs --: generateResourceSeed() :-- Failed to recognize " + asteroid.asteroidClass);
                break;
        }
        return 0;
    }

    //--------------------< Random Position Methods > --------------------//
    private Vector2 getRand_Pos_OffRarity(Rarity rarity)
    {
        float innerRestriction = 0f, outerRestriction = 1f;
        if (rarity == Rarity.Common)
        {
            innerRestriction = 0.70f;
            outerRestriction = .95f;
        }
        else if (rarity == Rarity.Commodity)
        {
            innerRestriction = 0.33f;
            outerRestriction = 0.68f;
        }
        else if (rarity == Rarity.Exotic)
        {
            innerRestriction = 0.0f;
            outerRestriction = 0.25f;
        }

        float r = spawnRadius * Mathf.Sqrt(Random.Range(innerRestriction, outerRestriction));
        //float theta = Random.Range(0.0f, 1.0f) * 2 * Mathf.PI;
        float theta = Random.Range(0.0f, 2 * Mathf.PI);

        float x = r * Mathf.Cos(theta);
        float y = r * Mathf.Sin(theta);

        Vector2 randPosition = new Vector2(x, y);
        return randPosition;
    }

    private Vector2 getRandomPositionNoRestriction()
    {
        float r = spawnRadius * Mathf.Sqrt(Random.Range(0.0f, 1.0f));
        float theta = Random.Range(0.0f, 1.0f) * 2 * Mathf.PI;

        float x = r * Mathf.Cos(theta);
        float y = r * Mathf.Sin(theta);

        Vector2 randPosition = new Vector2(x, y);
        return randPosition;
    }
}
