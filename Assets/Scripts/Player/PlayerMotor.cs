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

    private Cooldown movementCooldown;

    [SerializeField]
    private Tilemap groundTileMap;

    [SerializeField]
    private Tilemap collisionTileMap;

    //Temporary variables
    private Vector2 lastInput;
    private Vector2 lastMove;

    private void Start()
    {
        movementCooldown = new Cooldown(moveSpeed);
    }

    public void ProcessMove(Vector2 input)
    {
        Vector2 forcedInput;

        forcedInput = ForceOrthogonalMovement(input);

        if (movementCooldown.IsCoolingDown)
        {
            return;
        }

        if (CanMove(forcedInput))
        {
            transform.position += (Vector3)forcedInput;
            movementCooldown.StartCooldown();
        }

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
        Vector3Int gridPosition = groundTileMap.WorldToCell(transform.position + (Vector3)direction);

        if (!groundTileMap.HasTile(gridPosition) || collisionTileMap.HasTile(gridPosition))
        {
            return false;
        }

        return true;
    }
}
