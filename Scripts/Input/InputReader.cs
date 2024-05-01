
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputReader")]

public class InputReader : ScriptableObject,
    InputSystem.IGameplayActions,
    InputSystem.IPlayerActions,
    InputSystem.ISatelliteActions,
    // InputSystem.IUIActions,
    InputSystem.IRobotBuddyActions
{
    private InputSystem inputSystem;

    private Control.State lastControlState;
    private Control.State currentControlState;

    private Dictionary<Control.State, InputActionMap> stateToActionMap;

    public static InputReader Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        if (inputSystem == null)
        {
            inputSystem = new InputSystem();
            inputSystem.Gameplay.SetCallbacks(this);
            inputSystem.Player.SetCallbacks(this);
            inputSystem.Satellite.SetCallbacks(this);
            // inputSystem.UI.SetCallbacks(this);
            inputSystem.RobotBuddy.SetCallbacks(this);
            Debug.Log("Input System initialized and callbacks set.");
        }

        InitializeActionMaps();
        SetControlState(Control.State.Player);
    }

    private void InitializeActionMaps()
    {
        stateToActionMap = new Dictionary<Control.State, InputActionMap>
        {
            { Control.State.Player, inputSystem.Player },
            { Control.State.Satellite, inputSystem.Satellite },
            // { Control.State.UI, inputSystem.UI },
            { Control.State.RobotBuddyAlpha, inputSystem.RobotBuddy },
            { Control.State.RobotBuddyBeta, inputSystem.RobotBuddy }
        };

        EnableAllActionMaps();
    }

    private void EnableAllActionMaps()
    {
        inputSystem.Gameplay.Enable();
        inputSystem.Player.Enable();
        inputSystem.Satellite.Enable();
        // inputSystem.UI.Enable();
        inputSystem.RobotBuddy.Enable();
        // Debug.Log("All action maps have been enabled.");
    }

    public void SetControlState(Control.State targetState)
    {
        foreach (var state in stateToActionMap)
        {
            if (state.Key == targetState)
            {
                state.Value.Enable();
            }
            else
            {
                state.Value.Disable();
            }
        }

        if (targetState != Control.State.UI)
        {
            inputSystem.Gameplay.Enable();
        }
        else
        {
            inputSystem.Gameplay.Disable();
        }

        if (targetState == Control.State.RobotBuddyAlpha)
        {
            inputSystem.RobotBuddy.Enable();
        }

        lastControlState = currentControlState;
        currentControlState = targetState;
        Debug.Log($"Control state changed to: {currentControlState}");
    }

    // -------------------------------------------------------------------
    // Define events

    // [Header("Gameplay Events")]
    [SerializeField] private ControlStateEvent ControlStateUpdated;
    [SerializeField] private VoidEvent ZoomIn;
    [SerializeField] private VoidEvent ZoomOut;
    [SerializeField] private VoidEvent RotateCameraWithPlayer;

    [Header("Movement Events")]
    [SerializeField] private Vector2Event PlayerMove;
    [SerializeField] private BoolEvent PlayerJump;
    [SerializeField] private BoolEvent PlayerSprint;
    [SerializeField] private BoolEvent PlayerCrouch;
    [SerializeField] private BoolEvent PlayerInteract;
    [Header("Player UI Events")]
    [SerializeField] private VoidEvent PlayerBuildOverlay;
    [SerializeField] private VoidEvent PlayerBuildOverlayCycleLeft;
    [SerializeField] private VoidEvent PlayerBuildOverlayCycleRight;
    [SerializeField] private VoidEvent PlayerInventoryOverlay;
    [SerializeField] private VoidEvent PlayerFlashlightToggle;
    [SerializeField] private VoidEvent PlayerPickupRobot;
    [SerializeField] private VoidEvent PlayerObjectiveOverlay;
    [Header("Satellite Events")]
    [SerializeField] private Vector2Event SatelliteMove;
    [SerializeField] private BoolEvent SatelliteScan;
    [SerializeField] private VoidEvent SatelliteControlToggle;

    [Header("UI Events")]
    [SerializeField] private VoidEvent GamePause;
    [SerializeField] private VoidEvent GameResume;
    [Header("Robot Buddy")]
    [SerializeField] private Vector2Event RobotBuddyMove;
    [SerializeField] private BoolEvent RobotBuddyInteract;
    [SerializeField] private BoolEvent RobotJump;

    // -------------------------------------------------------------------
    // Gameplay action map

    public void OnSwitchControlState(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Control.State nextState = currentControlState;
            Debug.Log("");  // Consider adding a meaningful log message here

            GameObject currentAsteroid = AsteroidManager.Instance.GetCurrentAsteroid();

            // First, check if a robot can be switched to regardless of the player's location
            bool canSwitchToRobotAlpha = RobotManager.Instance.GetRobotAlphaBuilt() && !PlayerManager.Instance.GetIsCarryingRobotAlpha();
            bool canSwitchToRobotBeta = RobotManager.Instance.GetRobotBetaBuilt() && !PlayerManager.Instance.GetIsCarryingRobotBeta();

            switch (currentControlState)
            {
                case Control.State.Player:
                    // Prevent satellite control in a cave
                    if (currentAsteroid != null && !CaveManager.Instance.GetIsPlayerInCave() && !MiniGameManager.Instance.GetIsInMiniGame())
                    {
                        SatelliteData satelliteData = AsteroidManager.Instance.satelliteMap.ContainsKey(currentAsteroid.name) ? AsteroidManager.Instance.satelliteMap[currentAsteroid.name] : null;
                        if (satelliteData != null && satelliteData.isBuilt)
                        {
                            nextState = Control.State.Satellite;
                        }
                    }

                    // Allow switching to robot buddy if satellite switch was not done
                    if (nextState == Control.State.Player && canSwitchToRobotAlpha)
                    {
                        nextState = Control.State.RobotBuddyAlpha;
                    }
                    break;
                case Control.State.Satellite:
                    if (canSwitchToRobotAlpha)
                    {
                        nextState = Control.State.RobotBuddyAlpha;
                    }
                    else
                    {
                        nextState = Control.State.Player;
                    }
                    break;
                case Control.State.RobotBuddyAlpha:
                    if (canSwitchToRobotBeta)
                    {
                        nextState = Control.State.RobotBuddyBeta;
                    }
                    else
                    {
                        nextState = Control.State.Player;
                    }
                    break;
                case Control.State.RobotBuddyBeta:
                    nextState = Control.State.Player;
                    break;
            }

            if (nextState != currentControlState) // Only change state if different
            {
                SetControlState(nextState);
                ControlStateUpdated.Raise(currentControlState);
            }
        }
    }

    public void OnGamePause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            SetControlState(Control.State.UI);
            GamePause.Raise();
        }
    }

    public void OnZoomIn(InputAction.CallbackContext context)
    {
        if (currentControlState != Control.State.Player)
        {
            return;
        }

        if (context.phase == InputActionPhase.Performed)
        {
            ZoomIn.Raise();
        }
    }

    public void OnZoomOut(InputAction.CallbackContext context)
    {
        if (currentControlState != Control.State.Player)
        {
            return;
        }

        if (context.phase == InputActionPhase.Performed)
        {
            ZoomOut.Raise();
        }
    }

    public void OnRotateCameraWithPlayer(InputAction.CallbackContext context)
    {
        if (currentControlState == Control.State.Satellite)
        {
            return;
        }

        if (context.phase == InputActionPhase.Performed)
        {
            RotateCameraWithPlayer.Raise();
        }
    }

    // -------------------------------------------------------------------
    // Player action map

    public void OnPlayerMove(InputAction.CallbackContext context)
    {
        PlayerMove.Raise(context.ReadValue<Vector2>());
    }

    public void OnPlayerSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PlayerSprint.Raise(true);
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            PlayerSprint.Raise(false);
        }
    }

    public void OnPlayerJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PlayerJump.Raise(true);
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            PlayerJump.Raise(false);
        }
    }

    public void OnPlayerCrouch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PlayerCrouch.Raise(true);
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            PlayerCrouch.Raise(false);
        }
    }

    public void OnPlayerInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PlayerInteract.Raise(true);
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            PlayerInteract.Raise(false);
        }
    }

    public void OnPlayerBuildOverlay(InputAction.CallbackContext context)
    {
        PlayerBuildOverlay.Raise();
    }
    public void OnPlayerBuildOverlayCycleLeft(InputAction.CallbackContext context)
    {
        PlayerBuildOverlayCycleLeft.Raise();
    }
    public void OnPlayerBuildOverlayCycleRight(InputAction.CallbackContext context)
    {
        PlayerBuildOverlayCycleRight.Raise();
    }

    public void OnPlayerInventoryOverlay(InputAction.CallbackContext context)
    {
        PlayerInventoryOverlay.Raise();
    }
    public void OnPlayerFlashlight(InputAction.CallbackContext context){
        PlayerFlashlightToggle.Raise();
    }

    public void OnPlayerObjectiveOverlay(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed){
            PlayerObjectiveOverlay.Raise();
        }
    }
    public void OnPlayerPickupRobot(InputAction.CallbackContext context){
        if(context.phase == InputActionPhase.Performed){
            PlayerPickupRobot.Raise();
        }
    }
    // -------------------------------------------------------------------
    // Satellite action map

    public void OnSatelliteMove(InputAction.CallbackContext context)
    {
        SatelliteMove.Raise(context.ReadValue<Vector2>());
    }

    public void OnSatelliteScan(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            SatelliteScan.Raise(true);
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            SatelliteScan.Raise(false);
        }
    }

    public void OnSatelliteControlToggle(InputAction.CallbackContext context)
    {
        SatelliteControlToggle.Raise();
    }

    // -------------------------------------------------------------------
    // UI action map

    public void OnGameResume(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            SetControlState(lastControlState);
            GameResume.Raise();
        }
    }

    // -------------------------------------------------------------------
    // RobotBuddy action map

    public void OnRobotBuddyMove(InputAction.CallbackContext context)
    {
        RobotBuddyMove.Raise(context.ReadValue<Vector2>());
    }

    public void OnRobotBuddyInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            RobotBuddyInteract.Raise(true);
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            RobotBuddyInteract.Raise(false);
        }
    }

    public void OnRobotJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            RobotJump.Raise(true);
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            RobotJump.Raise(false);
        }
    }
}
