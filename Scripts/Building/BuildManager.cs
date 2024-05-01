using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }
    [Header("Industry")]
    [SerializeField] public GameObject ExtractorPrefab;
    [SerializeField] public GameObject CommercialExtractorPrefab;
    [SerializeField] public GameObject IndustrialExtractorPrefab;

    [Header("Robotics")]
    [SerializeField] public GameObject RobotBuddyAlphaObject;
    private bool hasBuiltRobotBuddyAlpha = false;
    [SerializeField] public GameObject RobotBuddyBetaObject;
    private bool hasBuiltRobotBuddyBeta = false;
    [SerializeField] public GameObject LaunchPadPrefab;
    private bool hasBuiltLaunchPad = false;

    [Header("Events")]
    public VoidEvent satelliteSpawn;
    [SerializeField] private VoidEvent buildRobotBuddyAlpha;
    [SerializeField] private VoidEvent buildRobotBuddyBeta;

    [Header("Mutable")]
    [SerializeField] private GameObject buildUIObject;
    private UIBuildManager buildUI;
    [SerializeField, ReadOnly] private bool hasBuiltExosuit = false;
    [SerializeField, ReadOnly] private bool hasBuiltJetpack = false;

    void Awake()
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
        if (satelliteSpawn == null)
        {
            satelliteSpawn = ScriptableObject.CreateInstance<VoidEvent>();
        }
        buildUI = buildUIObject.GetComponent<UIBuildManager>();
    }
    public void OnBuildObjEvent(BuildingComponents.BuildingType type){
        //Turns off Build overlay after an obj has been bought
        if(buildUI.IsActive() == true){
            buildUI.OnPlayerBuildOverlay();
        }

        switch(type){
            case BuildingComponents.BuildingType.Extractor:
                SpawnNewEntity(ExtractorPrefab);
                return;
            case BuildingComponents.BuildingType.CommercialExtractor:
                SpawnNewEntity(CommercialExtractorPrefab);
                return;
            case BuildingComponents.BuildingType.IndustrialExtractor:
                SpawnNewEntity(IndustrialExtractorPrefab);
                return;
            case BuildingComponents.BuildingType.Exosuit:
                    hasBuiltExosuit = true;
                return;
            case BuildingComponents.BuildingType.JetPack:
                    hasBuiltJetpack = true;
                return;
            case BuildingComponents.BuildingType.Cybernetics:
                    CyberneticsManager.Instance.SetCyberneticsBuilt();
                return;
            case BuildingComponents.BuildingType.RobotBuddy:
                if(hasBuiltRobotBuddyAlpha == false){
                    TeleportRobotBuddy(RobotBuddyAlphaObject);
                    hasBuiltRobotBuddyAlpha = true;
                    buildRobotBuddyAlpha.Raise();
                }else if(hasBuiltRobotBuddyBeta == false){
                    TeleportRobotBuddy(RobotBuddyBetaObject);
                    hasBuiltRobotBuddyBeta = true;
                    buildRobotBuddyBeta.Raise();
                }
                return;
            case BuildingComponents.BuildingType.Satellite:
                satelliteSpawn.Raise();
                return;
            case BuildingComponents.BuildingType.LaunchPad:
                    if(hasBuiltLaunchPad == false){
                        SpawnNewEntity(LaunchPadPrefab);
                        hasBuiltLaunchPad = true;
                    }
                return;
        }
    }
    public void SpawnNewEntity(GameObject prefab)
    {
        Vector3 screenPos = new Vector3(375, 285, 10f);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Instantiate(prefab, worldPos, Quaternion.identity);
    }

    public void TeleportRobotBuddy(GameObject robotBuddy){
        Vector3 screenPos = new Vector3(475, 285, 0f);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;
        robotBuddy.transform.position = worldPos;
    }


    //API
    public bool HasBuiltExosuit(){
        return hasBuiltExosuit;
    }
    public bool HasBuiltJetpack(){
        return hasBuiltJetpack;
    }
}
