using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Scripting.APIUpdating;

public class InputHandler : MonoBehaviour
{
    private PlayerControls playerControls;

    private PlayerMotor motor;
    private bool moveHeld;

    private void Awake()
    {
        playerControls = new PlayerControls();
        motor = GetComponent<PlayerMotor>();

        playerControls.OnFoot.Move.started += ctx =>
        {
            if (ctx.interaction is TapInteraction)
            {
                print("YEs");
                moveHeld = false;
                motor.ProcessTurn(playerControls.OnFoot.Move.ReadValue<Vector2>());
            }


            else
            {
                print("wtf");
                moveHeld = true;
            }

        };
    }

    private void Update()
    {
        if (moveHeld)
        {
            motor.ProcessMove(playerControls.OnFoot.Move.ReadValue<Vector2>());
        }
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
