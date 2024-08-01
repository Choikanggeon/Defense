using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMove : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private EnemyMovement enemyMovement;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        if (Mathf.Abs(enemyMovement.rb.velocity.y) > Mathf.Abs(enemyMovement.rb.velocity.x))
        {
            if (enemyMovement.rb.velocity.y < 0)
            {
                PlayAnimation("BatFront");
            }
            else
            {
                PlayAnimation("BatBack");
            }
        }
        else
        {
            if (enemyMovement.rb.velocity.x < 0)
            {
                FlipSprite(false);
                PlayAnimation("BatLeft");
            }
            else
            {
                FlipSprite(true);
                PlayAnimation("BatLeft");
            }
        }
    }

    private void FlipSprite(bool flipX)
    {
        spriteRenderer.flipX = flipX;
    }

    private void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }
}
