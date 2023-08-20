using System;
using System.Collections;
using System.Collections.Generic;
using Scrips;
using UnityEngine;

public class SpriteControler : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;

    void Update()
    {
        switch (PlayerController.State)
        {
            case CatState.Jump:
                animator.SetBool("cat_walk", false);
                animator.SetTrigger("jump");
                break;
            case CatState.PreparingJump:
                animator.SetBool("cat_walk", false);
                animator.SetTrigger("prepare_jump");
                break;
            case CatState.ClimbRightWall:
                animator.SetBool("cat_walk", false);
                animator.SetTrigger("climb");
                spriteRenderer.flipX = false;
                break;
            case CatState.ClimbLeftWall:
                animator.SetBool("cat_walk", false);
                animator.SetTrigger("climb");
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
