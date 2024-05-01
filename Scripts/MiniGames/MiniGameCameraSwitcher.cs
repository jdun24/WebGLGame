using UnityEngine;
using Cinemachine;

public class MiniGameCameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerCamera; // Variable to store the current active camera
    [SerializeField] private CinemachineVirtualCamera SatelliteCamera;
    [SerializeField] private CinemachineVirtualCamera RobotBuddyAlphaCamera;
    [SerializeField] private CinemachineVirtualCamera RobotBuddyBetaCamera;
    [SerializeField] private CinemachineVirtualCamera miniGameCamera; // Variable to store the mini game camera

    // Method to switch to mini game camera
    public void SwitchToMiniGameCamera()
    {
        // Store the current active camera and switch to mini game camera
        playerCamera.Priority = 0;
        SatelliteCamera.Priority = 0;
        RobotBuddyAlphaCamera.Priority = 0;
        RobotBuddyBetaCamera.Priority = 0;
        
        miniGameCamera.Priority = 100;
    }

    // Method to switch back from mini game camera
    public void SwitchBackFromMiniGameCamera()
    {
        // Switch back to the previously active camera
        switch(GameManager.Instance.GetCurrentControlState()){
            case Control.State.Player:
                playerCamera.Priority = 100;
            break;
            case Control.State.Satellite:
                SatelliteCamera.Priority = 100;
            break;
            case Control.State.RobotBuddyAlpha:
                RobotBuddyAlphaCamera.Priority = 100;
            break;
            case Control.State.RobotBuddyBeta:
                RobotBuddyBetaCamera.Priority = 100;
            break;
            default:
                Debug.LogWarning("[MiniGameCameraSwitcher] Current Control.State fell through all switch cases, setting player camera");
                playerCamera.Priority = 100;
                break;
        }
        
        miniGameCamera.Priority = 0;
    }
}
