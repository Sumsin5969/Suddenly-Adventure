using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final_Boss : StateMachineBehaviour
{
    Transform player;
    Rigidbody2D rb;
    Animator anim;
    public float attackRange;
    public float attackRange2;
    public float moveRange;
    public float banishRange;
    public float banishAppearRange;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        anim = animator.GetComponent<Animator>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector2.Distance(player.position, rb.position) <= moveRange)
        {
            anim.SetBool("isMove", true);
        }

        // attackRange 안에 들어오면 공격1    
        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            animator.SetTrigger("isAttack1");
        }
        if (Vector2.Distance(player.position, rb.position) <= attackRange2)
        {
            animator.SetTrigger("isAttack2");
        }

        if (Vector2.Distance(player.position, rb.position) <= banishRange)
        {
            animator.SetBool("Banish", true);
        }
        if (Vector2.Distance(player.position, rb.position) >= banishAppearRange)
        {
            animator.SetBool("Banish", false);
        }

        if (animator.GetBool("Banish"))
        {
            animator.gameObject.layer = 17;
        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("isAttack1");
        animator.ResetTrigger("isAttack2");
    }
}
