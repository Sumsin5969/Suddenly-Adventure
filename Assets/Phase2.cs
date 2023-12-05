using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase2 : StateMachineBehaviour
{
    public float speed;

    Rigidbody2D rb;
    Animator anim;
    Transform transform;
    public float attackRange;
    public float attackRange2;
    public float moveRange;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        anim = animator.GetComponent<Animator>();
        transform = anim.transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //anim.SetBool("isWalk", true);
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // attackRange 안에 들어오면 && attackRange2 보다 밖이면 공격1    
        //if (Vector2.Distance(player.position, rb.position) <= attackRange)
        //{
        //    animator.SetTrigger("isAttack");
        //}
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.ResetTrigger("isAttack");
    }
}
