using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CanvasManager : MonoBehaviour
{
    PlayerInputAction inputActions;
    GameObject crosshairs;

    bool shootMode = true;

    private void Awake()
    {
        inputActions = new PlayerInputAction();
        crosshairs = transform.GetChild(3).gameObject;
        crosshairs.SetActive(true);
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Lock.performed += OnRClik;
    }

    private void OnDisable()
    {
        inputActions.Player.Lock.performed -= OnRClik;

        inputActions.Player.Disable();
    }

    private void OnRClik(InputAction.CallbackContext context)
    {
        if (shootMode)
        {
            crosshairs.SetActive(true);
        }
        else 
        {
            crosshairs.SetActive(false);
        }
        shootMode = !shootMode;
    }
}
