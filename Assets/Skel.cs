using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skel : MonoBehaviour
{
    public Transform player;
    public bool isFlip = false;
    public float moveRange;

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capCollider;

    public int enemyhealth;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        capCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void EnemyHit(int damage)
    {
        enemyhealth -= damage;

        if (enemyhealth <= 0) // 적 사망
        {
            anim.SetTrigger("Death");
            anim.Play("Death", 0);
            gameObject.layer = 14;
        }
        else // 적 피격
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            Transform playerTransform = playerObject.transform;
            anim.SetTrigger("Hit");
            anim.Play("Hit", 0);

            int add = transform.position.x - playerTransform.position.x > 0 ? 1 : -1;
            rigid.AddForce(new Vector2(add, 0.5f) * 2, ForceMode2D.Impulse);
        }
    }

    public void OffDamaged()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void LookPlayer()
    {
        Vector3 flip = transform.localScale;
        flip.z *= -1f;
        
        if(transform.position.x > player.position.x && isFlip)
        {
            transform.localScale = flip;
            transform.Rotate(0, 180, 0);
            isFlip = false;
        }
        else if (transform.position.x < player.position.x && !isFlip)
        {
            transform.localScale = flip;
            transform.Rotate(0, 180, 0);
            isFlip = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; // 색상 설정
        Gizmos.DrawWireSphere(transform.position, moveRange);
    }
}
