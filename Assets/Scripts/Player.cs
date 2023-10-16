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
        // ?í”„ ?íƒœê°€ ?„ë‹ ???í”„ ê°€??(ë¬´í•œ?í”„ ë°©ì?)
        if (Input.GetKeyDown(KeyCode.UpArrow) && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }

        // ¹æÇâ ÀüÈ¯
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

        float h = Input.GetAxis("Horizontal"); // ?¤ë³´???…ë ¥ê°?

        
            
        // ???…ë ¥???ˆëŠ” ê²½ìš°?ë§Œ ê°€?ì„ ì£¼ë„ë¡??˜ì •
        if (Mathf.Abs(h) > 0.1f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        // ??ƒ ìµœë? ?ë„ë¡??¤ì •
        if (isMoving)
        {
            rigid.velocity = new Vector2(h * maxSpeed, rigid.velocity.y);
        }

        // ë¬´ë¸Œ ? ë‹ˆë©”ì´??
        anim.SetBool("isWalking", isMoving);

        if (curTime <= 0)
        {
            // 'Z' Å°¸¦ »ç¿ëÇØ °ø°İ
            if (Input.GetKey(KeyCode.Z))
            {
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

                // °ø°İ ¹üÀ§ ¾È¿¡¼­ °ø°İÇÏ¸é µğ¹ö±× ·Î±× Ãâ·Â
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

    // °ø°İ ¹üÀ§ ±×¸®±â
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

    void FixedUpdate()
    {
        if(rigid.velocity.y < 0) // ?´ë ¤ê°??Œë§Œ ?ˆì´ìºìŠ¤?¸ë? ??
        {
            // ?ˆì´ìºìŠ¤??ê·¸ë¦¬ê¸?
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            // ?ˆì´ìºìŠ¤???ˆíŠ¸, ?ˆì´?´ë§ˆ?¤í¬ ?¸ì‹
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            // ì°©ì?
            if(rayHit.collider != null)
                if(rayHit.distance < 1.2f)
                    anim.SetBool("isJumping", false);
                    //Debug.Log(rayHit.collider.name);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // ?¼ê²©
    {
        // ?Œë ˆ?´ì–´ê°€ ëª¬ìŠ¤?°ë‘ ?‘ì´‰??
        if (collision.gameObject.tag == "Enemy")
        {
            OnDamaged(collision.transform.position);
        }
    }
    private void OnDamaged(Vector2 targetPos) // ?¼ê²©???¤ì •
    {
        // ?ˆì´?´ë? PlayDamegedë¡?ë³€ê²?
        gameObject.layer = 11;

        // ?¼ê²©???‰ìƒ ë°??¬ëª…???¤ì •
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // ?¼ê²©??ë°€?¤ë‚¨
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 20, ForceMode2D.Impulse);

        // ? ë‹ˆë©”ì´??
        anim.SetTrigger("doDamaged");
        Invoke("OffDamaged", 1); // Player ?ˆì´?´ë¡œ ?Œì•„ê°€???œê°„(ë¬´ì ?œê°„?¤ì •)
    }
    private void OffDamaged() // ?ˆì´?´ë? Playerë¡?ë³€ê²?
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
