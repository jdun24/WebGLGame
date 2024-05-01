using System.Collections;
using UnityEngine;
using Cinemachine;

public class RobotManager : MonoBehaviour
{
    public static RobotManager Instance { get; private set; }

    [Header("Events")]

    [Header("Mutable")]

    [Header("ReadOnly")]
    [SerializeField, ReadOnly] private bool hasBuiltAlpha = false;
    [SerializeField, ReadOnly] private bool hasBuiltBeta = false;
    // Alpha
    [SerializeField, ReadOnly] private GameObject robotAlphaObject;
    [SerializeField, ReadOnly] private bool robotAlphaGrounded;
    [SerializeField, ReadOnly] private Vector2 robotAlphaPosition;
    // Beta
    [SerializeField, ReadOnly] private GameObject robotBetaObject;
    [SerializeField, ReadOnly] private bool robotBetaGrounded;
    [SerializeField, ReadOnly] private Vector2 robotBetaPosition;

    // Not for display
    private RobotBuddyController robotController;
    private UnityEngine.Vector3 lastCoordinates = Vector3.zero;
    private Quaternion lastRotation = Quaternion.Euler(0,0,0);

    // -------------------------------------------------------------------
    // Handle events

    // ALPHA
    public void OnBuildRobotAlpha()
    {
        robotAlphaObject = GameObject.Find("RobotBuddyAlpha");
        if (robotAlphaObject == null)
        {
            Debug.Log("[RobotManager]: robotAlpha not found in game");
            return;
        }

        SoundFXManager.Instance.PlaySound(SFX.Robot.Status.Initialize, robotAlphaObject.transform, 1f);

        hasBuiltAlpha = true;
    }

    public void OnRobotAlphaGrounded(bool state)
    {
        robotAlphaGrounded = state;
        // Debug.Log("[RobotManager]: robotAlphaGrounded: " + state);
    }

    public void OnRobotAlphaPositionUpdated(Vector2 position)
    {
        robotAlphaPosition = position;
        // Debug.Log("[GameManager]: robotAlphaPosition: " + position);
    }

    // BETA
    public void OnBuildRobotBeta()
    {
        robotBetaObject = GameObject.Find("RobotBuddyBeta");
        if (robotBetaObject == null)
        {
            Debug.Log("[RobotManager]: robotBetaObject not found in game");
            return;
        }

        SoundFXManager.Instance.PlaySound(SFX.Robot.Status.Initialize, robotAlphaObject.transform, 1f);

        hasBuiltBeta = true;
    }

    public void OnRobotBetaGrounded(bool state)
    {
        robotBetaGrounded = state;
        // Debug.Log("[RobotManager]: robotBetaGrounded: " + state);
    }

    public void OnRobotBetaPositionUpdated(Vector2 position)
    {
        robotBetaPosition = position;
        // Debug.Log("[GameManager]: robotBetaPosition: " + position);
    }

    // void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     SceneChange(scene);
    // }

    // void OnSceneUnloaded(Scene scene)
    // {
    //     SceneChange(SceneManager.GetSceneByName("AsteroidScene"));
    // }

    // -------------------------------------------------------------------
    // API

    // ALPHA
    public bool GetRobotAlphaBuilt()
    {
        return hasBuiltAlpha;
    }

    public GameObject GetRobotAlphaObject()
    {
        return robotAlphaObject;
    }

    public bool GetRobotAlphaGrounded()
    {
        return robotAlphaGrounded;
    }

    public Vector2 GetRobotAlphaPosition()
    {
        return robotAlphaPosition;
    }

    // BETA
    public bool GetRobotBetaBuilt()
    {
        return hasBuiltBeta;
    }

    public GameObject GetRobotBetaObject()
    {
        return robotBetaObject;
    }

    public bool GetRobotBetaGrounded()
    {
        return robotBetaGrounded;
    }

    public Vector2 GetRobotBetaPosition()
    {
        return robotBetaPosition;
    }

    // -------------------------------------------------------------------
    // Class

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

    void OnEnable()
    {
        // SceneManager.sceneLoaded += OnSceneLoaded;
        // SceneManager.sceneUnloaded += OnSceneUnloaded;
    }


    void OnDisable()
    {
        // SceneManager.sceneLoaded -= OnSceneLoaded;
        // SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

}
