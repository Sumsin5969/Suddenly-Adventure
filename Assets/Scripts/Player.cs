using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 점프 상태가 아닐 시 점프 가능 (무한점프 방지)
        if (Input.GetKeyDown(KeyCode.UpArrow) && !anim.GetBool("isJumping")) 
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }

        // 방향 전환
        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        float h = Input.GetAxis("Horizontal"); // 키보드 입력값

        // 항상 최대 속도로 설정
        rigid.velocity = new Vector2(h * maxSpeed, rigid.velocity.y);

        // 무브 애니메이션
        if (Mathf.Abs(rigid.velocity.x) < 0.5)
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
        


        if(rigid.velocity.y < 0) // 내려갈 때만 레이캐스트를 쏨
        {
            // 레이캐스트 그리기
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            // 레이캐스트 히트, 레이어마스크 인식
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            // 착지
            if(rayHit.collider != null)
                if(rayHit.distance < 1.2f)
                    anim.SetBool("isJumping", false);
                    //Debug.Log(rayHit.collider.name);
        }
    }
}
