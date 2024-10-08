using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements.Experimental;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
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
    private float lerpTime = 0;

    private void Start()
    {
        movementCooldown = new Cooldown(moveSpeed);
        animationController = GetComponent<AnimationController>();

        targetPosition = transform.position;
    }

    private void Update()
    {
        if (movementCooldown.IsCoolingDown)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, lerpTime / moveSpeed);
            lerpTime += Time.deltaTime;
        }

        else
        {
            lerpTime = 0;
            transform.position = targetPosition;
        }
    }

    public void ProcessMove(Vector2 input)
    {
        Vector2 forcedInput;

        forcedInput = ForceOrthogonalMovement(input);

        if (movementCooldown.IsCoolingDown)
        {
            return;
        }

        if (CanMove(forcedInput) && input != Vector2.zero)
        {
            startPosition = transform.position;
            targetPosition = transform.position + (Vector3)forcedInput;
            movementCooldown.StartCooldown();
        }

        animationController.UpdateSprite(forcedInput);

        lastInput = input;
        lastMove = forcedInput;
    }

    public void ProcessTurn(Vector2 input)
    {
        Vector2 forcedInput = ForceOrthogonalMovement(input);

        animationController.UpdateSprite(forcedInput);
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
