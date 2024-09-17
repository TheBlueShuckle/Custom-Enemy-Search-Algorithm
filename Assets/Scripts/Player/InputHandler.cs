using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class InputHandler : MonoBehaviour
{
    private PlayerControls playerControls;

    private PlayerMotor motor;

    private void Awake()
    {
        playerControls = new PlayerControls();
        motor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        motor.ProcessMove(playerControls.OnFoot.Move.ReadValue<Vector2>());
    }

    #region - Enable / Disable
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    #endregion
}
