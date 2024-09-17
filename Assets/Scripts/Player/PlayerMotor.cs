using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements.Experimental;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float bufferTime;

    [SerializeField]
    private float bufferCooldownTime;

    private Cooldown movementCooldown;
    private MovementBuffer movementBuffer;

    private Cooldown bufferCooldown;

    [SerializeField]
    private Tilemap groundTilemap;

    [SerializeField]
    private Tilemap collisionTilemap;

    //Temporary variables
    private Vector2 lastInput;
    private Vector2 lastMove;

    private void Start()
    {
        movementCooldown = new Cooldown(moveSpeed);
        bufferCooldown = new Cooldown(bufferCooldownTime);
    }

    private void Update()
    {
        if (movementBuffer != null && !movementBuffer.BufferHasTimeOut && !movementCooldown.IsCoolingDown)
        {
            ProcessMove(movementBuffer.GetBuffer());
            movementBuffer = null;
        }
    }

    public void ProcessMove(Vector2 input)
    {
        Vector2 forcedInput;

        forcedInput = ForceOrthogonalMovement(input);

        if (movementCooldown.IsCoolingDown)
        {
            if (!bufferCooldown.IsCoolingDown && input != Vector2.zero)
            {
                movementBuffer = new MovementBuffer(input, bufferTime);
            } 

            return;
        }

        if (CanMove(forcedInput))
        {
            transform.position += (Vector3)forcedInput;
            movementCooldown.StartCooldown();
        }

        lastInput = input;
        lastMove = forcedInput;
        bufferCooldown.StartCooldown();
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

        if (!groundTilemap.HasTile(gridPosition) || collisionTilemap.HasTile(gridPosition))
        {
            return false;
        }

        return true;
    }
}
