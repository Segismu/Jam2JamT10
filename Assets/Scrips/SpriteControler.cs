using System;
using System.Collections;
using System.Collections.Generic;
using Scrips;
using UnityEngine;

public class SpriteControler : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    bool alreadyFlippedFromWallJump = false;

    void Update()
    {
        switch (PlayerController.State)
        {
            case CatState.Jump:
                alreadyFlippedFromWallJump = false;
                animator.SetBool("cat_walk", false);
                animator.SetTrigger("jump");
                break;
            case CatState.PreparingJump:
                alreadyFlippedFromWallJump = false;
                animator.SetBool("cat_walk", false);
                animator.SetTrigger("prepare_jump");
                break;
            case CatState.ClimbRightWall:
                alreadyFlippedFromWallJump = false;
                animator.SetBool("cat_walk", false);
                animator.SetTrigger("climb");
                spriteRenderer.flipX = false;
                break;
            case CatState.ClimbLeftWall:
                alreadyFlippedFromWallJump = false;
                animator.SetBool("cat_walk", false);
                animator.SetTrigger("climb");
                spriteRenderer.flipX = true;
                break;
            case CatState.Walk:
                alreadyFlippedFromWallJump = false;
                DefineWalkSprite();
                break;
            case CatState.WallJump:
                animator.SetBool("cat_walk", false);
                animator.SetTrigger("jump");
                if (!alreadyFlippedFromWallJump)
                {
                    alreadyFlippedFromWallJump = true;
                    spriteRenderer.flipX = !spriteRenderer.flipX;
                }
                break;
                
        }

    }

    void DefineWalkSprite()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput == 0)
        {
            animator.SetBool("cat_walk", false);
            animator.SetTrigger("idle");
        }else if (horizontalInput > 0)
        {
            animator.SetBool("cat_walk", true);
            spriteRenderer.flipX = false;
        }
        else
        {
            animator.SetBool("cat_walk", true);
            spriteRenderer.flipX = true;
        }
    }
}
