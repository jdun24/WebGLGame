using UnityEngine;
using UnityEngine.UI;

public class RobotUIBuddyManager : MonoBehaviour
{
    [SerializeField] private GameObject alphaUIOverlay;
    [SerializeField] private GameObject betaUIOverlay;
    [SerializeField] private Image chargeFillAlphaMeter;
    [SerializeField] private Image chargeFillBetaMeter;
    private Control.State currentState = Control.State.Player;
    private readonly float fullFillAmt = 1;
    private readonly float maxChargeAmt = 600.0f;
    private float alphaFillAmt = 1;
    private float betaFillAmt = 1;
    public void Start()
    {
        chargeFillAlphaMeter.enabled = false;
        chargeFillBetaMeter.enabled = false;
    }

    public void OnControlStateUpdated(Control.State newState)
    {
        currentState = newState;
        UpdateUIOnState();
    }
    public void OnAdjustRobotUI(packet.RobotUIPacket packet)
    {
        float clampVal = Mathf.Clamp(packet.newCharge / maxChargeAmt, 0f, 1f);
        //Debug.Log($"setting UI to: {charge}\tclamped: {clampVal}");
        if (packet.robotToChange == Control.State.RobotBuddyAlpha)
        {
            chargeFillAlphaMeter.fillAmount = clampVal;
        }
        else if (packet.robotToChange == Control.State.RobotBuddyBeta)
        {
            chargeFillBetaMeter.fillAmount = clampVal;
        }
    }

    private void UpdateUIOnState()
    {
        if (currentState == Control.State.RobotBuddyAlpha)
        {
            alphaUIOverlay.SetActive(true);
            betaUIOverlay.SetActive(false);
        }
        else if (currentState == Control.State.RobotBuddyBeta)
        {
            betaUIOverlay.SetActive(true);
            alphaUIOverlay.SetActive(false);
        }
        else
        {
            alphaUIOverlay.SetActive(true);
            betaUIOverlay.SetActive(true);
        }
    }

    public void OnBuildRobotAlpha()
    {
        chargeFillAlphaMeter.enabled = true;
    }
    public void OnBuildRobotBeta()
    {
        chargeFillBetaMeter.enabled = true;
    }
    private float Clamp01(float val)
    {
        return val / maxChargeAmt;
    }
}
