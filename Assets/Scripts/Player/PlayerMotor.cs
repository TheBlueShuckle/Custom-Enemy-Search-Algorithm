using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements.Experimental;
using UnityEngine.Windows;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private float defaultMoveSpeed;
    [SerializeField] private float defaultSprintSpeed;
    public bool IsSprinting;

    private float moveSpeed;

    private Cooldown movementCooldown;

    [SerializeField]
    private Tilemap groundTilemap;

    [SerializeField]
    private Tilemap collisionTilemap;

    private AnimationController animationController;

    //Temporary variables
    private Vector2 lastInput;
    private Vector2 lastMove;

    private Vector2 targetPosition;
    private Vector2 startPosition;

    private void Start()
    {
        moveSpeed = defaultMoveSpeed;

        movementCooldown = new Cooldown(moveSpeed);
        animationController = GetComponent<AnimationController>();

        targetPosition = transform.position;
    }

    private void Update()
    {
        if (transform.position != (Vector3)targetPosition)
        {
            transform.position = targetPosition;
        }

        if (movementCooldown.IsCoolingDown)
        {
            animationController.LerpSprite(startPosition, targetPosition, moveSpeed);
        }

        else
        {
            moveSpeed = UpdateMoveSpeed();

            movementCooldown.ChangeCooldownTime(moveSpeed);
            animationController.ResetLerp();
        }
    }

    private float UpdateMoveSpeed()
    {
        if (IsSprinting)
        {
            return defaultSprintSpeed;
        }

        return defaultMoveSpeed;
    }

    private void Move(Vector2 forcedInput)
    {
        if (!CanMove(forcedInput))
        {
            return;
        }

        startPosition = transform.position;
        targetPosition = transform.position + (Vector3)forcedInput;
        movementCooldown.StartCooldown();
    }

    public void ProcessHeldInput(Vector2 input)
    {
        if (movementCooldown.IsCoolingDown)
        {
            return;
        }

        Vector2 forcedInput;

        forcedInput = ForceOrthogonalMovement(input);

        if (input != Vector2.zero)
        {
            Move(forcedInput);
        }

        animationController.UpdateSprite(forcedInput);

        lastInput = input;
        lastMove = forcedInput;
    }

    public void ProcessTapInput(Vector2 input)
    {
        if (movementCooldown.IsCoolingDown)
        {
            return;
        }

        Vector2 forcedInput = ForceOrthogonalMovement(input);

        if (forcedInput == lastMove)
        {
            Move(forcedInput);
        }

        animationController.UpdateSprite(forcedInput);

        lastInput = input;
        lastMove = forcedInput;
    }

    private Vector2 ForceOrthogonalMovement(Vector2 input)
    {
        if (input.x == 0 || 
            input.y == 0 || 
            input == Vector2.zero)
        {
            return input;
        }

        if (input != lastInput)
        {
            if (lastInput.x != 0)
            {
                input.x = 0;
                input.y = (float)Math.Round(input.y, 0);
            }

            else
            {
                input.y = 0;
                input.x = (float)Math.Round(input.x, 0);
            }
        }

        else
        {
            input = lastMove;
        }

        return input;
    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);

        return !(!groundTilemap.HasTile(gridPosition) || collisionTilemap.HasTile(gridPosition));
    }
}
