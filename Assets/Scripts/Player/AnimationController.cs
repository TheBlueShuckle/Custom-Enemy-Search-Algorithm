using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] GameObject sprite;
    private Animator playerAnimator;

    private float lerpTime = 0;


    private void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
    }

    public void LerpSprite(Vector2 startPosition, Vector2 targetPosition, float moveSpeed)
    {
        sprite.transform.position = Vector2.Lerp(startPosition, targetPosition, lerpTime / moveSpeed);
        lerpTime += Time.deltaTime;
    }

    public void ResetLerp()
    {
        lerpTime = 0;
        sprite.transform.localPosition = Vector3.zero;
    }

    public void UpdateSprite(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            return;
        }

        //Change Sprite to player-right
        if (direction.x > 0)
        {
            playerAnimator.Play("move-right");
        }

        else if (direction.x < 0)
        {
            //Change Sprite to player-left
            playerAnimator.Play("move-left");

        }

        else if (direction.y > 0)
        {
            //Change Sprite to player-up
            playerAnimator.Play("move-up");

        }

        else
        {
            //Change Sprite to player-down
            playerAnimator.Play("move-down");

        }
    }
}
