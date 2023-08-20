using System;
using System.Collections;
using System.Collections.Generic;
using Scrips;
using UnityEngine;

public class SpriteControler : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animation walkAnimation;
    [SerializeField] Sprite idle;
    [SerializeField] Sprite prepareJump;
    [SerializeField] Sprite jump;
    [SerializeField] Sprite climb;

    void Update()
    {
        switch (PlayerController.State)
        {
            case CatState.Jump:
                walkAnimation.Stop();
                spriteRenderer.sprite = jump;
                break;
            case CatState.PreparingJump:
                walkAnimation.Stop();
                spriteRenderer.sprite = prepareJump;
                break;
            case CatState.ClimbRightWall:
                walkAnimation.Stop();
                spriteRenderer.sprite = climb;
                spriteRenderer.flipX = false;
                break;
            case CatState.ClimbLeftWall:
                walkAnimation.Stop();
                spriteRenderer.sprite = climb;
                spriteRenderer.flipX = true;
                break;
            case CatState.Walk:
                DefineWalkSprite();
                break;
        }

    }

    void DefineWalkSprite()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput == 0)
        {
            walkAnimation.Stop();
            spriteRenderer.sprite = idle;
        }else if (horizontalInput > 0)
        {
            if (!walkAnimation.isPlaying)
                walkAnimation.Play();
            spriteRenderer.flipX = false;
        }
        else
        {
            if (!walkAnimation.isPlaying)
                walkAnimation.Play();
            spriteRenderer.flipX = true;
        }
    }
}
