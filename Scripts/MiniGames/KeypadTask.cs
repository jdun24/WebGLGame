using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeypadTask : MonoBehaviour
{
    public TextMeshProUGUI cardCode;

    public TextMeshProUGUI inputCode;

    public int codeLength = 5;

    public float codeResetTimeInSeconds = 0.5f;

    private bool isResetting = false;

    public BoolEvent winCondition;

    private void OnEnable()
    {
        string code = string.Empty;

        for (int i = 0; i < codeLength; i++)
        {
            code += Random.Range(1, 10);
        }

        cardCode.text = code;
        inputCode.text = string.Empty;
    }

    public void ButtonClick(int number)
    {
        SoundFXManager.Instance.PlaySound(SFX.UI.Click, this.transform, 1f);

        if (isResetting) { return; }

            inputCode.text += number;

        if (inputCode.text == cardCode.text)
        {
            inputCode.text = "Correct";

            SoundFXManager.Instance.PlaySound(SFX.MiniGame.Won, this.transform, 0.5f);

            StartCoroutine(CompleteTaskWithDelay());
        }
        else if (inputCode.text.Length >= codeLength)
        {
            inputCode.text = "failed";

            SoundFXManager.Instance.PlaySound(SFX.MiniGame.Error, this.transform, 1f);

            StartCoroutine(ResetCode());
        }
    }

    private IEnumerator CompleteTaskWithDelay()
    {
        yield return new WaitForSeconds(0.5f); // Delay for 1 second

        winCondition.Raise(true);
    }


    private IEnumerator ResetCode()
    {
        isResetting = true;

        yield return new WaitForSeconds(codeResetTimeInSeconds);

        inputCode.text = string.Empty;

        isResetting = false;
    }

}
