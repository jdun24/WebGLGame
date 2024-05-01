using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Reflection;
using System.Linq;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] GameObject objectiveOverlay;
    [SerializeField, ReadOnly] private bool isOverlayActive = true;

    private void Start()
    {
        // isOverlayActive = true;
        isOverlayActive = false;
        objectiveOverlay.SetActive(isOverlayActive);
    }

    // -------------------------------------------------------------------
    // Handle events

    public void OnPlayerObjectiveOverlay()
    {
        isOverlayActive = !isOverlayActive;
        objectiveOverlay.SetActive(isOverlayActive);
    }

    // -------------------------------------------------------------------

    
}
