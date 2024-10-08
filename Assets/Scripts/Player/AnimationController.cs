using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator playerAnimator;


    private void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
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
