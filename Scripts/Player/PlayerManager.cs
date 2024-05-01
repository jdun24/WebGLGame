using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cinemachine;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [Header("Events")]

    [Header("Mutable")]
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject CyberneticToolObject;
    [SerializeField] private GameObject MainSpawn;

    [Header("ReadOnly")]
    [SerializeField, ReadOnly] private bool playerGrounded;
    [SerializeField, ReadOnly] private Vector2 playerPosition;
    [SerializeField, ReadOnly] private bool playerInteracting;

    // Not for display
    private PlayerController playerController;
    private UnityEngine.Vector3 lastCoordinates = Vector3.zero;
    private Quaternion lastRotation = Quaternion.Euler(0,0,0);
    [SerializeField] private GameObject playerUIObject;
    private bool isCarryingRobotAlpha = false;
    private bool isCarryingRobotBeta = false;
    private float currentJumpForce = 3.0f;
    private float initJumpForce = 3.0f;
    private float improvedJumpForce = 7.5f;
    //private PlayerUIManager playerUIManager;

    // -------------------------------------------------------------------
    // Handle events

    public void OnPlayerGrounded(bool state)
    {
        playerGrounded = state;
        // Debug.Log("[GameManager]: playerGrounded: " + state);
    }

    public void OnPlayerPositionUpdated(Vector2 position)
    {
        playerPosition = position;
        //Debug.Log("[GameManager]: playerPosition: " + position);
    }

    public void OnPlayerInteracting(bool interacting)
    {
        playerInteracting = interacting;
    }

    public void OnTechUpEvent(packet.TechUpPacket packet)
    {
        if (packet.building == BuildingComponents.BuildingType.Exosuit && packet.TechToLevel == 1)
        {
            currentJumpForce = improvedJumpForce;
            playerController.UpdateJumpForce(currentJumpForce);
        }
    }

    // -------------------------------------------------------------------
    // API

    public bool GetIsCarryingRobotAlpha()
    {
        return isCarryingRobotAlpha;
    }

    public bool GetIsCarryingRobotBeta()
    {
        return isCarryingRobotBeta;
    }

    public GameObject GetPlayerObject()
    {
        return playerObject;
    }

    public bool GetPlayerGrounded()
    {
        return playerGrounded;
    }

    public Vector2 GetPlayerPosition()
    {
        return playerPosition;
    }

    public bool GetPlayerInteracting()
    {
        return playerInteracting;
    }

    public Vector3 GetLastCoordinates()
    {
        return lastCoordinates;
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
        playerController = playerObject.GetComponent<PlayerController>();
    }

    // set the players initial position and jump force
    public void SetScenePosition(string SceneName, [Optional] Transform SpawnLocation)
    {
        // put the player but to their original position if they come back to the asteroid scene
        switch (SceneName)
        {
            case "AsteroidScene":
                if (this.lastCoordinates != Vector3.zero && this.lastRotation != Quaternion.Euler(0,0,0))
                {
                    //Debug.Log(1);
                    this.playerObject.transform.position = this.lastCoordinates;
                    this.playerObject.transform.rotation = this.lastRotation;
                    this.playerController.UpdateJumpForce(currentJumpForce);
                }
                else
                {
                    // put the player to the default spawn position in the scene
                    //Debug.Log(2);
                    this.playerObject.transform.position = MainSpawn.transform.position;
                    this.playerObject.transform.rotation = Quaternion.Euler(0,0,0);
                    this.playerController.UpdateJumpForce(currentJumpForce);
                }
                break;

            default:
                // put the player to the default spawn position in the scene
                if (SpawnLocation != null)
                {
                    //Debug.Log(3);
                    this.playerObject.transform.position = SpawnLocation.position;
                    this.playerObject.transform.rotation = Quaternion.Euler(0,0,0);
                    this.playerController.UpdateJumpForce(0.5f);
                }
                break;
        }
    }

    // set the players last known coordinates
    public void SetLastPosition()
    {
        this.lastCoordinates = playerObject.transform.position;
        this.lastRotation = playerObject.transform.rotation;
    }
    public void SetCarryingAlpha(bool set){
        isCarryingRobotAlpha = set;
    }
    public void SetCarryingBeta(bool set){
        isCarryingRobotBeta = set;
    }
}
