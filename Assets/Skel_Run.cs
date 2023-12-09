using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skel_Run : StateMachineBehaviour
{
    public float speed = 2.5f;

    Transform player;
    Rigidbody2D rb;
    Skel skel;
    Animator anim;
    public float attackRange;
    public float moveRange;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        skel = animator.GetComponent<Skel>();
        anim = animator.GetComponent<Animator>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if (Vector2.Distance(player.position, rb.position) <= moveRange)
       {
            anim.SetBool("isWalk", true);
            skel.LookPlayer();
            Vector2 target = new Vector2(player.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
       }
       else
        {
            anim.SetBool("isWalk", false);
        }

        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            animator.SetTrigger("isAttack");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("isAttack");
    }
}
