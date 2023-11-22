using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss1_Health : MonoBehaviour
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
    public void Boss1Hit(int damage)
    {
        enemyhealth -= damage;

        if (enemyhealth <= 0) // 적 사망
        {
            anim.SetTrigger("Die");
            gameObject.layer = 11;
        }
        else // 적 피격
        {
            anim.SetTrigger("Damaged");
        }
    }
}
