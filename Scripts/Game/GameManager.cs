using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // [Header("Events")]

    [Header("Mutable")]
    public InputReader inputReader;

    [Header("ReadOnly")]
    [SerializeField, ReadOnly] private Control.State currentControlState;

    // Not for display

    // -------------------------------------------------------------------
    // Handle events

    public void OnControlStateUpdated(Control.State controlState)
    {
        currentControlState = controlState;
    }

    public void OnGamePause()
    {
        // pauseMenu.SetActive(true);
        // Debug.Log("[GameManager]: Game paused");
    }

    public void OnGameResume()
    {
        // pauseMenu.SetActive(false);
        // Debug.Log("[GameManager]: Game resumed");
    }

    // -------------------------------------------------------------------
    // API

    public Control.State GetCurrentControlState()
    {
        return currentControlState;
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

        if (inputReader != null)
        {
            // Debug.Log("InputReader has been loaded.");
        }
        else
        {
            Debug.LogError("InputReader is not assigned in the GameManager.");
        }
    }

    public void Start()
    {
        SoundFXManager.Instance.PlayRandomSoundOfType(typeof(SFX.Music.Asteroid), PlayerManager.Instance.GetPlayerObject().transform, 0.5f, 1f);
    }
}
