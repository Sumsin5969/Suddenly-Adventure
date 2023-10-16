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
    bool isMoving = false;
    private float curTime;
    public float coolTime = 0.5f;
    public Transform pos;
    public Vector3 boxSize;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // ?�프 ?�태가 ?�닐 ???�프 가??(무한?�프 방�?)
        if (Input.GetKeyDown(KeyCode.UpArrow) && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }

        // ���� ��ȯ
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

        float h = Input.GetAxis("Horizontal"); // ?�보???�력�?

        
            
        // ???�력???�는 경우?�만 가?�을 주도�??�정
        if (Mathf.Abs(h) > 0.1f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        // ??�� 최�? ?�도�??�정
        if (isMoving)
        {
            rigid.velocity = new Vector2(h * maxSpeed, rigid.velocity.y);
        }

        // 무브 ?�니메이??
        anim.SetBool("isWalking", isMoving);

        if (curTime <= 0)
        {
            // 'Z' Ű�� ����� ����
            if (Input.GetKey(KeyCode.Z))
            {
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

                // ���� ���� �ȿ��� �����ϸ� ����� �α� ���
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

    // ���� ���� �׸���
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

    void FixedUpdate()
    {
        if(rigid.velocity.y < 0) // ?�려�??�만 ?�이캐스?��? ??
        {
            // ?�이캐스??그리�?
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            // ?�이캐스???�트, ?�이?�마?�크 ?�식
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            // 착�?
            if(rayHit.collider != null)
                if(rayHit.distance < 1.2f)
                    anim.SetBool("isJumping", false);
                    //Debug.Log(rayHit.collider.name);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // ?�격
    {
        // ?�레?�어가 몬스?�랑 ?�촉??
        if (collision.gameObject.tag == "Enemy")
        {
            OnDamaged(collision.transform.position);
        }
    }
    private void OnDamaged(Vector2 targetPos) // ?�격???�정
    {
        // ?�이?��? PlayDameged�?변�?
        gameObject.layer = 11;

        // ?�격???�상 �??�명???�정
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // ?�격??밀?�남
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 20, ForceMode2D.Impulse);

        // ?�니메이??
        anim.SetTrigger("doDamaged");
        Invoke("OffDamaged", 1); // Player ?�이?�로 ?�아가???�간(무적?�간?�정)
    }
    private void OffDamaged() // ?�이?��? Player�?변�?
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
