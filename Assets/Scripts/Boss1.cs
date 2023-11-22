using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

// 보스 이동 기능
public class Boss1 : StateMachineBehaviour
{
    public float speed = 2.5f;

    Transform player;
    Rigidbody2D rb;
    boss2 boss;
    Animator anim;
    public float attackRange;
    public float attackRange2;
    public float moveRange;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<boss2>();
        anim = animator.GetComponent<Animator>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector2.Distance(player.position, rb.position) <= moveRange)
        {
            anim.SetBool("isWalk", true);
            boss.LookPlayer();
            Vector2 target = new Vector2(player.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
        
        // attackRange 안에 들어오면 && attackRange2 보다 밖이면 공격1    
        if (Vector2.Distance(player.position, rb.position) <= attackRange && Vector2.Distance(player.position, rb.position) > attackRange2)
        {
            animator.SetTrigger("isAttack");
        }
        // attackRange2 안에 들어오면 공격2
        if (Vector2.Distance(player.position, rb.position) <= attackRange2)
        {
            animator.SetTrigger("isAttack2");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("isAttack");
        animator.ResetTrigger("isAttack2");
    }
}

