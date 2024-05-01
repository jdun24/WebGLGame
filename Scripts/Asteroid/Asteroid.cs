using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using static UnityEditor.Progress;

public enum AsteroidClass
{
    S_Class, A_Class, B_Class, C_Class, D_Class, F_Class
}

public class Asteroid : MonoBehaviour
{
    public float size { get; set; }
    private GameObject prefab;
    private List<Resource> resourceList = new List<Resource>();
    public SpawnResources sr;
    public AsteroidClass asteroidClass { get; set; }
    [SerializeField] private SpawnResourceEvent spawnResourceEvent;
    public string positionTag { get; private set; } // To store the position part from the name

    // -------------------------------------------------------------------
    // Class

    private void Awake()
    {
        positionTag = ExtractPositionFromName(this.gameObject.name);
        if (string.IsNullOrEmpty(positionTag))
        {
            Debug.LogError("Asteroid name does not contain a valid position format: " + this.gameObject.name);
        }
    }

    private string ExtractPositionFromName(string name)
    {
        int underscoreIndex = name.IndexOf('_');
        if (underscoreIndex != -1 && underscoreIndex < name.Length - 1)
        {
            // Return the substring starting just after the underscore
            return name.Substring(underscoreIndex + 1);
        }
        return null; // Return null if no underscore is found or it's at the end of the string
    }

    public void InstantiateAsteroid(float Size, int numberOfResources, AsteroidClass asteroidClass, GameObject prefab)
    {
        // Set the size based on the average scale of the GameObject
        size = Size;
        this.asteroidClass = asteroidClass;
        this.prefab = prefab;
        if (prefab != null)
        {
            if (asteroidClass != AsteroidClass.F_Class)
            {
                sr = new SpawnResources(Size, numberOfResources, this);
            }
        }
        else
        {
            Debug.LogError("Prefab not assigned to Asteroid. Please assign the prefab to asteroid Manager in the Unity Editor.");
        }
    }

    public void SpawnResource(Resource addedResource, int iter)
    {
        GameObject newResourceObject = Instantiate(prefab, addedResource.Position + (Vector2)transform.position, Quaternion.identity);
        SetNewResourceObject(newResourceObject, addedResource, iter);
    }

    //Makes the resource the desired color
    public void SetNewResourceObject(GameObject newResourceObject, Resource addedResource, int iter)
    {
        SpriteRenderer spriteRenderer = newResourceObject.GetComponent<SpriteRenderer>();

        // Set color with alpha set to 0 to make the sprite invisible until discovered
        Color invisibleColor = addedResource.Color;
        invisibleColor.a = 0; // Set alpha to 0 for invisibility
        spriteRenderer.color = invisibleColor;

        //Debug.Log(this.name + " " + transform.parent.name);
        newResourceObject.transform.localScale = addedResource.depositSize;
        newResourceObject.transform.parent = this.transform;
        newResourceObject.name = $"{addedResource.Name}_" + iter;

        spawnResourceEvent.Raise(new packet.ResourceGameObjectPacket(newResourceObject, addedResource));
        resourceList.Add(addedResource);
    }

    //Funciton purely used by SpawnResources.cs --: spawnObjects() :-- here so we only ever need one resourceList per asteroid
    public bool IsValidPosition(Vector3 position, int i)
    {
        float minDistanceBetweenObjects = 7f;
        foreach (Resource resource in resourceList)
        {
            if (resource != null)
            {
                float distanceToResource = Vector2.Distance(resource.Position, position);
                //keeps resources from being too close to eachother, as well as outside of the asteroid in edge cases
                if (distanceToResource < minDistanceBetweenObjects)
                {
                    return false; // Position is too close to an existing resource
                }
                //Debug.Log($"Resource_{i} distance to {resource.resourceType}: {distanceToResource}");
            }
        }

        return true;
    }
}
