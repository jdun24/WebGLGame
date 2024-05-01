using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackLayers : MonoBehaviour
{
    public GameObject[] crackLayers;
    public BoolEvent WinConditionEvent;
    int index;

    int clickTarget;
    int clickCounter;
    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        clickTarget = UnityEngine.Random.Range(2, 4);
        clickCounter = 0;
        crackLayers[0].SetActive(true);
        crackLayers[1].SetActive(false);
        crackLayers[2].SetActive(false);
        crackLayers[3].SetActive(false);
        crackLayers[4].SetActive(false);
        crackLayers[5].SetActive(false);
    }

    private void OnMouseUp()
    {
        SoundFXManager.Instance.PlaySound(SFX.Cave.MineResource, this.transform, 1f);
        clickCounter += 1;
        if (clickCounter == clickTarget)
        {

            if (index < crackLayers.Length - 1)
            {
                index += 1;
                if (index == crackLayers.Length - 1)
                {
                    SoundFXManager.Instance.PlaySound(SFX.MiniGame.Won, this.transform, 0.5f);

                    MiniGameManager.Instance.SetIsInMiniGame(false);

                    //WinConditionEvent.Raise(true);

                    StartCoroutine(CompleteTaskWithDelay());
                }
            }

            clickCounter = 0;
            clickTarget = UnityEngine.Random.Range(3, 6);
            crackLayers[index - 1].SetActive(false);
            crackLayers[index].SetActive(true);

        }
    }

    private IEnumerator CompleteTaskWithDelay()
    {
        yield return new WaitForSeconds(0.25f); // Delay for 1 second

        WinConditionEvent.Raise(true);
    }
}
