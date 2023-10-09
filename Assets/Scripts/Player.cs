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
    bool isMoving = false;

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

        // 키 입력이 있는 경우에만 가속을 주도록 수정
        if (Mathf.Abs(h) > 0.1f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        // 항상 최대 속도로 설정
        if (isMoving)
        {
            rigid.velocity = new Vector2(h * maxSpeed, rigid.velocity.y);
        }

        // 무브 애니메이션
        anim.SetBool("isWalking", isMoving);
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

    private void OnCollisionEnter2D(Collision2D collision) // 피격
    {
        // 플레이어가 몬스터랑 접촉시
        if (collision.gameObject.tag == "Enemy")
        {
            OnDamaged(collision.transform.position);
        }
    }
    private void OnDamaged(Vector2 targetPos) // 피격시 설정
    {
        // 레이어를 PlayDameged로 변경
        gameObject.layer = 11;

        // 피격시 색상 및 투명도 설정
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // 피격시 밀려남
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 20, ForceMode2D.Impulse);

        // 애니메이션
        anim.SetTrigger("doDamaged");
        Invoke("OffDamaged", 1); // Player 레이어로 돌아가는 시간(무적시간설정)
    }
    private void OffDamaged() // 레이어를 Player로 변경
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
