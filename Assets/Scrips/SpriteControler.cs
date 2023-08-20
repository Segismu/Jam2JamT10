using System;
using System.Collections;
using System.Collections.Generic;
using Scrips;
using UnityEngine;

public class SpriteControler : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] Sprite idle;
    [SerializeField] Sprite prepareJump;
    [SerializeField] Sprite jump;
    [SerializeField] Sprite climb;

    void Update()
    {
        switch (PlayerController.State)
        {
            case CatState.Jump:
                animator.SetBool("cat_walk", false);
                animator.SetTrigger("jump");
                spriteRenderer.sprite = jump;
                break;
            case CatState.PreparingJump:
                animator.SetBool("cat_walk", false);
                animator.SetTrigger("prepare_jump");
                spriteRenderer.sprite = prepareJump;
                break;
            case CatState.ClimbRightWall:
                animator.SetBool("cat_walk", false);
                animator.SetTrigger("climb");
                spriteRenderer.sprite = climb;
                spriteRenderer.flipX = false;
                break;
            case CatState.ClimbLeftWall:
                animator.SetBool("cat_walk", false);
                animator.SetTrigger("climb");
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
            animator.SetBool("cat_walk", false);
            animator.SetTrigger("idle");
            spriteRenderer.sprite = idle;
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
