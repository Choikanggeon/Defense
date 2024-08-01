using UnityEngine;

public class SlimeMove : MonoBehaviour
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
                PlayAnimation("FrontSlime");
            }
            else
            {
                PlayAnimation("BackSlime");
            }
        }
        else
        {
            if (enemyMovement.rb.velocity.x > 0)
            {
                FlipSprite(false);
                PlayAnimation("RightSlime");
            }
            else
            {
                FlipSprite(true);
                PlayAnimation("RightSlime");
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
