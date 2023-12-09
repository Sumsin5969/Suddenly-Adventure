using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss_Health : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    public int enemyhealth;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FinalBossHit(int damage)
    {
        if(!anim.GetBool("Banish"))
        {
            enemyhealth -= damage;

            if (enemyhealth <= 0) // 적 사망
            {
                anim.SetTrigger("Death");
                gameObject.layer = 14;

                BoxCollider2D collider = GetComponent<BoxCollider2D>();
                collider.tag = "Boss1_Death";
            }
            else // 적 피격
            {
                // anim.SetTrigger("Damaged");
                spriteRenderer.color = new Color(1, 1, 1, 0.4f);
                Invoke("OffDamaged", 0.5f);
            }
        }
        

        //if (enemyhealth <= 5)
        //{
        //    anim.SetBool("isRage", true);
        //}
    }
    public void OffDamaged()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
