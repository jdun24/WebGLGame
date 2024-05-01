using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConditionChecker : MonoBehaviour
{
    public BoolEvent winCondition;
    public GameObject[] lights;

    public void winCheck()
    {
        // Check if all lights are turned on
        bool allLightsOn = CheckAllLightsOn();

        if (allLightsOn)
        {
            Debug.Log("MiniGame Won");

            SoundFXManager.Instance.PlaySound(SFX.MiniGame.Won, this.transform, 0.5f);

            StartCoroutine(CompleteTaskWithDelay());

            //winCondition.Raise(true);
            // Implement your win condition actions here
        }
    }

    private bool CheckAllLightsOn()
    {
        foreach (GameObject light in lights)
        {
            if (light != null && !light.activeSelf)
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator CompleteTaskWithDelay()
    {
        yield return new WaitForSeconds(0.5f); // Delay for 1 second

        winCondition.Raise(true);
    }
}
