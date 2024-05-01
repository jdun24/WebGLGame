using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance { get; private set; }

    public List<GameObject> prefabsToChooseFrom;

    public GameObject caveMiniGame;
    public GameObject parentObject; // Parent GameObject to contain the instantiated prefab

    //private Vector3 desiredScale = new Vector3(2f, 2f, 2f);

    private GameObject instantiatedPrefab; // Hold reference to the instantiated prefab
    public MiniGameCameraSwitcher cameraSwitcher;

    public VoidEvent isFixedEvent;

    public VoidEvent isCollected;

    public GameObject ExtractorTaskBG;

    public GameObject CaveTaskBG;

    private bool isInMiniGame = false;

    public bool GetIsInMiniGame()
    {
        return isInMiniGame;
    }

    public void SetIsInMiniGame(bool state)
    {
        isInMiniGame = state;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Method to generate and place a prefab inside another object
    public void GenerateAndPlacePrefab()
    {
        if (prefabsToChooseFrom.Count == 0)
        {
            Debug.LogError("No prefabs assigned to the list.");
            return;
        }

        // Check if there's already an instantiated prefab
        if (instantiatedPrefab != null)
        {
            Debug.LogWarning("Prefab already instantiated. Cannot generate another prefab until the current one is destroyed.");
            return;
        }

        ExtractorTaskBG.SetActive(true);

        // Randomly choose a prefab from the list
        GameObject prefabToInstantiate = prefabsToChooseFrom[Random.Range(0, prefabsToChooseFrom.Count)];

        // Instantiate the chosen prefab inside the parent GameObject
        instantiatedPrefab = Instantiate(prefabToInstantiate, parentObject.transform);

        // Set the scale of the instantiated prefab
       //instantiatedPrefab.transform.localScale = desiredScale;

        // Switch camera to mini game camera
        cameraSwitcher.SwitchToMiniGameCamera();
    }

    // Method to destroy the instantiated prefab
    public void DestroyInstantiatedPrefab()
    {
        if (instantiatedPrefab != null)
        {
            // Switch back to the player camera
            isFixedEvent.Raise();

            cameraSwitcher.SwitchBackFromMiniGameCamera();

            // Destroy the instantiated prefab
            Destroy(instantiatedPrefab);
            ExtractorTaskBG.SetActive(false);
            // Set the reference to null since the prefab is destroyed
            instantiatedPrefab = null;
        }
        else
        {
            Debug.LogWarning("No instantiated prefab to destroy.");
        }
    }

    public void GenerateAndPlaceCaveMiniGamePrefab(){

        CaveTaskBG.SetActive(true);

         // Randomly choose a prefab from the list
        GameObject prefabToInstantiate = caveMiniGame;

        // Instantiate the chosen prefab inside the parent GameObject
        instantiatedPrefab = Instantiate(prefabToInstantiate, parentObject.transform);

        SetIsInMiniGame(true);

        // Set the scale of the instantiated prefab
        //instantiatedPrefab.transform.localScale = desiredScale;

        // Switch camera to mini game camera
        cameraSwitcher.SwitchToMiniGameCamera();
    }

    public void DestroyInstantiatedCaveMiniGamePrefab()
    {
        if (instantiatedPrefab != null)
        {


            // Switch back to the player camera
            isCollected.Raise();

            cameraSwitcher.SwitchBackFromMiniGameCamera();

            // Destroy the instantiated prefab
            Destroy(instantiatedPrefab);
            CaveTaskBG.SetActive(false);

            // Set the reference to null since the prefab is destroyed
            instantiatedPrefab = null;
        }
        else
        {
            Debug.LogWarning("No instantiated prefab to destroy.");
        }
    }
}
