using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CapsuleCollider2D capsuleCollider;
    bool isMoving = false;
    private float curTime;
    public float coolTime = 0.5f;
    public Transform pos;
    public Vector3 boxSize;
    public GameManager gameManager;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        // 무한점프 방지
        if (Input.GetKeyDown(KeyCode.UpArrow) && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }

        // 방향 전환
        if(Input.GetButton("Horizontal"))
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }

        float h = Input.GetAxis("Horizontal"); // 키보드 입력값

        
            
        // 키 입력이 있는 경우에만 애니메이션이 나옴
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

        if (curTime <= 0)
        {
            // 'Z' 키를 사용해 공격
            if (Input.GetKey(KeyCode.Z))
            {
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

                // 공격 범위 안에서 공격하면 디버그 로그 출력
                foreach (Collider2D collider in collider2Ds)
                {
                    Debug.Log(collider.tag);
                }

                anim.SetTrigger("attack");
                curTime = coolTime;
            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }
    }

    // 공격 범위 그리기
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, boxSize);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            // Point 획득
            bool isBronze = collision.gameObject.name.Contains("Bronze");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");
            if(isBronze)
                gameManager.stagePoint += 50;
            else if(isSilver)
                gameManager.stagePoint += 100;
            else if(isGold)
                gameManager.stagePoint += 300;
            // Item 삭제
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Finish")
        {
            // Next Stage
            gameManager.NextStage();
        }
    }
    private void OnDamaged(Vector2 targetPos) // 피격시 설정
    {
        // 체력 감소
        gameManager.HealthDown();

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

    public void OnDie()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        spriteRenderer.flipY = true;
        capsuleCollider.enabled = false;
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }
}
