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
        // �������� ����
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

        float h = Input.GetAxis("Horizontal"); // Ű���� �Է°�

        
            
        // Ű �Է��� �ִ� ��쿡�� �ִϸ��̼��� ����
        if (Mathf.Abs(h) > 0.1f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        // �׻� �ִ� �ӵ��� ����
        if (isMoving)
        {
            rigid.velocity = new Vector2(h * maxSpeed, rigid.velocity.y);
        }

        // ���� �ִϸ��̼�
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
        if(rigid.velocity.y < 0) // ������ ���� ����ĳ��Ʈ�� ��
        {
            // ����ĳ��Ʈ �׸���
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            // ����ĳ��Ʈ ��Ʈ, ���̾��ũ �ν�
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            // ����
            if(rayHit.collider != null)
                if(rayHit.distance < 1.2f)
                    anim.SetBool("isJumping", false);
                    //Debug.Log(rayHit.collider.name);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // �ǰ�
    {
        // �÷��̾ ���Ͷ� ���˽�
        if (collision.gameObject.tag == "Enemy")
        {
            OnDamaged(collision.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            // Point ȹ��
            bool isBronze = collision.gameObject.name.Contains("Bronze");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");
            if(isBronze)
                gameManager.stagePoint += 50;
            else if(isSilver)
                gameManager.stagePoint += 100;
            else if(isGold)
                gameManager.stagePoint += 300;
            // Item ����
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Finish")
        {
            // Next Stage
            gameManager.NextStage();
        }
    }
    private void OnDamaged(Vector2 targetPos) // �ǰݽ� ����
    {
        // ü�� ����
        gameManager.HealthDown();

        // ���̾ PlayDameged�� ����
        gameObject.layer = 11;

        // �ǰݽ� ���� �� ���� ����
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // �ǰݽ� �з���
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 20, ForceMode2D.Impulse);

        // �ִϸ��̼�
        anim.SetTrigger("doDamaged");
        Invoke("OffDamaged", 1); // Player ���̾�� ���ư��� �ð�(�����ð�����)
    }
    private void OffDamaged() // ���̾ Player�� ����
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
