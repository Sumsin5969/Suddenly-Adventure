using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // 마찰력
        if(Input.GetButtonUp("Horizontal")) 
        {
            rigid.velocity = new Vector2 (rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        // 방향 전환
        if(Input.GetButtonDown("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }
        // 무빙 애니메이션
        if(Mathf.Abs(rigid.velocity.x) < 0.2)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }
        
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal"); // 키보드 입력값

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
        
        if (rigid.velocity.x > maxSpeed) // 오른쪽 최고 속도
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // 왼쪽 최고 속도
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
    }
}
