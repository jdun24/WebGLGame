using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    private bool isInRange;
    private KeyCode interactKey = KeyCode.E;
    public TextMeshProUGUI reminderText;

    // Update is called once per frame
    void Update()
    {
        if (isInRange && Input.GetKeyDown(interactKey))
        {
            string CaveScene = gameObject.tag;
            Transform CaveSpawn = CaveManager.Instance.EnterCaveScene(CaveScene);
            PlayerManager.Instance.SetLastPosition();
            PlayerManager.Instance.SetScenePosition(CaveScene, CaveSpawn);

            SoundFXManager.Instance.PlaySound(SFX.Cave.Enter, PlayerManager.Instance.GetPlayerObject().transform, 1f);

            SoundFXManager.Instance.StopSoundsOfType(typeof(SFX.Music.Asteroid));
            SoundFXManager.Instance.PlayRandomSoundOfType(typeof(SFX.Music.Cave), PlayerManager.Instance.GetPlayerObject().transform, 0.5f, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerManager.Instance.GetPlayerObject())
        {
            isInRange = true;
            reminderText.text = "Press E to enter Cave!";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerManager.Instance.GetPlayerObject())
        {
            isInRange = false;
            reminderText.text = "";
        }
    }
}
