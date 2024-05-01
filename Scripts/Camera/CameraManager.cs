using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [Header("Events")]

    [Header("Mutable")]
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera satelliteCamera;
    [SerializeField] private CinemachineVirtualCamera robotBuddyAlphaCamera;
    [SerializeField] private CinemachineVirtualCamera robotBuddyBetaCamera;

    [Header("ReadOnly")]
    [SerializeField, ReadOnly] private GameObject currentObject;
    [SerializeField, ReadOnly] private CinemachineVirtualCamera currentCamera;
    [SerializeField, ReadOnly] private bool rotateCameraWithPlayer = true;

    // Not for display
    private float cameraRotationSpeed = 2.5f;
    private float minZoom = 10f; // Minimum zoom level
    private float maxZoom = 30f; // Maximum zoom level
    private float zoomSpeed = 2f; // Speed of zoom transition
    private float zoomIncrement = 1f; // Amount to zoom on each scroll
    private float targetZoom; // Target zoom level
    private Quaternion targetRotation;

    // -------------------------------------------------------------------
    // Handle events

    public void OnRotateCameraWithPlayer()
    {
        rotateCameraWithPlayer = !rotateCameraWithPlayer;

        // Set the target rotation based on the new state
        if (rotateCameraWithPlayer)
        {
            // Target the player's rotation
            if (currentObject != null)
            {
                targetRotation = currentObject.transform.rotation;
            }
        }
        else
        {
            // Target the upright rotation (world up)
            targetRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void OnZoomIn()
    {
        Zoom(-zoomIncrement);
    }

    public void OnZoomOut()
    {
        Zoom(+zoomIncrement);
    }

    public void OnControlStateUpdated(Control.State controlState)
    {
        switch (controlState)
        {
            case Control.State.Player:
                ActivateCamera(playerCamera, PlayerManager.Instance.GetPlayerObject());
                break;
            case Control.State.Satellite:
                GameObject satellite = SatelliteManager.Instance.GetCurrentSatelliteObject();
                GameObject asteroid = AsteroidManager.Instance.GetCurrentAsteroid();
                ActivateSatelliteCamera(satellite, asteroid);
                break;
            case Control.State.RobotBuddyAlpha:
                if (!PlayerManager.Instance.GetIsCarryingRobotAlpha())
                    ActivateCamera(robotBuddyAlphaCamera, RobotManager.Instance.GetRobotAlphaObject());
                break;
            case Control.State.RobotBuddyBeta:
                if (!PlayerManager.Instance.GetIsCarryingRobotBeta())
                    ActivateCamera(robotBuddyBetaCamera, RobotManager.Instance.GetRobotBetaObject());
                break;
        }
    }

    // -------------------------------------------------------------------
    // Base

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

        currentCamera = playerCamera;
        currentObject = PlayerManager.Instance.GetPlayerObject();

        SetInitialZoomLevel(10f);
    }

    private void FixedUpdate()
    {
        if (currentCamera != null)
        {
            if (rotateCameraWithPlayer && currentObject != null)
            {
                // Slerp to the player's rotation
                targetRotation = currentObject.transform.rotation;
            }

            // Always Slerp towards the target rotation whether it's the player's or upright
            Quaternion currentRotation = currentCamera.transform.rotation;
            Quaternion smoothRotation = Quaternion.Slerp(currentRotation, targetRotation, cameraRotationSpeed * Time.deltaTime);

            currentCamera.transform.rotation = smoothRotation;
        }
    }

    void Update()
    {
        playerCamera.m_Lens.OrthographicSize = Mathf.SmoothDamp(playerCamera.m_Lens.OrthographicSize, targetZoom, ref zoomSpeed, 0.1f);
    }

    // -------------------------------------------------------------------
    // Functions

    private void ActivateSatelliteCamera(GameObject satellite, GameObject asteroid)
    {
        ActivateCamera(satelliteCamera, satellite, satellite.transform);
        if (asteroid != null)
        {
            // Calculate the 2D distance between the satellite and the asteroid's center
            float distance = Vector2.Distance(new Vector2(satellite.transform.position.x, satellite.transform.position.y),
                                              new Vector2(asteroid.transform.position.x, asteroid.transform.position.y));

            // Calculate additional buffer for the top side to keep the satellite in frame
            float additionalBuffer = distance * 0.05f;  // 5% of the distance as buffer

            // Set the camera's tracked object offset to center the view
            var transposer = satelliteCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            transposer.m_TrackedObjectOffset.y = -(distance / 2 + additionalBuffer / 2); // Offset slightly more towards the top

            // Adjust the orthographic size of the camera based on the distance and additional buffer
            satelliteCamera.m_Lens.OrthographicSize = (distance / 2) + additionalBuffer;
        }
        else
        {
            Debug.LogError("Asteroid object is null. Unable to set camera properties.");
        }
    }

    private void ActivateCamera(CinemachineVirtualCamera camera, GameObject targetObject, Transform followTransform = null)
    {
        SetAllCamerasLowPriority();

        camera.Priority = 100;
        currentCamera = camera;
        currentObject = targetObject;

        if (followTransform != null)
        {
            camera.Follow = followTransform;
            camera.LookAt = followTransform;
        }
    }

    private void SetAllCamerasLowPriority()
    {
        playerCamera.Priority = 0;
        satelliteCamera.Priority = 0;
        robotBuddyAlphaCamera.Priority = 0;
        robotBuddyBetaCamera.Priority = 0;
    }

    void Zoom(float increment)
    {

        playerCamera.m_Lens.OrthographicSize = Mathf.Clamp(playerCamera.m_Lens.OrthographicSize + increment, minZoom, maxZoom);
        targetZoom = Mathf.Clamp(playerCamera.m_Lens.OrthographicSize + increment, minZoom, maxZoom);
    }

    public void SetInitialZoomLevel(float initialZoom)
    {
        if (initialZoom >= minZoom && initialZoom <= maxZoom)
        {
            if (playerCamera != null)
            {
                playerCamera.m_Lens.OrthographicSize = initialZoom;
                targetZoom = initialZoom;
            }
        }
    }
}
